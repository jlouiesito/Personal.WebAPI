using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Personal.WebAPI.Configurations;
using Personal.WebAPI.Context;
using Personal.WebAPI.Helper;
using Personal.WebAPI.Interfaces;
using Personal.WebAPI.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Personal.WebAPI.Models.DB_model;
using static Personal.WebAPI.Models.Enum_model;
using static Personal.WebAPI.Models.Request;
using static Personal.WebAPI.Models.Response;

namespace Personal.WebAPI
{
    public class Personal_Repository : IPersonal_Repository
    {
        private readonly Personal_Context _context;
        private readonly IOptions<JwtConfig> _jwt;
        private const string key = "Bs4nis35sphBA7uycZqaTwSxmcFQ1qSN";
        private readonly EmailHelper _emailHelper;
        private readonly SmtpConfig _config;
        private const int KeySize = 32; // 256 bits
        private const int IvSize = 16; // 128 bits
        public Personal_Repository(Personal_Context context, IOptions<SmtpConfig> config, IOptions<JwtConfig> jwt)
        {
            _context = context;
            _config = config.Value;
            _emailHelper = new EmailHelper(config.Value);
            _jwt = jwt;
        }
        #region Agent
        public async Task<GetAgentListResponse> GetAgentList(GetAgentListRequest request)
        {
            GetAgentListResponse response = new GetAgentListResponse();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                request.Validate();
                if (request.filter.Count() == 0)
                {
                    response.agent = _context.tagent.Select(e => new tagentLimitFields { id = e.id,name = e.name, phoneExtension = e.phoneExtension,email = e.email,status = e.status}).ToList();
                }
                else 
                {
                    using (var context = _context)
                    {
                        var query = context.tagent.AsQueryable();

                        // Apply filters dynamically
                        foreach (var filter in request.filter)
                        {
                            if (filter.Key == "name" && filter.Value is string name)
                            {
                                query = query.Where(e => e.name == name);
                            }
                            if (filter.Key == "email" && filter.Value is string email)
                            {
                                query = query.Where(e => e.email == email);
                            }
                            if (filter.Key == "status" && filter.Value is string status)
                            {
                                AgentStatus enumStatus = (AgentStatus)Enum.Parse(typeof(AgentStatus), status);
                                query = query.Where(e => e.status == enumStatus);
                            }
                            
                        }

                        response.agent = query.Select(e => new tagentLimitFields { id = e.id, name = e.name, phoneExtension = e.phoneExtension, email = e.email, status = e.status }).ToList();
                    }
                }

                sw.Stop();
                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetAgentList,Success,{secs}");
            }
            catch (Exception ex) 
            {
                response.message = ex.Message;
                response.isSuccess = false;

                sw.Stop();
                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetAgentList,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<GetAgentResponse> GetAgent(GetAgentRequest request)
        {
            GetAgentResponse response = new GetAgentResponse();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                request.Validate();
                response.agent = _context.tagent.Where(x => x.id == request.id).FirstOrDefault();
                sw.Stop();
                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetAgent,Success,{secs}");

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.isSuccess = false;

                sw.Stop();
                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetAgent,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<DefaultResponse> DeleteAgent(DeleteAgentRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            await using var categoryTrans = await _context.Database.BeginTransactionAsync();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                request.Validate();
                _context.tagent.RemoveRange(_context.tagent.Where(x => request.id == x.id));
                await _context.SaveChangesAsync();
                await categoryTrans.CommitAsync();

                response.isSuccess = true; 
                response.message = "Success";

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},DeleteAgent,Success,{secs}");
            }
            catch (Exception ex)
            {
                await categoryTrans.RollbackAsync();
                response.message = ex.Message;
                response.isSuccess = false;

                sw.Stop();
                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},DeleteAgent,Error,{secs},{ex.Message}");

            }
            return response;
        }
        public async Task<DefaultResponse> SaveAgent(SaveAgentRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            await using var contextTransaction = await _context.Database.BeginTransactionAsync();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                request.Validate();
                string tempPassword = GenerateTemporaryPassword();

                tagent agentModel = new tagent();
                agentModel.id = 0;
                agentModel.name = request.name;
                agentModel.email = request.email;
                agentModel.password = ConvertPassword(tempPassword);
                agentModel.phoneExtension = request.phoneExtension;
                agentModel.isConfirmed = false;
                agentModel.status = AgentStatus.Offline;
                _context.Add(agentModel);
                await _context.SaveChangesAsync();

                
                _emailHelper.SendEmailPassword(request._username, request.email, tempPassword);
                await contextTransaction.CommitAsync();

                response.isSuccess = true;
                response.message = "Success";

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},SaveAgent,Success,{secs}");
            }
            catch (Exception ex)
            {
                await contextTransaction.RollbackAsync();
                response.message = ex.Message;
                response.isSuccess = false;

                sw.Stop();
                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},SaveAgent,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<DefaultResponse> EditAgent(EditAgentRequest request)
        {
            DefaultResponse response = new DefaultResponse();

            await using var contextTransaction = await _context.Database.BeginTransactionAsync();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                request.Validate();
                tagent agentModel = new tagent();
                agentModel.id = request.id;
                agentModel.name = request.name;
                agentModel.email = request.email;
                agentModel.phoneExtension = request.phoneExtension;
                agentModel.status = request.status;
                _context.Update(agentModel);
                await _context.SaveChangesAsync();
                await contextTransaction.CommitAsync();

                response.isSuccess = true;
                response.message = "Success";

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},EditAgent,Success,{secs}");

            }
            catch (Exception ex)
            {
                await contextTransaction.RollbackAsync();
                response.message = ex.Message;
                response.isSuccess = false;

                sw.Stop();
                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},EditAgent,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<DefaultResponse> EditAgentStatus(EditAgentStatusRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            await using var contextTransaction = await _context.Database.BeginTransactionAsync();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                request.Validate();
                tagent agentDetails = _context.tagent.Where(x => x.id == request.id).FirstOrDefault();
                if (agentDetails!=null)
                {
                    agentDetails.status = request.status;
                    _context.Update(agentDetails);
                    await _context.SaveChangesAsync();
                    await contextTransaction.CommitAsync();
                }
                else 
                {
                    throw new Exception("Record not found.");
                }
                response.isSuccess = true;
                response.message = "Success";

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},EditAgentStatus,Success,{secs}");
            }
            catch (Exception ex)
            {
                await contextTransaction.RollbackAsync();
                response.message = ex.Message;
                response.isSuccess = false;

                sw.Stop();
                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},EditAgentStatus,Error,{secs},{ex.Message}");

            }
            return response;
        }
        #endregion
        #region Customer
        public async Task<GetCustomerListResponse> GetCustomerList(GetCustomerListRequest request)
        {
            GetCustomerListResponse response = new GetCustomerListResponse();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                request.Validate();
                if (request.filter.Count() == 0)
                {
                    response.customer = _context.tcustomer.ToList();
                }
                else
                {
                    using (var context = _context)
                    {
                        var query = context.tcustomer.AsQueryable();

                        // Apply filters dynamically
                        foreach (var filter in request.filter)
                        {
                            if (filter.Key == "name" && filter.Value is string name)
                            {
                                query = query.Where(e => e.name == name);
                            }
                            if (filter.Key == "email" && filter.Value is string email)
                            {
                                query = query.Where(e => e.email == email);
                            }
                            if (filter.Key == "phoneNumber" && filter.Value is string phoneNumber)
                            {
                                query = query.Where(e => e.phoneNumber == phoneNumber);
                            }

                        }

                        response.customer = query.ToList();
                    }
                }
                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetCustomerList,Success,{secs}");
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.isSuccess = false;

                sw.Stop();
                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetCustomerList,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<GetCustomerResponse> GetCustomer(GetCustomerRequest request)
        {
            GetCustomerResponse response = new GetCustomerResponse();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                request.Validate();
                response.customer = _context.tcustomer.Where(x => x.id == request.id).FirstOrDefault();

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetCustomer,Success,{secs}");

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetCustomer,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<DefaultResponse> DeleteCustomer(DeleteCustomerRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            await using var categoryTrans = await _context.Database.BeginTransactionAsync();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                request.Validate();
                _context.tcustomer.RemoveRange(_context.tcustomer.Where(x => request.id == x.id));
                await _context.SaveChangesAsync();
                await categoryTrans.CommitAsync();

                response.isSuccess = true;
                response.message = "Success";

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetCustomer,Success,{secs}");
            }
            catch (Exception ex)
            {
                await categoryTrans.RollbackAsync();
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetCustomer,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<DefaultResponse> SaveCustomer(SaveCustomerRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            await using var contextTransaction = await _context.Database.BeginTransactionAsync();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                request.Validate();
                tcustomer customerModel = new tcustomer();
                customerModel.id = 0;
                customerModel.name = request.name;
                customerModel.email = request.email;
                _context.Add(customerModel);
                await _context.SaveChangesAsync();
                await contextTransaction.CommitAsync();

                response.isSuccess = true;
                response.message = "Success";

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetCustomer,Success,{secs}");
            }
            catch (Exception ex)
            {
                await contextTransaction.RollbackAsync();
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetCustomer,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<DefaultResponse> EditCustomer(EditCustomerRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            await using var contextTransaction = await _context.Database.BeginTransactionAsync();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                request.Validate();
                tcustomer customerModel = new tcustomer();
                customerModel.id = request.id;
                customerModel.name = request.name;
                customerModel.email = request.email;
                customerModel.LastContactDate = request.LastContactDate;
                _context.Update(customerModel);
                await _context.SaveChangesAsync();
                await contextTransaction.CommitAsync();

                response.isSuccess = true;
                response.message = "Success";

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},EditCustomer,Success,{secs}");
            }
            catch (Exception ex)
            {
                await contextTransaction.RollbackAsync();
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},EditCustomer,Error,{secs},{ex.Message}");
            }
            return response;
        }
        #endregion
        #region Call
        public async Task<GetCallListResponse> GetCallList(GetCallListRequest request)
        {
            GetCallListResponse response = new GetCallListResponse();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                request.Validate();

                var query = _context.tcall.AsQueryable();
                // Apply filters
                if (request.status.HasValue)
                {
                    var status = request.status.Value; 
                    query = query.Where(c => c.status == status);
                }
                if (request.startTime.HasValue)
                {
                    query = query.Where(c => c.startTime >= request.startTime.Value);
                }
                if (request.endTime.HasValue)
                {
                    query = query.Where(c => c.endTime <= request.endTime.Value);
                }

                if (request.agentId.HasValue)
                {
                    query = query.Where(c => c.agentId == request.agentId.Value);
                }


                if (request.customerId.HasValue)
                {
                    query = query.Where(c => c.customerId == request.customerId.Value);
                }

                // Get total count before applying pagination
                var totalCount = await query.CountAsync();

                // Apply pagination
                var calls = await query
                    .Skip((request.pageNumber - 1) * request.pageSize)
                    .Take(request.pageSize)
                    .ToListAsync();

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetCallList,Success,{secs}");

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetCallList,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<GetCallResponse> GetCall(GetCallRequest request)
        {
            GetCallResponse response = new GetCallResponse();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                request.Validate();
                response.call = _context.tcall.Where(x => x.id == request.id).FirstOrDefault();

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetCall,Success,{secs}");
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetCall,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<DefaultResponse> DeleteCall(DeleteCallRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            await using var contextTransaction = await _context.Database.BeginTransactionAsync();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                request.Validate();
                _context.tcall.RemoveRange(_context.tcall.Where(x => request.id == x.id));
                await _context.SaveChangesAsync();
                await contextTransaction.CommitAsync();

                response.isSuccess = true;
                response.message = "Success";

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},DeleteCall,Success,{secs}");
            }
            catch (Exception ex)
            {
                await contextTransaction.RollbackAsync();
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},DeleteCall,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<DefaultResponse> SaveCall(SaveCallRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            await using var contextTransaction = await _context.Database.BeginTransactionAsync();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                request.Validate();
                tcall callModel = new tcall();
                callModel.id = 0;
                callModel.customerId = request.customerId;
                callModel.agentId = request.agentId;
                callModel.startTime = request.startTime;
                callModel.endTime = request.endTime;
                callModel.status = request.status;
                callModel.notes = request.notes;
                _context.Update(callModel);
                await _context.SaveChangesAsync();
                await contextTransaction.CommitAsync();

                response.isSuccess = true;
                response.message = "Success";

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},SaveCall,Success,{secs}");
            }
            catch (Exception ex)
            {
                await contextTransaction.RollbackAsync();
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},SaveCall,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<DefaultResponse> EditCall(EditCallRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            await using var contextTransaction = await _context.Database.BeginTransactionAsync();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                request.Validate();
                tcall callModel = new tcall();
                callModel.id = request.id;
                callModel.customerId = request.customerId;
                callModel.agentId = request.agentId;
                callModel.startTime = request.startTime;
                callModel.endTime = request.endTime;
                callModel.status = request.status;
                callModel.notes = request.notes;
                _context.Update(callModel);
                await _context.SaveChangesAsync();
                await contextTransaction.CommitAsync();

                response.isSuccess = true;
                response.message = "Success";

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},EditCall,Success,{secs}");
            }
            catch (Exception ex)
            {
                await contextTransaction.RollbackAsync();
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},EditCall,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<DefaultResponse> AssignCall(AssignCallRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            await using var contextTransaction = await _context.Database.BeginTransactionAsync();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                request.Validate();
                tcall callDetails = _context.tcall.Where(x => x.id == request.callId).FirstOrDefault();
                if (callDetails != null)
                {
                    callDetails.agentId = request.agentId;
                    _context.Update(callDetails);
                    await _context.SaveChangesAsync();
                    await contextTransaction.CommitAsync();
                }
                else
                    throw new Exception("Call not found");
                response.isSuccess = true;
                response.message = "Success";

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},EditCall,Success,{secs}");
            }
            catch (Exception ex)
            {
                await contextTransaction.RollbackAsync();
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},EditCall,Error,{secs},{ex.Message}");
            }
            return response;
        }
        #endregion
        #region Ticket
        public async Task<GetTicketListResponse> GetTicketList(GetTicketListRequest request)
        {
            GetTicketListResponse response = new GetTicketListResponse();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                request.Validate();
                var query = _context.tticket.AsQueryable();
                // Apply filters
                if (request.status.HasValue)
                {
                    var status = request.status.Value;
                    query = query.Where(c => c.status == status);
                }
                if (request.createdAt.HasValue)
                {
                    query = query.Where(c => c.createdAt >= request.createdAt.Value);
                }
                if (request.priority.HasValue)
                {
                    var priority = request.priority.Value;
                    query = query.Where(c => c.priority == priority);
                }
                if (request.agentId.HasValue)
                {
                    query = query.Where(c => c.agentId == request.agentId.Value);
                }
                if (request.updateAt.HasValue)
                {
                    query = query.Where(c => c.updateAt == request.updateAt.Value);
                }
                if (request.customerId.HasValue)
                {
                    query = query.Where(c => c.customerId == request.customerId.Value);
                }

                // Get total count before applying pagination
                var totalCount = await query.CountAsync();

                // Apply pagination
                var calls = await query
                    .Skip((request.pageNumber - 1) * request.pageSize)
                    .Take(request.pageSize)
                    .ToListAsync();
                //if (request.filter.Count() == 0)
                //{
                //    response.ticket = _context.tticket.ToList();
                //}
                //else
                //{
                //    using (var context = _context)
                //    {
                //        var query = context.tticket.AsQueryable();

                //        // Apply filters dynamically
                //        foreach (var filter in request.filter)
                //        {
                //            if (filter.Key == "customerId" && int.TryParse(filter.Value?.ToString(), out int customerId))
                //            {
                //                query = query.Where(e => e.customerId == customerId);
                //            }
                //            if (filter.Key == "agentId" && int.TryParse(filter.Value?.ToString(), out int agentId))
                //            {
                //                query = query.Where(e => e.agentId == agentId);
                //            }
                //            if (filter.Key == "status" && filter.Value is string status)
                //            {
                //                TicketStatus enumStatus = (TicketStatus)Enum.Parse(typeof(TicketStatus), status);
                //                query = query.Where(e => e.status == enumStatus);
                //            }

                //        }

                //        response.ticket = query.ToList();
                //    }
                //}

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetTicketList,Success,{secs}");

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetTicketList,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<GetTicketResponse> GetTicket(GetTicketRequest request)
        {
            GetTicketResponse response = new GetTicketResponse();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                request.Validate();
                response.ticket = _context.tticket.Where(x => x.id == request.id).FirstOrDefault();

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetTicket,Success,{secs}");
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetTicket,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<DefaultResponse> DeleteTicket(DeleteTicketRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            await using var contextTransaction = await _context.Database.BeginTransactionAsync();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                request.Validate();
                _context.tticket.RemoveRange(_context.tticket.Where(x => request.id == x.id));
                await _context.SaveChangesAsync();
                await contextTransaction.CommitAsync();

                response.isSuccess = true;
                response.message = "Success";

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},DeleteTicket,Success,{secs}");
            }
            catch (Exception ex)
            {
                await contextTransaction.RollbackAsync();
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},DeleteTicket,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<DefaultResponse> SaveTicket(SaveTicketRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            await using var contextTransaction = await _context.Database.BeginTransactionAsync();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                request.Validate();
                tticket ticketModel = new tticket();
                ticketModel.id = 0;
                ticketModel.customerId = request.customerId;
                ticketModel.agentId = request.agentId;
                ticketModel.status = request.status;
                ticketModel.priority = request.priority;
                ticketModel.createdAt = request.createdAt;
                ticketModel.updateAt = request.updateAt;
                ticketModel.description = request.description;
                ticketModel.resolution = request.resolution;
                _context.Add(ticketModel);
                await _context.SaveChangesAsync();
                await contextTransaction.CommitAsync();

                response.isSuccess = true;
                response.message = "Success";

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},SaveTicket,Success,{secs}");
            }
            catch (Exception ex)
            {
                await contextTransaction.RollbackAsync();
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},SaveTicket,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<DefaultResponse> EditTicket(EditTicketRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            await using var contextTransaction = await _context.Database.BeginTransactionAsync();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                request.Validate();
                tticket ticketModel = new tticket();
                ticketModel.id = request.id;
                ticketModel.customerId = request.customerId;
                ticketModel.agentId = request.agentId;
                ticketModel.status = request.status;
                ticketModel.priority = request.priority;
                ticketModel.createdAt = request.createdAt;
                ticketModel.updateAt = request.updateAt;
                ticketModel.description = request.description;
                ticketModel.resolution = request.resolution;
                _context.Add(ticketModel);
                await _context.SaveChangesAsync();
                await contextTransaction.CommitAsync();

                response.isSuccess = true;
                response.message = "Success";

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},EditTicket,Success,{secs}");
            }
            catch (Exception ex)
            {
                await contextTransaction.RollbackAsync();
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},EditTicket,Error,{secs},{ex.Message}");
            }
            return response;
        }
        public async Task<DefaultResponse> AssignTicket(AssignTicketRequest request)
        {
            DefaultResponse response = new DefaultResponse();
            await using var contextTransaction = await _context.Database.BeginTransactionAsync();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                request.Validate();
                tticket ticketDetails = _context.tticket.Where(x => x.id == request.ticketId).FirstOrDefault();
                if (ticketDetails != null)
                {
                    ticketDetails.agentId = request.agentId;
                    _context.Update(ticketDetails);
                    await _context.SaveChangesAsync();
                    await contextTransaction.CommitAsync();
                }
                else
                    throw new Exception("Ticket not found");
                response.isSuccess = true;
                response.message = "Success";

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},AssignTicket,Success,{secs}");
            }
            catch (Exception ex)
            {
                await contextTransaction.RollbackAsync();
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},AssignTicket,Error,{secs},{ex.Message}");
            }
            return response;
        }
        #endregion
        #region Login
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            LoginResponse response = new LoginResponse();

            await using var contextTransaction = await _context.Database.BeginTransactionAsync();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                request.Validate();
                tagent agentDetails = _context.tagent.Where(x => x.email.Equals(request.username)).FirstOrDefault();

                if (agentDetails != null)
                {
                    if (Decrypt(agentDetails.password).Equals(request.password))
                    {
                        if (agentDetails.isConfirmed)
                        {
                            response.token = GenerateToken(agentDetails.email, new Dictionary<string, string>() { ["role"] = "agent" });
                        }
                        else 
                        {
                            if (string.IsNullOrEmpty(request.token))
                            {
                                response.token = GenerateTempPasswordToken(agentDetails.email, request.password, 15);
                            }
                            else 
                            {
                                if (IsTempPasswordTokenValid(request.token))
                                {
                                    agentDetails.password = ConvertPassword(request.password);
                                    agentDetails.isConfirmed = true;

                                    _context.Update(agentDetails);
                                    await _context.SaveChangesAsync();
                                    await contextTransaction.CommitAsync();
                                }
                                else 
                                {
                                    throw new Exception("Invalid username or password.");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!agentDetails.isConfirmed)
                        {
                            if (IsTempPasswordTokenValid(request.token))
                            {
                                agentDetails.password = ConvertPassword(request.password);
                                agentDetails.isConfirmed = true;

                                _context.Update(agentDetails);
                                await _context.SaveChangesAsync();
                                await contextTransaction.CommitAsync();
                            }
                            else
                            {
                                throw new Exception("Invalid username or password.");
                            }
                        }
                        else
                            throw new Exception("Invalid username or password.");
                    }
                }
                else 
                {
                    throw new Exception("Invalid username or password.");
                }
                

                response.isSuccess = true;
                response.message = "Success";

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request.username},Login,Success,{secs}");

            }
            catch (Exception ex)
            {
                await contextTransaction.RollbackAsync();
                response.message = ex.Message;
                response.isSuccess = false;

                sw.Stop();
                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request.username},Login,Error,{secs},{ex.Message}");
            }
            return response;
        }
        #endregion
        #region Stats
        public async Task<GetStatsResponse> GetStats(DefaultRequest request)
        {
            GetStatsResponse response = new GetStatsResponse();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                response.agent = await _context.tagent
    .Select(agent => new StatsDetails
    {
        id = agent.id,
        name = agent.name,
        numberOfCalls = _context.tcall.Count(call => call.agentId == agent.id),
        averageCallDuration = _context.tcall
            .Where(call => call.agentId == agent.id)
            .Select(call => EF.Functions.DateDiffMinute(call.startTime, call.endTime))
            .DefaultIfEmpty(0)
            .Average(),
        numberOfTickets = _context.tticket.Count(ticket => ticket.agentId == agent.id)
    })
    .ToListAsync();

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetStats,Success,{secs}");
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.isSuccess = false;

                double dsecs = Convert.ToDouble(sw.ElapsedMilliseconds) / 1000;
                string secs = dsecs.ToString("0.##");
                Console.WriteLine($"Agent,{request._username},GetStats,Error,{secs},{ex.Message}");
            }
            return response;
        }
        #endregion
        #region Sub Funtions
        public string GenerateTemporaryPassword() 
        {
            string tempPassword = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
            char[] chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            char[] specialChar = "^$*.[]{}()?-!@#%&/,><':;|_`+=".ToCharArray();
            char[] number = "0123456789".ToCharArray();
            Random random = new Random();
            StringBuilder tempPassBuilder = new StringBuilder(tempPassword);
            tempPassBuilder[3] = number[random.Next(number.Length - 1)];
            tempPassBuilder[5] = chars[random.Next(chars.Length - 1)];
            tempPassBuilder[6] = chars[random.Next(chars.Length - 1)].ToString().ToUpper()[0];
            tempPassBuilder[7] = specialChar[random.Next(specialChar.Length - 1)];
            return tempPassBuilder.ToString();
        }
        public static string ConvertPassword(string password)
        {
            // Generate a salt
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // Ensure the key is 256 bits (32 bytes)
            if (keyBytes.Length != KeySize)
            {
                throw new ArgumentException("Key must be 256 bits (32 characters).");
            }

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Generate a random IV
                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(password);
                    byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    // Combine IV and cipher text
                    byte[] result = new byte[iv.Length + cipherBytes.Length];
                    Array.Copy(iv, 0, result, 0, iv.Length);
                    Array.Copy(cipherBytes, 0, result, iv.Length, cipherBytes.Length);

                    return Convert.ToBase64String(result);
                }
            }
        }
        public static string Decrypt(string encryptedText)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            // Ensure the key is 256 bits (32 bytes)
            if (keyBytes.Length != KeySize)
            {
                throw new ArgumentException("Key must be 256 bits (32 characters).");
            }

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Extract the IV from the first 16 bytes
                byte[] iv = new byte[IvSize];
                Array.Copy(encryptedBytes, 0, iv, 0, iv.Length);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] cipherText = new byte[encryptedBytes.Length - iv.Length];
                    Array.Copy(encryptedBytes, iv.Length, cipherText, 0, cipherText.Length);

                    byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
        public string GenerateTempPasswordToken(string username, string tempPassword, int expireMinutes = 15)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenClaims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim("tempPassword", tempPassword), // Store the temporary password in the claim
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

            var token = new JwtSecurityToken(
                issuer: _jwt.Value.Issuer,
                audience: _jwt.Value.Audience,
                claims: tokenClaims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public bool IsTempPasswordTokenValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Value.Key));

            try
            {
                // Validate the token
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero // No tolerance for expiration
                }, out var validatedToken);

                // Check if the token is a valid JwtSecurityToken and contains the "tempPassword" claim
                var jwtToken = validatedToken as JwtSecurityToken;
                if (jwtToken == null || !jwtToken.Claims.Any(c => c.Type == "tempPassword"))
                {
                    return false; // Token is invalid if the "tempPassword" claim is missing
                }

                // If needed, you can also perform additional checks here (e.g., expiration, issuer)
                var expiration = jwtToken.ValidTo;
                if (expiration < DateTime.UtcNow)
                {
                    return false; // Token has expired
                }

                // The token is valid
                return true;
            }
            catch (Exception)
            {
                // If any exception is thrown, the token is not valid
                return false;
            }
        }
        public string GenerateToken(string username, Dictionary<string, string> claims, int expireMinutes = 30)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenClaims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
        };

            foreach (var claim in claims)
            {
                tokenClaims.Add(new Claim(claim.Key, claim.Value));
            }

            var token = new JwtSecurityToken(
                issuer: _jwt.Value.Issuer,
                audience: _jwt.Value.Audience,
                claims: tokenClaims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
