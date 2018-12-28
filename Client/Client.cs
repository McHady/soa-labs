using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace Client
{
    public class Client
    {
        private string host;

        private string port;

        private WebRequest request;

        private string prefix;

        public Client(string host, string port)
        {
            this.host = host;
            this.port = port;
            this.prefix = $"http://{host}:{port}";
        }


        public delegate void LogHandler(object sender, LogEvent e);

        public event LogHandler ResponseLogger;

        public event LogHandler RequestLogger;

        public bool Ping()
        {
            request = (HttpWebRequest)WebRequest.Create(prefix +"/Ping");

            request.Method = "GET";

            RequestLogger?.Invoke(this, new LogEvent("/Ping"));



            using (var responseMessage = (HttpWebResponse) request.GetResponse())
            {

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    using (var stream = responseMessage.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            ResponseLogger?.Invoke(this, new LogEvent(reader.ReadToEnd()));
                        }
                    }

                    return true;

                }
            }

            return false;
        }


        public string GetInputData()
        {
            request = (HttpWebRequest)WebRequest.Create(prefix + "/GetInputData");

            request.Method = "GET";

            RequestLogger?.Invoke(this, new LogEvent("/GetInputData"));

            var responseMessage = (HttpWebResponse)request.GetResponse();

            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                using (var stream = responseMessage.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        ResponseLogger?.Invoke(this, new LogEvent("There's some data"));
                        return reader.ReadToEnd();
                    }
                }

               

            }
            
            ResponseLogger?.Invoke(this, new LogEvent("No data"));

            return "";
        }

        public void WriteAnswer(string serialize)
        {
            request = (HttpWebRequest)WebRequest.Create(prefix + "/WriteAnswer");

            request.Method = "POST";

            RequestLogger?.Invoke(this, new LogEvent("/WriteAnswer"));

            var data = System.Text.Encoding.UTF8.GetBytes(serialize);

            request.ContentType = "application/x-www--form-urlencoded";

            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var responseMessage = (HttpWebResponse)request.GetResponse();

            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                using (var stream = responseMessage.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        ResponseLogger?.Invoke(this, new LogEvent(reader.ReadToEnd()));
                    }
                }

            }
        }
    }
}
