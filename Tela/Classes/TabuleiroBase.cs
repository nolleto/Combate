using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tela.Classes
{
    public abstract class TabuleiroBase
    {
        public abstract void PosicionarInimigo_Received(Posicao posicao, Peca peca);
        public abstract void MovimentarInimigo_Received(Posicao posicaoAntiga, Posicao posicaoNova, Peca peca);
        public abstract void MatarPecaInimiga_Received(Posicao inimigo, Posicao amigo);
        public abstract void MatarPecaAmiga_Received(Posicao amigo, Posicao inimigo);
        public abstract void MatarAmbasPeca_Received(Posicao amigo, Posicao inimigo);
        public abstract void EspiarPeca_Received(Posicao posicaoEspia, Posicao posicaoEspiada);
        public abstract void UpdateStatus_Received();
        public abstract void UpdateStatusSerial_Received();
        public abstract void IniciarPartida_Received(bool minhaRodada);
        public abstract void SuaVez_Received();
        public abstract void DeclararDerrota_Received();
        public abstract void AtualizarTabuleiro_Received();
    }
}
