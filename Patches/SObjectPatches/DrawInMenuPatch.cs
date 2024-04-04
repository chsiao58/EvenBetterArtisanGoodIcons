using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Threading;
using SObject = StardewValley.Object;

namespace BetterArtisanGoodIcons.Patches.SObjectPatches
{
    internal class DrawInMenuPatch
    {
        /// <summary>Draw the correct texture based on <see cref="SObject.preservedParentSheetIndex"/> or <see cref="SObject.Name"/>.</summary>
        public static bool Prefix(SObject __instance, SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
        {
            if (!ArtisanGoodsManager.GetDrawInfo(__instance, out Texture2D spriteSheet, out Rectangle position, out Rectangle iconPosition))
                return true;

            spriteBatch.Draw(Game1.shadowTexture, location + new Vector2(Game1.tileSize / 2, Game1.tileSize * 3 / 4), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White * 0.5f, 0.0f, new Vector2(Game1.shadowTexture.Bounds.Center.X, Game1.shadowTexture.Bounds.Center.Y), 3f, SpriteEffects.None, layerDepth - 0.0001f);

            spriteBatch.Draw(spriteSheet, location + new Vector2((int)(Game1.tileSize / 2 * (double)scaleSize), (int)(Game1.tileSize / 2 * (double)scaleSize)), new Microsoft.Xna.Framework.Rectangle?(position), Color.White * transparency, 0.0f, new Vector2(8f, 8f) * scaleSize, Game1.pixelZoom * scaleSize, SpriteEffects.None, layerDepth);

            //By popular demand, don't show icons near the mouse cursor, which are drawn with lowered transparency.
            if (transparency >= 1 && iconPosition != Rectangle.Empty)
                spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2(Game1.tileSize / 6 * scaleSize, Game1.tileSize / 6 * scaleSize), new Microsoft.Xna.Framework.Rectangle?(iconPosition), Color.White * transparency, 0.0f, new Vector2(4f, 4f), (3f / 2f) * scaleSize, SpriteEffects.None, layerDepth);

            if (drawStackNumber && __instance.maximumStackSize() > 1 && (scaleSize > 0.3 && __instance.Stack != int.MaxValue) && __instance.Stack > 1)
                Utility.drawTinyDigits(__instance.Stack, spriteBatch, location + new Vector2(Game1.tileSize - Utility.getWidthOfTinyDigitString(__instance.Stack, 3f * scaleSize) + 3f * scaleSize, (float)(Game1.tileSize - 18.0 * scaleSize + 2.0)), 3f * scaleSize, 1f, Color.White);
            if (drawStackNumber && __instance.Quality > 0)
            {
                float num = __instance.Quality < 4 ? 0.0f : (float)((Math.Cos(Game1.currentGameTime.TotalGameTime.Milliseconds * Math.PI / 512.0) + 1.0) * 0.0500000007450581);
                spriteBatch.Draw(Game1.mouseCursors, location + new Vector2(12f, Game1.tileSize - 12 + num), new Microsoft.Xna.Framework.Rectangle?(__instance.Quality < 4 ? new Rectangle(338 + (__instance.Quality - 1) * 8, 400, 8, 8) : new Rectangle(346, 392, 8, 8)), Color.White * transparency, 0.0f, new Vector2(4f, 4f), (float)(3.0 * scaleSize * (1.0 + num)), SpriteEffects.None, layerDepth);
            }
            if (__instance.Category == -22 && (double)__instance.scale.Y < 1.0)
                spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)location.X, (int)(location.Y + (Game1.tileSize - 2 * Game1.pixelZoom) * (double)scaleSize), (int)(Game1.tileSize * (double)scaleSize * __instance.scale.Y), (int)((2 * Game1.pixelZoom) * (double)scaleSize)), Utility.getRedToGreenLerpColor(__instance.scale.Y));

            return false;
        }
    }
}