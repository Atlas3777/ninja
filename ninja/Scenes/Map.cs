using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ninja.Model;
using Penumbra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ninja.Scenes
{
    internal class Map
    {
        Player player;

        private List<Rectangle> textureTileList;
        private Texture2D textureAtlas;
        private Dictionary<Vector2, int> tilemap;
        private int scaleTM = 119;


        private PenumbraComponent penumbra;
        public Spotlight spotlight;
        public PointLight PointLight;
        public Light light;

        public Map(PenumbraComponent penumbraComponent)
        {
            penumbra = penumbraComponent;
        }

        public void InitializePlayer(Player player)
        {
            this.player = player;
        }

        

        public void Load(ContentManager contentManager)
        {
            textureAtlas = contentManager.Load<Texture2D>("atlas");

            spotlight = new Spotlight();
            spotlight.ShadowType = ShadowType.Occluded;
            spotlight.Position = new Vector2(1000, 200);
            spotlight.Scale = new Vector2(600, 600);
            spotlight.Radius = 0;
            spotlight.Color = Color.Gainsboro;
            spotlight.ConeDecay = 1f;
            spotlight.Position = new Vector2(300, 400);


            light = new PointLight();
            light.Position = new Vector2(300, 1000);
            light.Radius = 100;
            light.Scale = new Vector2(200, 200);
            light.Color = new Color(100, 100, 100);


            PointLight = new PointLight();
            PointLight.Scale = new Vector2(3000, 3000);
            PointLight.Position = new Vector2(1000, 100);
            PointLight.ShadowType = ShadowType.Solid;

            penumbra.Lights.Add(PointLight);
            penumbra.Lights.Add(spotlight);
            penumbra.Lights.Add(light);

            penumbra.AmbientColor = new Color(20,20,20);
        }

        public void InicialiseMap()
        {
            tilemap = LoadMap("../../../Data/map.csv");
            textureTileList = new()
            {
                new Rectangle(0,0,8,8),
                new Rectangle(0,8,8,8),
                new Rectangle(0,16,8,8)
            };
            InitializeHull();
        }

        private void InitializeHull()
        {
            foreach (var texture in tilemap)
            {
                var x = (int)texture.Key.X * scaleTM;
                var y = (int)texture.Key.Y * scaleTM;

                var hull = new Hull();
                foreach (var pos in GetPositionСorner(x, y))
                {
                    hull.Points.Add(pos);
                }
                penumbra.Hulls.Add(hull);
            }
        }

        private float speedRot = 0.01f;
        public void Update()
        {
            //spotlight.Rotation += speedRot;

            //if (spotlight.Rotation >= Math.PI || spotlight.Rotation <= 0)
            //{
            //    spotlight.Rotation = 0;
            //    speedRot = -speedRot;
            //}
            //spotlight.Position = player.position + new Vector2(110, 100);

            //spotlight.Rotation = player.rotation;

            //light.Position = player.position + new Vector2(110, 100);
        }

        public void GetLight()
        {

        }
        public void Drow(SpriteBatch spriteBatch)
        {
            foreach (var texture in tilemap)
            {
                var rectanglePositonInMap = new Rectangle(
                    (int)texture.Key.X * scaleTM,
                    (int)texture.Key.Y * scaleTM,
                    scaleTM,
                    scaleTM
                    );
                var positionTextureInAtlas = textureTileList[texture.Value - 1];
                spriteBatch.Draw(textureAtlas, rectanglePositonInMap, positionTextureInAtlas, Color.White);
            }
        }

        public List<Rectangle> GetCollisionRect()
        {
            var colGroup = new List<Rectangle>();
            foreach (var texture in tilemap)
            {
                if (texture.Value != 0)
                {
                    var x = (int)texture.Key.X * scaleTM;
                    var y = (int)texture.Key.Y * scaleTM;

                    var rectanglePositonInMap = 
                        new Rectangle(x,y,scaleTM,scaleTM);

                    colGroup.Add(rectanglePositonInMap);
                }
            }
            return colGroup;
        }

        private List<Vector2> GetPositionСorner(int x, int y)
        {
            var list = new List<Vector2>();
            list.Add(new Vector2(x, y));
            list.Add(new Vector2(x+scaleTM, y));
            list.Add(new Vector2(x + scaleTM, y + scaleTM));
            list.Add(new Vector2(x, y + scaleTM));
            return list;
        }

        private Dictionary<Vector2, int> LoadMap(string filepath)
        {
            var result = new Dictionary<Vector2, int>();

            var reader = new StreamReader(filepath);

            int y = 0;
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                var items = line.Split(';');

                for (int x = 0; x < items.Length; x++)
                {
                    if (int.TryParse(items[x], out int value))
                    {
                        if (value > 0)
                        {
                            result[new Vector2(x, y)] = value;
                        }
                    }
                }
                y++;
            }
            return result;
        }
    }
}
