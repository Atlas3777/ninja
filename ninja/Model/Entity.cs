using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ninja.Model
{
    internal abstract class Entity
    {
        public bool IsAlive = true;
        public Vector2 position;
        public Vector2 velocity = Vector2.Zero;
        public bool onGround;
        public float speed;

        protected Rectangle CalculateBounds(Rectangle rectangle, Vector2 pos, int OFFSETX, int offsetTop)
        {
            return new Rectangle(
                (int)pos.X + OFFSETX,
                (int)pos.Y + offsetTop,
                rectangle.Width - (2 * OFFSETX),
                rectangle.Height - offsetTop);
        }

        public virtual void Update()
        {

        }

        public virtual void Drow()
        {

        }

        public void ChengePosition()
        {
            
        }
    }
}
