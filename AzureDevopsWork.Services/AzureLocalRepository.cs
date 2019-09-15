using System.Collections.Generic;
using AzureDevopsWork.Services.Context;
using AzureDevopsWork.Services.Models.DTO;
using AzureDevopsWork.Services.Models.Entities;
using System.Linq;

namespace AzureDevopsWork.Services
{
    public class AzureLocalRepository
    {

        public void Save(List<ItemsDTO> items)
        {
            RecuperaItemsIds(items);

            foreach (var item in items)
            {
                if (item.Id == null || item.Id == 0)
                {
                    Include(item);
                }
                else
                {
                    Edit(item);
                }
            }
        }

        private void RecuperaItemsIds(List<ItemsDTO> items)
        {
            using (var db = new AzWorkItemContext())
            {
                var workItems = db.WorkItems
                    .Select(w => new
                    {
                        w.Id,
                        w.IdWorkItem
                    })
                    .ToArray();

                foreach (var item in items)
                {
                    item.Id = workItems
                        .Where(w => w.IdWorkItem == item.IdWorkItem)
                        .Select(w => w.Id)
                        .FirstOrDefault();
                }
            }
        }

        private void Include(ItemsDTO items)
        {
            using (var db = new AzWorkItemContext())
            {
                var newWorkItem = FillWorkItem(items);

                db.WorkItems.Add(newWorkItem);
                db.SaveChanges();
            }
        }

        private void Edit(ItemsDTO items)
        {
            using (var db = new AzWorkItemContext())
            {
                var workItemOriginal = db.WorkItems.FirstOrDefault(w => w.Id == items.Id);
                workItemOriginal.WorkItemType = items.WorkItemType;
                workItemOriginal.Title = items.Title;
                workItemOriginal.CreatedDate = items.CreatedDate;

                db.SaveChanges();
            }
        }
        private WorkItem FillWorkItem(ItemsDTO items)
        {
            return new WorkItem
            {
                Title = items.Title,
                CreatedDate = items.CreatedDate,
                WorkItemType = items.WorkItemType,
                IdWorkItem = items.IdWorkItem.Value
            };
        }

    }
}
