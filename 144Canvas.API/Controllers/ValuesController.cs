using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _144Canvas.API.Data;
using Microsoft.AspNetCore.Authorization;

namespace _144Canvas.API.Controllers
{

    /*
     *
     *  Possibly redundant. We don't use values in our database
     *
     */
    
    [Authorize (Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;
        public ValuesController(DataContext dc){_context=dc;}

        // GET api/values
        [HttpGet]
        public IActionResult GetValues(){
            //var values = _context.Users.ToList();
            return Ok(null);
            //return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
