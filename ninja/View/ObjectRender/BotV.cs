using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static ninja.View.Enums.FaceDirect;

namespace ninja.View.ObjectRender
{
    public class BotV
    {
        //public SpriteV Sprite;

        public Animation runAnimation;
        public Animation idleAnimation;
        public Animation jumpAnimation;
        public Animation deadAnimation;
        public Animation attackAnimation;

        public Animation currentAnimation;

        public Vector2 position;

        public FaceDirection direction = FaceDirection.Right;


        public BotV(Animation runAnimation, Animation idleAnimation, Animation jumpAnimation, Animation deadAnimation, Animation attackAnimation)
        {
            this.runAnimation = runAnimation;
            this.idleAnimation = idleAnimation;
            this.jumpAnimation = jumpAnimation;
            this.deadAnimation = deadAnimation;
            this.attackAnimation = attackAnimation;

            currentAnimation = idleAnimation;
        }

        public void UpdateAnimation()
        {
            runAnimation.Position = position;
            idleAnimation.Position = position;
            jumpAnimation.Position = position;
            deadAnimation.Position = position;
            attackAnimation.Position = position;
            currentAnimation.Position = position;

            runAnimation.Update();
            idleAnimation.Update();
            jumpAnimation.Update();
            deadAnimation.Update();
            attackAnimation.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects flip = direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            currentAnimation.Drow(spriteBatch, flip);
        }
    }
}
