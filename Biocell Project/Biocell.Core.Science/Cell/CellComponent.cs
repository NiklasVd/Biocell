using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core
{
    public abstract class CellComponent : Entity
    {
        protected Cell Cell { get; private set; }

        protected CellComponent()
        {
        }

        public void Implement(Cell cell)
        {
            Cell = cell;
        }
        public void Remove()
        {
            Cell = null;
        }

        protected virtual void OnImplement()
        {

        }
        protected virtual void OnRemove()
        {

        }
    }
}
