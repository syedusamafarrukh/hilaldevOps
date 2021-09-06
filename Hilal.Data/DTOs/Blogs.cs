using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class Blogs
    {
        public Guid Id { get; set; }
        public string HeaderImage { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThunbnilUrl { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsPublished { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdateBy { get; set; }
    }
}
