using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent.ContextPoint.Exceptions;
using Rent.Data;
using Rent.Models;
using Rent.Models.Projects;
using Rent.Models.TimePlanning;
using Rent.Repositories.TimePlanning;

namespace Rent.Repositories
{
    public class ProjectRepository
    {
        private readonly RentContext _context;
        private readonly IRoleAuthenticationRepository _roleRepo;
        private readonly UserRepository _userRepo;
        private readonly ProjectRoleRepository _projectRoleRepository;
        private readonly FirestoreCommunicationRepository _firestoreRepository;

        public ProjectRepository(RentContext context, IRoleAuthenticationRepository roleRepo, UserRepository userRepo, ProjectRoleRepository projectRoleRepository, FirestoreCommunicationRepository firestoreRepository)
        {
            _context = context;
            _roleRepo = roleRepo;
            _userRepo = userRepo;
            _projectRoleRepository = projectRoleRepository;
            _firestoreRepository = firestoreRepository;
        }

        public async Task<int> Create(int requester, int? parentId, string name)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                string path = null;
                if (parentId != null)
                {
                    var parent = _context.Project.FirstOrDefault(p => p.ParentID == parentId);

                    if (parent != null)
                    {
                        if (parent.ParentID == null)
                        {
                            // Parent is root - do nothing
                        }
                        else if (string.IsNullOrEmpty(parent.Path))
                        {
                            // Parent is child of root and has no path, path starts here
                            path = parent.Name;
                        }
                        else
                        {
                            // Parent has a path and this is extended in the following way...
                            path = parent.Path + "*/*" + parent.Name;
                        }
                    }

                }
                var project = new Project { Name = name, ParentID = parentId, Path = path };
                _context.Project.Add(project);
                await _context.SaveChangesAsync();
                return project.ID;
            }
            else
                throw new UnauthorizedAccessException();
        }

        public async Task Update(int requester, int projectId, Project project)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                var projectToUpdate = _context.Project.Find(projectId);
                if (projectToUpdate == null) throw new NotFoundException();
                projectToUpdate.Name = project.Name;
                _context.Project.Update(projectToUpdate);
                await _context.SaveChangesAsync();
                await UpdateProjectsOfProject(projectId, projectToUpdate.Path);
            }
            else
                throw new UnauthorizedAccessException();
        }



        public async Task AddProjectsToUsers(int requester, int userId, ICollection<int> projectIds)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                var user = _context.User.Find(userId);
                if (user == null) throw new NotFoundException();

                var oldProjectConversations = _projectRoleRepository.GetConversationsOfUser(userId).ToList();

                var projectUsers = projectIds.Select(pId => new ProjectUser
                {
                    ProjectID = pId,
                    UserID = userId,
                });

                _context.ProjectUser.AddRange(projectUsers);
                await _context.SaveChangesAsync();
                _projectRoleRepository.UpdateProjectItemUsersFromUserID(userId);

                var newProjectConversations = _projectRoleRepository.GetConversationsOfUser(userId).ToList();
                await _firestoreRepository.UpdateUserConversations(user, oldProjectConversations, newProjectConversations);
            }
            else
                throw new UnauthorizedAccessException();
        }

        public async Task RemoveProjectsFromUser(int requester, int userId, ICollection<int> projectIds)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                var user = _context.User.Find(userId);
                if (user == null) throw new NotFoundException();

                var oldProjectConversations = _projectRoleRepository.GetConversationsOfUser(userId).ToList();

                var projectUser = _context.ProjectUser
                    .Where(pu => pu.UserID == userId)
                    .Where(pu => projectIds.Contains(pu.ProjectID));
                _context.ProjectUser.RemoveRange(projectUser);
                await _context.SaveChangesAsync();
                _projectRoleRepository.UpdateProjectItemUsersFromUserID(userId);

                var newProjectConversations = _projectRoleRepository.GetConversationsOfUser(userId).ToList();
                await _firestoreRepository.UpdateUserConversations(user, oldProjectConversations, newProjectConversations);
            }
            else
                throw new UnauthorizedAccessException();
        }

        public async Task UpdateProjectItemAccess(int requester, int projectItemId, ICollection<ProjectItemAccess> updatedProjectItemAccesses)
        {
            var projectRole = _context.User.Where(u => u.ID == requester).Select(u => u.ProjectRole).FirstOrDefault();

            if (projectRole == null || !projectRole.HasAllPermissions) throw new UnauthorizedAccessException();

            var projectItem = _context.ProjectItem.Where(pi => pi.ID == projectItemId).Include(pi => pi.FirestoreConversations).FirstOrDefault();
            if (projectItem == null) throw new NotFoundException();

            // FIND USERS WHO CAN SEE THE PROJECT-ITEM-FIRESTORE-CONVERSATION BEFORE THE UPDATE
            List<User> oldUsers = new List<User>();
            if (projectItem.ProjectItemType == ProjectItemType.FirestoreConversations && projectItem.FirestoreConversations != null)
                oldUsers = _context.User.Where(u => u.ProjectItemUsers.Any(piu => piu.ProjectItemID == projectItemId)).ToList();

            var projectItemAccesses = _context.ProjectItemAccess.Where(pia => pia.ProjectItemID == projectItemId).ToList();

            projectItemAccesses.ForEach((pia) =>
            {
                var changedVersion = updatedProjectItemAccesses.FirstOrDefault(u => u.ProjectItemID == pia.ProjectItemID && u.ProjectRoleID == pia.ProjectRoleID);
                if (changedVersion != null)
                {
                    pia.Read = changedVersion.Read;
                    pia.Write = changedVersion.Write;
                }
            });

            _context.ProjectItemAccess.UpdateRange(projectItemAccesses);
            await _context.SaveChangesAsync();
            _projectRoleRepository.UpdateProjectItemUsersFromProjectItemID(projectItemId);

            // UPDATE FIRESTORE CONVERSATION
            if (projectItem.ProjectItemType == ProjectItemType.FirestoreConversations && projectItem.FirestoreConversations != null)
            {
                var newUsers = _context.User.Where(u => u.ProjectItemUsers.Any(piu => piu.ProjectItemID == projectItemId)).ToList();
                await _firestoreRepository.UpdateConversationUsers(projectItem.FirestoreConversations, oldUsers, newUsers);
            }

        }


        private async Task<int> AddProjectItem(int requester, int projectId, ProjectItemType itemType, ICollection<ProjectItemAccess> specialAccesses = null)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                var accesses = _context.ProjectItemAccessTemplate
                    .Where(pt => pt.ProjectItemType == itemType).ToList()
                    .Select(pt => new ProjectItemAccess
                    {
                        ProjectRoleID = pt.ProjectRoleID,
                        Read = (specialAccesses?.FirstOrDefault(sa => sa.ProjectRoleID == pt.ProjectRoleID)?.Read) ?? pt.Read,
                        Write = (specialAccesses?.FirstOrDefault(sa => sa.ProjectRoleID == pt.ProjectRoleID)?.Write) ?? pt.Write,
                    }).ToList();

                var projectItem = new ProjectItem
                {
                    ProjectID = projectId,
                    ProjectItemType = itemType,
                    //Access = ApprovedAccessString(requester, access),
                    ProjectItemAccesses = accesses,
                };
                _context.ProjectItem.Add(projectItem);
                await _context.SaveChangesAsync();
                _projectRoleRepository.UpdateProjectItemUsersFromProjectItemID(projectItem.ID);

                return projectItem.ID;
            }
            else
                throw new UnauthorizedAccessException();
        }

        private bool DoesProjectContainItem(int projectId, ProjectItemType itemType)
        {
            var existingItemsCount = _context.ProjectItem
                .Where(pi => pi.ProjectID == projectId && pi.ProjectItemType == itemType).Count();
            return existingItemsCount > 0;
        }

        public async Task<int> AddAddress(int requester, int projectId, ICollection<ProjectItemAccess> specialAccesses = null)
        {
            if (!DoesProjectContainItem(projectId, ProjectItemType.Address))
            {
                var piId = await AddProjectItem(requester, projectId, ProjectItemType.Address, specialAccesses);
                var address = new Address { ProjectItemID = piId };
                _context.Address.Add(address);
                await _context.SaveChangesAsync();
                return piId;
            }
            throw new UnauthorizedAccessException();
        }

        public async Task<int> AddComment(int requester, int projectId, ICollection<ProjectItemAccess> specialAccesses = null)
        {
            //if (!DoesProjectContainItem(projectId, ProjectItemType.Comment))
            //{
            var piId = await AddProjectItem(requester, projectId, ProjectItemType.Comment, specialAccesses);
            var comment = new Comment { ProjectItemID = piId };
            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();
            return piId;
            //}
            //throw new UnauthorizedAccessException();
        }

        public async Task<int> AddLocation(int requester, int projectId, ICollection<ProjectItemAccess> specialAccesses = null)
        {
            if (!DoesProjectContainItem(projectId, ProjectItemType.Location))
                return await AddProjectItem(requester, projectId, ProjectItemType.Location, specialAccesses);
            throw new UnauthorizedAccessException();
        }

        public async Task<int> AddClient(int requester, int projectId, ICollection<ProjectItemAccess> specialAccesses = null)
        {
            if (!DoesProjectContainItem(projectId, ProjectItemType.Client))
            {
                var piId = await AddProjectItem(requester, projectId, ProjectItemType.Client, specialAccesses);
                var client = new Client
                {
                    ProjectItemID = piId
                };
                _context.Client.Add(client);
                await _context.SaveChangesAsync();

                return piId;
            }
            throw new UnauthorizedAccessException();
        }

        public async Task<int> AddProfileImage(int requester, int projectId, ICollection<ProjectItemAccess> specialAccesses = null)
        {
            if (!DoesProjectContainItem(projectId, ProjectItemType.ProfileImage))
                return await AddProjectItem(requester, projectId, ProjectItemType.ProfileImage, specialAccesses);
            throw new UnauthorizedAccessException();
        }

        public async Task<int> AddLogs(int requester, int projectId, ICollection<ProjectItemAccess> specialAccesses = null)
            => await AddProjectItem(requester, projectId, ProjectItemType.Logs, specialAccesses);

        public async Task<int> AddWork(int requester, int projectId, ICollection<ProjectItemAccess> specialAccesses = null)
            => await AddProjectItem(requester, projectId, ProjectItemType.Work, specialAccesses);

        public async Task<int> AddTasks(int requester, int projectId, ICollection<ProjectItemAccess> specialAccesses = null)
            => await AddProjectItem(requester, projectId, ProjectItemType.Tasks, specialAccesses);

        public async Task<int> AddQualityReports(int requester, int projectId, ICollection<ProjectItemAccess> specialAccesses = null)
            => await AddProjectItem(requester, projectId, ProjectItemType.QualityReports, specialAccesses);

        public async Task<int> AddDocumentFolders(int requester, int projectId, string title, ICollection<ProjectItemAccess> specialAccesses = null)
        {
            var piId = await AddProjectItem(requester, projectId, ProjectItemType.DocumentFolders, specialAccesses);
            var documentFolder = new DocumentFolder
            {
                ProjectItemID = piId,
                Title = title,
                Standard = false,
                Removable = true,
                HasParentPermissions = false,
                VisibleToAllRoles = true,
            };
            _context.DocumentFolder.Add(documentFolder);
            await _context.SaveChangesAsync();

            return piId;
        }

        public async Task<int> AddPost(int requester, int projectId, string title, string body, ICollection<ProjectItemAccess> specialAccesses = null)
        {
            var piId = await AddProjectItem(requester, projectId, ProjectItemType.Post, specialAccesses);
            var post = new Post
            {
                ProjectItemID = piId,
                Title = title,
                Body = body,
                UserId = requester,
                SendTime = DateTime.Now,
            };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return piId;
        }

        public async Task<int> AddFirestoreConversation(int requester, int projectId, string title, ICollection<ProjectItemAccess> specialAccesses = null)
        {
            var piId = await AddProjectItem(requester, projectId, ProjectItemType.FirestoreConversations, specialAccesses);
            var users = _context.User.Where(u => u.ProjectItemUsers.Any(piu => piu.ProjectItemID == piId)).ToList();
            var firestoreConversationId = await _firestoreRepository.CreateConversation(requester, true, title, users);
            var firestoreConversation = new FirestoreConversation
            {
                ConversationID = firestoreConversationId,
                ProjectItemID = piId,
            };
            _context.FirestoreConversation.Add(firestoreConversation);
            await _context.SaveChangesAsync();

            return piId;
        }

        public dynamic Get(int requester, int projectId)
        {
            var project = _context.Project.Where(p => p.ID == projectId).Select(Project.DetailedDTO()).FirstOrDefault();
            if (project == null) throw new NotFoundException();
            return project;
        }

        public IEnumerable<dynamic> GetAll(int requester)
        {
            var projects = _projectRoleRepository.GetProjectsOfUser(requester).Select(Project.StandardDTO());
            return projects;
        }

        public IEnumerable<dynamic> GetProjectsOfProject(int requester, int projectId)
        {
            var projects = _projectRoleRepository.GetProjectsOfUser(requester).Where(p => p.ParentID == projectId)
                .Select(Project.StandardDTO());
            return projects;
        }

        public IEnumerable<dynamic> GetProjectItemsOfProject(int requester, int projectId)
        {

            var projectRole = _context.User.Where(u => u.ID == requester).Select(u => u.ProjectRole).FirstOrDefault();

            if (projectRole == null) throw new UnauthorizedAccessException();

            var projects = _context.ProjectItem.Where(p => p.ProjectID == projectId)
                .Where(pi => projectRole.HasAllPermissions || pi.ProjectItemAccesses.Any(pia => pia.ProjectRoleID == projectRole.ID && pia.Read))
                .Select(ProjectItem.StandardDTO());
            return projects;
        }

        public IEnumerable<dynamic> GetDetailedProjectItemsOfProject(int requester, int projectId)
        {
            var projectRole = _context.User.Where(u => u.ID == requester).Select(u => u.ProjectRole).FirstOrDefault();

            if (projectRole == null || !projectRole.HasAllPermissions) throw new UnauthorizedAccessException();

            var projects = _context.ProjectItem.Where(p => p.ProjectID == projectId)
                .Select(ProjectItem.DetailedDTO());
            return projects;
        }

        public IEnumerable<dynamic> GetProjectsOfUser(int requester, int userId)
        {
            var projects = _context.ProjectUser.Where(pu => pu.UserID == userId)
                .Select(pu => pu.Project)
                .Select(Project.StandardDTO());
            return projects;
        }

        public IEnumerable<dynamic> GetProjectsOfNotUser(int requester, int userId)
        {
            var projects = _context.Project.Where(p => p.ProjectUsers
                .All(pu => pu.UserID != userId))
                .Select(Project.StandardDTO());
            return projects;
        }


        public async Task FIX_Users_Roles()
        {
            var users = _context.User.ToList().Select((user) =>
            {
                if (user.RoleID == 1 || user.RoleID == 2)
                {
                    user.UserRole = "Admin";
                    user.ProjectRoleID = 1;
                }
                if (user.RoleID == 3 || user.RoleID == 4)
                {
                    user.UserRole = "Manager";
                    user.ProjectRoleID = 2;
                }
                if (user.RoleID == 5 || user.RoleID == 6 || user.RoleID == 7)
                {
                    user.UserRole = "User";
                    user.ProjectRoleID = 3;
                }
                if (user.RoleID == 9)
                {
                    user.UserRole = "ClientAdmin";
                    user.ProjectRoleID = 4;
                }
                if (user.RoleID == 8)
                {
                    user.UserRole = "ClientManager";
                    user.ProjectRoleID = 5;
                }


                return user;
            });
            _context.User.UpdateRange(users);
            await _context.SaveChangesAsync();
        }

        public async Task FIX()
        {
            await FIX_Users_Roles();

            Console.WriteLine("1");
            var oldCustomer = _context.Customer.Where(c => c.ProjectItemID != null).ToList();
            oldCustomer.ForEach(c => c.ProjectItemID = null);
            Console.WriteLine("2");
            var oldLocation = _context.Location.Where(c => c.ProjectItemID != null).ToList();
            oldLocation.ForEach(c => c.ProjectItemID = null);
            Console.WriteLine("3");
            var oldCleaningTask = _context.CleaningTask.Where(c => c.ProjectItemID != null).ToList();
            oldCleaningTask.ForEach(c => c.ProjectItemID = null);
            Console.WriteLine("4");
            var oldDocumentFolder = _context.DocumentFolder.Where(c => c.ProjectItemID != null).ToList();
            oldDocumentFolder.ForEach(c => c.ProjectItemID = null);
            Console.WriteLine("5");
            var oldLocationLog = _context.LocationLog.Where(c => c.ProjectItemID != null).ToList();
            oldLocationLog.ForEach(c => c.ProjectItemID = null);
            Console.WriteLine("6");
            var oldQualityReport = _context.QualityReport.Where(c => c.ProjectItemID != null).ToList();
            oldQualityReport.ForEach(c => c.ProjectItemID = null);
            Console.WriteLine("7");

            _context.Customer.UpdateRange(oldCustomer);
            _context.Location.UpdateRange(oldLocation);
            _context.CleaningTask.UpdateRange(oldCleaningTask);
            _context.DocumentFolder.UpdateRange(oldDocumentFolder);
            _context.LocationLog.UpdateRange(oldLocationLog);
            _context.QualityReport.UpdateRange(oldQualityReport);
            _context.Comment.RemoveRange(_context.Comment.ToList());
            Console.WriteLine("8");
            await _context.SaveChangesAsync();

            Console.WriteLine("Looking for old");

            var oldProjects = _context.Project.ToList();
            var oldProjectItems = _context.ProjectItem.ToList();
            var oldProjectUsers = _context.ProjectUser.ToList();
            var oldProjectItemAccess = _context.ProjectItemAccess.ToList();

            Console.WriteLine("Found " + oldProjects.Count + " old projects");
            Console.WriteLine("Found " + oldProjectItems.Count + " old projectItems");
            Console.WriteLine("Found " + oldProjectUsers.Count + " old projectUsers");

            while (oldProjectItemAccess.Count > 0)
            {
                var toRemove = oldProjectItemAccess.Take(100);
                _context.ProjectItemAccess.RemoveRange(toRemove);
                await _context.SaveChangesAsync();

                oldProjectItemAccess.RemoveAll(i => toRemove.Contains(i));
            }

            while (oldProjectUsers.Count > 0)
            {
                var toRemove = oldProjectUsers.Take(100);
                _context.ProjectUser.RemoveRange(toRemove);
                await _context.SaveChangesAsync();

                oldProjectUsers.RemoveAll(i => toRemove.Contains(i));
            }


            Console.WriteLine("10");
            while (oldProjectItems.Count > 0)
            {
                var toRemove = oldProjectItems.Take(100);
                _context.ProjectItem.RemoveRange(toRemove);
                await _context.SaveChangesAsync();

                oldProjectItems.RemoveAll(i => toRemove.Contains(i));
            }

            Console.WriteLine("11");
            while (oldProjects.Count > 0)
            {
                var toRemove = oldProjects.Take(100);
                _context.Project.RemoveRange(toRemove);
                await _context.SaveChangesAsync();

                oldProjects.RemoveAll(i => toRemove.Contains(i));
            }
            Console.WriteLine("12");



            var projectRoles = _context.ProjectRole.Where(pr => !pr.HasAllPermissions).ToList();

            var projectItemAccessTemplates = new List<ProjectItemAccessTemplate>();

            projectRoles.ForEach((ProjectRole projectRole) =>
            {
                foreach (ProjectItemType type in Enum.GetValues(typeof(ProjectItemType)))
                {
                    var read = projectRole.Name == "Manager";
                    var write = projectRole.Name == "Manager";
                    if (type == ProjectItemType.ProfileImage)
                        read = true;
                    if (type == ProjectItemType.Work)
                        read = true;
                    if (type == ProjectItemType.Client)
                        read = true;
                    if (type == ProjectItemType.Location)
                        read = true;
                    if (type == ProjectItemType.QualityReports && projectRole.Name == "ClientAdmin")
                        read = true;

                    var template = new ProjectItemAccessTemplate
                    {
                        ProjectItemType = type,
                        ProjectRoleID = projectRole.ID,
                        Read = read,
                        Write = write,
                    };

                    projectItemAccessTemplates.Add(template);
                }
            });

            _context.ProjectItemAccessTemplate.AddRange(projectItemAccessTemplates);
            await _context.SaveChangesAsync();

            var rentId = await Create(0, null, "Rent");

            var customers = _context.Customer.ToList();

            var cleaningPlans = _context.CleaningPlan.ToList();

            var rootDocuments = _context.DocumentFolder.Where((df) => df.Standard == true).ToList();

            for (int rd = 0; rd < rootDocuments.Count; rd++)
            {
                var rootDocumentFolderId = await AddProjectItem(0, rentId, ProjectItemType.DocumentFolders,
                    new List<ProjectItemAccess> {
                    new ProjectItemAccess { ProjectRoleID = 3, Read = true },
                    new ProjectItemAccess { ProjectRoleID = 4, Read = true },
                    new ProjectItemAccess { ProjectRoleID = 5, Read = true },
                });//"RWRWR-R-R-");
                rootDocuments[rd].ProjectItemID = rootDocumentFolderId;
            }
            _context.DocumentFolder.UpdateRange(rootDocuments);
            await _context.SaveChangesAsync();

            for (int c = 0; c < customers.Count; c++)
            {
                var customer = customers[c];
                Console.WriteLine("Customer: " + customer.Name);
                // create a project for the customer
                var customerProjectId = await Create(0, rentId, customer.Name);



                // add a ProjectItem-comment to the customer-project
                var customerCommentItem = await AddComment(0, customerProjectId);
                // add a Comment pointing to the ProjectItem-comment
                _context.Comment.Add(new Comment { ProjectItemID = customerCommentItem, Title = "", Body = customer.Comment });
                //await _context.SaveChangesAsync();

                // add a ProjectItem-client to the customer-project
                var customerItemId = await AddClient(0, customerProjectId);
                var client = _context.Client.FirstOrDefault(cli => cli.ProjectItemID == customerItemId);

                var allCustomerUsers = _context.User.Where(u => u.CustomerID == customer.ID).ToList();
                allCustomerUsers.ForEach(acu => acu.ClientID = client.ID);
                _context.User.UpdateRange(allCustomerUsers);

                // update the customer to point to the ProjectItem-client
                customer.ProjectItemID = customerItemId;
                _context.Customer.Update(customer);
                //await _context.SaveChangesAsync();

                // find users attached to the customer
                var customerUsers = new HashSet<int?> {
                    customer.KeyAccountManagerID,
                    customer.MainUserID,
                    customer.SalesRepID,
                }.Where(i => i != null).Cast<int>().ToList();

                // add users as projectUsers to the project
                await _userRepo.AddUsersToProject(0, customerProjectId, customerUsers.ToList());

                // create ProjectItem-documentFolder for each customer documentFolder
                var generalFolderItemId = await AddProjectItem(0, customerProjectId, ProjectItemType.DocumentFolders,
                    new List<ProjectItemAccess> {
                        new ProjectItemAccess { ProjectRoleID = 3, Read = true },
                        new ProjectItemAccess { ProjectRoleID = 4, Read = true },
                        new ProjectItemAccess { ProjectRoleID = 5, Read = true },
                    });
                var privateFolderItemId = await AddProjectItem(0, customerProjectId, ProjectItemType.DocumentFolders,
                    new List<ProjectItemAccess> {
                        new ProjectItemAccess { ProjectRoleID = 2, Read = false, Write = false },
                        new ProjectItemAccess { ProjectRoleID = 3, Read = false, Write = false },
                        new ProjectItemAccess { ProjectRoleID = 4, Read = true, Write = true },
                        new ProjectItemAccess { ProjectRoleID = 5, Read = true, Write = true },
                    });
                var managementFolderItemId = await AddProjectItem(0, customerProjectId, ProjectItemType.DocumentFolders,
                    new List<ProjectItemAccess> {
                        new ProjectItemAccess { ProjectRoleID = 3, Read = false, Write = false },
                        new ProjectItemAccess { ProjectRoleID = 4, Read = true, Write = true },
                    });

                // find documentFolders attached to the customer
                var customerDocuments = _context.DocumentFolder
                    .Where(df =>
                    df.ID == customer.GeneralFolderID ||
                    df.ID == customer.ManagementFolderID ||
                    df.ID == customer.PrivateFolderID).ToList();

                // update the ProjectItemID to point to their respective ProjectItem-documentFolders
                customerDocuments.Where(df => df.ID == customer.GeneralFolderID).ToList()
                    .ForEach(df => df.ProjectItemID = generalFolderItemId);
                customerDocuments.Where(df => df.ID == customer.PrivateFolderID).ToList()
                    .ForEach(df => df.ProjectItemID = privateFolderItemId);
                customerDocuments.Where(df => df.ID == customer.ManagementFolderID).ToList()
                    .ForEach(df => df.ProjectItemID = managementFolderItemId);

                // update the documentFolders 
                _context.DocumentFolder.UpdateRange(customerDocuments);
                await _context.SaveChangesAsync();

                // find all locations of the customer
                var locations = _context.Location.Where(l => l.CustomerID == customer.ID).ToList();

                for (int l = 0; l < locations.Count; l++)
                {
                    var location = locations[l];
                    Console.WriteLine("Location: " + location.Name);

                    // create a project for the location
                    var locationProjectId = await Create(0, customerProjectId, location.Name);

                    // add a ProjectItem-comment to the customer-project
                    var locationCommentItem = await AddComment(0, locationProjectId);
                    // add a Comment pointing to the ProjectItem-comment
                    _context.Comment.Add(new Comment { ProjectItemID = locationCommentItem, Title = "", Body = location.Comment });
                    //await _context.SaveChangesAsync();

                    // add a ProjectItem-location to the location-project
                    var locationItemId = await AddLocation(0, locationProjectId);

                    // update the location to point to the ProjectItem-location
                    location.ProjectItemID = locationItemId;
                    _context.Location.Update(location);
                    //await _context.SaveChangesAsync();

                    // create a ProjectItem-documentFolder
                    var GeneralFolderID = await AddProjectItem(0, locationProjectId, ProjectItemType.DocumentFolders,
                        new List<ProjectItemAccess> {
                            new ProjectItemAccess { ProjectRoleID = 3, Read = true },
                            new ProjectItemAccess { ProjectRoleID = 4, Read = true },
                            new ProjectItemAccess { ProjectRoleID = 5, Read = true },
                        });
                    var CleaningplanFolderID = await AddProjectItem(0, locationProjectId, ProjectItemType.DocumentFolders,
                        new List<ProjectItemAccess> {
                            new ProjectItemAccess { ProjectRoleID = 3, Read = true },
                            new ProjectItemAccess { ProjectRoleID = 4, Read = true },
                            new ProjectItemAccess { ProjectRoleID = 5, Read = true },
                        });
                    var CleaningFolderID = await AddProjectItem(0, locationProjectId, ProjectItemType.DocumentFolders,
                        new List<ProjectItemAccess> {
                            new ProjectItemAccess { ProjectRoleID = 3, Read = true },
                            new ProjectItemAccess { ProjectRoleID = 4, Read = true },
                            new ProjectItemAccess { ProjectRoleID = 5, Read = true },
                        });
                    var WindowFolderID = await AddProjectItem(0, locationProjectId, ProjectItemType.DocumentFolders,
                        new List<ProjectItemAccess> {
                            new ProjectItemAccess { ProjectRoleID = 3, Read = true },
                            new ProjectItemAccess { ProjectRoleID = 4, Read = true },
                            new ProjectItemAccess { ProjectRoleID = 5, Read = true },
                        });
                    var FanCoilFolderID = await AddProjectItem(0, locationProjectId, ProjectItemType.DocumentFolders,
                        new List<ProjectItemAccess> {
                            new ProjectItemAccess { ProjectRoleID = 3, Read = true },
                            new ProjectItemAccess { ProjectRoleID = 4, Read = true },
                            new ProjectItemAccess { ProjectRoleID = 5, Read = true },
                        });
                    var PeriodicFolderID = await AddProjectItem(0, locationProjectId, ProjectItemType.DocumentFolders,
                        new List<ProjectItemAccess> {
                            new ProjectItemAccess { ProjectRoleID = 3, Read = true },
                            new ProjectItemAccess { ProjectRoleID = 4, Read = true },
                            new ProjectItemAccess { ProjectRoleID = 5, Read = true },
                        });

                    // find documentFolders attached to the location
                    var locationDocuments = _context.DocumentFolder
                        .Where(df =>
                        df.ID == location.GeneralFolderID ||
                        df.ID == location.CleaningplanFolderID ||
                        df.ID == location.CleaningFolderID ||
                        df.ID == location.WindowFolderID ||
                        df.ID == location.FanCoilFolderID ||
                        df.ID == location.PeriodicFolderID
                        ).ToList();

                    // update the ProjectItemID to point to their respective ProjectItem-documentFolders
                    locationDocuments.Where(df => df.ID == location.GeneralFolderID).ToList()
                        .ForEach(df => df.ProjectItemID = GeneralFolderID);

                    locationDocuments.Where(df => df.ID == location.CleaningplanFolderID).ToList()
                        .ForEach(df => df.ProjectItemID = CleaningplanFolderID);

                    locationDocuments.Where(df => df.ID == location.CleaningFolderID).ToList()
                        .ForEach(df => df.ProjectItemID = CleaningFolderID);

                    locationDocuments.Where(df => df.ID == location.WindowFolderID).ToList()
                        .ForEach(df => df.ProjectItemID = WindowFolderID);

                    locationDocuments.Where(df => df.ID == location.FanCoilFolderID).ToList()
                        .ForEach(df => df.ProjectItemID = FanCoilFolderID);

                    locationDocuments.Where(df => df.ID == location.PeriodicFolderID).ToList()
                        .ForEach(df => df.ProjectItemID = PeriodicFolderID);

                    // update the documentFolders 
                    _context.DocumentFolder.UpdateRange(locationDocuments);
                    //await _context.SaveChangesAsync();

                    // Find all users who are either SL (3) or LM (8) at the location
                    var locationUserIds = _context.LocationUser
                        .Where(lu => lu.LocationID == location.ID)
                        .Where(lu => lu.User.RoleID == 3 || lu.User.RoleID == 8)
                        .Select(lu => lu.UserID).ToList();
                    await _userRepo.AddUsersToProject(0, locationProjectId, locationUserIds);

                    // add a ProjectItem-logs to the location-project
                    var locationLogsItemId = await AddLogs(0, locationProjectId);

                    // find all logs for the location
                    var logs = _context.LocationLog.Where(log => log.LocationID == location.ID).ToList();
                    logs.ForEach(log => log.ProjectItemID = locationLogsItemId);

                    // update the logs to now point to the ProjectItem-logs
                    _context.LocationLog.UpdateRange(logs);
                    await _context.SaveChangesAsync();

                    // Iterate all cleaning plans
                    for (int cp = 0; cp < cleaningPlans.Count; cp++)
                    {
                        var cleaningPlan = cleaningPlans[cp];

                        // Find all users who are cleaning-personale at the location with a matching role to the current cp
                        var cpUserIds = _context.LocationUser
                            .Where(lu => lu.LocationID == location.ID)
                            .Where(lu =>
                            ((cleaningPlan.ID == 1 || cleaningPlan.ID == 4) && lu.User.RoleID == 5)
                            || (cleaningPlan.ID == 2 && lu.User.RoleID == 6)
                            || (cleaningPlan.ID == 3 && lu.User.RoleID == 7))
                            .Select(lu => lu.UserID).ToList();

                        Console.WriteLine("CleaningPlan: " + cleaningPlan.Description);

                        // find all tasks at the location with a matching cp-id to the current cleaning-plan
                        var tasks = _context.CleaningTask.Where(ct => ct.LocationID == location.ID && ct.Area != null && ct.Area.CleaningPlanID == cleaningPlan.ID).Include(t => t.Area).Include(t => t.Floor).ToList();

                        // See if there are any cleaningTasks at the location for this cleaningPlan or 
                        // users with the appropiate role to the cleaning-plan
                        if (tasks.Count > 0 || cpUserIds.Count > 0)
                        {
                            // Create a project for the cleaningPlan under the location-project
                            var cpProjectId = await Create(0, locationProjectId, cleaningPlan.Description);

                            // add cleaning-personale to the project
                            await _userRepo.AddUsersToProject(0, cpProjectId, cpUserIds);

                            // Create a ProjectItem-Tasks for the project
                            var cpProjectItemId = await AddTasks(0, cpProjectId);

                            // Update tasks to point to the new ProjectItem-Tasks
                            tasks.ForEach(t =>
                            {
                                t.ProjectItemID = cpProjectItemId;
                                t.Place = t.Floor?.Description;
                                t.Description = t.Area?.Description;
                            });
                            _context.CleaningTask.UpdateRange(tasks);
                            //await _context.SaveChangesAsync();

                            // if the cleaningPlan-folder represents "Rengøring", add qualityReports
                            if (cleaningPlan.ID == 1)
                            {
                                // Create a ProjectItem-QualityReport for the cleaningPlan-project
                                var qpProjectItemId = await AddQualityReports(0, cpProjectId);

                                // Find all quality-reports for the location
                                var qualityReports = _context.QualityReport.Where(qr => qr.LocationID == location.ID).ToList();
                                // have them point to the new ProjectItem-QualityReport
                                qualityReports.ForEach(qr => qr.ProjectItemID = qpProjectItemId);
                                _context.QualityReport.UpdateRange(qualityReports);
                                //await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();


            }
            await FIX_Project_Path();

            await FIX_Document_Folder_Root();

            var users = _context.User.ToList();
            users.ForEach(u =>
            {
                _projectRoleRepository.UpdateProjectItemUsersFromUserID(u.ID);
                Console.WriteLine("UpdateProjectItemUsersFromUserID for " + u.ID);
            });
        }

        public async Task FIX_Project_Path()
        {
            var roots = _context.Project.Where(p => p.ParentID == null).ToList();
            for (int i = 0; i < roots.Count; i++)
            {
                var project = roots[i];
                await UpdateProjectsOfProject(project.ID, null);
                Console.WriteLine(project.Name);
            }
            Console.WriteLine("FIX_Project_Path DONE");
        }

        private async Task UpdateProjectsOfProject(int parentId, string parentPath)
        {
            var projects = _context.Project.Where(p => p.ParentID == parentId).ToList();
            for (int i = 0; i < projects.Count; i++)
            {
                var project = projects[i];
                project.Path = parentPath;
                await UpdateProjectsOfProject(project.ID, (parentPath != null ? parentPath + "*/*" : "") + project.Name);
                //Console.WriteLine(project.Name);
            }

            _context.Project.UpdateRange(projects);
            await _context.SaveChangesAsync();
        }

        public async Task FIX_Document_Folder_Root()
        {
            var roots = _context.DocumentFolder.Where(df => df.ParentDocumentFolderID == null).ToList();
            for (int i = 0; i < roots.Count; i++)
            {
                var root = roots[i];
                await UpdateRootOfDocumentFolder(root.ID, root.ID);
            }
        }

        private async Task UpdateRootOfDocumentFolder(int parentId, int rootId)
        {
            var folders = _context.DocumentFolder.Where(p => p.ParentDocumentFolderID == parentId).ToList();
            for (int i = 0; i < folders.Count; i++)
            {
                var folder = folders[i];
                folder.RootDocumentFolderID = rootId;
                await UpdateRootOfDocumentFolder(folder.ID, rootId);
            }

            _context.DocumentFolder.UpdateRange(folders);
            await _context.SaveChangesAsync();
        }

        public async Task FIX_Conversations()
        {
            var admins = _context.User.Where(u => u.ProjectRole.HasAllPermissions).Select(u => u.ID).ToList();

            for (int c = 0; c < admins.Count; c++)
            {
                var admin = admins[c];
                await _firestoreRepository.AddAdmin(admin);
            }


            var locations = _context.Location.Include(p => p.ProjectItem).ToList();

            for (int c = 0; c < locations.Count; c++)
            {
                var location = locations[c];

                await AddFirestoreConversation(0, location.ProjectItem.ProjectID, "Management", new List<ProjectItemAccess> {
                        new ProjectItemAccess { ProjectRoleID = 2, Read = true, Write = true },
                        new ProjectItemAccess { ProjectRoleID = 5, Read = true, Write = true },
                    });

                await AddFirestoreConversation(0, location.ProjectItem.ProjectID, "Staff", new List<ProjectItemAccess> {
                        new ProjectItemAccess { ProjectRoleID = 2, Read = true, Write = true },
                        new ProjectItemAccess { ProjectRoleID = 3, Read = true, Write = true },
                    });
            }
            /*
            var customers = _context.Customer.Include(p => p.ProjectItem).ToList();
            for (int c = 0; c < customers.Count; c++)
            {
                var customer = customers[c];



                await AddFirestoreConversation(0, customer.ProjectItem.ProjectID, "Management", new List<ProjectItemAccess> {
                        new ProjectItemAccess { ProjectRoleID = 2, Read = true, Write = true },
                        new ProjectItemAccess { ProjectRoleID = 5, Read = true, Write = true },
                    });
            }
            */
        }

        /*
        public async Task FIX_Customers_To_Clients()
        {
            var customers = _context.Customer.ToList();
            for (var c = 0; c < customers.Count; c++)
            {
                var customer = customers[c];
                var client = new Client { ProjectItemID = customer.ProjectItemID };
                _context.Client.Add(client);
                await _context.SaveChangesAsync();
                var allCustomerUsers = _context.User.Where(u => u.CustomerID == customer.ID).ToList();
                allCustomerUsers.ForEach(acu => acu.ClientID = client.ID);
                _context.User.UpdateRange(allCustomerUsers);
                await _context.SaveChangesAsync();
            }
        }
        */

        //public async Task FixTasks()
        //{
        //    var tasks = _context.CleaningTask.Include(t => t.Area).Include(t => t.Floor).ToList();
        //    tasks.ForEach(t =>
        //    {
        //        t.Place = t.Floor?.Description;
        //        t.Description = t.Area?.Description;
        //    });
        //    _context.CleaningTask.UpdateRange(tasks);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task FixAccessStrings()
        //{
        //    var projectItems = _context.ProjectItem.ToList();
        //    projectItems.ForEach(pi =>
        //    {
        //        var newAccess = "";
        //        var permissions = pi.Access.ToArray().ToList();
        //        permissions.ForEach(p =>
        //        {
        //            var stringResult = "";
        //            if (p == 'R') stringResult = "R-";
        //            else if (p == 'W') stringResult = "RW";
        //            else stringResult = "--";

        //            newAccess = newAccess += stringResult;

        //            //return stringResult;
        //        });

        //        pi.Access = newAccess;
        //    });

        //    _context.ProjectItem.UpdateRange(projectItems);
        //    await _context.SaveChangesAsync();
        //}
    }
}
