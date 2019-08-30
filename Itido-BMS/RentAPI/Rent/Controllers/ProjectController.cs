using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rent.Models.Projects;
using Rent.Repositories;

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/Project")]
    public class ProjectController : ControllerExecutor
    {
        private readonly ProjectRepository _projectRepository;
        public ProjectController(ProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [HttpPost("Create/{name}")]
        public Task<IActionResult> Create(string name)
        => Executor(() => _projectRepository.Create(Requester, null, name));

        [HttpPost("Create/{name}/{parentId}")]
        public Task<IActionResult> Create(string name, int parentId)
        => Executor(() => _projectRepository.Create(Requester, parentId, name));

        [HttpPut("Update/{projectId}")]
        public Task<IActionResult> Update(int projectId, [FromBody] Project project)
        => Executor(() => _projectRepository.Update(Requester, projectId, project));

        [HttpPut("AddProjectsToUsers/{userId}")]
        public Task<IActionResult> AddProjects(int userId, [FromBody] ICollection<int> projectIds)
        => Executor(() => _projectRepository.AddProjectsToUsers(Requester, userId, projectIds));

        [HttpPut("RemoveProjectsFromUser/{userId}")]
        public Task<IActionResult> RemoveProjects(int userId, [FromBody] ICollection<int> projectIds)
        => Executor(() => _projectRepository.RemoveProjectsFromUser(Requester, userId, projectIds));

        [HttpPut("AddAddress/{projectId}")]
        public Task<IActionResult> AddAddress(int projectId)
        => Executor(() => _projectRepository.AddAddress(Requester, projectId));

        [HttpPut("AddComment/{projectId}")]
        public Task<IActionResult> AddComment(int projectId)
        => Executor(() => _projectRepository.AddComment(Requester, projectId));

        [HttpPut("AddLocation/{projectId}")]
        public Task<IActionResult> AddLocation(int projectId)
        => Executor(() => _projectRepository.AddLocation(Requester, projectId));

        [HttpPut("AddClient/{projectId}")]
        public Task<IActionResult> AddClient(int projectId)
        => Executor(() => _projectRepository.AddClient(Requester, projectId));

        [HttpPut("AddProfileImage/{projectId}")]
        public Task<IActionResult> AddProfileImage(int projectId)
        => Executor(() => _projectRepository.AddProfileImage(Requester, projectId));

        [HttpPut("AddLogs/{projectId}")]
        public Task<IActionResult> AddLogs(int projectId)
        => Executor(() => _projectRepository.AddLogs(Requester, projectId));

        [HttpPut("AddWork/{projectId}")]
        public Task<IActionResult> AddWork(int projectId)
        => Executor(() => _projectRepository.AddWork(Requester, projectId));

        [HttpPut("AddTasks/{projectId}")]
        public Task<IActionResult> AddTasks(int projectId)
        => Executor(() => _projectRepository.AddTasks(Requester, projectId));

        [HttpPut("AddQualityReports/{projectId}")]
        public Task<IActionResult> AddQualityReports(int projectId)
        => Executor(() => _projectRepository.AddQualityReports(Requester, projectId));

        [HttpPut("AddFolder/{projectId}/{title}")]
        public Task<IActionResult> AddFolder(int projectId, string title)
        => Executor(() => _projectRepository.AddDocumentFolders(Requester, projectId, title));

        [HttpPut("AddPost/{projectId}/{title}/{body}")]
        public Task<IActionResult> AddPost(int projectId, string title, string body)
        => Executor(() => _projectRepository.AddPost(Requester, projectId, title, body));

        [HttpPut("AddFirestoreConversation/{projectId}/{title}")]
        public Task<IActionResult> AddFirestoreConversation(int projectId, string title)
        => Executor(() => _projectRepository.AddFirestoreConversation(Requester, projectId, title));

        [HttpGet("Get/{projectId}")]
        public IActionResult Get(int projectId)
        => Executor(() => _projectRepository.Get(Requester, projectId));

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        => Executor(() => _projectRepository.GetAll(Requester));

        [HttpGet("GetProjectsOfProject/{projectId}")]
        public IActionResult GetProjectsOfProject(int projectId)
        => Executor(() => _projectRepository.GetProjectsOfProject(Requester, projectId));

        [HttpGet("GetProjectItemsOfProject/{projectId}")]
        public IActionResult GetProjectItemsOfProject(int projectId)
        => Executor(() => _projectRepository.GetProjectItemsOfProject(Requester, projectId));

        [HttpGet("GetDetailedProjectItemsOfProject/{projectId}")]
        public IActionResult GetDetailedProjectItemsOfProject(int projectId)
        => Executor(() => _projectRepository.GetDetailedProjectItemsOfProject(Requester, projectId));

        [HttpGet("GetOfUser/{userId}")]
        public IActionResult GetForUser([FromRoute] int userId)
        => Executor(() => _projectRepository.GetProjectsOfUser(Requester, userId));

        [HttpGet("GetOfNotUser/{userId}")]
        public IActionResult GetOfNotUser([FromRoute] int userId)
        => Executor(() => _projectRepository.GetProjectsOfNotUser(Requester, userId));


        [HttpPut("UpdateProjectItemAccess/{projectItemId}")]
        public Task<IActionResult> UpdateAccess([FromRoute] int projectItemId, [FromBody] ICollection<ProjectItemAccess> projectItemAccesses)
        => Executor(() => _projectRepository.UpdateProjectItemAccess(Requester, projectItemId, projectItemAccesses));


        /*
        [HttpPost("FIX")]
        public Task<IActionResult> FIX()
        => Executor(() => _projectRepository.FIX_Conversations());
        //*/

        /*
        [HttpPost("FIX_2")]
        public Task<IActionResult> FIX_2()
        => Executor(() => _projectRepository.FixTasks());
        */
    }
}
