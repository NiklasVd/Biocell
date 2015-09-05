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
        private readonly List<Molecule> dna; // Make this a bit more detailed? Chromosomes and stuff...

        public NucleusCellComponent(List<Molecule> dna)
        {
            this.dna = dna;
        }
    }
}
