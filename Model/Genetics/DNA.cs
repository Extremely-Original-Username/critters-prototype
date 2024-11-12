using Model.Config;
using Model.Genetics.Parts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Model.Genetics
{
    public class DNA
    {
        public string Code { get; protected set; }
        public int MaxChildren { get; set; }

        public DNA()
        {
            MaxChildren = GlobalConfig.maxChildParts;
            Code = getNewEmptyGene('B');
        }
        public DNA(int maxChildren)
        {
            MaxChildren = maxChildren;
            Code = getNewEmptyGene('B');
        }
        public DNA(DNA parentDna)
        {
            MaxChildren = parentDna.MaxChildren;
            Code = parentDna.Code;
        }

        public void Evolve()
        {
            Random rand = new Random();

            //TODO - make app settings
            int x = rand.Next(100);
            if (x < 20 && Code.Length > GlobalConfig.degenerationThrshold)
            {
                DeGenerativeEvolve();
            }
            else if (x < 70)
            {
                ReGenerativeEvolve();
            }
            else
            {
                GenerativeEvolve();
            }
        }

        protected void GenerativeEvolve()
        {
            Random rand = new Random();

            //Todo - make app settings for randomness AND OPTIMISE, random repeat of loop
            int tries = 20;
            while (tries > 0)
            {
                tries--;

                int target = rand.Next(Code.Length);
                if (Code[target] != '0') continue;

                string temp = Code.Substring(0, target);
                int depth = temp.Where(x => x == '(').Count() - temp.Where(x => x == ')').Count();

                float step = 10;
                float limit = 10;
                for (int i = 0; i < depth; i++)
                {
                    limit += step;
                    step *= 0.8f;
                }

                if (rand.Next(100) < limit) continue;

                addGene(target, PartDef.GetRandomGene());
                return;
            }
        }

        protected void ReGenerativeEvolve()
        {
            Random rand = new Random();

            int tries = 20;
            while (tries > 0)
            {
                tries--;
                int target = rand.Next(Code.Length);
                if ((PartDef.EmptyGeneChar + "()").Contains(Code[target])) continue;

                swapGene(target, PartDef.GetRandomGene());
                return;
            }
        }

        protected void DeGenerativeEvolve()
        {
            Random rand = new Random();

            int tries = 20;
            while (tries > 0)
            {
                tries--;
                int target = rand.Next(Code.Length);
                if ((PartDef.EmptyGeneChar + "()").Contains(Code[target])) continue;

                try
                {
                    removeGene(target);
                } catch (Exception ex) { }
                return;
            }
        }

        //Replace empty gene
        protected void addGene(int location, Char gene)
        {
            if (Code[location] != PartDef.EmptyGeneChar) throw new InvalidDataException("Gene must be empty");

            Code = Code.Remove(location, 1).Insert(location, getNewEmptyGene(gene));
        }

        //Remove a gene with no children
        protected void removeGene(int location)
        {
            if (
                (Code.Substring(location, 3 + MaxChildren) != getNewEmptyGene(Code[location]))
                ||
                (Code.Length <= getNewEmptyGene(PartDef.EmptyGeneChar).Length)
                )
                throw new InvalidDataException("Gene must not be empty, not be the only gene and have no children to be removed");

            Code = Code.Remove(location, 3 + MaxChildren);
            Code = Code.Insert(location, PartDef.EmptyGeneChar.ToString());
        }

        //Change an existing gene
        protected void swapGene(int location, Char newGene)
        {
            if (Code[location] == PartDef.EmptyGeneChar || "()".Contains(Code[location]))
                throw new InvalidDataException("Invalid gene to swap");

            Code = Code.Remove(location, 1).Insert(location, newGene.ToString());
        }

        protected string getNewEmptyGene(char defId)
        {
            string result = defId + "(";
            for (int i = 0; i < MaxChildren; i++)
            {
                result += PartDef.EmptyGeneChar;
            }
            result += ")";

            return result;
        }
    }
}
