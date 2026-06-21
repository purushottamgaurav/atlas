namespace DotNetMvc.ViewModels;

public class EditJobViewModel : CreateJobViewModel
{
    public int JobId { get; set; }
    public bool IsActive { get; set; } = true;
}
