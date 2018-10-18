using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DepiBelle.ViewModels
{
    public class WelcomeViewModel : ViewModelBase
    {
        private string _name;

        public ICommand ContinueCommand { get; set; }

        public string Name
        {
            get { return _name; }
            set { SetPropertyValue(ref _name, value); }
        }

        public WelcomeViewModel()
        {
            ContinueCommand = new Command(async () => await Continue());
        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            try
            {
                //RESOLVE ISSUE: 
                //Setting the name it fires the trigger of the continue button to set the value in IsEnabled=false
                //Have to put Binding Name TwoWay
                Name = "";
            }
            catch (Exception ex)
            {

            }
        }

        private async Task Continue()
        {

            await NavigationService.NavigateToAsync<HomeTabbedViewModel>(Name);
        }
    }
}
