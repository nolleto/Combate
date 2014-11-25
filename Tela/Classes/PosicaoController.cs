using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tela.Enums;

namespace Tela.Classes
{
    public class PosicaoController
    {
        public static string[] Colunas = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K" };

        private PecasController _PecasController = new PecasController();

        private List<PosicaoPeca> _Posicoes = new List<PosicaoPeca>();
        private List<Peca> _PecasPosicionamento;

        private List<Posicao> _PosicoesPosicionamento = new List<Posicao>();

        private Peca _PecaPosicionando;        
        private Peca _PecaMovimentando;

        public List<PosicaoPeca> Posicoes { get { return _Posicoes; } }
        public List<Peca> APosicionar { get { return _PecasController.PecasVivas; } }
        public List<Peca> PecasPosicionamento { get { return _PecasPosicionamento; } }

        public Peca PecaPosicionando { get { return _PecaPosicionando; } }
        public bool Posicionando { get { return _PecaPosicionando != null; } }
        public bool FaltaPosicionar { get { return _Posicoes.Count() < 40; } }

        public List<Posicao> PosicoesPosicionamento { get { return _PosicoesPosicionamento; } }

        public Peca PecaMovimentando { get { return _PecaMovimentando; } }
        public bool Movimentando { get { return _PecaMovimentando != null; } }

        public List<Peca> TodasPecas { get { return _PecasController.Pecas; } }

        public PosicaoController()
        {
            _PecasPosicionamento = _PecasController.PecasVivas;
        }

        public class PosicaoPeca
        {
            /*public PosicaoPeca()
            {
                this.Guid = Guid.NewGuid();
            }

            public Guid Guid { get; set; }*/
            public Peca Peca { get; set; }
            public Posicao Posicao { get; set; }
        }

        public void SetPosicoesPosicionamento()
        {
            int quadrados = Principal.Quadrados;
            for (int y = quadrados - 1; y >= quadrados - 4; y--)
            {
                for (int x = quadrados - 1; x >= 0; x--)
                {
                    _PosicoesPosicionamento.Add(new Posicao(x, y));
                }
            }
        }

        public void IniciarPosicionamento(Peca peca)
        {
            _PecaPosicionando = peca;
        }

        public void CancelarPosicionamento()
        {
            _PecaPosicionando = null;
        }

        public Peca TerminarPosicionamento(Posicao posicao)
        {
            var peca = _PecaPosicionando;
            var remove = _PecasPosicionamento.Where(x => x.Type == peca.Type).FirstOrDefault();
            var remove2 = _PosicoesPosicionamento.Where(o => o.X == posicao.X && o.Y == posicao.Y).FirstOrDefault();
            _PecasPosicionamento.Remove(remove);
            _PosicoesPosicionamento.Remove(remove2);
            CancelarPosicionamento();
            return peca;
        }

        public void IniciarMovimento(Peca peca)
        {
            _PecaMovimentando = peca;
        }

        public void CancelarMovimento()
        {
            _PecaMovimentando = null;
        }

        public Peca TerminarMovimento(Posicao posicao)
        {
            var peca = _PecaMovimentando;
            MatarPeca(posicao);
            CancelarMovimento();
            return peca;
        }

        public Peca GetPecaByPosicao(int x, int y)
        {
            var temp = _Posicoes.Where(o => o.Posicao.X == x && o.Posicao.Y == y).FirstOrDefault();
            if (temp != null)
            {
                return temp.Peca;
            }
            return null;
        }

        public void MatarPeca(Posicao posicao)
        {
            var remove = _Posicoes.Where(x => x.Posicao.X == posicao.X && x.Posicao.Y == posicao.Y).FirstOrDefault();
            _Posicoes.Remove(remove);
        }

        public Validacao SetPosicaoPeca(Peca peca, int x, int y)
        {
            if (_Posicoes.Any(p => p.Posicao != null && p.Posicao.X == x && p.Posicao.Y == y))
            {
                return Validacao.ValidacaoErro("A peça não pode ser colocada pois já existe uma outra peça no lugar.");
            }
            else if (!_PosicoesPosicionamento.Any(p => p.X == x && p.Y == y))
            {
                return Validacao.ValidacaoErro("A peça não pode ser colocada pois o lugar é inválido.");
            }
            else
            {
                var posicao = new Posicao(x, y);
                peca.AddPosicao(posicao);

                _Posicoes.Add(new PosicaoPeca()
                {
                    Peca = peca,
                    Posicao = posicao
                });
            }

            return Validacao.ValidacaoSucesso;
        }

        public Validacao SetPosicaoPecaInimigo(Peca peca, int x, int y)
        {
            var posicao = new Posicao(x, y);
            peca.AddPosicao(posicao);

            _Posicoes.Add(new PosicaoPeca()
            {
                Peca = peca,
                Posicao = posicao
            });

            return Validacao.ValidacaoSucesso;
        }

        public Validacao SetPosicaoPecaMovimento(_PanelPosicionamento info, int x, int y, Posicao[] posicoesValidas)
        {
            if (_Posicoes.Any(p => p.Posicao != null && p.Posicao.X == x && p.Posicao.Y == y))
            {
                return Validacao.ValidacaoErro("A peça não pode ser colocada pois já existe uma outra peça no lugar.");
            }
            else if (!posicoesValidas.Any(p => p.X == x && p.Y == y))
            {
                return Validacao.ValidacaoErro("A peça não pode se mover para esse lugar.");
            }
            else
            {
                var posicao = new Posicao(x, y);
                var retorno = TentandoMatarTempo(info.Peca, posicao);
                if (retorno.Sim)
                {
                    return Validacao.ValidacaoErro("Pare de enrolar.");
                }
                else
                {
                    info.Peca.AddPosicao(posicao);

                    _Posicoes.Add(new PosicaoPeca()
                    {
                        Peca = info.Peca,
                        Posicao = posicao
                    });
                }
            }

            return Validacao.ValidacaoSucesso;
        }

        public Validacao SetPosicaoPecaMovimentoInimiga(_PanelPosicionamento info, Posicao posicao)
        {
            info.Peca.AddPosicao(posicao);

            _Posicoes.Add(new PosicaoPeca()
            {
                Peca = info.Peca,
                Posicao = posicao
            });

            return Validacao.ValidacaoSucesso;
        }

        private _MataTempo TentandoMatarTempo(Peca peca, Posicao proximo)
        {
            var response = new _MataTempo();
            var length = peca.Movimentos.Count();
            if (length >= 2)
            {
                var penultimo = peca.Movimentos[length - 2];

                if (penultimo.X == proximo.X && penultimo.Y == proximo.Y)
                {
                    response.Sim = true;
                }
            }
            return response;
        }

        public class _MataTempo
        {
            public bool Sim { get; set; }
        }
    }
}
