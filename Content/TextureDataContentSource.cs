using StardewModdingAPI;
using System;
using System.Collections.Generic;

namespace BetterArtisanGoodIcons.Content
{
    /// <inheritdoc />
    /// <summary>A basic content source provider - is able to group data from each CustomTextureData together properly.</summary>
    internal abstract class TextureDataContentSource : IContentSource
    {
        public abstract CustomTextureData TextureData { get; }
        public abstract T Load<T>(string path);
        public abstract IManifest GetManifest();

        /// <summary>Group each texture path with its corresponding source list and artisan good type.</summary>
        public IEnumerable<Tuple<string, List<string>, ArtisanGood>> GetData()
        {
            yield return new Tuple<string, List<string>, ArtisanGood>(this.TextureData.Honey,
                this.TextureData.Flowers, ArtisanGood.Honey);

            yield return new Tuple<string, List<string>, ArtisanGood>(this.TextureData.Juice,
                this.TextureData.Vegetables, ArtisanGood.Juice);

            yield return new Tuple<string, List<string>, ArtisanGood>(this.TextureData.Pickles,
                this.TextureData.Vegetables, ArtisanGood.Pickles);

            yield return new Tuple<string, List<string>, ArtisanGood>(this.TextureData.Wine,
                this.TextureData.Fruits, ArtisanGood.Wine);

            yield return new Tuple<string, List<string>, ArtisanGood>(this.TextureData.Jelly,
                this.TextureData.Fruits, ArtisanGood.Jelly);
        }
    }
}
