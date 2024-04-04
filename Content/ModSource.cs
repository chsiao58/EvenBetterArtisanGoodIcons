using StardewModdingAPI;

namespace BetterArtisanGoodIcons.Content
{
    /// <inheritdoc />
    /// <summary>A content source that comes from a mod. </summary>
    internal class ModSource : TextureDataContentSource
    {
        private readonly IModHelper helper;

        public override CustomTextureData TextureData { get; }

        public ModSource(IModHelper helper)
        {
            this.helper = helper;
            //1.3.28
            //this.TextureData = helper.ReadJsonFile<CustomTextureData>("assets/data.json");
            //1.3.31
            this.TextureData = helper.Data.ReadJsonFile<CustomTextureData>("assets/data.json");
        }

        public override T Load<T>(string path)
        {
            return this.helper.ModContent.Load<T>(path);
        }

        public override IManifest GetManifest()
        {
            return this.helper.ModRegistry.Get(this.helper.ModRegistry.ModID).Manifest;
        }
    }
}
