namespace Server
{
    public class LogEvent
    {
        public string Message { get; }
        public string Code { get; }

        public LogEvent(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
