using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eEcomerce.BackEnd.Entities.Category;


[Table("Categories")]
public class Category : EntityBase
{
    [Required]
    public char Letter { get; set; }

    [Required]
    public string Name { get; set; } = null!;
}
