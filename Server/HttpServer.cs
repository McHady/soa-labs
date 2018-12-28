using System;
using System.Net;

namespace Server
{
    public class HttpServer : IDisposable
    {
        private readonly HttpListener listener;

        public HttpServer(string host, string port)
        {
            this.listener = new HttpListener();
            listener.Prefixes.Add($"http://{host}:{port}/");
        }

        public delegate void LogHandler(object sender, LogEvent e);

        public event LogHandler RequestLog;

        public event LogHandler ResponseLog;
        public void CyclingListen()
        {
            listener.Start();

            var controller = new Controller();

            controller.ResponseLog -= (s, e) => { };

            controller.ResponseLog += (s, e) =>
                ResponseLog?.Invoke(this, new LogEvent(e.Message, e.Code));

            while (true)
            {
                var context = listener.GetContext();
                var request = context.Request;

                RequestLog?.Invoke(this, new LogEvent(request.HttpMethod, request.RawUrl));

                var b = controller.SendResponse(request, context.Response);

                if (!b)
                    break;
            }
        }

        public void Dispose()
        {
            listener.Close();
        }
    }
}
