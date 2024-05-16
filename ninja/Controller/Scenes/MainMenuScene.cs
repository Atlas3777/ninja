using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ninja.Model;
using ninja.View.ObjectRender;
using ninja.View.Renderes;

namespace ninja.Controller.Scenes
{
    internal class MainMenuScene : Scene
    {
        public MainMenuScene()
            :base() { }

        public override void Update(GameTime gameTime)
        {
            Button buttonStart = EntryPoint.game.Renderer.buttonStart;
            Button buttonExit = EntryPoint.game.Renderer.buttonExit;

            bool mouseOverStart = buttonStart.Sprite.Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y);
            bool mouseOverExit = buttonExit.Sprite.Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && mouseOverStart)
            {
                SceneManager.ChengeScene("GameScene");
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && mouseOverExit)
            {
                EntryPoint.game.Exit();
            }
        }

        public override void Draw(MonogameRenderer renderer)
        {
            renderer.DrawMenu();
        }
    }
}
