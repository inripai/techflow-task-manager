using System.ComponentModel.DataAnnotations;

namespace techflow_task_manager.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        [Display(Name = "Título")]
        [Required(ErrorMessage = "Informe um título para a tarefa.")]
        public string Title { get; set; } = string.Empty;
        [Display(Name = "Descrição")]
        public string Description { get; set; } = string.Empty;
        [Display(Name = "Concluída")]
        public bool IsCompleted { get; set; }
    }
}
