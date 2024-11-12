using LibNoise;
using Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Objects.Environment
{
    public class LightMap
    {
        public float[,] Map {  get; set; }
        private float sinOffset;

        public LightMap(int Width, int Height)
        {
            Map = new float[Width, Height];

            var rand = new Random();
            var baseMap = new LibNoise.Primitive.SimplexPerlin()
            {
                Seed = rand.Next(),
                Quality = NoiseQuality.Best
            };
            var reductionMap = new LibNoise.Primitive.SimplexPerlin()
            {
                Seed = rand.Next(),
                Quality = NoiseQuality.Fast
            };
            float scale = GlobalConfig.lightNoiseScale;
            float altScale = GlobalConfig.shadowNoiseScale;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var noiseVal = baseMap.GetValue(x / scale, y / scale, rand.NextSingle() / scale)
                        - reductionMap.GetValue(x / altScale, y / altScale, rand.NextSingle() / altScale) * GlobalConfig.shadowEffectScale;
                    noiseVal = Math.Clamp((noiseVal + 1f) / 2f, 0, 1);

                    Map[x, y] = noiseVal;
                }
            }
        }
        
    
        public float getLightLevelAt(int x, int y)
        {
            if (x < 0 || x > Map.GetLength(0) - 1) DebugHelper.PrintWarning("LightMap.GetLightLevel", "X value is outwith acceptable range of 0-" + (Map.GetLength(0) - 1));
            if (y < 0 || y > Map.GetLength(1) - 1) DebugHelper.PrintWarning("LightMap.GetLightLevel", "Y value is outwith acceptable range of 0-" + (Map.GetLength(1) - 1));

            return Map[Math.Clamp(x, 0, Map.GetLength(0) - 1),
                        Math.Clamp(y, 0, Map.GetLength(1) - 1)] * GetCurrentTimeModifier();
        }

        public float GetCurrentTimeModifier()
        {
            float second = ((float)DateTime.Now.Second + (((float)DateTime.Now.Millisecond) / 1000));
            var result = MathF.Max(0.1f, MathF.Sin(second / (float)Math.PI / 6f));
            return result;
        }
    }
}
