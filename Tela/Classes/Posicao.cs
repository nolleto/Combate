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

        public class PosicaoTabulerio
        {
            public string Coluna { get; set; }
            public int Linha { get; set; }

            public string ToInfo()
            {
                return Coluna + Linha;
            }
        }
    }
}
