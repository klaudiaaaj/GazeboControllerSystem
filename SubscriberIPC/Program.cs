using Contracts.Services;
using System.IO.Pipes;

using (var pipeClient = new NamedPipeClientStream(".", "test-pipe", PipeDirection.In))
{
    Console.WriteLine("Subscriber is connecting to publisher...");
    pipeClient.Connect();
    Console.WriteLine("Connected to publisher!");

    using (StreamReader sr = new StreamReader(pipeClient))
    {
        while (true)
        {
            string message = sr.ReadLine();
            if (message == null) // End of the stream has been reached
            {
                Console.WriteLine("Connection closed by publisher. Exiting...");
                return;
            }
            Console.WriteLine($"Message received: {message}");
            PythonCaller pyt = new PythonCaller();
                await pyt.GazeboContractor(message);
       
        }
    }
}
