using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using System;
using SObject = StardewValley.Object;

namespace BetterArtisanGoodIcons.Patches.SObjectPatches
{
    class DrawPatch
    {
        /// <summary>Draw the correct texture for machines that are ready for harvest and displaying their results.</summary>
        /// <remarks>We can't just set <see cref="SObject.readyForHarvest"/> to be false during the draw call and draw the 
        /// tool tip ourselves because draw calls getScale, which actually modifies the object scale based upon <see cref="SObject.readyForHarvest"/>.</remarks>
        public static bool Prefix(SObject __instance, SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            if (!__instance.readyForHarvest.Value || !__instance.bigCraftable.Value || __instance.heldObject.Value == null || 
                !ArtisanGoodsManager.GetDrawInfo(__instance.heldObject.Value, out Texture2D spriteSheet, out Rectangle position, out Rectangle iconPosition))
                return true;
            Vector2 vector2 = __instance.getScale() * (float)Game1.pixelZoom;
            Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize)));
            Microsoft.Xna.Framework.Rectangle destinationRectangle = new Microsoft.Xna.Framework.Rectangle((int)((double)local.X - (double)vector2.X / 2.0) + (__instance.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0), (int)((double)local.Y - (double)vector2.Y / 2.0) + (__instance.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0), (int)((double)Game1.tileSize + (double)vector2.X), (int)((double)(Game1.tileSize * 2) + (double)vector2.Y / 2.0));
            spriteBatch.Draw(Game1.bigCraftableSpriteSheet, destinationRectangle, new Microsoft.Xna.Framework.Rectangle?(SObject.getSourceRectForBigCraftable(__instance.showNextIndex.Value ? __instance.ParentSheetIndex + 1 : __instance.ParentSheetIndex)), Color.White * alpha, 0.0f, Vector2.Zero, SpriteEffects.None, (float)((double)Math.Max(0.0f, (float)((y + 1) * Game1.tileSize - Game1.pixelZoom * 6) / 10000f) + (__instance.ParentSheetIndex == 105 ? 0.00350000010803342 : 0.0) + (double)x * 9.99999974737875E-06));
            if (__instance.Name.Equals("Loom") && __instance.MinutesUntilReady > 0)
                spriteBatch.Draw(Game1.objectSpriteSheet, __instance.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2f), 0.0f), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 435, 16, 16)), Color.White * alpha, __instance.scale.X, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0.0f, (float)((double)((y + 1) * Game1.tileSize) / 10000.0 + 9.99999974737875E-05 + (double)x * 9.99999974737875E-06)));
            if (__instance.isLamp.Value && Game1.isDarkOut(Game1.currentLocation))
                spriteBatch.Draw(Game1.mouseCursors, local + new Vector2((float)(-Game1.tileSize / 2f), (float)(-Game1.tileSize / 2f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(88, 1779, 32, 32)), Color.White * 0.75f, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0.0f, (float)((y + 1) * Game1.tileSize - Game1.pixelZoom * 5) / 10000f));
            if (__instance.ParentSheetIndex == 126 && __instance.Quality != 0)
                spriteBatch.Draw(FarmerRenderer.hatsTexture, local + new Vector2(-3f, -6f) * (float)Game1.pixelZoom, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle((__instance.Quality - 1) * 20 % FarmerRenderer.hatsTexture.Width, (__instance.Quality - 1) * 20 / FarmerRenderer.hatsTexture.Width * 20 * 4, 20, 20)), Color.White * alpha, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0.0f, (float)((y + 1) * Game1.tileSize - Game1.pixelZoom * 5) / 10000f) + (float)x * 1E-05f);

            float num = (float)(4.0 * Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2));
            spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize - 8), (float)(y * Game1.tileSize - Game1.tileSize * 3 / 2 - 16) + num)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((double)((y + 1) * Game1.tileSize) / 10000.0 + 9.99999997475243E-07 + (double)__instance.TileLocation.X / 10000.0 + (__instance.ParentSheetIndex == 105 ? 0.00150000001303852 : 0.0)));
            spriteBatch.Draw(spriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2), (float)(y * Game1.tileSize - Game1.tileSize - Game1.tileSize / 8) + num)), new Microsoft.Xna.Framework.Rectangle?(position), Color.White * 0.75f, 0.0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (float)((double)((y + 1) * Game1.tileSize) / 10000.0 + 9.99999974737875E-06 + (double)__instance.TileLocation.X / 10000.0 + (__instance.ParentSheetIndex == 105 ? 0.00150000001303852 : 0.0)));
            return false;
        }

    }
}
