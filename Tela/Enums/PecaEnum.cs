using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tela.Atributos;

namespace Tela.Enums
{
    public enum PecaEnum
    {
        [Atributos(1, 1)]
        Marshal,
        [Atributos(2, 1)]
        General,
        [Atributos(3, 2)]
        Colonels,
        [Atributos(4, 3)]
        Majors,
        [Atributos(5, 4)]
        Captains,
        [Atributos(6, 4)]
        Lieutenants,
        [Atributos(7, 4)]
        Sergeants,
        [Atributos(8, 5)]
        Miners,
        [Atributos(9, 8, false, true)]
        Scouts,
        [Atributos(10, 1, true)]
        Spy,
        [Atributos(6, true)]
        Bombs,
        [Atributos(1, bandeira: true)]
        Flags
    }

    public static class PecaEnumExtensions
    {
        public static int Quantidade(this PecaEnum p)
        {
            var attr = p.GetAttribute<AtributosAttribute>();
            return attr.Quantidade;
        }

    }
}
