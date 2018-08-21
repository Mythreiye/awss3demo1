using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.Extensions.NETCore.Setup;
using lsmsapp1.Services;

namespace lsmsapp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3Controller : ControllerBase
    {
        private readonly IS3Service _service;

        public S3Controller(IS3Service service)
        {
            _service = service;
        }
        // GET api/values
        [HttpGet]
        [Route("GetFile/{bucketName}")]
        public async Task<IActionResult> GetObjectFromS3Async([FromRoute] string bucketName)
        {
            await _service.GetObjectFromS3Async(bucketName);
            return Ok();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("{bucketName}")]
        public async Task<IActionResult> CreateBucket([FromRoute] string bucketName)
        {
            var response = await _service.CreateBucketAsync(bucketName);
            return Ok(response);
        }

        [HttpPost]
        [Route("AddFile/{bucketName}")]
        public async Task<IActionResult> AddFile([FromRoute] string bucketName)
        {
            await _service.UploadFileAsync(bucketName);
            return Ok();
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
