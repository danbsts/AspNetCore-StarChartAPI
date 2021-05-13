using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var result = _context.CelestialObjects.Find(id);
            if (result == null) return NotFound();
            result.Satellites = _context.CelestialObjects.Where(satelite => satelite.OrbitedObjectId == result.Id).ToList();
            return Ok(result);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var result = _context.CelestialObjects.Where(celObj => celObj.Name == name);
            if (!result.Any())
            {
                return NotFound();
            }
            foreach (var obj in result)
            {
                obj.Satellites = _context.CelestialObjects.Where(satelite => satelite.OrbitedObjectId == obj.Id).ToList();
            }
            return Ok(result.ToList());
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _context.CelestialObjects.ToList();
            result.ForEach(obj => obj.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == obj.Id).ToList());
            return Ok(result);
        }
    }
}
