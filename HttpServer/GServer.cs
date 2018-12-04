using System;
using System.IO;
using System.Net;
using HTTPServerLib;
using TinyJSON;

namespace HttpServer
{
    class GServer : HTTPServerLib.HttpServer
    {
        public const string Root = "../assets";

        public GServer(int port) : base(IPAddress.Any, port, Root)
        {
            Logger = new ConsoleLogger();
        }


        public override void OnGet(HttpRequest request, HttpResponse response)
        {
            string requestURL = request.URL.Replace("/", @"\").Replace("\\..", "").TrimStart('\\');
            Logger.Log("OnGet --> " + requestURL);

            //string requestFile = Path.Combine(ServerRoot, requestURL);
            string extension = Path.GetExtension(requestURL);

            if (!string.IsNullOrEmpty(extension))//获取图片css....
            {
                response = response.FromFile(requestURL);
            }
            else
            {
                switch (requestURL)
                {
                    case "":
                        response.SetContent(Utils.GetLoginHtml());
                        break;
                    case "info":

                        if (request.Params != null && request.Params.TryGetValue("type", out string type) &&
                            request.Params.TryGetValue("id", out string id))
                            try
                            {
                                response.SetContent(Utils.ShowInfo(type, id));
                            }
                            catch (Exception e)
                            {
                                Logger.Log(e);
                                response.NotFound();
                                response.Send();
                                return;
                            }
                        else
                            response.NotFound();
                        break;
                    default:
                        return;
                }
                response.Content_Type = "text/html; charset=UTF-8";
                response.StatusCode = "200";
            }
            //发送HTTP响应
            response.Send();
        }

        public override void OnPost(HttpRequest request, HttpResponse response)
        {
            Logger.Log("OnPost  -->" + request.URL);

            switch (request.URL)
            {
                case "/Clear/":
                    Node clear = request.ToJson();
                    Utils.Clear(clear);
                    break;
                case "/Rules/":
                    Node rule = request.ToJson();
                    Utils.AddRule(rule);
                    break;
                case "/Infos/":
                    string json = Utils.GetInfos();
                    response.FromJSON(json);
                    response.Send();
                    break;
                case "/Info/":
                    Node role = request.ToJson();
                    string info = Utils.GetInfo(role);
                    response.FromJSON(info);
                    response.Send();
                    break;
                case "/result":
                    
                    break;
            }






            ////设置返回信息
            //string content = string.Format("这是通过Post方式返回的数据:{0}", request.Params);

            ////构造响应报文
            //response.SetContent(content);
            //response.Content_Encoding = "utf-8";
            //response.StatusCode = "200";
            //response.Content_Type = "text/html; charset=UTF-8";
            //response.Headers["Server"] = "ExampleServer";

            ////发送响应
            //response.Send();
        }
    }

    class ConsoleLogger : ILogger
    {
        void ILogger.Log(object message)
        {
            Console.WriteLine(message);
        }
    }
}
