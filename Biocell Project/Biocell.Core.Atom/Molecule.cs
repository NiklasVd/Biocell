using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core.Atom
{
    public class Molecule : IBinder, IBindable
    {
        private readonly List<IBindable> bounds;

        public Molecule()
        {
            bounds = new List<IBindable>();
        }

        public bool Bind(IBindable to)
        {
            if (IsBindable(to) && to.IsBindable(this) &&
                !bounds.Contains(to))
            {
                bounds.Add(to);
                return true;
            }

            return false;
        }
        public bool Release(IBindable atom)
        {
            if (bounds.Remove(atom))
            {
                atom.LetRelease(this);
                return true;
            }

            return false;
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
            var types = new List<dynamic>();
            bounds.ForEach((b) =>
            {
                var tDyn = types.Find(t => t.type == b.GetType());
                if (tDyn != null)
                {
                    tDyn.count += 1;
                }
                else
                {
                    dynamic newTDyn = new ExpandoObject();
                    newTDyn.type = b.GetType();
                    newTDyn.b = b;
                    newTDyn.count = 1;

                    types.Add(newTDyn);
                }
            });

            var stringBuilder = new StringBuilder();
            types.ForEach(t =>
            {
                var bound = t.b as Atom;
                if (bound != null)
                    stringBuilder.Append("+" + (t.count > 1 ? t.count : "") + bound.shortcutName);
                else
                    stringBuilder.Append(bound.ToString());
            });

            return stringBuilder.ToString();
        }
    }
}
