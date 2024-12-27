using static Personal.WebAPI.Models.Enum_model;

namespace Personal.WebAPI.Models
{
    public class DB_model
    {
        public class tagent
        { 
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string phoneExtension { get; set; }
            public AgentStatus status { get; set; }
            public string password { get; set; }
            public bool isConfirmed { get; set; }
        }
        public class tcall
        {
            public int id { get; set; }
            public int customerId { get; set; }
            public int agentId { get; set; }
            public DateTime startTime { get; set; }
            public DateTime endTime { get; set; }
            public CallStatus status { get; set; }
            public string notes { get; set; }
        }
        public class tcustomer
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string phoneNumber { get; set; }
            public DateTime? LastContactDate { get; set; }
        }
        public class tticket
        {
            public int id { get; set; }
            public int customerId { get; set; }
            public int agentId { get; set; }
            public TicketStatus status { get; set; }
            public TicketPriority priority { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime updateAt { get; set; }
            public string description { get; set; }
            public string resolution { get; set; }
        }
    }
}
