using System;
using System.ComponentModel.DataAnnotations;

namespace TheWorld.ViewModels
{
  public class ContactViewModel
  {
    [Required]
    [StringLength(255, MinimumLength = 5)]
    public string FirstName { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string LastName { get; set; }

        [Required]
    [EmailAddress]
    public string Email { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string PostCode { get; set; }

        [Required]
    [StringLength(1024, MinimumLength = 5)]
    public string Message { get; set; }
  }
}
