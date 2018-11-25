using System;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Models;
using DepiBelle.Utilities;
using Xamarin.Forms;

namespace DepiBelle.ViewModels
{
    public class WelcomeViewModel : ViewModelBase
    {
        private string _name;
        private bool _canContinue;
        private TimeSpan _currentTime;

        public ICommand ContinueCommand { get; set; }

        public string Name
        {
            get { return _name; }
            set { SetPropertyValue(ref _name, value); }
        }

        public TimeSpan CurrentTime
        {
            get { return _currentTime; }
            set { SetPropertyValue(ref _currentTime, value); }
        }

        public bool CanContinue
        {
            get { return _canContinue; }
            set { SetPropertyValue(ref _canContinue, value); }
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
                CurrentTime = DateTime.Now.TimeOfDay;
                Name = "";
                CanContinue = false;
                var dateNow = DateTime.Now;

            }
            catch (Exception ex)
            {

            }
        }

        private async Task Continue()
        {
            var navParam = new HomeTabbedNavigationParam() { Name = Name, Time = DateConverter.ShortTime(CurrentTime) };
            await NavigationService.NavigateToAsync<HomeTabbedViewModel>(navParam);
        }
    }
}
