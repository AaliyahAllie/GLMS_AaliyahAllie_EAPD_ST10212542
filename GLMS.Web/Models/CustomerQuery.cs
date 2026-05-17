using System.ComponentModel.DataAnnotations;

namespace GLMS.Web.Models
{
    public class CustomerQuery
    {
        public int CustomerQueryId { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        public string ServiceNeeded { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }
}