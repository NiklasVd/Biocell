using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core.Atom
{
    public abstract class Atom : IBindable
    {
        public readonly string name, shortcutName;

        protected Atom(string name, string shortcutName)
        {
            this.name = name;
            this.shortcutName = shortcutName;
        }

        public Molecule InitiateBind(Atom to)
        {
            var molecule = new Molecule();
            if (molecule.Bind(this) && molecule.Bind(to))
                return molecule;
            else return null;
        }

        public virtual bool IsBindable(IBindable to)
        {
            return true;
        }

        public virtual void LetBind(IBindable to)
        {
        }

        public virtual void LetRelease(IBindable of)
        {
        }

        public override string ToString()
        {
            return shortcutName;
        }
    }
}
