using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ninja.Scenes;
using Penumbra;
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
        private readonly Animation jumpAnimation;
        private readonly Animation fallAnimation;

        private Animation currentAmination;

        private Spotlight fov;

        public Rectangle fovRectangle;

        public Rectangle bounds;
        public Vector2 position;
        public Vector2 velocity;

        private SpriteEffects flip;

        public float rotation = 0;

        public float speed = 500f;
        private const int SCALE = 3;
        private const int OFFSETX = 50 * SCALE;
        private const int offsetTop = 40 * SCALE;
        private bool onGround;

        //TODO:GRAVITY на все энтети
        private const float GRAVITY = 1000f;
        private const float JUMP = 800f;

        Map map;
        public Rectangle PlayerRectangle
        {
            get
            {
                return new Rectangle(
                idleAnimation.RectPositions.X,
                idleAnimation.RectPositions.Y,
                idleAnimation.RectPositions.Width,
                idleAnimation.RectPositions.Height);
            }
        }
        public Enemy(Animation runAnim, Animation idleAmin,
            Animation jumpAnim, Animation fallAnim, Map map)
        {

            position = new Vector2(500, -100);
            runAnimation = runAnim;
            idleAnimation = idleAmin;
            jumpAnimation = jumpAnim;
            fallAnimation = fallAnim;

            runAnimation.position = position;
            idleAnimation.position = position;
            jumpAnimation.position = position;

            runAnimation.SCALE = SCALE;
            idleAnimation.SCALE = SCALE;
            jumpAnimation.SCALE = SCALE;
            fallAnimation.SCALE = SCALE;

            currentAmination = idleAnimation;
            this.map = map;

            fov = new Spotlight();
            fov.Scale = new(150 * SCALE, 100 * SCALE);

            fovRectangle = new Rectangle((int)bounds.X, (int)bounds.Y, 150 * SCALE, 50 * SCALE);

            bounds = CalculateBounds(position);

        }

        public void Initialize(PenumbraComponent penumbra)
        {
            penumbra.Lights.Add(fov);
        }
        public Rectangle CalculateBounds(Vector2 pos)
        {
            return new Rectangle(
                (int)pos.X + OFFSETX,
                (int)pos.Y + offsetTop,
                PlayerRectangle.Width - (2 * OFFSETX),
                PlayerRectangle.Height - offsetTop);
        }

        private void UpdateVelocity()
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                rotation = -(float)Math.PI;
                flip = SpriteEffects.FlipHorizontally;
                velocity.X = -speed;
                if (onGround)
                    currentAmination = runAnimation;
                else currentAmination = jumpAnimation;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                rotation = 0;
                flip = SpriteEffects.None;
                velocity.X = speed;

                if (onGround)
                    currentAmination = runAnimation;
                else currentAmination = jumpAnimation;
            }
            else
            {
                velocity.X = 0;
                if (onGround)
                    currentAmination = idleAnimation;
                else currentAmination = jumpAnimation;
            }

            if (keyboardState.IsKeyDown(Keys.Up) && onGround)
            {
                velocity.Y = -JUMP;
                currentAmination = jumpAnimation;
                onGround = false;
            }

            if (velocity.Y > 0)
            {
                currentAmination = fallAnimation;
            }
            velocity.Y += GRAVITY * Globals.Time;
        }
        private void UpdatePosition()
        {
            //if(PlayerRectangle.Y > 0)
            //    начитается баги

            onGround = false;
            var newPos = position + velocity * Globals.Time;
            var newRect = CalculateBounds(newPos);

            foreach (var collider in map.UpdatingCpllisions(newRect))
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

                newRect = CalculateBounds(new Vector2(position.X, newPos.Y));
                if (newRect.Intersects(collider))
                {
                    if (velocity.Y >= 0)
                    {
                        newPos.Y = collider.Top - PlayerRectangle.Height;
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
            jumpAnimation.position = position;
            fallAnimation.position = position;

            runAnimation.Update();
            idleAnimation.Update();
            jumpAnimation.Update();
            fallAnimation.Update();

            //velocity.Normalize();

            fov.Rotation = rotation;


            fov.Position = position + new Vector2(50, 50) * SCALE;
            fovRectangle = new Rectangle((int)bounds.X, (int)bounds.Y, 150 * SCALE, 50 * SCALE);

            if (rotation != 0)
            {
                fov.Position = new Vector2(bounds.Center.X,bounds.Center.Y - 30);
                fovRectangle = new Rectangle((int)bounds.X - 80 * SCALE, (int)bounds.Y, 150 * SCALE, 50 * SCALE);
            }

            bounds = CalculateBounds(position);
        }

        public void Drow(SpriteBatch spriteBatch)
        {
            currentAmination.Drow(spriteBatch, flip);

        }
    }
}
