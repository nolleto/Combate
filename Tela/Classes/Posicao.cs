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

        public Posicao Inverter()
        {
            var quadros = Principal.Quadrados - 1;
            return new Posicao(
                quadros - X,
                quadros - Y
            );
        }

        public PosicaoTabulerio ToPosicaoTabuleiro()
        {
            return new PosicaoTabulerio()
            {
                Coluna = PosicaoController.Colunas[X],
                Linha = Y
            };
        }

        public bool Compare(Posicao p)
        {
            return Compare(p.X, p.Y);
        }

        public bool Compare(int x, int y)
        {
            return X == x && Y == y;
        }

        public class PosicaoTabulerio
        {
            public string Coluna { get; set; }
            public int Linha { get; set; }

            public string GetInfo()
            {
                return Coluna + (Linha + 1);
            }
        }
    }
}
