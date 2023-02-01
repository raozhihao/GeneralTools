using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.General.Interfaces;

using Newtonsoft.Json;

namespace TaskLibs
{
    public class NewsoftJsonConvert : IJsonConvert
    {
        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value); 
        }

        public object DeserializeObject(string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type);
        }

        public string SerializeObject(object serverResponse)
        {
           return JsonConvert.SerializeObject(serverResponse);
        }
    }
}
