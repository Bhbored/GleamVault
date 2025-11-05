using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class CategoryInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ItemInfo:BaseNotify
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HallMark { get; set; }
        public string weightUnit { get; set; }
        public float weight {  get; set; }
        public float UnitPrice { get; set; }
        public int CurrentStock { get; set; }
    }
}
