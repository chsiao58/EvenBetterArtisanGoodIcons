using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;

namespace BetterArtisanGoodIcons.Patches.FurniturePatches
{
    /// <summary>Draw the right texture for the object placed on furniture.</summary>
    internal class DrawPatch
    {
        public static bool Prefix(Furniture __instance, SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            if (__instance.heldObject.Value == null || __instance.heldObject.Value is Furniture ||
                !ArtisanGoodsManager.GetDrawInfo(__instance.heldObject.Value, out Texture2D spriteSheet, out Rectangle position, out Rectangle iconPosition))
                return true;
            if (x == -1)
            {
                Vector2 drawPos = new Vector2(__instance.boundingBox.X, __instance.boundingBox.Y - (__instance.sourceRect.Height * Game1.pixelZoom - __instance.boundingBox.Value.Height));
                spriteBatch.Draw(Game1.content.Load<Texture2D>(Furniture.furnitureTextureName), Game1.GlobalToLocal(Game1.viewport, drawPos), new Rectangle?(__instance.sourceRect.Value), Color.White * alpha, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, __instance.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, __instance.furniture_type.Value == 12 ? 0.0f : (__instance.boundingBox.Value.Bottom - 8) / 10000f);
            }
            else
                spriteBatch.Draw(Game1.content.Load<Texture2D>(Furniture.furnitureTextureName), Game1.GlobalToLocal(Game1.viewport, new Vector2(x * Game1.tileSize, y * Game1.tileSize - (__instance.sourceRect.Value.Height * Game1.pixelZoom - __instance.boundingBox.Value.Height))), new Rectangle?(__instance.sourceRect.Value), Color.White * alpha, 0.0f, Vector2.Zero, Game1.pixelZoom, __instance.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, __instance.furniture_type.Value == 12 ? 0.0f : (__instance.boundingBox.Value.Bottom - 8) / 10000f);

            spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(__instance.boundingBox.Value.Center.X - Game1.tileSize / 2, __instance.boundingBox.Value.Center.Y - (__instance.drawHeldObjectLow.Value ? Game1.tileSize / 2 : Game1.tileSize * 4 / 3))) + new Vector2(Game1.tileSize / 2f, Game1.tileSize * 5f / 6), new Rectangle?(Game1.shadowTexture.Bounds), Color.White * alpha, 0.0f, new Vector2(Game1.shadowTexture.Bounds.Center.X, Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, __instance.boundingBox.Value.Bottom / 10000f);
            spriteBatch.Draw(spriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(__instance.boundingBox.Value.Center.X - Game1.tileSize / 2, __instance.boundingBox.Value.Center.Y - (__instance.drawHeldObjectLow.Value ? Game1.tileSize / 2 : Game1.tileSize * 4 / 3))), new Rectangle?(position), Color.White * alpha, 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, (__instance.boundingBox.Value.Bottom + 1) / 10000f);
            return false;
        }
    }
}
