using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.GameData.Objects;
using StardewValley.ItemTypeDefinitions;
using System.Collections.Generic;
using SObject = StardewValley.Object;

namespace BetterArtisanGoodIcons
{
    /// <summary>Provides textures for certain artisan goods.</summary>
    internal class ArtisanGoodTextureProvider
    {
        /// <summary>The spritesheet to pull textures from.</summary>
        private readonly Texture2D spriteSheet;

        /// <summary>The rectangles that correspond to each item name.</summary>
        private readonly IDictionary<string, Rectangle> positions = new Dictionary<string, Rectangle>();

        /// <summary>The type of artisan good this provides texture for.</summary>
        private readonly ArtisanGood good;

        internal ArtisanGoodTextureProvider(Texture2D texture, List<string> names, ArtisanGood good)
        {
            this.spriteSheet = texture;
            this.good = good;

            //Get sprite positions assuming names go left to right
            int x = 0;
            int y = 0;
            foreach (string item in names)
            {
                this.positions[item] = new Rectangle(x, y, 16, 16);
                x += 16;
                if (x >= texture.Width)
                {
                    x = 0;
                    y += 16;
                }
            }

        }

        /// <summary>Gets the name of the source item used to create the given item.</summary>
        private bool GetSourceName(SObject item, string sourceIndex, out string sourceName)
        {
            //If the item name is equivalent to the base good, return _Base.
            if (item.Name == this.good.ToString())
            {
                sourceName = "_Base";
                return true;
            }

            //Lookup the name from the game's object information, or null if not found (a custom item that has its sourceIndex set incorrectly).
            if (Game1.objectData.TryGetValue(sourceIndex, out ObjectData information))
            {
                sourceName = information.Name;
                return true;
            }

            sourceName = null;
            return false;
        }

        /// <summary>Gets the index of the source item used to create the given item name.</summary>
        private bool GetIndexOfSource(SObject item, out string index)
        {
            //Use preservedParentSheetIndex for wine, jelly, pickles, and juice
            if (item.preservedParentSheetIndex.Value != null)
            {
                index = item.preservedParentSheetIndex.Value;
                return true;
            }

            index = null;
            return false;
        }

        /// <summary>Gets the info needed to draw the right texture for the given item.</summary>
        internal bool GetDrawInfo(SObject item, ref Texture2D textureSheet, ref Rectangle mainPosition, ref Rectangle iconPosition)
        {
            //TODO: This actually disallows changing the base texture b/c it won't get past the second if statement, <-- TODO: check if this is still relevent
            //TODO: also the != -1 check will also be false. <-- TODO: check if this is still relevent

            //Only yield new textures for base items. If removed, everything *should* still work, but it needs more testing.
            if (item.ParentSheetIndex != (int)this.good)
                return false;

            //If the index of the source item can't be found, exit.
            if (!this.GetIndexOfSource(item, out string sourceIndex))
                return false;

            ParsedItemData source = ItemRegistry.GetDataOrErrorItem("(O)" + item.preservedParentSheetIndex.Value);
            //Get the name of the item from its index, and from that, a new sprite.
            string sourceName = source.InternalName;
            if (!this.positions.TryGetValue(sourceName, out mainPosition))
                return false;

            textureSheet = this.spriteSheet;
            iconPosition = sourceIndex != null ? source.GetSourceRect() : Rectangle.Empty;
            return true;
        }
    }
}