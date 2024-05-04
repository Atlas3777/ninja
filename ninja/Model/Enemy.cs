using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ninja.Model
{
    internal class Enemy : Entity
    {
        private readonly Animation runAnimation;
        private readonly Animation idleAnimation;
        private readonly List<Rectangle> collisionsGroup;

        public Vector2 position;
        public Vector2 velocity;
        private SpriteEffects flip;

        public float rotation = 0;

        public float speed = 250f;
        private const int SCALE = 1;
        private const int OFFSETX = 50 * SCALE;
        private const int offsetTop = 40 * SCALE;

        private bool onGround;

        //TODO:GRAVITY на все энтети
        private const float GRAVITY = 1000f;
        private const float JUMP = 800f;

        public Rectangle PlayerRectangle
        {
            get
            {
                return new Rectangle(
                runAnimation.RectPositions.X,
                runAnimation.RectPositions.Y,
                runAnimation.RectPositions.Width,
                runAnimation.RectPositions.Height);
            }
        }
        public Enemy(Animation runAnim, Animation idleAmin, List<Rectangle> collisionsGroup)
        {
            runAnimation = runAnim;
            idleAnimation = idleAmin;
            this.collisionsGroup = collisionsGroup;
            runAnim.position = position;
            idleAmin.position = position;
            runAnimation.SCALE = SCALE;
            idleAnimation.SCALE = SCALE;

        }
        private Rectangle CalculateBounds(Vector2 pos)
        {
            return new Rectangle(
                (int)pos.X + OFFSETX,
                (int)pos.Y + offsetTop,
                PlayerRectangle.Width - (2 * OFFSETX),
                PlayerRectangle.Height - offsetTop);
        }

        private void UpdateVelocity()
        {
            //var keyboardState = Keyboard.GetState();

            //if (keyboardState.IsKeyDown(Keys.A))
            //{
            //    rotation = -(float)Math.PI;
            //    flip = SpriteEffects.FlipHorizontally;
            //    velocity.X = -speed;
            //}
            //else if (keyboardState.IsKeyDown(Keys.D))
            //{
            //    rotation = 0;
            //    flip = SpriteEffects.None;
            //    velocity.X = speed;
            //}
            //else velocity.X = 0;

            velocity.Y += GRAVITY * Globals.Time;

            if(position.X > 100)
                velocity.X += speed * Globals.Time;

            if(position.X > 1000)
                velocity.X -= speed * Globals.Time;

            //if (keyboardState.IsKeyDown(Keys.Space) && onGround)
            //{
            //    velocity.Y = -JUMP;
            //}
        }
        private void UpdatePosition()
        {
            onGround = false;
            var newPos = position + velocity * Globals.Time;
            var newRect = CalculateBounds(newPos);

            foreach (var collider in collisionsGroup)
            {
                if (newPos.X != position.X)
                {
                    newRect = CalculateBounds(new(newPos.X, position.Y));
                    if (newRect.Intersects(collider))
                    {
                        if (newPos.X > position.X)
                            newPos.X = collider.Left - PlayerRectangle.Width + OFFSETX;
                        else
                            newPos.X = collider.Right - OFFSETX;
                        continue;
                    }
                }

                newRect = CalculateBounds(new(position.X, newPos.Y));
                if (newRect.Intersects(collider))
                {
                    if (velocity.Y > 0)
                    {
                        newPos.Y = collider.Top - (PlayerRectangle.Height);
                        onGround = true;
                        velocity.Y = 0;
                    }
                    else
                    {
                        newPos.Y = collider.Bottom - offsetTop;
                        velocity.Y = 0;
                    }
                }
            }
            position = newPos;
        }

        public override void Update()
        {
            UpdateVelocity();
            UpdatePosition();
            

            runAnimation.position = position;
            idleAnimation.position = position;

            runAnimation.Update();
            idleAnimation.Update();

        }

        public void Drow(SpriteBatch spriteBatch)
        {
            if (velocity.X == 0)
                idleAnimation.Drow(spriteBatch, flip);
            else
                runAnimation.Drow(spriteBatch, flip);
        }
    }
}
