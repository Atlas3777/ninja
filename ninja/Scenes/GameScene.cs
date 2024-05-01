using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ninja.Controller;
using ninja.Model;
using Penumbra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ninja
{
    internal class GameScene : IScene
    {
        #region prop
        private readonly ContentManager contentManager;
        private SceneManager sceneManager;
        private Texture2D texture;
        private Song song;
        private SoundEffect effect;
        private SoundEffectInstance soundEffectInstance;
        private SpriteFont font;

        private int score = 0;

        private List<Rectangle> textureStore;
        private Texture2D textureAtlas;
        private Dictionary<Vector2, int> tilemap;
        private int scaleTM = 100;

        private Player player;
        private Enemy enemy;
        Texture2D runSpriteSheet;
        Texture2D idleSpriteSheet;

        PenumbraComponent penumbra;
        PointLight PointLight;
        Spotlight spotlight;


        

        #endregion

        public GameScene(ContentManager contentManager, SceneManager sceneManager, PenumbraComponent penumbra)
        {
            this.contentManager = contentManager;
            this.sceneManager = sceneManager;
            this.penumbra = penumbra;
            tilemap = LoadMap("../../../Data/map.csv");
            textureStore = new()
            {
                new Rectangle(0,0,8,8),
                new Rectangle(0,8,8,8)
            };

        }

        public void Load()
        {
            texture = contentManager.Load<Texture2D>("slime_1");
            song = contentManager.Load<Song>("Audio/fonMusic");
            effect = contentManager.Load<SoundEffect>("Audio/slime_jump");
            font = contentManager.Load<SpriteFont>("Fonts/slimeFont");
            textureAtlas = contentManager.Load<Texture2D>("atlas");

            runSpriteSheet = contentManager.Load<Texture2D>("_Run");
            idleSpriteSheet = contentManager.Load<Texture2D>("_Idle");

            var runAnim = new Animation(runSpriteSheet, 10, 10, new Vector2(120, 80));
            var idleAnim = new Animation(idleSpriteSheet, 10, 10, new Vector2(120, 80));

            var runAnimEnemy = new Animation(idleSpriteSheet, 10, 10, new Vector2(120, 80));
            var idleAnimEnemy = new Animation(idleSpriteSheet, 10, 10, new Vector2(120, 80));

            player = new Player(runAnim, idleAnim, GetCollisionRect());
            enemy = new Enemy(runAnimEnemy, idleAnimEnemy, GetCollisionRect());

            enemy.position = new Vector2(400, 400);

            var hull = new Hull();
            var hull2 = new Hull();
            var hull3 = new Hull();
            var hull4 = new Hull();


            hull.Points.Add(new Vector2(0, 200));
            hull.Points.Add(new Vector2(300, 200));
            hull.Points.Add(new Vector2(300, 300));
            hull.Points.Add(new Vector2(000, 300));

            hull2.Points.Add(new Vector2(500, 300));
            hull2.Points.Add(new Vector2(600, 300));
            hull2.Points.Add(new Vector2(600, 400));
            hull2.Points.Add(new Vector2(500, 400));

            hull3.Points.Add(new Vector2(800, 500));
            hull3.Points.Add(new Vector2(900, 500));
            hull3.Points.Add(new Vector2(900, 600));
            hull3.Points.Add(new Vector2(800, 600));

            hull4.Points.Add(new Vector2(0, 700));
            hull4.Points.Add(new Vector2(1300, 700));
            hull4.Points.Add(new Vector2(1300, 800));
            hull4.Points.Add(new Vector2(0, 800));

            penumbra.Hulls.Add(hull);
            penumbra.Hulls.Add(hull2);
            penumbra.Hulls.Add(hull3);
            penumbra.Hulls.Add(hull4);


            spotlight = new Spotlight();
            spotlight.ShadowType = ShadowType.Solid;
            spotlight.Position = new Vector2(1000, 200);
            spotlight.Scale = new Vector2(600, 600);
            spotlight.Radius = 0;
            spotlight.Color = Color.Gainsboro;
            spotlight.ConeDecay = 1f;


            PointLight = new PointLight();
            PointLight.Scale = new Vector2(1500, 1500);
            PointLight.Position = new Vector2(1000, 100);            


            penumbra.Lights.Add(PointLight);
            penumbra.Lights.Add(spotlight);

            //penumbra.AmbientColor = Color.Black;
        }

        public void Update(GameTime gameTime)
        {
            player.Update();
            enemy.Update();


            spotlight.Position = player.position + new Vector2(110, 100);

            spotlight.Rotation = player.rotation;

            //if (spotlight.Rotation >= Math.PI || spotlight.Rotation <= Math.PI/4)
            //{
            //    //spotlight.Rotation = 0;
            //    speedRot = -speedRot;
            //}
                


            if (Keyboard.GetState().IsKeyDown(Keys.N))
                sceneManager.AddScane(new ExitScene(contentManager, penumbra));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            DrowMap(spriteBatch);
            player.Drow(spriteBatch);
            enemy.Drow(spriteBatch);
            spriteBatch.DrawString(font, spotlight.Rotation.ToString(), Vector2.One, Color.White);
        }

        private void DrowMap(SpriteBatch spriteBatch)
        {
            foreach (var texture in tilemap)
            {
                var dest = new Rectangle(
                    (int)texture.Key.X * scaleTM,
                    (int)texture.Key.Y * scaleTM,
                    scaleTM,
                    scaleTM
                    );

                var src = textureStore[texture.Value - 1];
                spriteBatch.Draw(textureAtlas, dest, src, Color.White);
            }
        }

        private Dictionary<Vector2, int> LoadMap(string filepath)
        {
            var result = new Dictionary<Vector2, int>();

            var reader = new StreamReader(filepath);

            int y = 0;
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                var items = line.Split(',');

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

        private List<Rectangle> GetCollisionRect()
        {
            var colGroup = new List<Rectangle>();
            foreach (var texture in tilemap)
            {
                    var dest = new Rectangle(
                    (int)texture.Key.X * scaleTM,
                    (int)texture.Key.Y * scaleTM,
                    scaleTM,
                    scaleTM
                    );
                    colGroup.Add(dest);
            }
            return colGroup;
        }
    }
}
