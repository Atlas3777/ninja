using Microsoft.Xna.Framework.Input;
using ninja.Model;

namespace ninja.Controller
{
    public static class PlayerController
    {
        public static Player Player;

        private static MouseState prewState;
        public static void GetInput(KeyboardState keyboardState)
        {

            if (keyboardState.IsKeyDown(Keys.A))
            {
                Player.movement = -300.0f;
                EntryPoint.game.Renderer.player.currentAnimation = EntryPoint.game.Renderer.player.runAnimation;
                EntryPoint.game.Renderer.player.direction = View.Enums.FaceDirect.FaceDirection.Left;
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                Player.movement = 300.0f;
                EntryPoint.game.Renderer.player.currentAnimation = EntryPoint.game.Renderer.player.runAnimation;
                EntryPoint.game.Renderer.player.direction = View.Enums.FaceDirect.FaceDirection.Right;
            }
            else
            {
                EntryPoint.game.Renderer.player.currentAnimation = EntryPoint.game.Renderer.player.idleAnimation;
            }


            if (Mouse.GetState().LeftButton == ButtonState.Pressed/* && prewState.LeftButton != ButtonState.Pressed*/)
            {
                Player.IsAttacking = true;
                EntryPoint.game.Renderer.player.currentAnimation = EntryPoint.game.Renderer.player.attackAnimation;
            }

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                Player.isJumping = true;
            }

            if (keyboardState.IsKeyDown(Keys.R))
            {
                PlayerController.Player.Position = new(500,600);
            }
        }
        
    }
}
