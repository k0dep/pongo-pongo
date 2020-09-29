using System.Text;
using Newtonsoft.Json;
public static class Extensions
{
    public static byte[] JsonBytes(this object data)
    {
        return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data, Formatting.None));
    }
}