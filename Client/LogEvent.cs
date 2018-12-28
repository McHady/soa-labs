using System.Net.Http;

namespace Client
{
    public class LogEvent
    {
        public LogEvent(string Response)
        {
            this.Response = Response;
        }

        public string Response { get; internal set; }
    }
}