namespace Personal.WebAPI.Models
{
    public class Enum_model
    {
        public enum CallStatus
        {
            Queued, InProgress, Completed, Dropped
        }
        public enum AgentStatus
        {
            Available, Busy, Offline
        }
        public enum TicketStatus
        {
            Open, InProgress, Resolved, Closed
        }
        public enum TicketPriority
        {
            Low, Medium, High, Urgent
        }
    }
}
