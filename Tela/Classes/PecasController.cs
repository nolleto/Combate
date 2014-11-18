using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Tela.Enums;

namespace Tela.Classes
{
    public class PecasController
    {
        private List<Peca> _TodasPecas = new List<Peca>();
        private List<Peca> _PecasMortas = new List<Peca>();
        private List<Peca> _PecasVivas = new List<Peca>();

        public List<Peca> Pecas { get { return _TodasPecas; } }
        public List<Peca> PecasMortas { get { return _PecasMortas; } }
        public List<Peca> PecasVivas { get { return _PecasVivas; } }

        public PecasController()
        {
            var pecas = Enum.GetValues(typeof(PecaEnum))
                .Cast<PecaEnum>()
                .ToArray();

            foreach (PecaEnum peca in pecas)
            {
                var p = new Peca(peca);

                for (int i = 0; i < peca.Quantidade(); i++)
                {
                    _TodasPecas.Add(p);
                }
            }
            _PecasVivas = _TodasPecas;
        }

        public void MatarPeca(Peca peca)
        {
            _PecasVivas.Remove(peca);
            _PecasMortas.Add(peca);
        }        

    }
}
