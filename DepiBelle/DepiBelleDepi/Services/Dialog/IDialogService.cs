using System;
using System.Threading.Tasks;

namespace DepiBelleDepi.Services.Dialog
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);
    }
}
