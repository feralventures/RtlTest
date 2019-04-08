using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RtlTestRepository.Models;

namespace RtlTestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private RtlTestRepository.IService _service;

        public ShowsController(RtlTestRepository.IService service)
        {
            _service = service;
        }

        // GET api/shows
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int skip = 0, [FromQuery] int take = 10, CancellationToken cancellationToken = default)
        {
            var shows = await _service.GetShowsIncludingCast(skip, take, cancellationToken);

            return Ok(shows.Select(s => new
            {
                Id = s.TvMazeId,
                Name = s.Name,
                Cast = s.Cast.OrderByDescending(p => p.Birthday).Select(p => new
                {
                    Id = p.TvMazeId,
                    Name = p.Name,
                    Birthday = p.Birthday,
                }).ToList(),
            }).ToList());
        }
    }
}
