using CommunityToolkit.Mvvm.Input;
using DotNetMaui.Models;

namespace DotNetMaui.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}