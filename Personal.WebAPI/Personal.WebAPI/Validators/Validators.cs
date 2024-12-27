using FluentValidation;
using static Personal.WebAPI.Models.Enum_model;
using static Personal.WebAPI.Models.Request;

namespace Personal.WebAPI.Validators
{
    public class Validators
    {
        #region Agent
        public class GetAgentRequestValidator : AbstractValidator<GetAgentRequest>
        {
            public GetAgentRequestValidator()
            {
                RuleFor(x => x.id).NotEmpty().NotNull().WithMessage("Id is required.");
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
            }
        }
        public class DeleteAgentRequestValidator : AbstractValidator<DeleteAgentRequest>
        {
            public DeleteAgentRequestValidator()
            {
                RuleFor(x => x.id).NotEmpty().NotNull().WithMessage("Id is required.");
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
            }
        }
        public class GetListRequestValidator : AbstractValidator<GetAgentListRequest>
        {
            public GetListRequestValidator()
            {
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
            }
        }
        public class SaveAgentRequestValidator : AbstractValidator<SaveAgentRequest>
        {
            public SaveAgentRequestValidator()
            {
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
                RuleFor(x => x.name).NotEmpty().NotNull().WithMessage("name is required.");
                RuleFor(x => x.email).NotEmpty().NotNull().WithMessage("email is required.");
                RuleFor(x => x.phoneExtension).NotEmpty().NotNull().WithMessage("phoneExtension is required.");
                
            }
        }
        public class EditAgentRequestValidator : AbstractValidator<EditAgentRequest>
        {
            public EditAgentRequestValidator()
            {
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
                RuleFor(x => x.id).NotEmpty().NotNull().WithMessage("id is required.");
                RuleFor(x => x.email).NotEmpty().NotNull().WithMessage("email is required.");
                RuleFor(x => x.phoneExtension).NotEmpty().NotNull().WithMessage("phoneExtension is required.");
                RuleFor(x => x.status).NotEmpty().NotNull().WithMessage("status is required.");

                RuleFor(x => x.status)
            .Must(value => Enum.IsDefined(typeof(AgentStatus), value))
            .WithMessage("Invalid status value.");
            }
        }
        public class EditAgentStatusRequestValidator : AbstractValidator<EditAgentStatusRequest>
        {
            public EditAgentStatusRequestValidator()
            {
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
                RuleFor(x => x.id).NotEmpty().NotNull().WithMessage("id is required.");
                RuleFor(x => x.status).NotEmpty().NotNull().WithMessage("status is required.");
            }
        }
        #endregion
        #region Customer
        public class GetCustomerRequestValidator : AbstractValidator<GetCustomerRequest>
        {
            public GetCustomerRequestValidator()
            {
                RuleFor(x => x.id).NotEmpty().NotNull().WithMessage("Id is required.");
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
            }
        }
        public class DeleteCustomerRequestValidator : AbstractValidator<DeleteCustomerRequest>
        {
            public DeleteCustomerRequestValidator()
            {
                RuleFor(x => x.id).NotEmpty().NotNull().WithMessage("Id is required.");
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
            }
        }
        public class GetCustomerListRequestValidator : AbstractValidator<GetCustomerListRequest>
        {
            public GetCustomerListRequestValidator()
            {
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
            }
        }
        public class SaveCustomerRequestValidator : AbstractValidator<SaveCustomerRequest>
        {
            public SaveCustomerRequestValidator()
            {
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
                RuleFor(x => x.name).NotEmpty().NotNull().WithMessage("name is required.");
                RuleFor(x => x.email).NotEmpty().NotNull().WithMessage("email is required.");
                RuleFor(x => x.phoneNumber).NotEmpty().NotNull().WithMessage("phoneNumber is required.");
            }
        }
        public class EditCustomerRequestValidator : AbstractValidator<EditCustomerRequest>
        {
            public EditCustomerRequestValidator()
            {
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
                RuleFor(x => x.id).NotEmpty().NotNull().WithMessage("id is required.");
                RuleFor(x => x.email).NotEmpty().NotNull().WithMessage("email is required.");
                RuleFor(x => x.phoneNumber).NotEmpty().NotNull().WithMessage("phoneNumber is required.");
            }
        }
        #endregion
        #region Call
        public class GetCallRequestValidator : AbstractValidator<GetCallRequest>
        {
            public GetCallRequestValidator()
            {
                RuleFor(x => x.id).NotEmpty().NotNull().WithMessage("Id is required.");
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
            }
        }
        public class DeleteCallRequestValidator : AbstractValidator<DeleteCallRequest>
        {
            public DeleteCallRequestValidator()
            {
                RuleFor(x => x.id).NotEmpty().NotNull().WithMessage("Id is required.");
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
            }
        }
        public class GetCallListRequestValidator : AbstractValidator<GetCallListRequest>
        {
            public GetCallListRequestValidator()
            {
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
            }
        }
        public class SaveCallRequestValidator : AbstractValidator<SaveCallRequest>
        {
            public SaveCallRequestValidator()
            {
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
                RuleFor(x => x.customerId).NotEmpty().NotNull().WithMessage("customerId is required.");
                RuleFor(x => x.agentId).NotEmpty().NotNull().WithMessage("agentId is required.");
                RuleFor(x => x.startTime).NotEmpty().NotNull().WithMessage("startTime is required.");
                RuleFor(x => x.endTime).NotEmpty().NotNull().WithMessage("endTime is required.");
                RuleFor(x => x.status).NotEmpty().NotNull().WithMessage("status is required.");

                RuleFor(x => x.status)
            .Must(value => Enum.IsDefined(typeof(CallStatus), value))
            .WithMessage("Invalid status value.");
            }
        }
        public class EditCallRequestValidator : AbstractValidator<EditCallRequest>
        {
            public EditCallRequestValidator()
            {
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
                RuleFor(x => x.id).NotEmpty().NotNull().WithMessage("id is required.");
                RuleFor(x => x.customerId).NotEmpty().NotNull().WithMessage("customerId is required.");
                RuleFor(x => x.agentId).NotEmpty().NotNull().WithMessage("agentId is required.");
                RuleFor(x => x.startTime).NotEmpty().NotNull().WithMessage("startTime is required.");
                RuleFor(x => x.endTime).NotEmpty().NotNull().WithMessage("endTime is required.");
                RuleFor(x => x.status).NotEmpty().NotNull().WithMessage("status is required.");

                RuleFor(x => x.status)
            .Must(value => Enum.IsDefined(typeof(CallStatus), value))
            .WithMessage("Invalid status value.");
            }
        }
        public class AssignCallRequestValidator : AbstractValidator<AssignCallRequest>
        {
            public AssignCallRequestValidator()
            {
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
                RuleFor(x => x.callId).NotEmpty().NotNull().WithMessage("callId is required.");
                RuleFor(x => x.agentId).NotEmpty().NotNull().WithMessage("agentId is required.");
            }
        }
        #endregion
        #region Ticket
        public class GetTicketRequestValidator : AbstractValidator<GetTicketRequest>
        {
            public GetTicketRequestValidator()
            {
                RuleFor(x => x.id).NotEmpty().NotNull().WithMessage("Id is required.");
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
            }
        }
        public class DeleteTicketRequestValidator : AbstractValidator<DeleteTicketRequest>
        {
            public DeleteTicketRequestValidator()
            {
                RuleFor(x => x.id).NotEmpty().NotNull().WithMessage("Id is required.");
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
            }
        }
        public class GetTicketListRequestValidator : AbstractValidator<GetTicketListRequest>
        {
            public GetTicketListRequestValidator()
            {
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
            }
        }
        public class SaveTicketRequestValidator : AbstractValidator<SaveTicketRequest>
        {
            public SaveTicketRequestValidator()
            {
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
                RuleFor(x => x.customerId).NotEmpty().NotNull().WithMessage("customerId is required.");
                RuleFor(x => x.agentId).NotEmpty().NotNull().WithMessage("agentId is required.");
                RuleFor(x => x.status).NotEmpty().NotNull().WithMessage("status is required.");
                RuleFor(x => x.priority).NotEmpty().NotNull().WithMessage("priority is required.");
                RuleFor(x => x.createdAt).NotEmpty().NotNull().WithMessage("createdAt is required.");
                RuleFor(x => x.updateAt).NotEmpty().NotNull().WithMessage("updateAt is required.");
                RuleFor(x => x.description).NotEmpty().NotNull().WithMessage("description is required.");
                RuleFor(x => x.resolution).NotEmpty().NotNull().WithMessage("resolution is required.");

                RuleFor(x => x.status)
            .Must(value => Enum.IsDefined(typeof(TicketStatus), value))
            .WithMessage("Invalid status value.");

                RuleFor(x => x.priority)
            .Must(value => Enum.IsDefined(typeof(TicketPriority), value))
            .WithMessage("Invalid priority value.");
            }
        }
        public class EditTicketRequestValidator : AbstractValidator<EditTicketRequest>
        {
            public EditTicketRequestValidator()
            {
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
                RuleFor(x => x.id).NotEmpty().NotNull().WithMessage("id is required.");
                RuleFor(x => x.customerId).NotEmpty().NotNull().WithMessage("customerId is required.");
                RuleFor(x => x.agentId).NotEmpty().NotNull().WithMessage("agentId is required.");
                RuleFor(x => x.status).NotEmpty().NotNull().WithMessage("status is required.");
                RuleFor(x => x.priority).NotEmpty().NotNull().WithMessage("priority is required.");
                RuleFor(x => x.createdAt).NotEmpty().NotNull().WithMessage("createdAt is required.");
                RuleFor(x => x.updateAt).NotEmpty().NotNull().WithMessage("updateAt is required.");
                RuleFor(x => x.description).NotEmpty().NotNull().WithMessage("description is required.");
                RuleFor(x => x.resolution).NotEmpty().NotNull().WithMessage("resolution is required.");

                RuleFor(x => x.status)
            .Must(value => Enum.IsDefined(typeof(TicketStatus), value))
            .WithMessage("Invalid status value.");

                RuleFor(x => x.priority)
            .Must(value => Enum.IsDefined(typeof(TicketPriority), value))
            .WithMessage("Invalid priority value.");
            }
        } 
        public class AssignTicketRequestValidator : AbstractValidator<AssignTicketRequest>
        {
            public AssignTicketRequestValidator()
            {
                RuleFor(x => x._username).NotEmpty().NotNull().WithMessage("_username is required.");
                RuleFor(x => x.ticketId).NotEmpty().NotNull().WithMessage("ticketId is required.");
                RuleFor(x => x.agentId).NotEmpty().NotNull().WithMessage("agentId is required.");
            }
        }
        #endregion
        #region Login
        public class LoginRequestValidator : AbstractValidator<LoginRequest>
        {
            public LoginRequestValidator()
            {
                RuleFor(x => x.username).NotEmpty().NotNull().WithMessage("Invalid username or password.");
                RuleFor(x => x.password).NotEmpty().NotNull().WithMessage("Invalid username or password.");
            }
        }
        #endregion
    }
}
