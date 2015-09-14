using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core.Science
{
    public static class PeriodicTable
    {
        private static Dictionary<ElementType, Atom> atoms;
        private static Dictionary<MoleculeType, Molecule> molecules;

        static PeriodicTable()
        {
            atoms = new Dictionary<ElementType, Atom>();
            AddAllElements();

            molecules = new Dictionary<MoleculeType, Molecule>();
            AddAllMolecules();
        }

        public static Atom Get(ElementType type)
        {
            return (Atom)atoms[type].Generate();
        }
        public static Molecule Craft(MoleculeType type)
        {
            return (Molecule)molecules[type].Generate();
        }

        private static void AddAllElements()
        {
            var elementTypes = (ElementType[])Enum.GetValues(typeof(ElementType));
            for (int i = 0; i < elementTypes.Length; i++)
            {
                atoms.Add(elementTypes[i], new Atom(elementTypes[i]));
            }
        }
        private static void AddAllMolecules()
        {
            var moleculeTypes = (MoleculeType[])Enum.GetValues(typeof(MoleculeType));
            for (int i = 0; i < moleculeTypes.Length; i++)
            {
                var molecule = new Molecule();
                switch (moleculeTypes[i])
                {
                    case MoleculeType.CoOH:
                        molecule.Bind(Get(ElementType.Co), Get(ElementType.O), Get(ElementType.H));
                        break;

                    case MoleculeType.NH2:
                        molecule.Bind(Get(ElementType.N), Get(ElementType.H), Get(ElementType.H)); // Find a better for the double H add?
                        break;
                }

                molecules.Add(moleculeTypes[i], molecule);
            }
        }
    }

    public enum ElementType
    {
        H,
        He,
        Li,
        Be,
        B,
        C,
        N,
        O,
        F,
        Ne,
        Na,
        Mg,
        Al,
        Si,
        P,
        S,
        Cl,
        Ar,
        K,
        Ca,
        Sc,
        Ti,
        V,
        Cr,
        Mn,
        Fe,
        Co,
        Ni,
        Cu,
        Zn,
        Ga,
        Ge,
        As,
        Se,
        Br,
        Kr,
        Rb,
        Sr,
        Y,
        Zr,
        Nb,
        Mo,
        Tc,
        Ru,
        Rh,
        Pd,
        Ag,
        Cd,
        In,
        Sn,
        Sb,
        Te,
        I,
        Xe,
        Cs,
        Ba,
        Lu,
        Hf,
        Ta,
        W,
        Re,
        Os,
        Ir,
        Pt,
        Au,
        Hg,
        Tl,
        Pb,
        Bi,
        Po,
        At,
        Rn,
        Fr,
        Ra,

        La,
        Ce,
        Pr,
        Nd,
        Pm,
        Sm,
        Eu,
        Gd,
        Tb,
        Dy,
        Ho,
        Er,
        Tm,
        Yb,
        Ac,
        Th,
        Pa,
        U,
        Np,
        Pu,
        Am,
        Cm,
        Bk,
        Cf,
        Es
    }
    public enum MoleculeType
    {
        CoOH,
        NH2
    }
}
