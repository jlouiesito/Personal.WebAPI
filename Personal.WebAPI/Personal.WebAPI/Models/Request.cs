using FluentValidation;
using FluentValidation.Results;
using Personal.WebAPI.Validators;
using static Personal.WebAPI.Models.Enum_model;
using static Personal.WebAPI.Models.Response;
using static Personal.WebAPI.Validators.Validators;

namespace Personal.WebAPI.Models
{
    public class Request
    {
        public class DefaultRequest
        {
            public string _username { get; set; }
        }
        #region Agent
        public class GetAgentListRequest : DefaultRequest
        {
            public Dictionary<string,string> filter = new Dictionary<string,string>();
            public void Validate()
            {
                GetListRequestValidator validator = new GetListRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }

        }
        public class GetAgentRequest : DefaultRequest
        {
            public int id { get; set; }
            public void Validate()
            {
                GetAgentRequestValidator validator = new GetAgentRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }

        }
        public class DeleteAgentRequest : DefaultRequest
        {
            public int id { get; set; }
            public void Validate()
            {
                DeleteAgentRequestValidator validator = new DeleteAgentRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }

        }
        public class SaveAgentRequest : DefaultRequest
        {
            public string name { get; set; }
            public string email { get; set; }
            public string phoneExtension { get; set; }
            public void Validate()
            {
                SaveAgentRequestValidator validator = new SaveAgentRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }
        }
        public class EditAgentRequest : DefaultRequest
        {
            public int id { get; set; }
            public string email { get; set; }
            public string name { get; set; }
            public string phoneExtension { get; set; }
            public AgentStatus status { get; set; }
            public void Validate()
            {
                EditAgentRequestValidator validator = new EditAgentRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }
        }
        public class EditAgentStatusRequest : DefaultRequest
        {
            public int id { get; set; }
            public AgentStatus status { get; set; }
            public void Validate()
            {
                EditAgentStatusRequestValidator validator = new EditAgentStatusRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }
        }
        #endregion
        #region Customer
        public class GetCustomerListRequest : DefaultRequest
        {
            public Dictionary<string, string> filter = new Dictionary<string, string>();
            public void Validate()
            {
                GetCustomerListRequestValidator validator = new GetCustomerListRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }

        }
        public class GetCustomerRequest : DefaultRequest
        {
            public int id { get; set; }
            public void Validate()
            {
                GetCustomerRequestValidator validator = new GetCustomerRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }

        }
        public class DeleteCustomerRequest : DefaultRequest
        {
            public int id { get; set; }
            public void Validate()
            {
                DeleteCustomerRequestValidator validator = new DeleteCustomerRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }

        }
        public class SaveCustomerRequest : DefaultRequest
        {
            public string name { get; set; }
            public string email { get; set; }
            public string phoneNumber { get; set; }
            public DateTime LastContactDate { get; set; }
            public void Validate()
            {
                SaveCustomerRequestValidator validator = new SaveCustomerRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }
        }
        public class EditCustomerRequest : DefaultRequest
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string phoneNumber { get; set; }
            public DateTime LastContactDate { get; set; }
            public void Validate()
            {
                EditCustomerRequestValidator validator = new EditCustomerRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }
        }
        #endregion
        #region Calls
        public class GetCallListRequest : DefaultRequest
        {
            public int? customerId { get; set; }
            public int? agentId { get; set; }
            public DateTime? startTime { get; set; }
            public DateTime? endTime { get; set; }
            public CallStatus? status { get; set; }
            public int pageNumber { get; set; } = 1; // Default to first page
            public int pageSize { get; set; } = 10; // Default page size
            public void Validate()
            {
                GetCallListRequestValidator validator = new GetCallListRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }

        }
        public class GetCallRequest : DefaultRequest
        {
            public int id { get; set; }
            public void Validate()
            {
                GetCallRequestValidator validator = new GetCallRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }

        }
        public class DeleteCallRequest : DefaultRequest
        {
            public int id { get; set; }
            public void Validate()
            {
                DeleteCallRequestValidator validator = new DeleteCallRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }

        }
        public class SaveCallRequest : DefaultRequest
        {
            public int customerId { get; set; }
            public int agentId { get; set; } = 0;
            public DateTime startTime { get; set; }
            public DateTime endTime { get; set; }
            public CallStatus status { get; set; }
            public string notes { get; set; }
            public void Validate()
            {
                SaveCallRequestValidator validator = new SaveCallRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }
        }
        public class EditCallRequest : DefaultRequest
        {
            public int id { get; set; }
            public int customerId { get; set; }
            public int agentId { get; set; }
            public DateTime startTime { get; set; }
            public DateTime endTime { get; set; }
            public CallStatus status { get; set; }
            public string notes { get; set; }
            public void Validate()
            {
                EditCallRequestValidator validator = new EditCallRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }
        }
        public class AssignCallRequest : DefaultRequest
        {
            public int callId { get; set; }
            public int agentId { get; set; }
            public void Validate()
            {
                AssignCallRequestValidator validator = new AssignCallRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }
        }
        #endregion
        #region Ticket
        public class GetTicketListRequest : DefaultRequest
        {
            public int? customerId { get; set; }
            public int? agentId { get; set; }
            public TicketStatus? status { get; set; }
            public TicketPriority? priority { get; set; }
            public DateTime? createdAt { get; set; }
            public DateTime? updateAt { get; set; }

            public int pageNumber { get; set; } = 1; // Default to first page
            public int pageSize { get; set; } = 10; // Default page size
            public void Validate()
            {
                GetTicketListRequestValidator validator = new GetTicketListRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }

        }
        public class GetTicketRequest : DefaultRequest
        {
            public int id { get; set; }
            public void Validate()
            {
                GetTicketRequestValidator validator = new GetTicketRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }

        }
        public class DeleteTicketRequest : DefaultRequest
        {
            public int id { get; set; }
            public void Validate()
            {
                DeleteTicketRequestValidator validator = new DeleteTicketRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }

        }
        public class SaveTicketRequest : DefaultRequest
        {
            public int customerId { get; set; }
            public int agentId { get; set; }
            public TicketStatus status { get; set; }
            public TicketPriority priority { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime updateAt { get; set; }
            public string description { get; set; }
            public string resolution { get; set; }
            public void Validate()
            {
                SaveTicketRequestValidator validator = new SaveTicketRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }
        }
        public class EditTicketRequest : DefaultRequest
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
            public void Validate()
            {
                EditTicketRequestValidator validator = new EditTicketRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }
        }
        public class AssignTicketRequest : DefaultRequest
        {
            public int ticketId { get; set; }
            public int agentId { get; set; }
            public void Validate()
            {
                AssignTicketRequestValidator validator = new AssignTicketRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }
        }
        #endregion
        #region Login
        public class LoginRequest 
        {
            public string username { get; set; }
            public string password { get; set; }
            public string token { get; set; }
            public void Validate()
            {
                LoginRequestValidator validator = new LoginRequestValidator();
                ValidationResult result = validator.Validate(this);
                if (result.Errors.Count > 0)
                    throw new InvalidRequestException(result.Errors.Select(e => e.ErrorMessage));
            }
        }
        #endregion
    }
}
