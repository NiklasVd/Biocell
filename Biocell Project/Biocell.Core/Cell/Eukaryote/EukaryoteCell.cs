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
            ImplementCellComponent(new NucleusCellComponent());
        }
    }
}
