using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ninja
{
    public interface IScene
    {
        public void Load();
        public void Update(GameTime gameTame);
        public void Draw(SpriteBatch spriteBatch);
    }
}
