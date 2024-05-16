using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ninja.Extensions;
using ninja.View.ObjectRender;
using System;
using System.Collections.Generic;
using System.IO;

namespace ninja.View
{
    public static class ORInitializer
    {
        public static PlayerV CreatePlayer(ContentManager content)
        {
            var runSpriteSheet = content.Load<Texture2D>("PlayerAnim/runSheet");
            var idleSpriteSheet = content.Load<Texture2D>("PlayerAnim/idleSheet");
            var jumpSpriteSheet = content.Load<Texture2D>("PlayerAnim/jumpSheet");

            var runAnim = new Animation(runSpriteSheet, 24, 24, new Vector2(160, 100),3);
            var idleAnim = new Animation(idleSpriteSheet, 18, 18, new Vector2(160, 100),6);
            var jumpAnim = new Animation(jumpSpriteSheet, 19, 19, new Vector2(160, 100),5);

            var playerV = new PlayerV(runAnim, idleAnim, jumpAnim);
            return playerV;
        }
        public static BotV CreateBot(ContentManager content)
        {

            //    var runSpriteSheet = content.Load<Texture2D>("PlayerAnim/runSheet");
            //    var idleSpriteSheet = content.Load<Texture2D>("PlayerAnim/idleSheet");
            //    var jumpSpriteSheet = content.Load<Texture2D>("PlayerAnim/jumpSheet");

            //    var runAnim = new Animation(runSpriteSheet, 24, 24, new Vector2(160, 100), 3);
            //    var idleAnim = new Animation(idleSpriteSheet, 18, 18, new Vector2(160, 100), 6);
            //    var jumpAnim = new Animation(jumpSpriteSheet, 19, 19, new Vector2(160, 100), 5);

            //var BotV = new BotV(runAnim, idleAnim, jumpAnim);


            Texture2D botImage = content.Load<Texture2D>("GG");
            Rectangle botRectangle = new Rectangle(0,100,botImage.Width,botImage.Height);
            var botV = new BotV(new SpriteV(botRectangle, botImage));
            return botV;
        }

        public static List<BotV> CreateListBots(ContentManager content)
        {
            var bots = new List<BotV>();

            for (int i = 0; i < 2; i++)
            {
                bots.Add(CreateBot(content));
            }
            return bots;
        }


        public static Background CreateBackground(ContentManager content)
        {
            Texture2D backgroundImage = content.Load<Texture2D>("Background/WotColor");
            Rectangle backgroundRectangle = new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height);
            SpriteV backgroundSprite = new SpriteV(backgroundRectangle, backgroundImage);

            var background = new Background(backgroundSprite);
            return background;
        }

        public static Background CreateBackground1(ContentManager content)
        {
            Texture2D backgroundImage = content.Load<Texture2D>("Background/Wot");
            Rectangle backgroundRectangle = new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height);
            SpriteV backgroundSprite = new SpriteV(backgroundRectangle, backgroundImage);

            var background = new Background(backgroundSprite);
            return background;
        }

        public static Background CreateBackgroundMainMenu(ContentManager content)
        {
            Texture2D backgroundImage = content.Load<Texture2D>("Background/MainMenu");
            Rectangle backgroundRectangle = new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height);
            SpriteV backgroundSprite = new SpriteV(backgroundRectangle, backgroundImage);

            var background = new Background(backgroundSprite);
            return background;
        }
        public static Button CreateStartButton(ContentManager content)
        {
            Texture2D backgroundImage = content.Load<Texture2D>("UI/buttons/button");
            Rectangle backgroundRectangle = new Rectangle(860, 540, backgroundImage.Width, backgroundImage.Height);
            SpriteV backgroundSprite = new SpriteV(backgroundRectangle, backgroundImage);

            var button = new Button(backgroundSprite);
            return button;
        }
        public static Button CreateExitButton(ContentManager content)
        {
            Texture2D backgroundImage = content.Load<Texture2D>("UI/buttons/buttonExit");
            Rectangle backgroundRectangle = new Rectangle(860, 640, backgroundImage.Width, backgroundImage.Height);
            SpriteV backgroundSprite = new SpriteV(backgroundRectangle, backgroundImage);

            var button = new Button(backgroundSprite);
            return button;
        }

        public static TileMapV CreateTileMapV(ContentManager content)
        {
            Texture2D atlasImage = content.Load<Texture2D>("TileMap/Atlas2x64");

            var tilemap = LoaderMap.LoadMap("../../../Data/5_First.csv");
            var textureTileList = new List<Rectangle>();
            for (var j = 0; j < 6; j++)
                for (var i = 0; i < 10; i++)
                {
                    textureTileList.Add(new Rectangle(i * 64, j * 64, 64, 64));
                }

            TileMapV tileMap = new TileMapV(tilemap, textureTileList, atlasImage, 32);
            return tileMap;
        }

        public static TextV CreateText(ContentManager content)
        {
            var font = content.Load<SpriteFont>("Fonts/slimeFont");
            var position = new Vector2(50, 50);


            return new TextV(font, "New", position);
        }
    }
}
