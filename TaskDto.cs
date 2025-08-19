using System.ComponentModel.DataAnnotations;

namespace TaskManager
{
    public class TaskResponseDto //Output DTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public bool IsCompleted { get; set; }
    }

    public class TaskRequestDto //Input DTO(POST METHOD)
    {
        [Required(ErrorMessage = "Title Field is required")]
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }

    public class UpdateTaskDto //Inout DTO(Update)
    {
        [Required(ErrorMessage = "Title Field is required")]
        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }
}