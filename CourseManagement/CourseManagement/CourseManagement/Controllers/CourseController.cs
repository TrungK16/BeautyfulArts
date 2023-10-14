using CourseManagement.Models;
using CourseManagement.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagement.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CourseController : ControllerBase
	{
		private readonly DataContext _dataContext;

		public CourseController(DataContext _dataContext, IConfiguration configuration)
		{
			this._dataContext = _dataContext;
		}

		[HttpGet("courses")]
		public IActionResult GetCourses(string? search = "",
			float? minFee = null,
			float? maxFee = null,
			int? mentorId = null,
			int? pageIndex = 1,
			int? pageSize = 10)
		{
			try
			{
				var courses = _dataContext.Courses.Where(course =>
				(string.IsNullOrWhiteSpace(search) || course.CourseName.ToLower().Contains(search.ToLower()))
				&& ((minFee == null || course.Fee >= minFee) && (maxFee == null || course.Fee <= maxFee)) 
				&& (mentorId == null || course.AccId == mentorId))
				.Skip(pageIndex.Value - 1).Take(pageSize.Value);

				return courses?.Count() > 0 ? Ok(courses) : NotFound("No course found");
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "there was an error when getting the course." });
			}

		}

		[HttpGet("course/{id}")]
		public IActionResult GetCourseById(int id)
		{
			var course = _dataContext.Courses.FirstOrDefault(c => c.CourseId == id);

			return course != null ? Ok(course) : NotFound(new Response { Status = "Error", Message = "Could not find the course." });
		}

		[HttpPost("course")]
		public IActionResult CreateCourse(Course course)
		{
			try
			{
				if (course.Fee <= 0 || course.EnrollmentCount <= 0)
				{
					return BadRequest(new Response { Status = "Error", Message = "Invalid course's values." });
				}

				course.CreatedDate = DateTime.UtcNow;
				_dataContext.Courses.Add(course);
				_dataContext.SaveChanges();
				return Ok(course);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "there was an error when creating a course." });
				throw;
			}
		}
	}
}
