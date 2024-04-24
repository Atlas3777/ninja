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
    internal class Player
    { 
        private readonly List<Sprite> collisionsGroup;
        private readonly float speed = 5f;
        private Animation animationManager;

        public Vector2 position;
        public float SCALE = 20f;

        public float Speed => speed;

        public Player(Animation animationManager)
        {
            this.collisionsGroup = collisionsGroup;
            this.animationManager = animationManager;

        }
        public void Update(GameTime gameTime)
        {
            animationManager.Update();

            var changeX = 0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                changeX += Speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                changeX -= Speed;
            position.X += changeX;

            //foreach (var sprite in collisionsGroup)
            //{
            //    if ( sprite.Rect.Intersects(RectPositions))
            //    {
            //        position.X -= changeX;
            //    }
            //}

            var changeY = 0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                changeY -= Speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                changeY += Speed;

            position.Y += changeY;

            //foreach (var sprite in collisionsGroup)
            //{
            //    if (sprite.Rect.Intersects(RectPositions))
            //    {
            //        position.Y -= changeY;
            //    }
            //}
        }
        //public void Drow(SpriteBatch spriteBatch, Texture2D spriteSheet)
        //{
        //    animationManager.Drow(spriteBatch, spriteSheet);
        //}
    }
}
