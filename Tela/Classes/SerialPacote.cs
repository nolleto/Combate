using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Tela.Classes
{
    public class SerialPacote
    {
        public Peca Peca { get; set; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
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
