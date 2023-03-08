using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
public static class SpriteBatchExtensions
{
    public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
    {
        Texture2D pixelTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        pixelTexture.SetData(new Color[] { Color.White });

        spriteBatch.Draw(pixelTexture, rectangle, color);
    }
}
