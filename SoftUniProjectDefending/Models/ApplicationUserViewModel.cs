using Microsoft.AspNetCore.Identity;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace FaceitRankChecker.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
