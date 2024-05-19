using Microsoft.Xna.Framework;
using ninja.Extensions;
using ninja.Model.BotFields;
using System;
using System.Collections.Generic;

namespace ninja.Model
{
    public class Bot
    {
        public int HP;
        public bool IsAlive
        {
            get { return isAlive; }
        }
        bool isAlive;

        public Route Route;

        public BotStateMachine StateMachine;

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

        enum FaceDirection
        {
            Left = 1,
            Right = -1,
        }

        FaceDirection faceDirection = FaceDirection.Right;

        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        private const float MaxJumpTime = 0.65f;
        private const float JumpLaunchVelocity = -2500.0f;
        private const float GravityAcceleration = 3400.0f;
        private const float MaxFallSpeed = 550.0f;
        private const float JumpControlPower = 0.40f;

        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        private bool isOnGround;

        
        //private float movement;
        public float movement;

        //private bool isJumping;
        public bool isJumping;
        private bool wasJumping;
        private float jumpTime;

        private Rectangle localBounds;

        public int OffsetX = 100;
        public int OffsetY = 60;
        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Position.X + OffsetX;
                int top = (int)Position.Y + OffsetY;

                return new Rectangle(left, top, localBounds.Width - (OffsetX * 2), localBounds.Height - OffsetY);
            }
        }
        private readonly List<Rectangle> collisions;
        #endregion


        public bool IsAttacking;

        public Rectangle FieldOfView
        {
            get 
            {
                return new Rectangle(
                AttackRangeRectangle.X,
                AttackRangeRectangle.Y,
                AttackRangeRectangle.Width,
                AttackRangeRectangle.Height);
            }
        }
        private readonly int attackRange = 50;
        public Rectangle AttackRangeRectangle
        {
            get
            {
                int x;
                int top = BoundingRectangle.Top;
                if (faceDirection == FaceDirection.Right)
                {
                     x = BoundingRectangle.Right;
                     top = BoundingRectangle.Top;

                    return new Rectangle(x, top, attackRange, localBounds.Height - OffsetY);
                }

                x = BoundingRectangle.Left - attackRange;
                return new Rectangle(x, top, attackRange, localBounds.Height - OffsetY);
            }
        }
        public void ResetMovement()
        {
            movement = 0;
            isJumping = false;
        }

        public void Attack(Player player)
        {
            player.TakeDamage(1); 
        }
        public void TakeDamage(int amount)
        {
            HP -= amount;
            if (HP <= 0)
            {
                isAlive = false;
            }
        }


        public Bot(List<Rectangle> collisions, Rectangle localBounds)
        {
            isAlive = true;
            HP = 10;
            this.StateMachine = new BotStateMachine(this);
            StateMachine.CurrentState = StateMachine.BotStates["Patrolling"];
            Route = new Route(this);
            Position = new(1000, 500);  
            this.collisions = collisions;
            this.localBounds = localBounds;
        }

        #region physicsMethod
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
