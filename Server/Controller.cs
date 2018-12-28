using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Data;
using Serialize;

namespace Server
{
    public class Controller
    {
        private Input inputData = new Input();

        private Output outputData;

        public Controller()
        {
            var tmp = new List<decimal>();
            tmp.AddRange(inputData.Sums);

            foreach (var m in inputData.Muls)
            {
                tmp.Add(m);
            }

            outputData = new Output()
            {
                SumResult = inputData.Sums.Sum() * inputData.K,
                MulResult = inputData.Muls.Aggregate((x,y) => x*y),
                SortedInputs = (from i in tmp
                                orderby i
                                select i).ToArray()
            };
        }

        private HttpListenerResponse response;

        private string requestContent;
        public bool SendResponse(HttpListenerRequest request, HttpListenerResponse response)
        {
            this.response = response;

            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                requestContent = reader.ReadToEnd();
            }

            switch (request.RawUrl)
            {
                // изменил кейсы для теста клиента
                case "/Ping":
                    return PingMethod();

                case "/GetInputData":
                    return PostInputData();

                case "/WriteAnswer":
                    return GetAnswer();

                case "/Stop":
                    return Stop();
            }

            return true;
        }

        public delegate void LogHandler(object sender, LogEvent e);

        public event LogHandler ResponseLog;

        
        private bool Stop()
        {
            
            response.StatusCode = (int)HttpStatusCode.OK;

            using (var output = response.OutputStream)
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes("Stopped");

                output.Write(bytes, 0, bytes.Length);
            }

            ResponseLog?.Invoke(this, new LogEvent("Stopped", ""));

            return false;
        }

        private Serialization<Output> serialization = new Serialization<Output>("Json");
        private bool GetAnswer()
        {
            response.StatusCode = (int)HttpStatusCode.OK;

            var answer = "Wrong answer";

            if (outputData.Equals((Output)serialization.Deserialize(requestContent)))
                answer = "Right answer";


            using (var output = response.OutputStream)
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(answer);
                output.Write(bytes, 0, bytes.Length);
            }

            ResponseLog?.Invoke(this, new LogEvent("Answer sent", answer));
            return true;
        }

        private bool PingMethod()
        {
            response.StatusCode = (int)HttpStatusCode.OK;

            using (var output = response.OutputStream)
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes("Pinged");

                output.Write(bytes, 0, bytes.Length);
            }

            ResponseLog?.Invoke(this, new LogEvent("Pinged", ""));

            return true;
        }

        private bool PostInputData()
        {
            response.StatusCode = (int) HttpStatusCode.OK;

            using (var output = response.OutputStream)
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(serialization.Serialize(inputData));

                output.Write(bytes, 0, bytes.Length);
            }

            ResponseLog?.Invoke(this, new LogEvent("Input data is posted", ""));
            return true;
        }
    }

}