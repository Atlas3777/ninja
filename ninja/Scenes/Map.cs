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
        Texture2D background;
        Texture2D stolb;

        private List<Rectangle> textureTileList;
        private Texture2D textureAtlas;
        private Dictionary<Vector2, int> tilemap;
        public readonly int scaleTM = 40;
        public Vector2 mapSize;
        private int inversion = -1;
        private List<Rectangle> collisionsRectangle;


        private PenumbraComponent penumbra;

        public Spotlight spotlight;
        public Spotlight spotlight2;
        public Spotlight spotlight3;
        public Spotlight spotlightInHouse;
        public PointLight pointLight;
        public PointLight pointLightInHouse;
        public Light light;

        public Map(PenumbraComponent penumbraComponent)
        {
            penumbra = penumbraComponent;
            collisionsRectangle = new List<Rectangle>();

            tilemap = LoadMap("../../../Data/map.csv");
            textureTileList = new()
            {
                new Rectangle(0,0,40,40),
                new Rectangle(40,0,40,40),
                new Rectangle(0,0,1,1),
                new Rectangle(24,0,8,8),
                new Rectangle(0,16,8,8),
                new Rectangle(32,0,8,8),
                new Rectangle(40,0,8,8)
            };
            InitializeHull();
        }

        public void Initialize(Player player)
        {
            collisionsRectangle = GetCollisionRect();
            this.player = player;
        }

        public void Load(ContentManager contentManager)
        {
            textureAtlas = contentManager.Load<Texture2D>("atlas");
            background = contentManager.Load<Texture2D>("Background/fonPh4");
            stolb = contentManager.Load<Texture2D>("Decor/Stolb");

            spotlight = new Spotlight();
            spotlight.Position = new Vector2(500, 1200 * inversion);
            spotlight.Scale = new Vector2(2000, 1600);
            spotlight.Rotation = (float)Math.PI / 2;
            penumbra.Lights.Add(spotlight);

            spotlight2 = new Spotlight();
            spotlight2.Position = new Vector2(1200, 1200 * inversion);
            spotlight2.Scale = new Vector2(2000, 1600);
            spotlight2.Rotation = (float)Math.PI / 2;
            spotlight2.Enabled = false;
            penumbra.Lights.Add(spotlight2);

            spotlight3 = new Spotlight();
            spotlight3.Position = new Vector2(1900, 1200 * inversion);
            spotlight3.Scale = new Vector2(2000, 1600);
            spotlight3.Rotation = (float)Math.PI / 2;
            spotlight3.Enabled = false;
            penumbra.Lights.Add(spotlight3);


            
            spotlightInHouse = new Spotlight();
            spotlightInHouse.Position = new Vector2(1000 + 6*scaleTM, 160 * inversion);
            spotlightInHouse.Scale = new(1000, 2500);
            spotlightInHouse.Enabled = false;
            spotlightInHouse.Rotation = (float)Math.PI / 2;
            //spotlightInHouse.Radius = 100f;
            spotlightInHouse.ShadowType = ShadowType.Occluded;
            spotlightInHouse.Color = new Color(150, 150, 150);
            //penumbra.Lights.Add(spotlightInHouse);

            pointLight = new PointLight();
            pointLight.Position = spotlightInHouse.Position;
            pointLight.Scale = spotlightInHouse.Scale/2;
            pointLight.ShadowType = ShadowType.Occluded;
            pointLight.Enabled = false;
           
            
            //pointLight.CastsShadows = true;
            //penumbra.Lights.Add(pointLight);


            //penumbra.AmbientColor = Color.Black;
            penumbra.AmbientColor = new Color(10,10,10);
            //penumbra.AmbientColor = new Color(100,100,100);
            //penumbra.AmbientColor = Color.White;


            foreach (var texture in tilemap)
            {
                var x = (int)texture.Key.X * scaleTM;
                var y = (int)texture.Key.Y * scaleTM * inversion;

                if (texture.Value == 20)
                {
                    var newLight = new Spotlight();
                    newLight.Position = new Vector2(x,y);
                    newLight.Scale = new(1000, 2500);
                    newLight.Enabled = true;
                    newLight.Rotation = (float)Math.PI / 2;
                    penumbra.Lights.Add(newLight);
                    continue;
                }
            }
        }


        private void InitializeHull()
        {
            foreach (var texture in tilemap)
            {
                if (texture.Value > 8)
                    continue;
                var x = (int)texture.Key.X * scaleTM;
                var y = (int)texture.Key.Y * scaleTM * inversion;

                var hull = new Hull();
                foreach (var pos in GetPositionСorner(x, y))
                    hull.Points.Add(pos);
                penumbra.Hulls.Add(hull);
            }
        }

        public void Update()
        {
            if (player.position.X > 900)
            {
                spotlightInHouse.Enabled = true;
                pointLight.Enabled = true;
            }
            if (player.position.X > 700)
            {
                spotlight2.Enabled = true;
            }
            if (player.position.X < 900 || player.position.X > 1400)
            {
                spotlightInHouse.Enabled = false;
                pointLight.Enabled = false;
            }
                
            if(player.position.X > 1200)
                spotlight3.Enabled = true;
        }


        
        public void Drow(SpriteBatch spriteBatch, int layer = 0)
        {
            spriteBatch.Draw(
                stolb,
                new Rectangle(200, -360 , 120, 360),
                null,
                Color.White);
            spriteBatch.Draw(
                stolb,
                new Rectangle(1700, -360, 120, 360),
                null,
                Color.White);


            foreach (var texture in tilemap)
            {
                if (texture.Value > 8)
                    continue;
                var x = (int)texture.Key.X * scaleTM;
                var y = (int)texture.Key.Y * scaleTM * inversion;
                
                var rectanglePositonInMap = new Rectangle(
                    x,y,scaleTM,scaleTM);

                var positionTextureInAtlas = textureTileList[texture.Value - 1];
                spriteBatch.Draw(textureAtlas, rectanglePositonInMap, positionTextureInAtlas, Color.White, 0,Vector2.Zero, 0, layer);
            }
        }

        public List<Rectangle> GetCollisionRect()
        {
            foreach (var texture in tilemap)
            {
                if (texture.Value > 8)
                    continue;

                var x = (int)texture.Key.X * scaleTM;
                var y = (int)texture.Key.Y * scaleTM * inversion;

                var rectanglePositonInMap =
                    new Rectangle(x, y, scaleTM, scaleTM);

                collisionsRectangle.Add(rectanglePositonInMap);

            }
            return collisionsRectangle;
        }

        public List<Rectangle> UpdatingCpllisions(Rectangle bounds)
        {
            var answer = new List<Rectangle>();

            var leftX = (int)Math.Floor((float)bounds.Left/ scaleTM);
            var rightTileX = (int)Math.Ceiling((float)bounds.Right / scaleTM);
            var topTileY = (int)Math.Floor((float)bounds.Top / scaleTM);
            var bottomTileY = (int)Math.Ceiling((float)bounds.Bottom / scaleTM);

            foreach(var tile in collisionsRectangle)
            {
                if(tile.Top/scaleTM >= topTileY && tile.Bottom/scaleTM <= bottomTileY)
                    if(tile.Left / scaleTM >= leftX && tile.Right/scaleTM <= rightTileX)
                        answer.Add(tile);
            }

            return answer;
        }

        private List<Vector2> GetPositionСorner(int x, int y)
        {
            var list = new List<Vector2>();
            list.Add(new Vector2(x, y));
            list.Add(new Vector2(x + scaleTM, y));
            list.Add(new Vector2(x + scaleTM, y + scaleTM));
            list.Add(new Vector2(x, y + scaleTM));
            list.Select(x=>x.Y * inversion);
            return list;
        }

        private Dictionary<Vector2, int> LoadMap(string filepath)
        {
            var result = new Dictionary<Vector2, int>();

            var reader = new StreamReader(filepath);

            int y = 0;
            int xSize = 0; 
            string line;
            

            while ((line = reader.ReadLine()) != null)
            {
                var items = line.Split(';');
                xSize = Math.Max(items.Length, xSize);
                
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

            mapSize = new Vector2(xSize, y);
            return result;
        }
    }
}
