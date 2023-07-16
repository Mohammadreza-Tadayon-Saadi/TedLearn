using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TedLearn_Core.ViewModels.UserPanel
{
    public class ShowMyCoursesDto
    {
        public int UC_Id { get; set; }
        public string CourseTitle { get; set; }
        public string TeacherName { get; set; }
        public string? TeacherWebsite { get; set; }
        public DateTime LastOfUpdate { get; set; }
    }
}
