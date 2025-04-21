using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {

        [HttpGet("notfound")]// Get: api/Buggy/notfound
        public IActionResult GetNotFoundRequest()
        {
            return NotFound(); //404
        }


        [HttpGet("servererror")]// Get: api/Buggy/servererror
        public IActionResult GetServerErrorRequest()
        {
            throw new Exception(); //500
        }

        [HttpGet("badrequest")]// Get: api/Buggy/badrequest/1
        public IActionResult GetBadRequest(int id) // validation error
        {
            return BadRequest(); //400
        }

        [HttpGet("unauthorized")]// Get: api/Buggy/unauthorized
        public IActionResult GetUnAuthorizedRequest(int id) // unauthorized error
        {
            return Unauthorized(); //401
        }



    }
}
