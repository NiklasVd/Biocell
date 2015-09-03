using Biocell.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Game
{
    public sealed class GameScene
    {
        public readonly string name;
        internal List<Entity> entities;

        public GameScene(string name)
        {
            this.name = name;
            entities = new List<Entity>(500);
        }

        public void AddEntity(Entity entity)
        {
            if (!entities.Contains(entity))
            {
                entities.Add(entity);
            }
        }
        public void RemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }
    }
}
