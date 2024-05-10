using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ninja.Model
{
    internal class Bot : Entity
    {
        public Rectangle rect;
        Player player;
        
        public void r()
        {
            if (player.bounds.Intersects(rect))
            {
                

            }
        }

        public void Update()
        {

        }
    }
}
