using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TedLearn_Core.ViewModels.Orders
{
    public class ShowOrdersInUserPanelViewModel
    {
        public int OrderId { get; set; }
        public bool IsFinaly { get; set; }
        public DateTime OrderDate { get; set; }
        public List<string> CourseTitle { get; set; }
        public List<decimal> Price { get; set; }
        public decimal OrderDiscount { get; set; }
        public List<decimal> OrderDetailDiscounts { get; set; }
    }
}
