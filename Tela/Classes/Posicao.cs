using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tela.Classes
{
    public class Posicao
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Posicao() { }
        public Posicao(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Posicao ToEnemy()
        {
            var metade = Principal.Quadrados / 2;
            return new Posicao(
                X > metade ? X - metade : X + metade,
                Y > metade ? Y - metade : Y + metade
            );
        }
    }
}
