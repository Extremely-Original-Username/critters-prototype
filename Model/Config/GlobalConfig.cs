using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Config
{
    public static class GlobalConfig
    {
        //User Setting
        public const int overlayUpdateFrames = 20;

        //Arena settings
        public const int arenaWidth = 1000;      //Size of arena in pixels
        public const int arenaHeight = 700;

        public const int lightNoiseScale = 300;         //Scales of the perlin noise
        public const int shadowNoiseScale = 400;
        public const float shadowEffectScale = 1.6f;    //Multiplier on shadow perlin map subtraction

        public const int carbonGridSquareSize = 20;
        public const float baseCarbonLevel = 100f;

        //Agent settings
        public const int baseAgentSize = 10;    //Default size of agents in pixels

        //Critter settings
        public const int baseCritterCount = 100;  //Starting number of agents
        //public const int maxCritterCount = 1000;
        public const int baseEvolution = 10;  //Starting number of gene changes
        public const int degenerationThrshold = 18;
        public const int maxChildParts = 6; //Number of parts per part, including base
        public const int partOffset = 8; //Pixel offset of parts
    }
}
