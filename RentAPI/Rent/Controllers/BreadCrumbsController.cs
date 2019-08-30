using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.DTOs;
using Rent.Models;

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/BreadCrumbs")]

    public class BreadCrumbsController : ControllerExecutor
    {
        private readonly RentContext _context;

        public BreadCrumbsController(RentContext context)
        {
            _context = context;
        }

        [HttpGet("Location/{locationID}")]
        public IActionResult Location([FromRoute] int locationID)
        {
            var item = _context.Location.Find(locationID);
            if (item == null) return NotFound("Location not found");
            return Ok(item.Name);
        }

        [HttpGet("Floor/{floorID}")]
        public IActionResult Floor([FromRoute] int floorID)
        {
            var item = _context.Floor.Find(floorID);
            if (item == null) return NotFound("Floor not found");
            return Ok(item.Description);
        }

        [HttpGet("Plan/{planID}")]
        public IActionResult Plan([FromRoute] int planID)
        {
            var item = _context.CleaningPlan.Find(planID);
            if (item == null) return NotFound("Plan not found");
            return Ok(item.Description);
        }

        [HttpPost]
        public IActionResult BreadCrumbs([FromBody] string url) 
        {
            return Ok(Generate(new List<BreadCrumb>(), "", "", "", new Queue<string>(url.Split('/'))).Select(bc => new BreadCrumb { Title = bc.Title, Url = bc.Url + "/" }));
        }

        public List<BreadCrumb> Generate(List<BreadCrumb> breadCrumbs, string previousPath, string previousSegment, string previousPreviousSegment, Queue<string> rest)
        {
            if (rest.Count == 0)
            {
                return breadCrumbs;
            }

            var current = rest.Dequeue();

            if (string.IsNullOrEmpty(current))
                return breadCrumbs;

            var currentPath = previousPath + "/" + current;

            if (int.TryParse(current, out var id))
            {
                string title = "";
                string path = currentPath;
                if (previousSegment == "Locations")
                {
                    var location = _context.Location.Find(id);
                    title = location != null ? location.Name : "LocationNotFound";
                }
                if (previousSegment == "Customers")
                {
                    //return Generate(breadCrumbs, currentPath, current, previousSegment, rest);
                    var customer = _context.Customer.Find(id);
                    title = customer != null ? customer.Name : "CustomerNotFound";
                    
                    var requester = _context.User.Find(Requester);
                    if (requester.RoleID == 8 || requester.RoleID == 9)
                        title = "Hjem";
                }
                if (previousSegment == "Users")
                {
                    var user = _context.User.Find(id);
                    title = user != null ? user.FirstName : "UserNotFound";
                }
                if (previousSegment == "CleaningPlans")
                {
                    var cp = _context.CleaningPlan.Find(id);
                    if (cp != null)
                    {
                        if (cp.HasFloors)
                            title = "Etager til " + cp.Description;
                        else
                            title = "Områder til " + cp.Description;
                    }
                    else
                    {
                        title = "CleaningPlanNotFound";
                    }
                    rest.TryPeek(out var restString);
                    path += "/" + restString;
                }
                if (previousSegment == "QualityReports")
                {
                    var qp = _context.QualityReport.Find(id);
                    if (qp != null)
                    {
                        title = "Etager til " + qp.Time.ToString("D");
                    }
                    else
                    {
                        title = "QualityReportNotFound";
                    }

                    rest.TryPeek(out var restString);
                    path += "/" + restString;
                }
                if (previousSegment == "Floors")
                {
                    var floor = _context.Floor.Find(id);
                    if (floor != null)
                    {
                        title = "Områder til " + floor.Description;
                    }
                    else
                    {
                        title = "FloorNotFound";
                    }

                    rest.TryPeek(out var restString);
                    //path += "/" + restString;
                }

                if (previousSegment == "History")
                {
                    var task = _context.CleaningTask.Include(ct => ct.Area).FirstOrDefault(ct => ct.ID == id);
                    if (task != null)
                    {
                        title = "Afsluttede opgaver til " + task.Area.Description;
                    }
                    else
                    {
                        title = "TaskNotFound";
                    }

                    rest.TryPeek(out var restString);
                    //path += "/" + restString;
                }
                if(previousSegment == "Documents")
                {
                    var document = Root(id);

                    var documents = new List<DocumentDTO.Folder>();
                    while (document != null)
                    {
                        documents.Add(document);
                        document = document.Parent as DocumentDTO.Folder;
                    }

                    documents.Reverse();

                    for (int i = 0; i < documents.Count; i++)
                    {
                        var doc = documents[i];
                        var docTitle = doc.Title;
                        /*if (i == 0)
                            docTitle = "Dokumenter";*/
                        breadCrumbs.Add(new BreadCrumb { Title = docTitle, Url = previousPath + "/" + doc.ID});
                    }
                    return breadCrumbs;
                    //currentPath += "/" + rest.Dequeue();
                }

                breadCrumbs.Add(new BreadCrumb { Title = title, Url = path });
            }

            if (current == "Users")
            {
                breadCrumbs.Add(new BreadCrumb { Title = "Brugere", Url = currentPath });
            }
            /*
            if (current == "Locations")
            {
                breadCrumbs.Add(new BreadCrumb { Title = "Lokaliteter", Url = currentPath });
            }
            */
            if (current == "CleaningPlans")
            {
                breadCrumbs.Add(new BreadCrumb { Title = "Rengøringsplaner", Url = currentPath });
            }
            if (current == "QualityReports")
            {
                breadCrumbs.Add(new BreadCrumb { Title = "Kvalitetsrapporter", Url = currentPath });
            }
            if (current == "Customers")
            {
                var requester = _context.User.Find(Requester);
                if(requester.RoleID != 8 && requester.RoleID != 9)
                    breadCrumbs.Add(new BreadCrumb { Title = "Kunder", Url = currentPath });
            }
            if (current == "Documents")
            {
                breadCrumbs.Add(new BreadCrumb { Title = "Dokumenter", Url = currentPath });
                /*
                if(previousPreviousSegment == "Locations")
                {
                    rest.TryPeek(out var restString);
                    var document = Root(int.Parse(restString));

                    var documents = new List<DocumentDTO.Folder>();
                    while (document != null)
                    {
                        documents.Add(document);
                        document = document.Parent as DocumentDTO.Folder;
                    }

                    documents.Reverse();

                    for (int i = 0; i < documents.Count; i++)
                    {
                        var doc = documents[i];
                        var title = doc.Title;
                        if (i == 0)
                            title = "Dokumenter";

                        breadCrumbs.Add(new BreadCrumb { Title = title, Url = currentPath + "/" + doc.ID });
                    }
                    currentPath += "/" + rest.Dequeue();
                } 
                else if(previousPreviousSegment == "Customers")
                {
                    breadCrumbs.Add(new BreadCrumb { Title = "Dokumenter", Url = currentPath });
                    rest.Clear();
                }
                */
            }

            return Generate(breadCrumbs, currentPath, current, previousSegment, rest);
        }

        public DocumentDTO.Folder Root(int folderID)
        {
            DocumentDTO.Folder root = null;
            DocumentDTO.Folder previousDocument = null;
            int? id = folderID;
            while(id != null) {
                var currentDocument = _context.DocumentFolder.FirstOrDefault(d => d.ID == id);

                var currentDTO = new DocumentDTO.Folder()
                {
                    ID = currentDocument.ID, Title = currentDocument.Title
                };

                if(previousDocument != null) 
                {
                    previousDocument.Parent = currentDTO;
                }

				id = currentDocument.ParentDocumentFolderID;

                previousDocument = currentDTO;

                if (root == null)
                {
                    root = previousDocument;
                }
            }
            return root;
        }
    }
}