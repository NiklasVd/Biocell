using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core.Atom
{
    public interface IBinder
    {
        bool Bind(IBindable to);
        bool Release(IBindable of);
    }
}
