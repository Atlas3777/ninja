using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ninja.View.ObjectRender
{
    public class SpriteV
    {
        public SpriteV(Rectangle rectangle, Texture2D image)
        {
            this.Rectangle = rectangle;
            this.Image = image;
        }
        public Rectangle Rectangle;
        public Texture2D Image;
    }
}
