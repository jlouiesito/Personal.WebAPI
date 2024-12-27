using Microsoft.AspNetCore.Mvc;
using Personal.WebAPI.Interfaces;
using static Personal.WebAPI.Models.DB_model;
using static Personal.WebAPI.Models.Request;
using static Personal.WebAPI.Models.Response;

namespace Personal.WebAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class Personal_Controller
    {
        private readonly IPersonal_Repository _repo;
        public Personal_Controller(IPersonal_Repository repo)
        {
            _repo = repo;
        }

        #region Agent
        [HttpGet("agent/list")]
        public async Task<ActionResult<GetAgentListResponse>> GetAgentList([FromQuery] GetAgentListRequest request)
        {
            GetAgentListResponse response = new GetAgentListResponse();
            try
            {
                response = await _repo.GetAgentList(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpGet("agent/get")]
        public async Task<ActionResult<GetAgentResponse>> GetAgent([FromQuery] GetAgentRequest request)
        {
            GetAgentResponse response = new GetAgentResponse();
            try
            {
                response = await _repo.GetAgent(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpGet("agent/delete")]
        public async Task<ActionResult<DefaultResponse>> DeleteAgent([FromQuery] DeleteAgentRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            try
            {
                response = await _repo.DeleteAgent(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpPost("agent/save")]
        public async Task<ActionResult<DefaultResponse>> SaveAgent([FromBody] SaveAgentRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            try
            {
                response = await _repo.SaveAgent(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpPatch("agent/update")]
        public async Task<ActionResult<DefaultResponse>> EditAgent([FromBody] EditAgentRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            try
            {
                response = await _repo.EditAgent(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpPatch("agent/updatestatus")]
        public async Task<ActionResult<DefaultResponse>> EditAgentStatus([FromBody] EditAgentStatusRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            try
            {
                response = await _repo.EditAgentStatus(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        #endregion
        #region Customer
        [HttpGet("customer/list")]
        public async Task<ActionResult<GetCustomerListResponse>> GetCustomerList([FromQuery] GetCustomerListRequest request)
        {
            GetCustomerListResponse response = new GetCustomerListResponse();
            try
            {
                response = await _repo.GetCustomerList(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpGet("customer/get")]
        public async Task<ActionResult<GetCustomerResponse>> GetCustomer([FromQuery] GetCustomerRequest request)
        {
            GetCustomerResponse response = new GetCustomerResponse();
            try
            {
                response = await _repo.GetCustomer(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpGet("customer/delete")]
        public async Task<ActionResult<DefaultResponse>> DeleteAgent([FromQuery] DeleteCustomerRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            try
            {
                response = await _repo.DeleteCustomer(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpPost("customer/save")]
        public async Task<ActionResult<DefaultResponse>> SaveCustomer([FromBody] SaveCustomerRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            try
            {
                response = await _repo.SaveCustomer(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpPatch("customer/update")]
        public async Task<ActionResult<DefaultResponse>> EditCustomer([FromBody] EditCustomerRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            try
            {
                response = await _repo.EditCustomer(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        #endregion
        #region Call
        [HttpGet("call/list")]
        public async Task<ActionResult<GetCallListResponse>> GetCallList([FromQuery] GetCallListRequest request)
        {
            GetCallListResponse response = new GetCallListResponse();
            try
            {
                response = await _repo.GetCallList(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpGet("call/get")]
        public async Task<ActionResult<GetCallResponse>> GetCall([FromQuery] GetCallRequest request)
        {
            GetCallResponse response = new GetCallResponse();
            try
            {
                response = await _repo.GetCall(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpGet("call/delete")]
        public async Task<ActionResult<DefaultResponse>> DeleteCall([FromQuery] DeleteCallRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            try
            {
                response = await _repo.DeleteCall(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpPost("call/save")]
        public async Task<ActionResult<DefaultResponse>> SaveCall([FromBody] SaveCallRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            try
            {
                response = await _repo.SaveCall(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpPatch("call/update")]
        public async Task<ActionResult<DefaultResponse>> EditCall([FromBody] EditCallRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            try
            {
                response = await _repo.EditCall(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpPatch("call/assigncall")]
        public async Task<ActionResult<DefaultResponse>> AssignCall([FromBody] AssignCallRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            try
            {
                response = await _repo.AssignCall(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        #endregion
        #region Ticket
        [HttpGet("ticket/list")]
        public async Task<ActionResult<GetTicketListResponse>> GetTicketList([FromQuery] GetTicketListRequest request)
        {
            GetTicketListResponse response = new GetTicketListResponse();
            try
            {
                response = await _repo.GetTicketList(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpGet("ticket/get")]
        public async Task<ActionResult<GetTicketResponse>> GetTicket([FromQuery] GetTicketRequest request)
        {
            GetTicketResponse response = new GetTicketResponse();
            try
            {
                response = await _repo.GetTicket(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpGet("ticket/delete")]
        public async Task<ActionResult<DefaultResponse>> DeleteTicket([FromQuery] DeleteTicketRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            try
            {
                response = await _repo.DeleteTicket(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpPost("ticket/save")]
        public async Task<ActionResult<DefaultResponse>> SaveTicket([FromBody] SaveTicketRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            try
            {
                response = await _repo.SaveTicket(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpPatch("ticket/update")]
        public async Task<ActionResult<DefaultResponse>> EditTicket([FromBody] EditTicketRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            try
            {
                response = await _repo.EditTicket(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        [HttpPatch("ticket/assignticket")]
        public async Task<ActionResult<DefaultResponse>> AssignTicket([FromBody] AssignTicketRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            try
            {
                response = await _repo.AssignTicket(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        #endregion
        #region Login
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            LoginResponse response = new LoginResponse();
            try
            {
                response = await _repo.Login(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        #endregion
        #region Stats
        [HttpGet("stats")]
        public async Task<ActionResult<GetStatsResponse>> GetStats([FromQuery] DefaultRequest request)
        {
            GetStatsResponse response = new GetStatsResponse();
            try
            {
                response = await _repo.GetStats(request);
                if (!response.isSuccess)
                    throw new Exception(response.message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        #endregion
    }
}
