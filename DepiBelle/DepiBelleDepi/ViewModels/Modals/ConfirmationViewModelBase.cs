using System;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelleDepi.Models;
using Xamarin.Forms;

namespace DepiBelleDepi.ViewModels.Modals
{
    public class ConfirmationModalViewModel : ModalViewModelBase
    {

        private bool _processing;

        private Func<bool, Task> AfterCloseModal;
        public ICommand PlayAnimationCommand { get; set; }


        public bool Processing
        {
            get { return _processing; }
            set { SetPropertyValue(ref _processing, value); }
        }

        public ConfirmationModalViewModel()
        {
            Processing = true;
        }

        public override async Task InitializeAsync(object parameter = null)
        {

            AfterCloseModal = parameter as Func<bool, Task>;

            CloseModalCommand = new Command<Order>(async (order) =>
            {
                if (order != null)
                {
                    PlayAnimationCommand.Execute(new LottieProgress() { Loop = false, From = 0.5f, To = 1 });
                    Processing = false;
                    await Task.Delay(10000);
                }

                await CloseModal();
                await AfterCloseModal.Invoke(order == null);
            });

            PlayAnimationCommand.Execute(new LottieProgress() { Loop = true, From = 0, To = 0.5f });

        }

    }
}
