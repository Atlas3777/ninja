using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ninja.View.Renderes
{
    public abstract class AbstactRenderer
    {
        //public abstract void DrowMap();
        public abstract void DrawPlayer();
        public abstract void DrawGameMap();

        public abstract void DrawUI();
        public abstract void DrawMenu();
    }
}
