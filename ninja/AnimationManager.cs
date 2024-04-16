using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ninja
{
    public class AnimationManager
    {
        int frameCount;
        int columsCount;
        Vector2 size;

        int counter;
        int activeFrame;
        int interval;

        int rowPos;
        int colPos;


        public AnimationManager(int frameCount, int columsCount, Vector2 size)
        {
            this.frameCount = frameCount;
            this.columsCount = columsCount;
            this.size = size;

            counter = 0;
            activeFrame = 0;
            interval = 30;

            rowPos = 0; colPos = 0;
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
        public Rectangle GetFrame()
        {
            return new Rectangle(
                colPos * (int)size.X,
                rowPos * (int)size.Y, 
                (int)size.X,
                (int)size.Y
                );
        }
    }
}
