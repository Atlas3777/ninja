using Microsoft.Xna.Framework;
using ninja.View.Renderes;

namespace ninja.Controller.Scenes
{
    public abstract class Scene
    {
        public Scene()
        {
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(MonogameRenderer renderer);
        
    }
}
