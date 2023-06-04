namespace Contracts
{
    public class MessageWithArrivalTime
    {
        public MessageWithArrivalTime(string message, DateTime arrivalDate)
        {
            Message = message;
            ArrivalDate = arrivalDate;
        }
        public string Message { get; set; }
        public DateTime ArrivalDate { get; set; }

        public DateTime SendDate { get; set; }
    }
}
