using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tela.Classes
{
    public class PosicaoMovimentosController
    {
        private TabuleiroController _Tabulerio;

        public PosicaoMovimentosController(TabuleiroController tabuleiroController)
        {
            this._Tabulerio = tabuleiroController;
        }

        public _MovimentoInfo[] GetPosicoesMovimento(_PanelPosicionamento info)
        {
            var posicao = info.Posicao;
            var movimentos = new List<_MovimentoInfo>();
            var posicoesInimigo = _Tabulerio.Inimigo.Posicoes.Select(o => o.Posicao).ToArray();
            var posicoesAmigo = _Tabulerio.Amigo.Posicoes.Select(o => o.Posicao).ToArray();
            var obstaculos = _Tabulerio.Obstaculos;

            if (info.Peca.PassoLargo)
            {
                bool cima = true, 
                    baixo = true, 
                    direita = true, 
                    esquerda = true;
                for (int i = 1; i < _Tabulerio.Quadrados; i++)
                {
                    var pBaixo = PosicaoConvert(new Posicao(posicao.X, posicao.Y + i));
                    var pCima = PosicaoConvert(new Posicao(posicao.X, posicao.Y - i));
                    var pDireita = PosicaoConvert(new Posicao(posicao.X + i, posicao.Y));
                    var pEsquerda = PosicaoConvert(new Posicao(posicao.X - i, posicao.Y));

                    if (cima && LugarValido(pCima))
                    {
                        pCima.Amigo = GetPosicoesMovimento_Aux(pCima, posicoesAmigo);
                        pCima.Inimigo = GetPosicoesMovimento_Aux(pCima, posicoesInimigo);
                        pCima.Obstaculo = GetPosicoesMovimento_Aux(pCima, obstaculos);
                        movimentos.Add(pCima);
                        cima = pCima.Move;
                    }
                    if (baixo && LugarValido(pBaixo))
                    {
                        pBaixo.Amigo = GetPosicoesMovimento_Aux(pBaixo, posicoesAmigo);
                        pBaixo.Inimigo = GetPosicoesMovimento_Aux(pBaixo, posicoesInimigo);
                        pBaixo.Obstaculo = GetPosicoesMovimento_Aux(pBaixo, obstaculos);
                        movimentos.Add(pBaixo);
                        baixo = pBaixo.Move;
                    }
                    if (direita && LugarValido(pDireita))
                    {
                        pDireita.Amigo = GetPosicoesMovimento_Aux(pDireita, posicoesAmigo);
                        pDireita.Inimigo = GetPosicoesMovimento_Aux(pDireita, posicoesInimigo);
                        pDireita.Obstaculo = GetPosicoesMovimento_Aux(pDireita, obstaculos);
                        movimentos.Add(pDireita);
                        direita = pDireita.Move;                        
                    }
                    if (esquerda && LugarValido(pEsquerda))
                    {
                        pEsquerda.Amigo = GetPosicoesMovimento_Aux(pEsquerda, posicoesAmigo);
                        pEsquerda.Inimigo = GetPosicoesMovimento_Aux(pEsquerda, posicoesInimigo);
                        pEsquerda.Obstaculo = GetPosicoesMovimento_Aux(pEsquerda, obstaculos);
                        movimentos.Add(pEsquerda);
                        esquerda = pEsquerda.Move;
                    }
                    if (!esquerda && !direita && !cima && !baixo)
                    {
                        break;
                    }
                }
            }
            else
            {
                var temp = new Posicao[] { 
                    new Posicao(posicao.X - 1, posicao.Y),
                    new Posicao(posicao.X + 1, posicao.Y),
                    new Posicao(posicao.X, posicao.Y - 1),
                    new Posicao(posicao.X, posicao.Y + 1)
                };

                foreach (var p in temp)
                {
                    if (LugarValido(p))
                    {
                        var m = PosicaoConvert(p);
                        m.Amigo = GetPosicoesMovimento_Aux(p, posicoesAmigo);
                        m.Inimigo = GetPosicoesMovimento_Aux(p, posicoesInimigo);
                        movimentos.Add(m);
                    }
                }

            }

            return movimentos.ToArray();
        }

        private bool GetPosicoesMovimento_Aux(Posicao posicao, Posicao[] pecas)
        {
            return pecas.Any(o => o.X == posicao.X && o.Y == posicao.Y);
        }

        private _MovimentoInfo PosicaoConvert(Posicao p) {
            return new _MovimentoInfo()
            {
                X = p.X,
                Y = p.Y
            };
        }

        private bool LugarValido(Posicao p)
        {
            return p.X < _Tabulerio.Quadrados && p.X >= 0 && p.Y < _Tabulerio.Quadrados && p.Y >= 0;
        }

        public class _MovimentoInfo : Posicao
        {
            public bool Amigo { get; set; }
            public bool Inimigo { get; set; }
            public bool Obstaculo { get; set; }
            public bool Move { get { return !Amigo && !Inimigo && !Obstaculo; } }
        }
    }
}
