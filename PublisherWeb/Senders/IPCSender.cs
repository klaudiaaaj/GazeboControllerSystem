using System.IO.Pipes;

namespace PublisherWeb.Senders
{
    public class IPCSender : ISender
    {
        public void Send(string message)
        {
            using (var pipeServer = new NamedPipeServerStream("test-pipe"))
            {
                Console.WriteLine("Publisher is waiting for subscriber...");
                pipeServer.WaitForConnection();
                Console.WriteLine("Subscriber connected!");

                using (StreamWriter sw = new StreamWriter(pipeServer))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("Hello from publisher!");
                    Console.WriteLine("Message sent.");
                }
            }

        }
    }
}
