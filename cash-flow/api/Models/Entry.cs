using api.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class Entry
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public OperationType Type { get; set; }

    public Currency Currency { get; set; }

    public decimal Value { get; set; }

    public required string Description { get; set; }

    public DateTime Date { get; set; }
}
