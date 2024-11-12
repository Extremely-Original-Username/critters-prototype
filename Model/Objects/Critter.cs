using Model.Config;
using Model.Genetics;
using Model.Genetics.Parts;
using Model.Genetics.Parts.Base;
using Model.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Objects.Environment;

namespace Model.Objects
{
    public class Critter : Agent
    {
        public bool Dead { get; private set; } = false;
        public float maxEnergy { get; set; }
        public float energy { get; set; }
        public float metabolicRate { get; set; }
        public float partEfficiency = 1f;

        public float reproductionThreshold = 0.9f;
        public int excessPerPartForGuarunteedReproduction = 200;

        private DNA Dna { get; set; }
        private int PartCount { get; set; }

        public Part BasePart { get; }

        public Random r = new Random();

        public Critter(World world, Vector2<int> position, Vector2<int> size, Vector2<float> facingAngle, DNA dna = null) : base(world, position, size, facingAngle)
        {
            if (dna == null) dna = new DNA();
            Dna = dna;
            BasePart = ParseDNA(dna, out int temp);
            PartCount = temp;

            DoActionForPartAndChildren(BasePart, x =>
            {
                x.Definition.PresentEffect.Invoke(x, this);
                this.metabolicRate += x.Definition.MetabolicLoad * (float)x.Size.X * (float)x.Size.Y;
            });

            energy = maxEnergy / 2 + (r.Next(40) - 20); //Make these parameters too
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            //if (Dead) return;
            if (Dead && new Random().Next(999) > 900) World.RemoveAgent(this);

            //All critter bahviour below
            var excess = metabolise();
            DoActionForPartAndChildren(BasePart, x => { x.Definition.UpdateEffect.Invoke(x, this); }); //MUST be done first

            if (energy > 0 && excess > 0) tryReproduce(excess);
            else if (energy <= 0) die();
        }

        private float metabolise()
        {
            float result = 0;
            if(energy > maxEnergy * reproductionThreshold)
            {
                result = energy - maxEnergy * reproductionThreshold;
                if (energy > maxEnergy) energy = maxEnergy;
            }

            var energyLoss = metabolicRate / 100;
            energy -= energyLoss;
            var temp1 = World.CarbonMap.GetCarbonLevelAt(Position.X, Position.Y);
            World.CarbonMap.TryTakeCarbonAmountAt(Position.X, Position.Y, -(energyLoss));
            var temp2 = World.CarbonMap.GetCarbonLevelAt(Position.X, Position.Y);
            return result;
        }

        private void tryReproduce(float excess)
        {
            //if (World.getAgents().Length > GlobalConfig.maxCritterCount) return;

            if (!(excess / this.PartCount * 0.9 > r.Next(excessPerPartForGuarunteedReproduction))) return;

            energy = energy / 2;

            DNA childDna = new DNA(Dna);
            childDna.Evolve();
            World.addAgent(new Critter(World, new Vector2<int>(this.Position.X + r.Next(20) - 10, this.Position.Y + r.Next(20) - 10), Size, Vector2<float>.fromAngle(r.Next(360)), childDna));
        }

        private void die()
        {
            this.Dead = true;
        }

        public void Move(int x, int y)
        {
            Position.X += x;
            Position.Y += y;

            if (Position.X > World.Width - 2)
            {
                Position.X = 1;
            }
            else if (Position.X < 1)
            {
                Position.X = World.Width - 2;
            }

            if (Position.Y > World.Height - 2)
            {
                Position.Y = 1;
            }
            else if (Position.Y < 1)
            {
                Position.Y = World.Height - 2;
            }

            ValidatePosition();
        }

        private void DoActionForPartAndChildren(Part part, Action<Part> action)
        {
            action.Invoke(part);
            foreach (Part child in part.Children)
            {
                if (child != null)
                {
                    DoActionForPartAndChildren(child, action);
                }
            }
        }

        private Part ParseDNA(DNA dna, out int partCount)
        {
            if (dna.Code == "0")
            {
                bool test = true;
            }
            partCount = 0;
            Part result = generatePartFromDnaSegment(dna.Code, null, out partCount);

            return result;
        }

        private Part generatePartFromDnaSegment(string segment, Part? parent, out int count)
        {
            count = 1;
            if (segment == "0")
            {
                bool test = true;
            }
            if (!PartDef.PartList.ContainsKey(segment[0]) || segment[segment.Length - 1] != ')') 
                throw new InvalidDataException("Invalid DNA segment");

            //Get base part of current segment
            Part result = new Part(PartDef.PartList[segment[0]], parent);

            //Iterate until end of part children
            int currentIndex = segment.IndexOf("(") + 1;
            int currentChild = 0;
            while (currentChild < GlobalConfig.maxChildParts)
            {
                //Get current child part
                Part? current = new Part(PartDef.PartList[segment[currentIndex]], result);
                //If exists in mapping try and add children
                if (current.Definition != null)
                {
                    int endindex = currentIndex;
                    int depth = -1;
                    while (endindex < segment.Length)
                    {
                        char currentCharacter = segment[endindex];

                        if (currentCharacter == ')' && depth == 0) break;
                        else if (currentCharacter == ')') depth -= 1;
                        if (currentCharacter == '(') depth += 1;
                        endindex += 1;
                    }
                    //Recur to add child
                    string subSegment = segment.Substring(currentIndex, endindex - currentIndex + 1);
                    if (subSegment == "0")
                    {
                        bool test = true;
                    }

                    current = generatePartFromDnaSegment(subSegment, result, out int temp);
                    count += temp;

                    result.Children[currentChild] = current;
                    currentIndex = endindex;
                }
                currentIndex += 1;
                currentChild += 1;
            }

            return result;
        }
        
        public bool IsOwnerOfPart(Part part)
        {
            return BasePart.IsOrIsParentOf(part);
        }

        public int GetIndexOfPart(Part part)
        {
            if (!IsOwnerOfPart(part)) throw new InvalidDataException("Passed part must be child of critter");
            if (part.Parent == null) throw new ArgumentNullException("Passed part has no parent, so has no index");

            return part.Parent.Children.ToList().IndexOf(part);
        }
    
        public Vector2<int> getPartPosition(Part part)
        {
            if (!IsOwnerOfPart(part)) throw new InvalidDataException("Passed part must be child of critter");

            Vector2<int> result = new Vector2<int>();
            if (part.Parent == null) return this.Position;

            Vector2<double> relativeVector = PartDef.GetRelativePartPosition(GetIndexOfPart(part), GlobalConfig.maxChildParts, Vector2<float>.toAngle(FacingAngle));
            Vector2<int> parentPos = getPartPosition(part.Parent);//TODO: Use dynamic programming to optimise this
            return new Vector2<int>(parentPos.X + (int)(relativeVector.X * GlobalConfig.partOffset), parentPos.Y + (int)(relativeVector.Y * GlobalConfig.partOffset));
        } 
    }
}
