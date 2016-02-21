using System;
using System.ComponentModel.DataAnnotations;

namespace FunWithGit.ViewModels
{
  public class QuoteViewModel
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

  }
}
