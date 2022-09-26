using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rocky.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Microsoft.Build.Framework.Required]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Microsoft.Build.Framework.Required]
        [Range(1,Int32.MaxValue, ErrorMessage = "Display Order for category must be greater than 0")]
        public int DisplayOrder { get; set; }
    }
}
