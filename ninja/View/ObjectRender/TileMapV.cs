using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ninja.View.ObjectRender
{
    public class TileMapV
    {
        private Dictionary<Vector2, int> tilemap;
        private List<Rectangle> textureTileList;
        private Texture2D textureAtlas;
        private int scaleTM;


        public TileMapV(Dictionary<Vector2, int> tilemap, List<Rectangle> textureTileList, Texture2D textureAtlas, int scaleTM)
        {
            this.tilemap = tilemap;
            this.textureTileList = textureTileList;
            this.textureAtlas = textureAtlas;
            this.scaleTM = scaleTM;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var texture in tilemap)
            {
                var x = (int)texture.Key.X * scaleTM;
                var y = (int)texture.Key.Y * scaleTM;

                var rectanglePositonInMap = new Rectangle(
                    x, y, scaleTM, scaleTM);

                var positionTextureInAtlas = textureTileList[texture.Value];
                spriteBatch.Draw(textureAtlas, rectanglePositonInMap, positionTextureInAtlas, Color.White, 0, Vector2.Zero, 0, 0);
            }
        }
    }
}
