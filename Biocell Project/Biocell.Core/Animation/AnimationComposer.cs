using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core
{
    public static class AnimationComposer
    {
        public static Animation New(string name)
        {
            return new Animation(name);
        }

        // TODO: Add default animations
    }
}
