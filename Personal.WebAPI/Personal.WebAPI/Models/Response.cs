using Swashbuckle.AspNetCore.SwaggerGen;
using static Personal.WebAPI.Models.DB_model;
using static Personal.WebAPI.Models.Enum_model;

namespace Personal.WebAPI.Models
{
    public class Response
    {
        public class DefaultResponse
        {
            public bool isSuccess { get; set; } = true;
            public string message { get; set; } = "Success.";
        }
        #region Stats
        public class GetStatsResponse : DefaultResponse
        {
            public List<StatsDetails> agent { get; set; }
        }
        public class StatsDetails 
        {
            public int id { get; set; }
            public string name { get; set; }
            public int numberOfCalls { get; set; }
            public int numberOfTickets { get; set; }
            public double averageCallDuration { get; set; }
        }
        #endregion
        #region Agent
        public class GetAgentListResponse: DefaultResponse
        {
           public List<tagentLimitFields> agent { get; set; }
        }
        
        public class GetAgentResponse : DefaultResponse
        {
            public tagent agent { get; set; }
        }

        public class tagentLimitFields
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string phoneExtension { get; set; }
            public AgentStatus status { get; set; }
        }
        #endregion
        #region Customer
        public class GetCustomerListResponse : DefaultResponse
        {
            public List<tcustomer> customer { get; set; }
        }
        public class GetCustomerResponse : DefaultResponse
        {
            public tcustomer customer { get; set; }

        }
        #endregion
        #region Call
        public class GetCallListResponse : DefaultResponse
        {
            public List<tcall> call { get; set; }
            public int total { get; set; }
        }
        public class GetCallResponse : DefaultResponse
        {
            public tcall call { get; set; }

        }
        #endregion
        #region Ticket
        public class GetTicketListResponse : DefaultResponse
        {
            public List<tticket> ticket { get; set; }
        }
        public class GetTicketResponse : DefaultResponse
        {
            public tticket ticket { get; set; }

        }
        #endregion
        #region Login
        public class LoginResponse : DefaultResponse
        {
            public string token { get; set; }
        }
        #endregion
    }
}
