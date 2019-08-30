using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rent.ContextPoint;
using Rent.ContextPoint.Exceptions;
using Rent.Data;
using Rent.EmailTemplates;
using Rent.Helpers;
using Rent.Models;
using Rent.Repositories.TimePlanning;

namespace Rent.Repositories
{
    public class CustomerRepository
    {
        private readonly IRoleAuthenticationRepository _roleRepo;

        private readonly CustomerContext _customerContext;
        private readonly UserContext _userContext;
        private readonly LocationContext _locationContext;

        private readonly RentContext _rentContext;
        private readonly DocumentRepository _documentRepository;

        private readonly PermissionRepository _permissionRepository;
        private readonly LoginContext _loginContext;

        private readonly NotificationRepository _notificationRepository;
        private readonly PropCondition _propCondition;

        public CustomerRepository(IRoleAuthenticationRepository roleRepo,
            CustomerContext customerContext,
                                  UserContext userContext,
                                  LocationContext locationContext,
                                  RentContext rentContext,
                                  DocumentRepository documentRepository,
                                  PermissionRepository permissionRepository,
                                  LoginContext loginContext,
                                  NotificationRepository notificationRepository,
                                  PropCondition propCondition)
        {
            _roleRepo = roleRepo;

            _customerContext = customerContext;
            _userContext = userContext;
            _locationContext = locationContext;

            _rentContext = rentContext;
            _documentRepository = documentRepository;

            _permissionRepository = permissionRepository;
            _loginContext = loginContext;
            _notificationRepository = notificationRepository;
            _propCondition = propCondition;
        }

        public dynamic Get(int requester, int id)
        {
            return _customerContext.DetailedOne(requester, c => c.ID == id, "KeyAccountManager", "MainUser");
        }

        public IEnumerable<dynamic> GetAll(int requester)
        {
            return _customerContext.BasicOrdered(requester, c => true, (q) => q.OrderBy(c => c.Name));
        }

        public string CustomerName(int requester, int customerId)
        {
            return _customerContext.DatabaseOne(0, c => c.ID == customerId)?.Name;
        }

        public dynamic GetUsersCustomer(int requester, int userId)
        {
            var user = _userContext.DatabaseOne(requester, u => u.ID == userId);
            return _customerContext.DetailedOne(requester, c => c.ID == user.CustomerID);
        }

        public async Task Disable(int requester, int customerId)
        {
            await _customerContext.Update(requester, c => c.ID == customerId, c => { c.Disabled = true; });
            try
            {
                await _userContext.Update(requester, u => u.CustomerID == customerId, u => { u.Disabled = true; });
                await _locationContext.Update(requester, l => l.CustomerID == customerId, l => { l.Disabled = true; });
            }
            catch (NothingUpdatedException e) { }
        }

        public async Task Enable(int requester, int customerId)
        {
            await _customerContext.Update(requester, c => c.ID == customerId, c => { c.Disabled = false; });
        }

        public async Task<int> Create(int requester, Customer newCustomer)
        {
            if (_roleRepo.IsAdmin(requester))
            {
                var conversation = new Conversation { Title = newCustomer.Name, Open = false };
                _rentContext.Conversation.Add(conversation);
                await _rentContext.SaveChangesAsync();

                var customer = new Customer()
                {
                    ID = newCustomer.ID,
                    Name = newCustomer.Name,
                    ImageLocation = newCustomer.ImageLocation,
                    Comment = newCustomer.Comment,
                    KeyAccountManagerID = newCustomer.KeyAccountManagerID,
                    ConversationID = conversation.ID,
                    Created = DateTimeHelpers.GmtPlusOneDateTime(),
                    GeneralFolderID = await _documentRepository.GetCustomerGeneralFolder(requester),
                    PrivateFolderID = await _documentRepository.GetCustomerPrivateFolder(requester),
                    ManagementFolderID = await _documentRepository.GetCustomerManagementFolder(requester),
                    HasStandardFolders = true,

                };

                _rentContext.Customer.Add(customer);
                await _rentContext.SaveChangesAsync();

                return customer.ID;
            }
            throw new UnauthorizedAccessException();
        }

        public async Task Update(int requester, int customerId, Customer updateCustomer)
        {
            Console.WriteLine(updateCustomer == null);
            Console.WriteLine(updateCustomer);

            await _customerContext.Update(requester, c => c.ID == customerId, c =>
            {
                c.Name = updateCustomer.Name;
                c.Comment = updateCustomer.Comment;
                //c.Status = updateCustomer.Status;
                c.MainUserID = updateCustomer.MainUserID;
                c.KeyAccountManagerID = updateCustomer.KeyAccountManagerID;
                c.ImageLocation = updateCustomer.ImageLocation;
                //c.HasStandardFolders = updateCustomer.HasStandardFolders;
            });
            Console.WriteLine("Done?");
        }

        public async Task Invite(int requester, int customerId, string standardPassword)
        {
            if (_permissionRepository.Unauthorized(requester, "Customer", CRUDD.Create))
                throw new UnauthorizedAccessException();

            string header = "Velkommen til Rengøringsselskabet Rent's nye samarbejdsplatform.";
            foreach (var user in _userContext.Database(0, u => u.CustomerID == customerId))
            {
                var login = _loginContext.GetOne(0, l => l.ID == user.LoginID);

                var newPassword = !string.IsNullOrEmpty(standardPassword) ? standardPassword : RandomString(6);

                await _loginContext.Update(0, l => l.ID == user.LoginID, l =>
                {
                    login.Password = LoginRepository.HashPassword(newPassword);
                });
                await _notificationRepository.SendEmails(new List<User> { user }, header, CreateInviteEmail.Build(login.UserName, newPassword));
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}