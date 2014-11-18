using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tela.Classes
{
    public class Validacao
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }

        public static Validacao ValidacaoSucesso
        {
            get
            {
                return new Validacao()
                {
                    Sucesso = true
                };
            }
        }

        public static Validacao ValidacaoErro(string erro)
        {
            return new Validacao()
            {
                Mensagem = erro
            };
        }
    }    
}
