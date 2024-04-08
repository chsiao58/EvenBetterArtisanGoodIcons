using StardewModdingAPI;

namespace BetterArtisanGoodIcons.Content
{
    /// <inheritdoc />
    /// <summary>A content source that comes from Content Packs. </summary>
    internal class ContentPackSource : TextureDataContentSource
    {
        private readonly IContentPack pack;
        public override CustomTextureData TextureData { get; }

        public ContentPackSource(IContentPack pack)
        {
            this.pack = pack;
            TextureData = pack.ReadJsonFile<CustomTextureData>("data.json");
        }

        public override T Load<T>(string path)
        {
            return pack.ModContent.Load<T>(path);
        }

        public override IManifest GetManifest()
        {
            return pack.Manifest;
        }
    }
}
