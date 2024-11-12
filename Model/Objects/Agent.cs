using Model.Geometry;
using Model.Objects.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Objects
{
    public abstract class Agent : WorldObject
    {
        public Vector2<float> FacingAngle { get; set; }

        protected Agent(World world, Vector2<int> position, Vector2<int> size, Vector2<float> facingAngle) : base(world, position, size)
        {
            this.FacingAngle = facingAngle;
        }

        public virtual void OnUpdate()
        {
            FacingAngle.normalise();
        }

        protected void Translate(int x, int y)
        {
            Position.X += x;
            Position.Y += y;

            if (Position.X > World.Width)
            {
                Position.X = Position.X % World.Width;
            }
            if (Position.Y > World.Height)
            {
                Position.Y = Position.Y % World.Height;
            }
            if (Position.X < 0)
            {
                Position.X = World.Width + Position.X;
            }
            if (Position.Y < 0)
            {
                Position.Y = World.Height + Position.Y;
            }
        }
    }
}
