using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Biocell.Core
{
    public abstract class Cell : Entity
    {
        // TODO: Implement ID?
        private List<CellComponent> embeddedCellComponents;

        private bool isSplitting;
        public bool IsSplitting { get { return isSplitting; } }

        protected Cell()
        {
            embeddedCellComponents = new List<CellComponent>();
        }

        protected void ImplementCellComponent<T>(T cellComponent) where T : CellComponent
        {
            if (!embeddedCellComponents.Contains(cellComponent))
            {
                embeddedCellComponents.Add(cellComponent);
                cellComponent.Implement(this);
            }
        }
        protected void RemoveCellComponent<T>(T cellComponent) where T : CellComponent
        {
            if (embeddedCellComponents.Remove(cellComponent))
            {
                cellComponent.Remove();
            }
        }

        public void Split()
        {
            isSplitting = true;
        }
        public void PerformApoptosis()
        {

        }

        protected virtual void OnSplit()
        {

        }
        protected virtual void OnPerformApoptosis()
        {

        }
    }
}
