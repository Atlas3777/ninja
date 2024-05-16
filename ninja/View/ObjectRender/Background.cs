using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ninja.View.ObjectRender
{
    public class Background
    {
        private SpriteV sprite;
        public Background(SpriteV sprite)
        {
            this.sprite = sprite;
        }

        public void Drow(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.sprite.Image, this.sprite.Rectangle, Color.White);
        }
    }
}
