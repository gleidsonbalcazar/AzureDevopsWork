using System;

namespace AzureDevopsWork.Services.Models.DTO
{
    public class ItemsDTO
    {
        public int? Id { get; set; }
        public int? IdWorkItem { get; set; }
        public string Title { get; set; }
        public string WorkItemType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
