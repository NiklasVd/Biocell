using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core.Science
{
    public interface IBindable
    {
        void LetBind(IBindable to);
        void LetRelease(IBindable of);

        bool IsBindable(IBindable to);
        IBindable Generate();
    }
}
