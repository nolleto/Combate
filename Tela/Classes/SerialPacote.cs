using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Tela.Classes
{
    public class SerialPacote
    {
        public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static SerialPacote ConvertFromString(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;
            return JsonConvert.DeserializeObject<SerialPacote>(data);
        }
    }
}
