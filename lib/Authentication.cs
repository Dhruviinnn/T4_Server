using System.Text.Json;
using System.Text;

namespace AuthString
{
    public class Authentication
    {
        public string Encode(object data)
        {
            string text = JsonSerializer.Serialize(data);
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(textBytes);
        }
        public object Decode(string token)
        {
            byte[] textBytes = Convert.FromBase64String(token);
            string stringFormat = Encoding.UTF8.GetString(textBytes);
            return JsonSerializer.Deserialize<object>(stringFormat);
        }
    }
}