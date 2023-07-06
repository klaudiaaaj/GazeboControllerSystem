using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Contracts.Models;
using System.Text.Json;

namespace Publisher.Services
{
    public class AzureServiceBusSender : IAzureServiceBusSender
    {
        private string ConnectionString = ""; //hidden
        private readonly ServiceBusClient client;
        private readonly ServiceBusSender sender;
        private readonly IConfiguration _configuration;

        public AzureServiceBusSender(IConfiguration configuration)
        {
            _configuration = configuration;
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            var tokenCredential = new VisualStudioCredential(new VisualStudioCredentialOptions { TenantId = "ab840be7-206b-432c-bd22-4c20fdc1b261" });
            client = new ServiceBusClient(_configuration["AzureConnectionString"], tokenCredential);
            sender = client.CreateSender(_configuration["Azure_QueueName"]);
        }

        public async Task Send(IList<Joystic> message)
        {
            try
            {
                var list = message.Take(20); 

                using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

                for (int i = 1; i <= 29; i++)
                {
                    // try adding a message to the batch
                    if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {message[i]}")))
                    {
                        // if it is too large for the batch
                        throw new Exception($"The message {i} is too large to fit in the batch.");
                    }
                }
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"A batch of {messageBatch.Count} messages has been published to the queue.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }

            Console.WriteLine("Press any key to end the application");
            Console.ReadKey();
        }
    }
}
