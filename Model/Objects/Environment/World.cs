using Model.Config;
using Model.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibNoise;
using Model.Genetics;

namespace Model.Objects.Environment
{
    public class World
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public LightMap LightMap { get; set; }
        public CarbonMap CarbonMap { get; set; }

        private List<Agent> Agents { get; set; } = new List<Agent>();

        public delegate void OnUpdateDelegate();
        public event OnUpdateDelegate OnUpdate;

        public delegate void OnAgentCreatedDelegate(Agent agent);
        public event OnAgentCreatedDelegate OnAgentCreated;
        public delegate void OnAgentRemovedDelegate(Agent agent);
        public event OnAgentRemovedDelegate OnAgentRemoved;

        public World(int width, int height)
        {
            Width = width;
            Height = height;

            LightMap = new LightMap(width, height);
            CarbonMap = new CarbonMap(width, height);
        }

        public void Update()
        {
            if (OnUpdate != null)
            {
                OnUpdate();
            }

            for (int i = 0; i < Agents.Count; i++)
            {
                Agents[i].OnUpdate();
            }

            CarbonMap.Diffuse(0.02f);
        }

        public Agent[] getAgents() { return Agents.ToArray(); }

        public void addAgent(Agent agent)
        {
            Agents.Add(agent);

            if (OnAgentCreated != null)
            {
                OnAgentCreated(agent);
            }
        }

        public void RemoveAgent(Agent agent)
        {
            Agents.Remove(agent);

            if (OnAgentRemoved != null)
            {
                OnAgentRemoved(agent);
            }
        }
    }
}
