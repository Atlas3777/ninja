using Microsoft.Xna.Framework;
using ninja.Extensions;
using ninja.Model.BotFields;
using System;
using System.Collections.Generic;

namespace ninja.Model
{
    public class Bot
    {
        public Route Route;

        public BotStateMachine stateMachine;

        #region physicsProp
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;

        private float previousBottom;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -2500.0f;
        private const float GravityAcceleration = 3400.0f;
        private const float MaxFallSpeed = 550.0f;
        private const float JumpControlPower = 0.20f;

        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        bool isOnGround;


        //private float movement;
        public float movement;


        //private bool isJumping;
        public bool isJumping;
        private bool wasJumping;
        private float jumpTime;

        private Rectangle localBounds;

        public int OffsetX = 50 * 0;
        public int OffsetY = 37 * 0;
        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Position.X + OffsetX;
                int top = (int)Position.Y + OffsetY;

                return new Rectangle(left, top, localBounds.Width - (OffsetX * 2), localBounds.Height - OffsetY);
            }
        }
        private List<Rectangle> collisions;
        #endregion

        public Rectangle FieldOfView;


        //private Rectangle UpdateState(Player player)
        //{
        //    var target = player.BoundingRectangle.Center;
        //    return target;
        //}

        


        #region physicsMethod
        public Bot(List<Rectangle> collisions, Rectangle localBounds)
        {
            this.stateMachine = new BotStateMachine(this);
            
            stateMachine.CurrentState = stateMachine.BotStates["Patrolling"];
            Route = new Route(this);
            this.collisions = collisions;
            this.localBounds = localBounds;

        }

        public Rectangle CalculateCollision(Rectangle rectangle)
        {
            return new Rectangle((int)Position.X, (int)Position.Y, rectangle.Width, rectangle.Height);
        }


        public void ApplyPhysics(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = Position;

            velocity.X += movement * 50 * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);

            velocity.Y = DoJump(velocity.Y, gameTime);

            if (IsOnGround)
                velocity.X *= GroundDragFactor;
            else
                velocity.X *= AirDragFactor;

            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            Position += velocity * elapsed;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            HandleCollisions();

            if (Position.X == previousPosition.X)
                velocity.X = 0;

            if (Position.Y == previousPosition.Y)
                velocity.Y = 0;
        }

        private float DoJump(float velocityY, GameTime gameTime)
        {
            if (isJumping)
            {
                if ((!wasJumping && IsOnGround) || jumpTime > 0.0f)
                {
                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    jumpTime = 0.0f;
                }
            }
            else
            {
                jumpTime = 0.0f;
            }
            wasJumping = isJumping;

            return velocityY;
        }



        private void HandleCollisions()
        {
            Rectangle bounds = BoundingRectangle;

            isOnGround = false;

            foreach (var tileBounds in collisions)
            {
                Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);
                if (depth != Vector2.Zero)
                {
                    float absDepthX = Math.Abs(depth.X);
                    float absDepthY = Math.Abs(depth.Y);

                    if (absDepthY < absDepthX)
                    {
                        if (previousBottom <= tileBounds.Top)
                            isOnGround = true;

                        position = new Vector2(position.X, position.Y + depth.Y);
                        bounds = BoundingRectangle;
                    }
                    else
                    {
                        position = new Vector2(position.X + depth.X, position.Y);

                        bounds = BoundingRectangle;
                    }
                }
            }

            previousBottom = bounds.Bottom;
        }
        #endregion
    }
}
