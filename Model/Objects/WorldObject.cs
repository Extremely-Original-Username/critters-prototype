using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Config;
using Model.Geometry;
using Model.Objects.Environment;

namespace Model.Objects
{
    public abstract class WorldObject
    {
        public World World { get; }
        public Vector2<int> Position { get; }
        public Vector2<int> Size { get; }

        protected WorldObject(World world, Vector2<int> position, Vector2<int> size)
        {
            World = world;
            Position = position;
            Size = size;

            ValidatePosition();
        }

        private void validateXYByRef(ref int x, ref int y)
        {
            if (x < 0 || x > World.Width - 1)
            {
                DebugHelper.PrintWarning("WorldObject", "X value is outwith acceptable range of 0-" + (World.Width - 1));
                x = Math.Clamp(x, 0, GlobalConfig.arenaWidth * World.Width - 1);
            }
            if (y < 0 || y > World.Height - 1)
            {
                DebugHelper.PrintWarning("WorldObject", "Y value is outwith acceptable range of 0-" + (World.Height - 1));
                y = Math.Clamp(y, 0, GlobalConfig.arenaHeight * World.Height - 1);
            }
        }

        protected void ValidatePosition()
        {
            int tempx = Position.X, tempy = Position.Y;
            validateXYByRef(ref tempx, ref tempy);
            Position.X = tempx; Position.Y = tempy;
        }
    }
}
