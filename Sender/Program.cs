using System.Net;
using System.Text;

namespace Sender
{
    internal class Program
    {
        public static HttpClient _client;
        static void Main(string[] args)
        {
            Task.Factory.StartNew(() => Sender());
            Listener();

        }

        private static void Listener()
        {
            HttpListener _listener = new();
            _listener.Prefixes.Add("http://127.0.0.1:9587/");
            bool _enabled = false;
            while (!_enabled)
            {
                _listener.Start();
                //Console.WriteLine("Start listening ...");
                var context = _listener.GetContextAsync().Result;

                HttpListenerRequest req = context.Request;

               var obj= req.Cookies.First(x => x.Name == "Username" && x.Value == "psw");
                using Stream body = req.InputStream;
                using var reader = new StreamReader(body, req.ContentEncoding);
                string res1 = reader.ReadToEndAsync().Result;

                Console.WriteLine("Incoming Request:" + res1 + "\n");

                var res = context.Response;

                string text = "Request successful handled";

                byte[] bytes = Encoding.UTF8.GetBytes(text);

                res.ContentLength64 = bytes.Length;
                res.OutputStream.WriteAsync(bytes);

                res.OutputStream.FlushAsync();
                //await Console.Out.WriteLineAsync("Dasturni tugatishni istaysizmi? 1 | 0 ");

                //Boolean.TryParse(Console.ReadLine(), out _enabled);

            }
            _listener.Stop();
            Console.WriteLine("Finished!");
        }
        private static void Sender()
        {
            while (true)
            {
                _client = new HttpClient();
                while (true)
                {
                    Console.WriteLine("Xabar kiriting!");
                    StringContent content = new(Console.ReadLine() ?? " no message!");
                    HttpResponseMessage response = _client.PostAsync("http://127.0.0.1:9519/", content).Result;

                    Console.WriteLine("So`rov natijasi:" + response.Content.ReadAsStringAsync());
                }
            }
        }
    }
}