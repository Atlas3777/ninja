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
        float speed = 5f;
        public Player(Texture2D texture, Vector2 position) : base(texture, position) { }
            
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                position.X += speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                position.X -= speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                position.Y -= speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                position.Y += speed;

        }
    }
}
