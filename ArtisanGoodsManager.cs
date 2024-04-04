using BetterArtisanGoodIcons.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using System.Collections.Generic;
using SObject = StardewValley.Object;

namespace BetterArtisanGoodIcons
{
    /// <summary>Manages textures for all artisan goods.</summary>
    internal static class ArtisanGoodsManager
    {
        public static IMonitor Monitor;
        /// <summary>Texture managers that get the correct texture for each item.</summary>
        private static readonly IList<ArtisanGoodTextureProvider> TextureProviders = new List<ArtisanGoodTextureProvider>();

        /// <summary>Mod config.</summary>
        private static BetterArtisanGoodIconsConfig config;

        /// <summary>Initializes the manager.</summary>
        internal static void Init(IModHelper helper, IMonitor monitor)
        {
            Monitor = monitor;
            config = helper.ReadConfig<BetterArtisanGoodIconsConfig>();

            foreach (ArtisanGoodTextureProvider provider in ContentSourceManager.GetTextureProviders(helper, monitor))
                TextureProviders.Add(provider);
        }

        /// <summary>Gets the info needed to draw the correct texture.</summary>
        internal static bool GetDrawInfo(SObject output, out Texture2D textureSheet, out Rectangle mainPosition, out Rectangle iconPosition)
        {
            textureSheet = Game1.objectSpriteSheet;
            mainPosition = Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, output.ParentSheetIndex, 16, 16);
            iconPosition = Rectangle.Empty;

            foreach (ArtisanGoodTextureProvider manager in TextureProviders)
            {
                if (manager.GetDrawInfo(output, ref textureSheet, ref mainPosition, ref iconPosition))
                {
                    if (config.DisableSmallSourceIcons)
                        iconPosition = Rectangle.Empty;
                    return true;
                }
            }

            return false;
        }
    }
}