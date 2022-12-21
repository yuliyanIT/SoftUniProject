using FaceitRankChecker.Infrastructure.Data;
using FaceitRankChecker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FaceitRankChecker.Controllers
{
    
        [ApiController]
        [Route("api/[controller]")]
        public class MatchController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            private List<Match> _matches;
            public MatchController(ApplicationDbContext context)
            {
                _context = context;
            
            }

            [HttpGet]
            public IActionResult GetMatches()
            {
                var matches = _context;
                return Ok(matches);
            }

            [HttpPost]
            public IActionResult CreateMatch([FromBody] Match match)
            {
                _context.Add(match);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetMatches), new { id = match.Id }, match);
            }
        }


    }
    


