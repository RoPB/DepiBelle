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

        private Func<Task> AfterCloseModal;
        public ICommand PlayAnimationCommand { get; set; }
        public bool IsAnimationVisible
        {
            get { return _isAnimationVisible; }
            set { SetPropertyValue(ref _isAnimationVisible, value); }
        }


        public ConfirmationModalViewModel(){
            IsLoading = true;
        }

        public override async Task InitializeAsync(object parameter=null)
        {

            AfterCloseModal = parameter as Func<Task>;
            IsLoading = false;
            CloseModalCommand = new Command(async () =>
            {
                PlayAnimationCommand.Execute(new LottieProgress() { Loop = false, From = 0.5f, To = 1 });
                await CloseModal();
                await AfterCloseModal.Invoke();
            });

            IsAnimationVisible = true;
            PlayAnimationCommand.Execute(new LottieProgress() { Loop = false, From = 0, To = 1});//true 0.5f

        }

    }
}
