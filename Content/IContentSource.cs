using StardewModdingAPI;

namespace BetterArtisanGoodIcons.Content
{
    /// <summary>An abstraction over the ability to load textures and get manifest information. Used to unify loading vanilla and custom texture assets.</summary>
    internal interface IContentSource
    {
        T Load<T>(string path);
        IManifest GetManifest();
    }
}
