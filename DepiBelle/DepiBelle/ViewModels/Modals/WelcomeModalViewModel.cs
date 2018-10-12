using System;
using System.Threading.Tasks;

namespace DepiBelle.ViewModels.Modals
{
    public class WelcomeModalViewModel:ModalViewModelBase
    {
        public override Task InitializeAsync(object parameter)
        {
            return Task.FromResult(false);
        }
    }
}
