using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ninja.Controller;
using ninja.Scenes;
using Penumbra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ninja.Model
{
    internal class Player : Entity
    {
        private readonly Animation runAnimation;
        private readonly Animation idleAnimation;
        private readonly Animation jumpAnimation;
        private readonly Animation fallAnimation;

        private Animation currentAmination;

        private readonly List<Rectangle> collisionsGroup;

        public Vector2 position;
        public Vector2 velocity;
        private SpriteEffects flip;

        public float rotation = 0;

        public float speed = 500f;
        private const int SCALE = 3;
        private const int OFFSETX = 50 * SCALE;
        private const int offsetTop = 40 * SCALE;

        //private bool onGround;
        public bool onGround;

        //TODO:GRAVITY на все энтети
        private const float GRAVITY = 1000f;
        private const float JUMP = 400f;
        public float jumpForse = JUMP;

        KeyboardState PrewKeyboardState;
        Map map;
        public Rectangle bounds;

        public Rectangle RectangleForDrow
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
        public Player(Animation runAnim, Animation idleAmin, 
            Animation jumpAnim, Animation fallAnim, Map map)
        {

            position = new Vector2(280,-900);
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
            this.collisionsGroup = collisionsGroup;
            bounds = CalculateBounds(RectangleForDrow, position, OFFSETX, offsetTop);

            

        }
        private void UpdatePosition()
        {
            //if(PlayerRectangle.Y > 0)
            //    начитается баги

            onGround = false;
            var newPos = position + velocity * Globals.Time; // теоритическая новая позиция(можем зайти в стену)

            var newRect = CalculateBounds(RectangleForDrow, newPos, OFFSETX, offsetTop); // у игрока есть Прямоугольник, тут мы расчитываем его положение в новой позиции

            foreach (var collider in map.UpdatingCpllisions(newRect)) //тут пробегаемся по ближайшим каллайдерам
            {
                if (newPos.X != position.X) // если мы не стоим на месте
                {
                    newRect = CalculateBounds(RectangleForDrow, new(newPos.X, position.Y), OFFSETX, offsetTop); // это граница по вертикали
                    if (newRect.Intersects(collider))// если мы перелеклись с одним из ближайших коллайдеров по x
                    {
                        if (newPos.X > position.X) // если двигались вправо
                            newPos.X = collider.Left - RectangleForDrow.Width + OFFSETX; // мы остаемся у левого края колизии  
                        else
                            newPos.X = collider.Right - OFFSETX; // остаемся у правого края
                        continue;
                    }
                }

                newRect = CalculateBounds(RectangleForDrow, new Vector2(position.X, newPos.Y), OFFSETX, offsetTop); // то же самое по Y
                if (newRect.Intersects(collider))// пересечение
                {
                    if (velocity.Y >= 0) // если падаем
                    {
                        newPos.Y = collider.Top - RectangleForDrow.Height; // появляяемся сверху коллайдера
                        onGround = true;
                        velocity.Y = 0;
                    }
                    else // если прыгали и врезались
                    {
                        newPos.Y = collider.Bottom - offsetTop; // появляяемся снизу коллайдера
                        velocity.Y = 0;
                    }
                }
            }
            position = newPos;
        }



        private void UpdateVelocity()
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.A))
            {
                flip = SpriteEffects.FlipHorizontally;
                velocity.X = -speed;
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                flip = SpriteEffects.None;
                velocity.X = speed;
            }
            else
            {
                velocity.X = 0;
            }
            

            if (keyboardState.IsKeyDown(Keys.Space) && onGround)
            {
                jumpForse += 15f;
                jumpForse = MathHelper.Clamp(jumpForse, JUMP, 800);
            }

            if(keyboardState.IsKeyUp(Keys.Space) && PrewKeyboardState.IsKeyDown(Keys.Space) && onGround)
            {
                MakeJump();
                jumpForse = JUMP;
            }

            PrewKeyboardState = keyboardState;

            velocity.Y += GRAVITY * Globals.Time;
        }

        private void UpdateCurrentAnim()
        {
            if (velocity.X == 0 && onGround)
            {
                currentAmination = idleAnimation;
                return;
            }

            if (velocity.X > 0 && onGround)
            {
                currentAmination = runAnimation;
                return;
            }
                
            if (velocity.X < 0 && onGround)
            {
                currentAmination = runAnimation;
                return;
            }

            if (velocity.Y > 0 && !onGround)
            {
                currentAmination = fallAnimation;
                return;
            }

            if (velocity.Y < 0 && !onGround)
            {
                currentAmination = jumpAnimation;
                return;
            }
        }
        
        private void MakeJump()
        {
            velocity.Y = -jumpForse;
        }

        public override void Update()
        {
            UpdateVelocity();
            UpdateCurrentAnim();
            UpdatePosition();
            

            runAnimation.position = position;
            idleAnimation.position = position;
            jumpAnimation.position = position;
            fallAnimation.position = position;

            runAnimation.Update();
            idleAnimation.Update();
            jumpAnimation.Update();
            fallAnimation.Update();

            bounds = CalculateBounds(RectangleForDrow, position, OFFSETX, offsetTop);
        }

        public void Drow(SpriteBatch spriteBatch, int layer = 0)
        {
            currentAmination.Drow(spriteBatch, flip, layer);
        }
    }
}
