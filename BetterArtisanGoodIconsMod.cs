using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BetterArtisanGoodIcons.Extensions;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley.Objects;

namespace BetterArtisanGoodIcons
{
    /// <summary>Draws different icons for different Artisan Good types.</summary>
    /// <remarks>Honey does not save the original item in <see cref="StardewValley.Object.preservedParentSheetIndex"/> so we have to use its name to determine its type, resulting in
    /// honey and non-honey versions of things.</remarks>
    public class BetterArtisanGoodIconsMod : Mod
    {
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            ArtisanGoodsManager.Init(this.Helper, this.Monitor);

            Harmony harmony = new Harmony("cat.betterartisangoodicons");

            //Don't need to override draw for Object because artisan goods can't be placed down.
            Type objectType = typeof(ColoredObject);
            IList<Tuple<string, Type, Type>> replacements = new List<Tuple<string, Type, Type>>
            {
                {"drawWhenHeld", objectType, typeof(Patches.SObjectPatches.DrawWhenHeldPatch)},
                {"drawInMenu", objectType, typeof(Patches.SObjectPatches.DrawInMenuPatch)},
                {"draw", objectType, typeof(Patches.SObjectPatches.DrawPatch)},
                {"drawWhenHeld", typeof(StardewValley.Object), typeof(Patches.SObjectPatches.DrawWhenHeldPatch)},   // 
                {"drawInMenu", typeof(StardewValley.Object), typeof(Patches.SObjectPatches.DrawInMenuPatch)},       // for honey, since honey is not a ColoredObject
                {"draw", typeof(StardewValley.Object), typeof(Patches.SObjectPatches.DrawPatch)},                   // 
                {"draw", typeof(Furniture), typeof(Patches.FurniturePatches.DrawPatch)}
            };

            foreach (Tuple<string, Type, Type> replacement in replacements)
            {
                MethodInfo original = replacement.Item2.GetMethods(BindingFlags.Instance | BindingFlags.Public).ToList().Find(m => m.Name == replacement.Item1);

                MethodInfo prefix = replacement.Item3.GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(item => item.Name == "Prefix");
                MethodInfo postfix = replacement.Item3.GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(item => item.Name == "Postfix");

                harmony.Patch(original, prefix == null ? null : new HarmonyMethod(prefix), postfix == null ? null : new HarmonyMethod(postfix));
            }
        }
    }
}