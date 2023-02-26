using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Classes.Common
{
    static class Serializer
    {
        static string Serialize<Type>(Type obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        static Type Deserialize<Type>(string str)
        {
            return JsonSerializer.Deserialize<Type>(str ?? "");
        }
    }
}
