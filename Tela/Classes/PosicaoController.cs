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

        private bool _TudoPosicionado;
        private PecasController _PecasController = new PecasController();
        private List<PosicaoPeca> _Posicoes = new List<PosicaoPeca>();

        private List<Peca> _PecasNaoPosicionadas;
        private List<Posicao> _PosicoesValidas = new List<Posicao>();

        private Peca _PecaEmPosicionamento;        
        private Peca _PecaEmMovimento;

        public List<PosicaoPeca> Posicoes { get { return _Posicoes; } }
        public List<Peca> APosicionar { get { return _PecasController.PecasVivas; } }
        public List<Peca> PecasPosicionamento { get { return _PecasNaoPosicionadas; } }

        public Peca PecaEmPosicionamento { get { return _PecaEmPosicionamento; } }
        public Peca PecaEmMovimento { get { return _PecaEmMovimento; } }
        
        public bool PosicionandoPeca { get { return _PecaEmPosicionamento != null; } }
        public bool MovimentandoPeca { get { return _PecaEmMovimento != null; } }

        public bool FaltaPosicionar { get { return !_TudoPosicionado; } }
        public bool PecasPosicionadas { get { return _TudoPosicionado; } }

        public List<Posicao> PosicoesValidasPosicionamento { get { return _PosicoesValidas; } }
        public List<Peca> TodasPecas { get { return _PecasController.Pecas; } }

        public PosicaoController()
        {
            _PecasNaoPosicionadas = _PecasController.PecasVivas;
        }

        public class PosicaoPeca
        {            
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
                    _PosicoesValidas.Add(new Posicao(x, y));
                }
            }
        }

        public void IniciarPosicionamento(Peca peca)
        {
            _PecaEmPosicionamento = peca;
        }

        public void CancelarPosicionamento()
        {
            _PecaEmPosicionamento = null;
        }

        public Peca TerminarPosicionamento(Posicao posicao)
        {
            var peca = _PecaEmPosicionamento;
            var remove = _PecasNaoPosicionadas.Where(x => x.Type == peca.Type).FirstOrDefault();
            var remove2 = _PosicoesValidas.Where(p => p.Compare(posicao)).FirstOrDefault();
            _PecasNaoPosicionadas.Remove(remove);
            _PosicoesValidas.Remove(remove2);
            CancelarPosicionamento();
            _TudoPosicionado = _Posicoes.Count() >= 40;
            return peca;
        }

        public void IniciarMovimento(Peca peca)
        {
            _PecaEmMovimento = peca;
        }

        public void CancelarMovimento()
        {
            _PecaEmMovimento = null;
        }

        public Peca TerminarMovimento(Posicao posicao)
        {
            var peca = _PecaEmMovimento;
            MatarPeca(posicao);
            CancelarMovimento();
            return peca;
        }

        public Peca GetPecaMovimento(Posicao posicao)
        {
            return _Posicoes.Where(o => o.Posicao.Compare(posicao)).FirstOrDefault().Peca;
        }

        public Peca GetPecaByPosicao(Posicao posicao)
        {
            return GetPecaByPosicao(posicao.X, posicao.Y);
        }

        public Peca GetPecaByPosicao(int x, int y)
        {
            var temp = _Posicoes.Where(o => o.Posicao.Compare(x, y)).FirstOrDefault();
            if (temp != null)
            {
                return temp.Peca;
            }
            return null;
        }

        public void MatarPeca(Posicao posicao)
        {
            var remove = _Posicoes.Where(x => x.Posicao.Compare(posicao)).FirstOrDefault();
            _Posicoes.Remove(remove);
        }

        public Validacao SetPosicaoPeca(Peca peca, int x, int y)
        {
            if (_Posicoes.Any(p => p.Posicao != null && p.Posicao.Compare(x, y)))
            {
                return Validacao.ValidacaoErro("A peça não pode ser colocada pois já existe uma outra peça no lugar.");
            }
            else if (!_PosicoesValidas.Any(p => p.Compare(x, y)))
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
            if (_Posicoes.Any(p => p.Posicao != null && p.Posicao.Compare(x ,y)))
            {
                return Validacao.ValidacaoErro("A peça não pode ser colocada pois já existe uma outra peça no lugar.");
            }
            else if (!posicoesValidas.Any(p => p.Compare(x, y)))
            {
                return Validacao.ValidacaoErro("A peça não pode se mover para esse lugar.");
            }
            else
            {
                var posicao = new Posicao(x, y);
                if (TentandoMatarTempo(info.Peca, posicao))
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

        private bool TentandoMatarTempo(Peca peca, Posicao proximo)
        {
            var length = peca.Movimentos.Count();
            if (length > 2)
            {
                var a = peca.Movimentos[length - 1];
                var b = peca.Movimentos[length - 2];
                var c = peca.Movimentos[length - 3];                
                
                if (proximo.Compare(b) && a.Compare(c))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
