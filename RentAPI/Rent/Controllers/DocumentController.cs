using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rent.Data;
using Rent.DTOs;
using Rent.Models;
using Rent.Repositories;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/Document")]
    public class DocumentController : Controller
    {
        private string ThisPermission = "Document";
        private readonly RentContext _context;
        private readonly DocumentRepository _documentRepository;
        private readonly PermissionRepository _permissionRepository;
        private readonly IStorage _storageRepository;

        private readonly NotificationRepository _notificationRepository;

        public DocumentController(RentContext context, DocumentRepository documentRepository, NotificationRepository notificationRepository, PermissionRepository permissionRepository, IStorage storageRepository)
        {
            _context = context;
            _documentRepository = documentRepository;
            _permissionRepository = permissionRepository;
            _notificationRepository = notificationRepository;
            _storageRepository = storageRepository;
        }

        [HttpGet("GetForLocation/{locationID}")]
        [Authorize]
        public async Task<IActionResult> GetForLocation([FromRoute] int locationID)
        {
            var folder = await _documentRepository.GetForLocation(UserID, locationID);

            if(folder == null)
            {
                return NotFound();
            }
            return Ok(folder);
        }

        [HttpGet("GetForCustomer/{customerID}")]
        [Authorize]
        public async Task<IActionResult> GetForCustomer([FromRoute] int customerID)
        {
            var folder = await _documentRepository.GetForCustomer(UserID, customerID);

            if (folder == null)
            {
                return NotFound();
            }
            return Ok(folder);
        }

        [HttpDelete("{folderID}")]
        [Authorize]
        public async Task<IActionResult> RemoveFolder([FromRoute] int folderID)
        {
            var folder = await _context.DocumentFolder.FindAsync(folderID);
            if(folder == null)
            {
                return NotFound("Folderen kunne ikke findes");
            }
            else if(folder.ParentDocumentFolderID == null)
            {
                return BadRequest("Rodmapper kan ikke fjernes");
            }
            else if(!folder.Removable)
            {
                return NotFound("Denne folder kan ikke fjernes");
            }
            var success = await _documentRepository.RemoveFolder(UserID, folder);
            if (success)
                return NoContent();
            else
                return BadRequest();
        }

        [HttpGet("GetForFolder/{folderID}")]
        [Authorize]
        public async Task<IActionResult> GetForFolder([FromRoute] int folderID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var folder = await _documentRepository.Get(UserID, folderID);

            if (folder == null)
                return NotFound("Folder not found");

            return Ok(folder);
        }
        /*
        [HttpGet("GetDocumentItemContent/{documentID}/{containerName}")]
        [Authorize]
        public async Task<IActionResult> GetDocumentItemContent([FromRoute] int documentID, [FromRoute] string containerName)
        {
            var byteArray = await _documentRepository.GetContent(containerName, documentID);

            if (byteArray == null)
                return BadRequest();

            return Ok(byteArray);
        }*/

        [HttpPost("AddFolder/{folderID}/{title}")]
        [Authorize]
        public async Task<IActionResult> AddFolder([FromRoute] int folderID, [FromRoute] string title)
        {
            return Ok(await _documentRepository.AddFolder(UserID, folderID, title, true));
        }

        [HttpGet("GetSASTokenDownload/{containerName}")]
        [Authorize]
        public async Task<IActionResult> GetSASTokenDownload([FromRoute] string containerName)
        {
            if (ImageContainer(containerName))
            {
                return Ok(await _storageRepository.GetContainerSas(containerName));
            }
            else {

                if(int.TryParse(containerName.Split('-')[0], out var docId))
                {
                    if (await _documentRepository.HasEditPermission(docId, UserID))
                    {
                        return Ok(await _storageRepository.GetContainerSas(containerName, true));
                    }
                    else if (await _documentRepository.HasReadPermission(docId, UserID))
                    {
                        return Ok(await _storageRepository.GetContainerSas(containerName));
                    }
                }
            }

            return BadRequest();
        }

        [HttpGet("GetSASTokenUpload/{containerName}")]
        public async Task<IActionResult> GetSasToken([FromRoute] string containerName)
        {
            if(ImageContainer(containerName))
            {
                return Ok(await _storageRepository.GetSASTokenUpload(containerName));
            } else {

                if (int.TryParse(containerName.Split('-')[0], out var docId))
                {
                    if (await _documentRepository.HasEditPermission(docId, UserID))
                    {
                        return Ok(await _storageRepository.GetSASTokenUpload(containerName));
                    }
                }
            }
            return BadRequest();
        }

        static string[] ImageContainers = new[] { "image", "thumbnail-50", "thumbnail-150", "thumbnail-300", "thumbnail-500" };

        bool ImageContainer(string containerName)
        {
            return ImageContainers.Contains(containerName);
        }
        /*
        [AllowAnonymous]
        [HttpGet("FixFolders")]
        public async Task<IActionResult> FixFolders()
        {
            var locations = _context.Location.ToList();

            foreach(var l in locations)
            {
                l.CleaningFolderID = await _documentRepository.GetStandardCleaningFolder(1, "Rengøring");
                l.WindowFolderID = await _documentRepository.GetStandardCleaningFolder(1, "Vinduespudsning");
                l.FanCoilFolderID = await _documentRepository.GetStandardCleaningFolder(1, "Fan coil");
                l.PeriodicFolderID = await _documentRepository.GetStandardCleaningFolder(1, "Periodisk rengøring");
            }

            _context.Location.UpdateRange(locations);
            await _context.SaveChangesAsync();

            return Ok();
        }
        */

        int UserID => Int32.Parse(User.Claims.ToList()[0].Value);
    }
}
