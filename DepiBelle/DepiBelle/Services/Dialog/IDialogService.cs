using System;
using System.Threading.Tasks;

namespace DepiBelle.Services.Dialog
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);
    }
}
