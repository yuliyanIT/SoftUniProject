using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace FaceitRankChecker.Models
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
