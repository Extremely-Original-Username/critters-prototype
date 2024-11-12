using Model.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Model.Geometry;
using System.Text;
using System.Threading.Tasks;
using Model.Config;
using Model.Objects.Environment;



namespace Model.Genetics.Parts.Base
{
    public class PartDef
    {
        public char Id { get; }
        public string Name { get; }
        public string Description { get; }
        public float MetabolicLoad { get; }
        public Vector2<int> Size { get; }
        public Action<Part, Critter> PresentEffect { get; }
        public Action<Part, Critter> UpdateEffect { get; }

        public PartDef(char id, string name, string description, float metabolicLoad, Action<Part, Critter> presentEffect, Action<Part, Critter> updateEffect)
        {
            Id = id;
            Name = name;
            Description = description;
            MetabolicLoad = metabolicLoad;
            PresentEffect = presentEffect;
            UpdateEffect = updateEffect;
        }

        public static char GetRandomGene()
        {
            var result = PartList.Keys.Where(x => x != EmptyGeneChar).ToList()[new Random().Next(PartList.Count - 1)];
            if (result == '0')
            {
                var test = true;
            }
            return result;
        }

        public const char EmptyGeneChar = '0';
        public static readonly Dictionary<char, PartDef> PartList = new Dictionary<char, PartDef>
        {
            { '0', null },

            { 'B', new PartDef(
                'B',
                "Body",
                "Stores energy",
                0.75f,
                (p, c) => { c.maxEnergy += 1 * p.Size.X * p.Size.Y; c.partEfficiency += 0.1f; },
                (p, c) => { return; }
                )},

            { 'P', new PartDef(
                'P',
                "Photosynthesis",
                "Generates energy in bright areas",
                0.5f,
                (p, c) => { return; },
                (p, c) => {
                    float targetAmount = c.World.LightMap.getLightLevelAt(c.Position.X, c.Position.Y) * p.Size.X * p.Size.Y * c.partEfficiency;
                    float effectiveness = c.World.CarbonMap.GetCarbonLevelAt(c.Position.X, c.Position.Y) / GlobalConfig.baseCarbonLevel;
                    c.energy += c.World.CarbonMap.TryTakeCarbonAmountAt(c.Position.X, c.Position.Y, targetAmount * effectiveness);;
                }
                )},
            { 'A', new PartDef(
                'A',
                "Anaerobic",
                "Generates energy even in dark areas - very sensitive to carbon concentration",
                0.6f,
                (p, c) => { c.partEfficiency -= 0.2f; },
                (p, c) => {
                    float targetAmount = 0.75f * p.Size.X * p.Size.Y;
                    float effectiveness = MathF.Pow(c.World.CarbonMap.GetCarbonLevelAt(c.Position.X, c.Position.Y) / GlobalConfig.baseCarbonLevel, 2f);
                    c.energy += c.World.CarbonMap.TryTakeCarbonAmountAt(c.Position.X, c.Position.Y, targetAmount * effectiveness);;
                }
                )},
            { 'M', new PartDef(
                'M',
                "Move",
                "Moves the critter",
                2f,
                (p, c) => { return; },
                (p, c) =>
                    {
                        if(c.r.Next(100) > 60 * c.partEfficiency){
                            int dx = 0, dy = 0;
                            if (Math.Abs(c.FacingAngle.X) > c.r.NextSingle()) dx = (int)Math.Clamp(c.FacingAngle.X * 1000, -1, 1);
                            if (Math.Abs(c.FacingAngle.Y) > c.r.NextSingle()) dy = (int)Math.Clamp(c.FacingAngle.Y * 1000, -1, 1);
                            c.Move(dx, dy);
                        };
                    }
                )},
            { 'R', new PartDef(
                'R',
                "Rotate",
                "Rotates the critter",
                1.2f,
                (p, c) => { return; },
                (p, c) => { return;  if (c.r.Next(100) > 90) c.FacingAngle = Vector2<float>.fromAngle(Vector2<float>.toAngle(c.FacingAngle) + c.r.Next(3) - 1); }
                )},
            { 'F', new PartDef(
                'F',
                "Flagella",
                "Randomly moves the critter",
                1f,
                (p, c) => { return; },
                (p, c) => { c.Move(c.r.Next(3) - 1, c.r.Next(3) - 1); }
                )},
        };

        //Gets normal vector of part position based total potential children and an offset
        public static Vector2<double> GetRelativePartPosition(int position, int of, float offsetAngle = 0)
        {
            double angle = (2 * Math.PI * position / of) + offsetAngle;

            return new Vector2<double>(Math.Cos(angle), Math.Sin(angle));
        }
    }
}
