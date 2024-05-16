using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ninja.View.ObjectRender
{
    public class BotV
    {
        public SpriteV Sprite;
        public BotV(SpriteV sprite)
        {
            this.Sprite = sprite;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite.Image, this.Sprite.Rectangle, Color.White);
        }
    }
}
