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
    internal class Player : Sprite
    { 
        private readonly List<Sprite> collisionsGroup;
        private readonly float speed = 5f;


        public float Speed => speed;

        public Player(Texture2D texture, Vector2 position, List<Sprite> collisionsGroup) :
            base(texture, position)
        {
            this.collisionsGroup = collisionsGroup;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var changeX = 0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                changeX += Speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                changeX -= Speed;
            position.X += changeX;

            foreach (var sprite in collisionsGroup)
            {
                if (sprite != this && sprite.Rect.Intersects(Rect))
                {
                    position.X -= changeX;
                }
            }

            var changeY = 0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                changeY -= Speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                changeY += Speed;

            position.Y += changeY;

            foreach (var sprite in collisionsGroup)
            {
                if (sprite != this && sprite.Rect.Intersects(Rect))
                {
                    position.Y -= changeY;
                }
            }
        }
    }
}
