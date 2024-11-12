using Model.Config;
using Model.Genetics.Parts.Base;
using Model.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Model.Genetics.Parts
{
    public class Part
    {
        public PartDef Definition { get; }
        public Part? Parent { get; }
        public Part[] Children { get; }
        public Vector2<int> Size = new Vector2<int>(GlobalConfig.baseAgentSize, GlobalConfig.baseAgentSize);

        public Part(PartDef definition, Part? parent)
        {
            this.Definition = definition;
            this.Parent = parent;
            this.Children = new Part[GlobalConfig.maxChildParts];
        }

        public bool IsOrIsParentOf(Part part)
        {
            if (this == part) return true;
            //If this has a child
            else if (!(this.Children.Where(x => x == null).Count() == this.Children.Count()))
            {
                foreach (var child in this.Children.Where(x => x != null))
                {
                    if (child.IsOrIsParentOf(part))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
