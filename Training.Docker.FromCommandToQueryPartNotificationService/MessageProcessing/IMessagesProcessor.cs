namespace Training.Docker.FromCommandToQueryPartNotificationService.MessageProcessing
{
    public interface IMessagesProcessor
    {
         void ProcessMessageFromMessageBroker();
    }
}