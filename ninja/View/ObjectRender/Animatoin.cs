using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ninja.View.ObjectRender
{
    public class Animation
    {
        private readonly Texture2D spriteSheet;

        private int frameCount;
        private int columsCount;
        private Vector2 size;

        private int counter = 0;
        private int activeFrame = 0;
        private readonly int interval = 3;

        private int rowPos = 0;
        private int colPos = 0;

        public Vector2 position { get; set; }

        public Animation(Texture2D spriteSheet, int frameCount, int columsCount, Vector2 size, int interval = 3)
        {
            this.frameCount = frameCount;
            this.columsCount = columsCount;
            this.size = size;
            this.spriteSheet = spriteSheet;
            this.interval = interval;
        }

        public void Update()
        {
            counter++;
            if (counter > interval)
            {
                counter = 0;
                NextFrame();
            }
        }

        private void NextFrame()
        {
            activeFrame++;
            colPos++;

            if (activeFrame >= frameCount)
            {
                ResetAnimation();
            }

            if (colPos >= columsCount)
            {
                colPos = 0;
                rowPos++;
            }
        }

        private void ResetAnimation()
        {
            activeFrame = 0;
            colPos = 0;
            rowPos = 0;
        }

        public void Drow(SpriteBatch spriteBatch, SpriteEffects spriteEffect = SpriteEffects.None)
        {
            spriteBatch.Draw(
                spriteSheet,
                this.RectPositions,
                this.GetFrame(),
                Color.White,
                0,
                Vector2.Zero,
                spriteEffect,
                0);
        }

        public Rectangle GetFrame()
        {
            return new Rectangle(colPos * (int)size.X, rowPos * (int)size.Y,(int)size.X, (int)size.Y);
        }

        public Rectangle RectPositions
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            }
        }
    }
}
