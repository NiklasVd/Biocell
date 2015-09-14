using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core.Science
{
    public interface IBinder<T>
    {
        T Bind(IBindable to);
        T Bind(params IBindable[] toChain);
        T BindRange(IBindable[] toRange);

        T Release(IBindable of);
    }
}
