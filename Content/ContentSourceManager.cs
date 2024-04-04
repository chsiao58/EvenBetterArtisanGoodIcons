using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;

namespace BetterArtisanGoodIcons.Content
{
    /// <summary>Handles alternate textures from content packs.</summary>
    internal class ContentSourceManager
    {
        /// <summary>Map an <see cref="ArtisanGood"/> to its source name to make content pack debugging easier.</summary>
        private static readonly IDictionary<ArtisanGood, string> artisanGoodToSourceType = new Dictionary<ArtisanGood, string>
        {
            {ArtisanGood.Honey, "Flowers" },
            {ArtisanGood.Jelly, "Fruits" },
            {ArtisanGood.Wine, "Fruits" },
            {ArtisanGood.Juice, "Vegetables" },
            {ArtisanGood.Pickles, "Vegetables" }
        };

        /// <summary>Gets all valid <see cref="ArtisanGoodTextureProvider"/>s that can be used to get artisan good icons.</summary>
        internal static IEnumerable<ArtisanGoodTextureProvider> GetTextureProviders(IModHelper helper, IMonitor monitor)
        {
            //Load content packs first so vanilla icons can be overwritten
            foreach (IContentPack pack in helper.ContentPacks.GetOwned())
            {
                foreach (ArtisanGoodTextureProvider provider in TryLoadContentSource(new ContentPackSource(pack), monitor))
                    yield return provider;
            }

            //Load vanilla icons
            foreach (ArtisanGoodTextureProvider provider in TryLoadContentSource(new ModSource(helper), monitor))
                yield return provider;
        }

        /// <summary>Tries to load textures for all artisan good types for a given <see cref="IContentSource"/>.</summary>
        private static IEnumerable<ArtisanGoodTextureProvider> TryLoadContentSource(TextureDataContentSource contentSource, IMonitor monitor)
        {
            foreach (Tuple<string, List<string>, ArtisanGood> item in contentSource.GetData())
            {
                if (TryLoadTextureProvider(contentSource, item.Item1, item.Item2, item.Item3, monitor,
                    out ArtisanGoodTextureProvider provider))
                    yield return provider;
            }
        }

        /// <summary>Tries to load a texture given the <see cref="IContentSource"/>, the path to the texture, the list of source names for it, and the good type.</summary>
        private static bool TryLoadTextureProvider(IContentSource contentSource, string imagePath, List<string> source, ArtisanGood good, IMonitor monitor, out ArtisanGoodTextureProvider provider)
        {
            provider = null;

            if (imagePath == null)
                return false;

            IManifest manifest = contentSource.GetManifest();
            if (source == null || source.Count == 0 || source.Any(item => item == null))
            {
                monitor.Log($"Couldn't load {good} from {manifest.Name} ({manifest.UniqueID}) because it has an invalid source list ({artisanGoodToSourceType[good]}).", LogLevel.Warn);
                monitor.Log($"{artisanGoodToSourceType[good]} must not be null, must not be empty, and cannot have null items inside it.", LogLevel.Warn);
            }
            else
            {
                try
                {
                    provider = new ArtisanGoodTextureProvider(contentSource.Load<Texture2D>(imagePath), source, good);
                    return true;
                }
                catch (Exception)
                {
                    monitor.Log($"Couldn't load {good} from {manifest.Name} ({manifest.UniqueID}) because the {good} texture file path is invalid ({imagePath}).", LogLevel.Warn);
                }
            }

            return false;
        }
    }
}