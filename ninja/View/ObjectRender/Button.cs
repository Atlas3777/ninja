using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ninja.View.ObjectRender
{
    public class Button
    {
        public SpriteV Sprite;

        public Button(SpriteV sprite)
        {
            this.Sprite = sprite;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite.Image, this.Sprite.Rectangle, Color.White);
        }

    }
}
