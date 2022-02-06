using Hard.Business.Interfaces;

namespace Hard.Business.Notifications
{
    public class Notification : INotification
    {
        public string Message { get; }

        public Notification(string message)
        {
            Message = message;
        }               
    }
}
