using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core.Science
{
    public class Molecule : IBinder<Molecule>, IBindable
    {
        private readonly List<IBindable> bounds;

        public Molecule()
        {
            bounds = new List<IBindable>();
        }

        public Molecule Bind(IBindable to)
        {
            if (IsBindable(to) && to.IsBindable(this) &&
                !bounds.Contains(to))
            {
                bounds.Add(to);
            }

            return this;
        }
        public Molecule Bind(params IBindable[] toChain)
        {
            for (int i = 0; i < toChain.Length; i++)
            {
                Bind(toChain[i]);
            }

            return this;
        }
        public Molecule BindRange(IBindable[] toRange)
        {
            for (int i = 0; i < toRange.Length; i++)
            {
                Bind(toRange[i]);
            }

            return this;
        }

        public Molecule Release(IBindable of)
        {
            if (bounds.Remove(of))
            {
                of.LetRelease(this);
            }

            return this;
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
                    stringBuilder.Append("+" + (t.count > 1 ? t.count : "") + bound.ToString());
                else
                    stringBuilder.Append(bound.ToString());
            });

            return stringBuilder.ToString();
        }

        public IBindable Generate()
        {
            return new Molecule().BindRange(bounds.ConvertAll((b) => b.Generate()).ToArray());
        }

        public static Molecule operator +(Molecule a, IBindable b)
        {
            return a.Bind(b);
        }
        public static Molecule operator -(Molecule a, IBindable b)
        {
            return a.Release(b);
        }
    }
}
