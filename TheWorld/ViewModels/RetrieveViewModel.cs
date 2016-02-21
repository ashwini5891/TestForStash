using System;
using System.ComponentModel.DataAnnotations;

namespace FunWithGit.ViewModels
{
  public class RetrieveViewModel
  {
        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string QuoteNumber { get; set; }

  }
}
