

namespace ninja.Controller
{
    public static class EntryPoint
    {
        public static NinjaGame game;
        static void Main()
        {
            using(game=new NinjaGame())
                game.Run();
        }
    }
}
