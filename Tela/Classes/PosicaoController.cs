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

        public PosicaoController()
        {
            _PecasPosicionamento = _PecasController.PecasVivas;
        }

        public class PosicaoPeca
        {
            public PosicaoPeca()
            {
                this.Guid = Guid.NewGuid();
            }

            public Guid Guid { get; set; }
            public Peca Peca { get; set; }
            public int PosX { get; set; }
            public int PosY { get; set; }
        }

        public void SetPosicoesPosicionamento(int quadrados)
        {
            for (int x = quadrados - 1; x >= quadrados - 4; x--)
            {
                for (int y = quadrados - 1; y >= 0; y--)
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

        public Peca TerminarPosicionamento()
        {
            var peca = _PecaPosicionando;
            var remove = _PecasPosicionamento.Where(x => x.Type == peca.Type).FirstOrDefault();
            _PecasPosicionamento.Remove(remove);
            CancelarPosicionamento();
            return peca;
        }

        public void IniciarMovimento(Peca peca)
        {
        }

        public void CancelarMovimento()
        {
        }

        public Peca TerminarMovimento()
        {
            var peca = _PecaMovimentando;
            return peca;
        }

        public Validacao SetPeca(Peca peca, int x, int y)
        {
            if (_Posicoes.Any(p => p.PosX == x && p.PosY == y))
            {
                return Validacao.ValidacaoErro("A peça não pode ser colocada pois já existe uma outra peça no lugar.");
            }
            else if (!_PosicoesPosicionamento.Any(p => p.X == x && p.Y == y))
            {
                return Validacao.ValidacaoErro("A peça não pode ser colocada pois o lugar é inválido.");
            }
            else
            {
                _Posicoes.Add(new PosicaoPeca()
                {
                    Peca = peca,
                    PosX = x,
                    PosY = y
                });
            }

            return Validacao.ValidacaoSucesso;
        }

    }
}
