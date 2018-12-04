using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TinyJSON;

namespace HttpServer
{
    class Utils
    {
        public static void Clear(Node node)
        {
            Console.WriteLine("开始清除所有");

            string path = Path.Combine(GServer.Root, "_config");
            string[] files = Directory.GetFiles(path, "*.json");
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

        public static void AddRule(Node rule)
        {
            string name = rule["Name"].ToString();
            Node rules = rule["Rule"];
            Console.WriteLine($"添加规则-->{name}");

            string path = Path.Combine(GServer.Root, $"_config/{name}.json");
            if(File.Exists(path))
                File.Delete(path);

            string json = new Printer(true).String(rules);
            File.WriteAllText(path, json);
        }

        public static string GetInfos()
        {
            Console.WriteLine($"获取所有信息");
            Node node = Node.NewArray();
            string path = Path.Combine(GServer.Root, "_infos");
            string[] files = Directory.GetFiles(path, "*.json");
            foreach (var file in files)
            {
                node.Add(Node.NewString(Path.GetFileNameWithoutExtension(file)));
            }
            return new Printer().String(node);
        }

        public static string GetInfo(Node role)
        {
            Console.WriteLine($"获取 {role["Name"].ToString()} 信息");
            if (role.IsTable() && role["Name"].IsString())
            {
                string name = role["Name"].ToString();
                string path = Path.Combine(GServer.Root, $"_infos/{name}.json");
                if (File.Exists(path))
                    return File.ReadAllText(path);
            }

            return "";
        }

        public static string GetLoginHtml()
        {
            string path = Path.Combine(GServer.Root, "_config");
            string[] files = Directory.GetFiles(path, "*.json");
            StringBuilder builder = new StringBuilder();
            foreach (var file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file);

                builder.AppendLine("<div class=\"w3_main_grid\">");
                builder.AppendLine("<div class=\"w3_main_grid_right\">");
                builder.AppendLine($"<input type = \"submit\" value=\"{name}\" onclick=\"login('{name}')\">");
                builder.AppendLine("</div>");
                builder.AppendLine("</div>");
            }

            string htmlPath = Path.Combine(GServer.Root, "index.html");
            string html = File.ReadAllText(htmlPath);
            return html.Replace("%BUTTONLIST%", builder.ToString());
        }
        
        public static string ShowInfo(string type, string id)
        {
            string path = Path.Combine(GServer.Root, $"_config/{type}.json");
            if (!File.Exists(path))
                return "<html><body><h1>参数错误</h1></body></html>";

            string def = File.ReadAllText(path);
            Node defNode = new Parser().Load(def);

            Dictionary<string, Node> infos;
            string infoPath = Path.Combine(GServer.Root, $"_infos/{id}.json");
            if (File.Exists(infoPath))
            {
                string defInfo = File.ReadAllText(infoPath);
                Node inf = new Parser().Load(defInfo);
                infos = (Dictionary<string, Node>) inf;
            }
            else
            {
                infos = new Dictionary<string, Node>();
            }

            string htmlPath = Path.Combine(GServer.Root, "info.html");
            string html = File.ReadAllText(htmlPath);


            StringBuilder builder = new StringBuilder();
            Dictionary<string, Node> types = (Dictionary<string, Node>) defNode;
            foreach (var node in types)
            {
                string holder = node.Value["Des"].ToString();
                string val = "";
                if (infos.TryGetValue(node.Key, out Node v))
                    val = v.ToString();

                builder.AppendLine("<div class=\"w3_agileits_main_grid w3l_main_grid\">");
                builder.AppendLine("<span class=\"agileits_grid\">");
                if ((bool) node.Value["Required"])
                {
                    builder.AppendLine($"<label>{node.Value["Des"]}<span class=\"star\">*</span></label>");
                    builder.AppendLine($"<input type = \"text\" id=\"{node.Key}\" placeholder=\"{holder}\" value=\"{val}\" required=\"\">");
                }
                else
                {
                    builder.AppendLine($"<label>{node.Value["Des"]}</label>");
                    builder.AppendLine($"<input type = \"text\" id=\"{node.Key}\" placeholder=\"{holder}\" value=\"{val}\">");
                }

                builder.AppendLine("</span>");
                builder.AppendLine("</div>");
            }

            return html.Replace("%CONTENT%", builder.ToString());
        }
    }
}
