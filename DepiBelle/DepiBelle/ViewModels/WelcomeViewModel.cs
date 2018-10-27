using System;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Models;
using Xamarin.Forms;

namespace DepiBelle.ViewModels
{
    public class WelcomeViewModel : ViewModelBase
    {
        private string _name;
        private string _hour; 
        private string _minutes;
        private string _placeHolderHour;
        private string _placeHolderMinutes;
        private bool _canContinue;

        public ICommand ContinueCommand { get; set; }

        public string Name
        {
            get { return _name; }
            set { SetPropertyValue(ref _name, value); }
        }

        public string Hour
        {
            get { return _hour; }
            set { SetPropertyValue(ref _hour, value); }
        }

        public string Minutes
        {
            get { return _minutes; }
            set { SetPropertyValue(ref _minutes, value); }
        }

        public string PlaceHolderHour
        {
            get { return _placeHolderHour; }
            set { SetPropertyValue(ref _placeHolderHour, value); }
        }
        public string PlaceHolderMinutes
        {
            get { return _placeHolderMinutes; }
            set { SetPropertyValue(ref _placeHolderMinutes, value); }
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
                Name = "";
                Hour = "";
                Minutes = "";
                CanContinue = false;
                var dateNow = DateTime.Now;
                PlaceHolderHour = $"{dateNow.Hour}";
                PlaceHolderMinutes = $"{dateNow.Minute.ToString("00")}";

            }
            catch (Exception ex)
            {

            }
        }

        private async Task Continue()
        {
            var navParam = new HomeTabbedNavigationParam() { Name = Name, Time = $"{Hour}:{Minutes}" };
            await NavigationService.NavigateToAsync<HomeTabbedViewModel>(navParam);
        }
    }
}
