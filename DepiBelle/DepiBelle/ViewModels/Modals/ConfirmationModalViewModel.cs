using System;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Models;
using Xamarin.Forms;

namespace DepiBelle.ViewModels.Modals
{
    public class ConfirmationModalViewModel : ModalViewModelBase
    {

        private bool _processing;
        private string _name;
        private int _number;

        private Func<bool, Task> AfterCloseModal;
        public ICommand PlayAnimationCommand { get; set; }


        public bool Processing
        {
            get { return _processing; }
            set { SetPropertyValue(ref _processing, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetPropertyValue(ref _name, value); }
        }

        public int Number
        {
            get { return _number; }
            set { SetPropertyValue(ref _number, value); }
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
                    Number = order.Number;
                    Name = order.Name;
                    PlayAnimationCommand.Execute(new LottieProgress() { Loop = false, From = 0.5f, To = 1 });
                    Processing = false;
                    await Task.Delay(10000);
                }

                await CloseModal();
                await AfterCloseModal.Invoke(order==null);
            });

            PlayAnimationCommand.Execute(new LottieProgress() { Loop = true, From = 0, To = 0.5f });

        }

    }
}
