using System.ComponentModel.DataAnnotations.Schema;

namespace People.API.Models
{
    [Table("people")]
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
