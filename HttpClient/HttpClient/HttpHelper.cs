using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TinyJSON;

namespace HttpClient
{
    class HttpHelper
    {
        public static string URL = "http://118.24.219.236:8080/";

        public static void Clear(Node node)
        {
            PostJson(URL + "Clear/", Node.NewTable());
        }

        public static void SendRules(Node node)
        {
            PostJson(URL + "Rules/", node);

            //var data1 = new Printer().Bytes(node);

            //var request = (HttpWebRequest)WebRequest.Create(URL + "Rules/");
            //request.Method = "POST";

            //var requestStream = request.GetRequestStream();
            //var data = new Printer().Bytes(node);
            //requestStream.Write(data, 0, data.Length);
            //var response = request.GetResponse();

            //using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            //{
            //    var content = reader.ReadToEnd();
            //}
        }
        
        public static Node GetAllInfos()
        {
            SetAllowUnsafeHeaderParsing20(true);
            var request = (HttpWebRequest)WebRequest.Create(URL + "Infos/");
            request.Method = "POST";
            var response = request.GetResponse();

            Node node = null;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                var json = reader.ReadToEnd();
                node = new Parser().Load(json);
            }

            return node;
        }


        public static Node GetInfo(string name)
        {
            Node node = Node.NewTable();
            node["Name"] = Node.NewString(name);
            string info = PostJson(URL+"Info/", node, true);

            if (string.IsNullOrEmpty(info))
                return null;
            return new Parser().Load(info);
        }


        public static string PostJson(string url, Node node, bool needResponse = false)
        {
            SetAllowUnsafeHeaderParsing20(true);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";

            var data = new Printer().Bytes(node);
            request.ContentLength = data.Length;

            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();

            if (!needResponse)
                return "";


            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            Stream stream = response.GetResponseStream();

            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string result = reader.ReadToEnd();
            return result;

            //System.Net.HttpWebResponse response;
            //response = (System.Net.HttpWebResponse)request.GetResponse();
            //System.IO.Stream s;
            //s = response.GetResponseStream();
            //string StrDate = "";
            //string strValue = "";
            //StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            //while ((StrDate = Reader.ReadLine()) != null)
            //{
            //    strValue += StrDate + "\r\n";
            //}
            //return strValue;
        }


        public static bool SetAllowUnsafeHeaderParsing20(bool useUnsafe)
        {
            //Get the assembly that contains the internal class
            System.Reflection.Assembly aNetAssembly = System.Reflection.Assembly.GetAssembly(typeof(System.Net.Configuration.SettingsSection));
            if (aNetAssembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                Type aSettingsType = aNetAssembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (aSettingsType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created allready the property will create it for us.
                    object anInstance = aSettingsType.InvokeMember("Section",
                        System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.NonPublic, null, null, new object[] { });

                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the framework is unsafe header parsing should be allowed or not
                        System.Reflection.FieldInfo aUseUnsafeHeaderParsing = aSettingsType.GetField("useUnsafeHeaderParsing", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, useUnsafe);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
