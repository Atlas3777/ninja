using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ninja
{
    public class Animation
    {
        public int SCALE = 1;
        private readonly Texture2D spriteSheet;

        #region prop
        private int frameCount;
        private int columsCount;
        private Vector2 size;

        private int counter;
        private int activeFrame;
        private readonly int interval;

        private int rowPos;
        private int colPos;
        #endregion


        public Vector2 position { get; set; }

        public Animation
            (Texture2D animation, int frameCount, int columsCount, Vector2 size, int interval = 3)
        {
            this.frameCount = frameCount;
            this.columsCount = columsCount;
            this.size = size;

            counter = 0;
            activeFrame = 0;
            this.interval = interval;

            rowPos = 0; colPos = 0;

            this.spriteSheet = animation;
        }

        public virtual void Update()
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

        public virtual void Drow
            (SpriteBatch spriteBatch, SpriteEffects spriteEffect = SpriteEffects.None, int layer = 0)
        {

            spriteBatch.Draw(
                spriteSheet,
                this.RectPositions,
                this.GetFrame(),
                Color.White,
                0,
                Vector2.One,
                spriteEffect,
                layer
                );
        }

        public Rectangle GetFrame()
        {
            return new Rectangle(
                colPos * (int)size.X,
                rowPos * (int)size.Y, 
                (int)size.X,
                (int)size.Y 
                );
        }

        public Rectangle RectPositions
        {
            get
            {
                return new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    (int)size.X * SCALE,
                    (int)size.Y * SCALE);
            }
        }
    }
}
