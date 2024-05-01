using ninja.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ninja.Controller
{
    internal class SceneManager
    {
        private readonly Stack<IScene> sceneStack;

        public SceneManager()
        {
            sceneStack = new();
        }

        public void AddScane(IScene scene)
        {
            scene.Load();
            sceneStack.Push(scene);
        }
        public void RemoveScene()
        {
            sceneStack.Pop();
        }
        public IScene GetCurrentScene()
        {
            return sceneStack.Peek();
        }

    }
}
