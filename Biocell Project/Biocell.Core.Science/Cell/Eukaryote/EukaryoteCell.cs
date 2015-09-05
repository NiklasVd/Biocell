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

        private EukaryoteCellMitosePhase mitosePhase;
        public EukaryoteCellMitosePhase MitosePhase
        {
            get { return mitosePhase; }
        }

        public EukaryoteCell()
        {
            ImplementCellComponent(new NucleusCellComponent(new List<Molecule>()));
            mitosePhase = EukaryoteCellMitosePhase.None;
        }

        public override void Split()
        {
            base.Split();
        }
    }

    public enum EukaryoteCellMitosePhase
    {
        None
        // TODO: Implement phases
    }
}
