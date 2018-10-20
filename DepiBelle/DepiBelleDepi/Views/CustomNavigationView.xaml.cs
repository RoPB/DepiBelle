using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace DepiBelleDepi.Views
{
    public partial class CustomNavigationView : NavigationPage
    {
        public CustomNavigationView()
        {
            InitializeComponent();
        }

        public CustomNavigationView(Page root) : base(root)
        {
            InitializeComponent();
        }
    }
}
