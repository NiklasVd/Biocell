using Biocell.Core.Science;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core
{
    public class EukaryoteCell : Cell
    {
        public readonly NucleusCellComponent nucleus;

        public EukaryoteCell()
        {
            ImplementCellComponent(new NucleusCellComponent(new List<Molecule>()));
        }

        protected override void OnSplit()
        {
            base.OnSplit();
        }

        protected override void OnPerformApoptosis()
        {
            base.OnPerformApoptosis();
        }
    }

    //public enum EukaryoteCellMitosePhase
    //{
    //    None
    //    // TODO: Implement phases
    //}
}
