using ninja.Controller.Scenes;
using System.Collections.Generic;

namespace ninja.Controller
{
    public static class SceneManager
    {
        public static Scene CurrentScene;
        public static Dictionary<string,Scene> Scenes = new();
        private static MainMenuScene mainMenuScene;
        private static GameScene gameScene;


        public static void Initialize()
        {
            mainMenuScene = new MainMenuScene();
            gameScene = new GameScene();

            Scenes.Add("MainMenuScene", mainMenuScene);
            Scenes.Add("GameScene", gameScene);
        }

        public static void ChengeScene(string sceneName)
        {
            CurrentScene = Scenes[sceneName];
        }
    }
}
