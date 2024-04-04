using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using System;
using SObject = StardewValley.Object;

namespace BetterArtisanGoodIcons.Patches.SObjectPatches
{
    internal class DrawWhenHeldPatch
    {
        /// <summary>Draw the correct texture based on <see cref="SObject.preservedParentSheetIndex"/> or <see cref="SObject.Name"/>.</summary>
        public static bool Prefix(SObject __instance, SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            if (!ArtisanGoodsManager.GetDrawInfo(__instance, out Texture2D spriteSheet, out Rectangle position, out Rectangle iconPosition))
                return true;

            //By popular demand, don't show icons when held.
            //if (iconPosition != Rectangle.Empty)
            //    spriteBatch.Draw(Game1.objectSpriteSheet, objectPosition + new Vector2(1, 1), new Microsoft.Xna.Framework.Rectangle?(iconPosition), Color.White, 0.0f, new Vector2(4f, 4f), Game1.pixelZoom / 2, SpriteEffects.None, Math.Max(0.0f, (f.getStandingY() + 2) / 10000f));

            spriteBatch.Draw(spriteSheet, objectPosition, position, Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, Math.Max(0.0f, (f.StandingPixel.Y + 2) / 10000f));
            return false;
        }
    }
}