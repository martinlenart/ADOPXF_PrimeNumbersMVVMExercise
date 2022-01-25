

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrimeNumbers.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageFlyout : ContentPage
    {
        public ListView ListView;

        public MainPageFlyout()
        {
            InitializeComponent();

            BindingContext = new MainPageFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class MainPageFlyoutViewModel
        {
            public ObservableCollection<MainPageFlyoutMenuItem<int?>> MenuItems { get; set; }

            public MainPageFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<MainPageFlyoutMenuItem<int?>>(new[]
                {
                    new MainPageFlyoutMenuItem<int?> { Id = 0, Title = "About Primes", TargetType=typeof(AboutPage) },
                    new MainPageFlyoutMenuItem<int?> { Id = 1, Title = "Debug Console", TargetType=typeof(ConsolePage) },
                    new MainPageFlyoutMenuItem<int?> { Id = 2, Title = "Find Primenumbers 1", TargetType=typeof(PrimesPage1) },
                    new MainPageFlyoutMenuItem<int?> { Id = 3, Title = "Find Primenumbers 2", TargetType=typeof(PrimesPage2), Param = 5},
                    new MainPageFlyoutMenuItem<int?> { Id = 4, Title = "Find Primenumbers 3", TargetType=typeof(PrimesPage3), Param = 5},
                    new MainPageFlyoutMenuItem<int?> { Id = 5, Title = "Find Primenumbers 4", TargetType=typeof(PrimesPage4), Param = 5},
                });
            }
        }
    }
}