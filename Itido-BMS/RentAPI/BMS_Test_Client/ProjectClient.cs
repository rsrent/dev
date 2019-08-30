using System;
using System.Threading.Tasks;
using BMS_Test_Client.Extensions;
using System.Collections.Generic;

namespace BMS_Test_Client
{
    public class ProjectClient
    {
        private readonly ClientController _client;
        public ProjectClient(ClientController client)
        {
            _client = client;
        }

        public async Task Run()
        {

            //await FIX();
            // await Get(2823);


            //await ProjectRoleTest();

            //await GetProjectItemsOfProject(4184);

            //await UpdateAccessTest();

            //await AddComment(2823);

            /*
            await GetProjectInfo(94);
            await GetProjectInfo(95);
            await GetProjectInfo(96);
            await GetProjectInfo(97);
            await GetProjectInfo(98);
            await GetProjectInfo(99);
    */



            //var id = await Create(null, "Test projekt");
            //await Get(id);
            //await AddComment(id, "AMUCE");
            //await GetAllItemsOfProject(id);
            //await GetAll();
            //await AddComment();
            //await GetAllItemsOfProject();

            //await Create(null, "Rasmus TEST");
            //await GetProjectsOfProjects();
            //await AddUserToProjects();
            //await RemoveUserToProjects();
        }

        async Task GetProjectInfo(int projectId)
        {
            await Get(projectId);
            await GetProjectsOfProject(projectId);
            await GetProjectItemsOfProject(projectId);
            await GetUsersOfProject(projectId);
        }

        public async Task GetProjectsOfProjects()
        {
            var result = await _client.GetMany("Project/GetProjectsOfProject/4184");
            result.Print();

        }

        public async Task AddUserToProjects()
        {
            var projects = new List<dynamic> {
                5548
           };
            var res = await _client.PutNoContent("Project/AddProjectsToUsers/155", projects);
        }
        public async Task RemoveUserToProjects()
        {
            var projects = new List<dynamic> {
                5543
           };
            var res = await _client.PutNoContent("Project/RemoveProjectsFromUser/155", projects);
        }



        public async Task<int> Create(int? parentId, string name)
        {
            var result = await _client.PostId("Project/Create/" + name + (parentId != null ? ("/" + parentId) : ""));
            Console.WriteLine("Result: " + result);
            return result;
        }

        public async Task Get(int projectId)
        {
            var result = await _client.Get("Project/Get/" + projectId);
            result.Print();
        }

        public async Task GetAll()
        {
            var result = await _client.GetMany("Project/GetAll");
            result.Print();
        }

        public async Task GetProjectsOfProject(int projectId)
        {
            var result = await _client.GetMany("Project/GetProjectsOfProject/" + projectId);
            result.Print();
        }

        public async Task<int> AddComment(int projectId)
        {
            var result = await _client.PutId("Project/AddComment/" + projectId);
            Console.WriteLine("Result: " + result);
            return result;
        }

        public async Task<int> AddComment(int projectId, string access)
        {
            var result = await _client.PutId("Project/AddComment/" + projectId + "/" + access);
            Console.WriteLine("Result: " + result);
            return result;
        }

        public async Task GetProjectItemsOfProject(int projectId)
        {
            var result = await _client.GetMany("Project/GetProjectItemsOfProject/" + projectId);
            result.Print();
        }

        public async Task GetUsersOfProject(int projectId)
        {
            var result = await _client.GetMany("Users/GetOfProject/" + projectId);
            result.Print();
        }

        public async Task UpdateAccessTest()
        {
            var result = await _client.PutNoContent("Project/UpdateProjectItemAccess/8734/AWUR-");
            Console.WriteLine(result);
        }

        public async Task FIX()
        {
            var result = await _client.PostNoContent("Project/FIX");
            Console.WriteLine("Result: " + result);
        }

        public async Task FIX_Tasks()
        {
            var result = await _client.PostNoContent("Project/FIX_2");
            Console.WriteLine("Result: " + result);
        }

        public async Task ProjectRoleTest()
        {
            var result = await _client.Get("ProjectRole/Test");
            Console.WriteLine("Result: " + result);
        }
    }
}


//https://renttesting.azurewebsites.net/api/Project/UpdateProjectItemAccess/8734/AWUR-