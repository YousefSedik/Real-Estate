using System.ComponentModel.DataAnnotations;

namespace RealStats.ViewModel
{
    public class ReportIssueViewModel
    {
        [Required(ErrorMessage = "Please enter a title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; }
    }
}