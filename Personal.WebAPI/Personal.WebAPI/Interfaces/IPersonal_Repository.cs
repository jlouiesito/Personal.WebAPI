using Personal.WebAPI.Models;

namespace Personal.WebAPI.Interfaces
{
    public interface IPersonal_Repository
    {
        Task<Response.DefaultResponse> AssignCall(Request.AssignCallRequest request);
        Task<Response.DefaultResponse> AssignTicket(Request.AssignTicketRequest request);
        Task<Response.DefaultResponse> DeleteAgent(Request.DeleteAgentRequest request);
        Task<Response.DefaultResponse> DeleteCall(Request.DeleteCallRequest request);
        Task<Response.DefaultResponse> DeleteCustomer(Request.DeleteCustomerRequest request);
        Task<Response.DefaultResponse> DeleteTicket(Request.DeleteTicketRequest request);
        Task<Response.DefaultResponse> EditAgent(Request.EditAgentRequest request);
        Task<Response.DefaultResponse> EditAgentStatus(Request.EditAgentStatusRequest request);
        Task<Response.DefaultResponse> EditCall(Request.EditCallRequest request);
        Task<Response.DefaultResponse> EditCustomer(Request.EditCustomerRequest request);
        Task<Response.DefaultResponse> EditTicket(Request.EditTicketRequest request);
        Task<Response.GetAgentResponse> GetAgent(Request.GetAgentRequest request);
        Task<Response.GetAgentListResponse> GetAgentList(Request.GetAgentListRequest request);
        Task<Response.GetCallResponse> GetCall(Request.GetCallRequest request);
        Task<Response.GetCallListResponse> GetCallList(Request.GetCallListRequest request);
        Task<Response.GetCustomerResponse> GetCustomer(Request.GetCustomerRequest request);
        Task<Response.GetCustomerListResponse> GetCustomerList(Request.GetCustomerListRequest request);
        Task<Response.GetStatsResponse> GetStats(Request.DefaultRequest request);
        Task<Response.GetTicketResponse> GetTicket(Request.GetTicketRequest request);
        Task<Response.GetTicketListResponse> GetTicketList(Request.GetTicketListRequest request);
        Task<Response.LoginResponse> Login(Request.LoginRequest request);
        Task<Response.DefaultResponse> SaveAgent(Request.SaveAgentRequest request);
        Task<Response.DefaultResponse> SaveCall(Request.SaveCallRequest request);
        Task<Response.DefaultResponse> SaveCustomer(Request.SaveCustomerRequest request);
        Task<Response.DefaultResponse> SaveTicket(Request.SaveTicketRequest request);
    }
}
