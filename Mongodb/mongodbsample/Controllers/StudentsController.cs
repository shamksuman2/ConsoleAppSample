using Microsoft.AspNetCore.Mvc;
using mongodbsample.Models;
using mongodbsample.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mongodbsample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentsController(IStudentService studentService) {
            _studentService = studentService;
        }
        // GET: api/<StudentsController>
        [HttpGet]
        public IEnumerable<Students> Get()
        {
            return _studentService.Get();
        }

        // GET api/<StudentsController>/5
        [HttpGet("{id}")]
        public Students Get(string id)
        {
            return _studentService.Get(id);
        }

        // POST api/<StudentsController>
        [HttpPost]
        public ActionResult<Students> Post([FromBody] Students value)
        {
            _studentService.Create(value);
            return CreatedAtAction(nameof(Post), value);
        }

        // PUT api/<StudentsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] string value)
        {
            var student = _studentService.Get(id);
            if (student == null) { _studentService.Update(student); }
            return NoContent();
        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var student = _studentService.Get(id);
            if (student == null) { _studentService.Remove(id); }
            return Ok($"Student deleted with name :{student.Name}");
        }
    }
}
