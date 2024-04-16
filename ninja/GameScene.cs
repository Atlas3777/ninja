using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        private readonly ContentManager contentManager;
        private SceneManager sceneManager;
        private Texture2D texture;
        private Song song;
        private SoundEffect effect;
        private SoundEffectInstance soundEffectInstance;
        private SpriteFont font;
        private Texture2D spriteSheet;
        private int score = 0;
        private KeyboardState prevKBState;
        private bool is_space_pressed = false;

        private Dictionary<Vector2, int> tilemap;
        private List<Rectangle> textureStore;
        private Texture2D textureAtlas;
        public GameScene(ContentManager contentManager, SceneManager sceneManager)
        {
            this.contentManager = contentManager;
            this.sceneManager = sceneManager;
            tilemap = LoadMap("../../../Data/map.csv");
            textureStore = new()
            {
                new Rectangle(0,0,8,8),
                new Rectangle(0,8,8,8)
            };

        }

        private Dictionary<Vector2, int> LoadMap(string filepath)
        {
            Dictionary<Vector2, int> result = new();

            StreamReader reader = new(filepath);

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

        public void Draw(SpriteBatch spriteBatch, AnimationManager am)
        {
            foreach (var item in tilemap)
            {
                var dest = new Rectangle(
                    (int)item.Key.X * 64,
                    (int)item.Key.Y * 64,
                    64,
                    64
                    );

                Rectangle src = textureStore[item.Value - 1];
                spriteBatch.Draw(textureAtlas, dest, src, Color.White);
            }


            spriteBatch.DrawString(font, "Score: " + score, new Vector2(0, 0), Color.White);

            spriteBatch.Draw(
                spriteSheet,
                new Rectangle(100, 100, 100, 100),
                am.GetFrame(),
                Color.White);
        }

        public void Load()
        {
            texture = contentManager.Load<Texture2D>("slime_1");
            song = contentManager.Load<Song>("Audio/fonMusic");
            effect = contentManager.Load<SoundEffect>("Audio/slime_jump");

            font = contentManager.Load<SpriteFont>("Fonts/slimeFont");
            spriteSheet = contentManager.Load<Texture2D>("slime_sheet_2");

            soundEffectInstance = effect.CreateInstance();
            soundEffectInstance.IsLooped = true;

            textureAtlas = contentManager.Load<Texture2D>("atlas");
        }

        public void Update(GameTime gameTame)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                MediaPlayer.Pause();
                sceneManager.AddScane(new ExitScene(contentManager));
            }

            KeyboardState currentKBState = Keyboard.GetState();

            if (currentKBState.IsKeyDown(Keys.Space) && !prevKBState.IsKeyDown(Keys.Space))
                MediaPlayer.Play(song);
            if (currentKBState.IsKeyDown(Keys.P) && !prevKBState.IsKeyDown(Keys.P))
                MediaPlayer.Pause();
            if (currentKBState.IsKeyDown(Keys.R) && !prevKBState.IsKeyDown(Keys.R))
                MediaPlayer.Resume();

            if (currentKBState.IsKeyDown(Keys.M) && !prevKBState.IsKeyDown(Keys.M))
                effect.Play();


            prevKBState = currentKBState;           

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !is_space_pressed)
            {
                score++;
                is_space_pressed = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Space))
                is_space_pressed = false;
        }
    }
}
