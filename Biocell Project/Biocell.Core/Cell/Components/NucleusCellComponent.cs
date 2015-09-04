using Biocell.Core.Science;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core
{
    public class NucleusCellComponent : CellComponent
    {
        // TODO: Implement DNA
        private readonly List<Molecule> fullDna;

        public NucleusCellComponent()
        {
            fullDna = new List<Molecule>();
        }


    }
}
