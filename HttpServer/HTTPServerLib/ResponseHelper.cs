using System;
using HttpServer;
using System.IO;
using System.Text;

namespace HTTPServerLib
{
    public static class ResponseHelper
    {
        public static HttpResponse FromFile(this HttpResponse response, string fileName)
        {
            string path = Path.Combine(GServer.Root, fileName).Replace("\\", "/");
            if (!File.Exists(path))
            {
                Console.WriteLine(path + "  is not found.");
                return response.NotFound();
            }

            var content = File.ReadAllBytes(path);
            var contentType = FileContentType.GetMimeTypeFromPath(fileName);
            response.SetContent(content);
            response.Content_Type = contentType;
            response.StatusCode = "200";
            return response;
        }

        public static HttpResponse FromXML(this HttpResponse response, string xmlText)
        {
            response.SetContent(xmlText);
            response.Content_Type = "text/xml";
            response.StatusCode = "200";
            return response;
        }

        public static HttpResponse FromXML<T>(this HttpResponse response, T entity) where T : class
        {
            return response.FromXML("");
        }

        public static HttpResponse FromJSON(this HttpResponse response, string jsonText)
        {
            response.SetContent(jsonText);
            response.Content_Type = "text/json";
            response.StatusCode = "200";
            return response;
        }

        public static HttpResponse FromJSON<T>(this HttpResponse response, T entity) where T : class
        {
            return response.FromJSON("");
        }

        public static HttpResponse FromText(this HttpResponse response, string text)
        {
            response.SetContent(text);
            response.Content_Type = "text/plain";
            response.StatusCode = "200";
            return response;
        }
        
        public static HttpResponse NotFound(this HttpResponse response)
        {
            string path = Path.Combine(GServer.Root, "404.html");
            if (File.Exists(path))
            {
                string html = File.ReadAllText(path);
                response.SetContent(html);
            }
            else
                response.SetContent("<html><body><h1>404 - Not Found</h1></body></html>");
            response.StatusCode = "404";
            response.Content_Type = "text/html";
            return response;
        }

        /*
        private static string GetMimeFromFile(string filePath)
        {
            IntPtr mimeout;
            if (!File.Exists(filePath))
                throw new FileNotFoundException(string.Format("File {0} can't be found at server.", filePath));

            int MaxContent = (int)new FileInfo(filePath).Length;
            if (MaxContent > 4096) MaxContent = 4096;
            byte[] buf = new byte[MaxContent];

            using (FileStream fs = File.OpenRead(filePath))
            {
                fs.Read(buf, 0, MaxContent);
                fs.Close();
            }

            int result = FindMimeFromData(IntPtr.Zero, filePath, buf, MaxContent, null, 0, out mimeout, 0);
            if (result != 0)
                throw Marshal.GetExceptionForHR(result);

            string mime = Marshal.PtrToStringUni(mimeout);
            Marshal.FreeCoTaskMem(mimeout);

            return mime;
        }

        [DllImport("urlmon.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
        static extern int FindMimeFromData(IntPtr pBC,
              [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
              [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeParamIndex = 3)] 
              byte[] pBuffer,
              int cbSize,
              [MarshalAs(UnmanagedType.LPWStr)]  
              string pwzMimeProposed,
              int dwMimeFlags,
              out IntPtr ppwzMimeOut,
              int dwReserved);
         */
    }
}
