using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;

namespace ninja
{
    public interface IScene
    {
        public void Initialize();
        public void Load();
        public void Update(GameTime gameTame);
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
