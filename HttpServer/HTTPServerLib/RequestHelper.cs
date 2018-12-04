using TinyJSON;

namespace HTTPServerLib
{
    public static class RequestHelper
    {
        public static Node ToJson(this HttpRequest request)
        {
            if (request.Headers != null && request.Headers.TryGetValue("Content-Length", out string len))
            {
                if (int.TryParse(len, out int length))
                {
                    byte[] bytes = new byte[length];
                    var stream = request.GetRequestStream();
                    stream.Read(bytes, 0, length);
                    return new Parser().Load(bytes);
                }
            }
            return Node.NewNull();
        }

    }
}
