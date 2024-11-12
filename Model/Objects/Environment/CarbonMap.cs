using Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Objects.Environment
{
    public class CarbonMap
    {
        private float[,] Map { get; set; }
        public double totalCarbon { get; private set; } = 0;
        
        public CarbonMap(int width, int height)
        {
            Map = new float[pixelToGridSquare(width), pixelToGridSquare(height)];

            for (int y = 0; y < Map.GetLength(1); y++)
            {
                for (int x = 0; x < Map.GetLength(0); x++)
                {
                    Map[x, y] = GlobalConfig.baseCarbonLevel;
                    totalCarbon += Map[x, y];
                }
            }
        }

        public float GetCarbonLevelAt(int x, int y)
        {
            validateXY(ref x, ref y);

            return Map[pixelToGridSquare(x), pixelToGridSquare(y)];
        }

        public float TryTakeCarbonAmountAt(int x, int y, float targetAmount)
        {
            validateXY(ref x, ref y);

            float startVal = Map[pixelToGridSquare(x), pixelToGridSquare(y)];

            Map[pixelToGridSquare(x), pixelToGridSquare(y)] -= targetAmount;
            if (Map[pixelToGridSquare(x), pixelToGridSquare(y)] < 0) Map[pixelToGridSquare(x), pixelToGridSquare(y)] = 0;

            var result = startVal - Map[pixelToGridSquare(x), pixelToGridSquare(y)];
            totalCarbon -= result;
            return result;
        }

        public void Diffuse(float targetAmount)
        {
            for (int y = 0; y < Map.GetLength(1); y++)
            {
                for (int x = 0; x < Map.GetLength(0); x++)
                {
                    try
                    {
                        var current = Map[x, y];
                        if (y + 1 < Map.GetLength(1) - 1 && Map[x, y + 1] < current)
                        {
                            transferCarbon(x, y, x, y + 1, targetAmount);
                        }
                        if (x - 1 >= 0 && Map[x - 1, y] < current)
                        {
                            transferCarbon(x, y, x - 1, y, targetAmount);
                        }
                        if (x + 1 < Map.GetLength(0) - 1 && Map[x + 1, y] < current)
                        {
                            transferCarbon(x, y, x + 1, y, targetAmount);
                        }
                        if (y - 1 >= 0 && Map[x, y - 1] < current)
                        {
                            transferCarbon(x, y, x, y - 1, targetAmount);
                        }
                    }
                    catch (IndexOutOfRangeException e) {  }
                }
            }
        }

        private void transferCarbon(int x1, int y1, int x2, int y2, float maxAmount)
        {
            var transfer = Math.Min(maxAmount, (Map[x1, y1] - Map[x2, y2]) / 2);

            Map[x1, y1] -= transfer;
            Map[x2, y2] += transfer;
        }

        private int pixelToGridSquare(int pixel)
        {
            return pixel / GlobalConfig.carbonGridSquareSize;
        }

        private void validateXY(ref int x, ref int y)
        {
            if (pixelToGridSquare(x) < 0 || pixelToGridSquare(x) > Map.GetLength(0) - 1)
            {
                DebugHelper.PrintWarning("CarbonMap", "X value is outwith acceptable range of 0-" + (Map.GetLength(0) - 1));
                x = Math.Clamp(x, 0, GlobalConfig.carbonGridSquareSize * Map.GetLength(0) - 1);
            }
            if (pixelToGridSquare(y) < 0 || pixelToGridSquare(y) > Map.GetLength(1) - 1)
            {
                DebugHelper.PrintWarning("CarbonMap", "Y value is outwith acceptable range of 0-" + (Map.GetLength(1) - 1));
                y = Math.Clamp(y, 0, GlobalConfig.carbonGridSquareSize * Map.GetLength(1) - 1);
            }
        }
    }
}
