using System.Linq;
using AzureDevopsWork.Services.Context;
using AzureDevopsWork.Services.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AzureDevopsWork.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        public ActionResult<ItemsDTO[]> GetWorkItems()
        {
            using (var db = new AzWorkItemContext())
            {
                return db.WorkItems
                    .Select(w => new ItemsDTO()
                    {
                        Id = w.Id,
                        Title = w.Title,
                        CreatedDate = w.CreatedDate,
                        WorkItemType = w.WorkItemType
                    })
                    .ToArray();
            }
        }
    }
}