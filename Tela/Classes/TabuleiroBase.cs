using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tela.Classes
{
    public abstract class TabuleiroBase
    {
        public abstract void PosicionarInimigo(Posicao posicao, Peca peca);
        public abstract void MovimentarInimigo(Posicao posicaoAntiga, Posicao posicaoNova, Peca peca);
        public abstract void MatarPecaInimiga(Posicao posicao);
        public abstract void MatarPecaAmiga(Posicao posicao);
        public abstract void EspiarPeca(Posicao posicaoEspia, Posicao posicaoEspiada);
    }
}
