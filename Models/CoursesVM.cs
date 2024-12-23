using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CoursesVM
    {
        public List<Course> Data {  get; set; }
        public int currentPage {  get; set; }
        public int pageSize { get; set; }
        public int totalPages { get; set; }
        public List<Category> Categories { get; set; }
    }
}
