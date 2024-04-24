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
        #region prop
        int frameCount;
        int columsCount;
        Vector2 size;

        int counter;
        int activeFrame;
        int interval;

        int rowPos;
        int colPos;
        #endregion

        Texture2D spriteSheet;

        public Vector2 position;

        public Animation
            (int frameCount, int columsCount, Vector2 size/*, Texture2D spriteSheet*/)
        {
            this.frameCount = frameCount;
            this.columsCount = columsCount;
            this.size = size;

            counter = 0;
            activeFrame = 0;
            interval = 30;

            rowPos = 0; colPos = 0;

            //this.spriteSheet = spriteSheet;
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

        public virtual void Drow(SpriteBatch spriteBatch, Texture2D spriteSheet)
        {
            spriteBatch.Draw(
                spriteSheet,
                this.RectPositions,
                this.GetFrame(),
                Color.White);
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
                    (int)size.X*20,
                    (int)size.Y*20);
            }
        }
    }
}
