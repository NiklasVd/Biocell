using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core.Science
{
    public class Atom : IBindable
    {
        public readonly ElementType type;
        //public short electrons, protons;

        internal Atom(ElementType type)
        {
            this.type = type;
        }

        public Molecule InitiateBind(Atom to)
        {
            return new Molecule().Bind(this, to);
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
            return type.ToString();
        }

        public IBindable Generate()
        {
            return new Atom(type);
        }

        public static Molecule operator *(Atom atom, int multiplicator)
        {
            var molecule = new Molecule();
            molecule.Bind(atom);

            if (multiplicator > 0)
            {
                for (int i = 0; i < multiplicator - 1; i++)
                {
                    molecule.Bind(atom.Generate());
                }
            }

            return molecule;
        }
        public static Molecule operator +(Atom atom, IBindable b)
        {
            return new Molecule().Bind(atom, b);
        }

        public static implicit operator Molecule(Atom atom)
        {
            return new Molecule().Bind(atom);
        }
    }
}
