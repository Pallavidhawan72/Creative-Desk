using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreativeDesk.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = null!;

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Deadline is required")]
        public DateTime Deadline { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total Amount must be zero or positive")]
        public decimal TotalAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount Paid must be zero or positive")]
        public decimal AmountPaid { get; set; }

        [BindNever]
        [Display(Name = "Remaining Amount")]
        public decimal RemainingAmount { get; set; }

        [Required(ErrorMessage = "Client is required")]
        public int? ClientId { get; set; }

        public Client? Client { get; set; }

        public ICollection<ProjectDesigner> ProjectDesigners { get; set; } = new List<ProjectDesigner>();

        // New property for view use only
        [NotMapped]
        public IEnumerable<Designer> Designers => ProjectDesigners?.Select(pd => pd.Designer) ?? Enumerable.Empty<Designer>();
    }
}
