using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Model;

public class TaskModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Title Field is required")]
    public string Title { get; set; } = string.Empty;

    [Column(TypeName = "boolean")]
    public bool IsCompleted { get; set; }
}