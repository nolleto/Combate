using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Tela.Classes
{
    public class SerialPacote
    {
        public static string INICIO = "BEGIN";
        public static string FIM = "END";

        public Enums.PecaEnum PecaEnum { get; set; }
        public Posicao Posicao { get; set; }
        public Posicao PosicaoAux { get; set; }
        public Enums.DueloEnum DueloEnum { get; set; }
        public Enums.SerialPacoteEnum Info { get; set; }

        public bool Inimgo { get; set; }

        public string ToJsonString()
        {
            return string.Concat( 
                SerialPacote.INICIO,
                JsonConvert.SerializeObject(this),
                SerialPacote.FIM
            );
        }

        public static SerialPacote ConvertFromString(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<SerialPacote>(data);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }    
}
