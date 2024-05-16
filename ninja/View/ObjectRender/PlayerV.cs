using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ninja.View.ObjectRender
{
    public class PlayerV
    {
        //public SpriteV Sprite;

        public Animation runAnimation;
        public Animation idleAnimation;
        public Animation jumpAnimation;

        public Animation currentAnimation;

        public Vector2 position;

        public enum FaceDirection
        {
            Left = 1,
            Right = -1,
        }
        public FaceDirection direction = FaceDirection.Right;


        public PlayerV(Animation runAnimation, Animation idleAnimation, Animation jumpAnimation)
        {
            this.runAnimation = runAnimation;
            this.idleAnimation = idleAnimation;
            this.jumpAnimation = jumpAnimation;
            currentAnimation = idleAnimation;
        }

        public void UpdateAnimation()
        {
            runAnimation.position = position;
            idleAnimation.position = position;
            jumpAnimation.position = position;
            currentAnimation.position = position;

            runAnimation.Update();
            idleAnimation.Update();
            jumpAnimation.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects flip = direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            currentAnimation.Drow(spriteBatch, flip);
        }
    }
}
