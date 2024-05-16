using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ninja.View.ObjectRender
{
    public class TextV
    {
        private SpriteFont font;
        public string Text;
        public Vector2 position;

        public TextV(SpriteFont font, string text, Vector2 position)
        {
            this.font = font;
            Text = text;
            this.position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, Text, position, Color.White);

        }
    }
}
