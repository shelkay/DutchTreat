namespace DutchTreat.Services
{
    public interface IMailService
    {
        void SendMessage(string recipient, string subject, string body);
    }
}