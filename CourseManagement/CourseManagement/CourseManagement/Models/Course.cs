using System;

namespace CourseManagement.Models
{
	public class Course
	{
		public int CourseId { get; set; }
		public int AccId  { get; set; }
		public string CourseName { get; set; }
		public string? Image { get; set; }
		public double Fee { get; set; }
		public string AuthorName { get; set; }
        public bool Status { get; set; }
		public DateTime CreatedDate { get; set; }
		public int EnrollmentCount { get; set; }
	}
}
