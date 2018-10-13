using System;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Models.Lottie;
using Xamarin.Forms;

namespace DepiBelle.ViewModels.Modals
{
    public class ConfirmationModalViewModel : ModalViewModelBase
    {
        private bool _isAnimationVisible;

        public ICommand PlayAnimationCommand { get; set; }
        public bool IsAnimationVisible
        {
            get { return _isAnimationVisible; }
            set { SetPropertyValue(ref _isAnimationVisible, value); }
        }

        public ConfirmationModalViewModel()
        {

        }

        public override async Task InitializeAsync(object parameter=null)
        {
            _isAnimationVisible = true;
            PlayAnimationCommand.Execute(new LottieProgress(){Loop=true,From=0.5f,To=1});
        }

    }
}
