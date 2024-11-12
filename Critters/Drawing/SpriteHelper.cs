using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Model.Config;
using Model.Geometry;
using System.Collections.Generic;
using System;
using System.Linq;
using Model.Objects;
using Model.Genetics.Parts;
using Model.Genetics.Parts.Base;
using Model.Objects.Environment;

namespace Critters.Drawing
{
    public class SpriteHelper
    {
        GraphicsDevice GraphicsDevice;

        public SpriteHelper(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        public Dictionary<Agent, Texture2D[]> GenerateAgentSprites(List<Agent> agents)
        {
            Dictionary<Agent, Texture2D[]> result = new Dictionary<Agent, Texture2D[]>();

            foreach (var agent in agents)
            {
                if (agent.GetType() == typeof(Critter))
                {
                    result.Add(agent, GenerateCritterSprite((Critter)agent));
                }
                else result.Add(agent, GenerateAgentSprite(agent));
            }

            return result;
        }

        //Add part parameter if associated with part
        private Texture2D GenerateCircleSprite(Vector2<int> size, Color color, Part part = null)
        {
            // Create a new Texture2D with the specified width and height
            var result = new Texture2D(GraphicsDevice, size.X, size.Y);
            if (part != null) result = new PartTexture(GraphicsDevice, size.X, size.Y, part);

            // Initialize a color array for the texture data
            var dataColors = new Color[result.Width * result.Height];

            // Calculate the radius of the circle
            int radius = Math.Min(size.X, size.Y) / 2 - 1; //-1 helps with smaller sizes
            int centerX = size.X / 2;
            int centerY = size.Y / 2;

            for (var y = 0; y < size.Y; y++)
            {
                for (var x = 0; x < size.X; x++)
                {
                    // Calculate the distance from the center of the texture
                    int distX = x - centerX;
                    int distY = y - centerY;
                    double distance = Math.Sqrt(distX * distX + distY * distY);

                    // If the distance is less than or equal to the radius, color the pixel
                    if (distance <= radius)
                    {
                        dataColors[y * size.X + x] = color; // White color with full opacity
                    }
                    else
                    {
                        dataColors[y * size.X + x] = new Color(0, 0, 0, 0); // Transparent
                    }
                }
            }

            // Set the texture data
            result.SetData(dataColors);

            return result;
        }

        public Texture2D[] GenerateAgentSprite(Agent agent)
        {
            return new Texture2D[1] { GenerateCircleSprite(agent.Size, new Color(255, 255, 255, 255)) };
        }

        public Texture2D[] GenerateCritterSprite(Critter agent)
        {
            return GeneratePartSprite(agent.BasePart);
        }

        //Recursive function to generate critter from base part
        private Texture2D[] GeneratePartSprite(Part part)
        {
            List<Texture2D> result;
            Color color = PartColors.ContainsKey(part.Definition.Id)? PartColors[part.Definition.Id] : Color.Magenta;

            result = new List<Texture2D>() { GenerateCircleSprite(part.Size, color, part) };
            foreach (var child in part.Children)
            {
                if (child != null && child.Definition != null)
                {
                    result.AddRange(GeneratePartSprite(child));
                }
            }

            return result.ToArray();
        }
        public class PartTexture : Texture2D
        {
            public Part Part { get; private set; }
            public PartTexture(GraphicsDevice graphicsDevice, int width, int height, Part part) : base(graphicsDevice, width, height)
            {
                Part = part;
            }
        }

        public Texture2D GenerateWorldSprite(World world)
        {
            var result = new Texture2D(GraphicsDevice, world.Width, world.Height);
            var dataColors = new Color[result.Width * result.Height];

            for (int y = 0; y < world.Height; y++)
            {
                for (int x = 0; x < world.Width; x++)
                {
                    float lightLevel = world.LightMap.getLightLevelAt(x, y);
                    float value = 255f * lightLevel;
                    dataColors[y * world.Width + x] = new Color(
                        Convert.ToInt32(255f * lightLevel),
                        Convert.ToInt32(235f * lightLevel),
                        Convert.ToInt32(225f * lightLevel),
                        1000);
                }
            }

            result.SetData(dataColors);
            return result;
        }

        public Texture2D GenerateCarbonOverlay(World world)
        {
            var result = new Texture2D(GraphicsDevice, world.Width, world.Height);
            var dataColors = new Color[result.Width * result.Height];

            for (int y = 0; y < world.Height; y++)
            {
                for (int x = 0; x < world.Width; x++)
                {
                    float carbonLevel = world.CarbonMap.GetCarbonLevelAt(x, y);
                    float value = (carbonLevel / (GlobalConfig.baseCarbonLevel) * 3);
                    dataColors[y * world.Width + x] = new Color(value, 0, 0, 0.4f);
                }
            }

            result.SetData(dataColors);
            return result;
        }

        private static Dictionary<char, Color> PartColors = new Dictionary<char, Color>()
        {
            { '0', Color.Magenta },
            { 'B', Color.White },
            { 'P', Color.Green },
            { 'A', Color.DarkRed },
            { 'M', Color.DarkBlue },
            { 'R', Color.Cyan },
            { 'F', Color.Blue },
        };
    }
}
