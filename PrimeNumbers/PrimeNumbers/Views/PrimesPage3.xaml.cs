using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using PrimeNumbers.Services;
using PrimeNumbers.Models;

namespace PrimeNumbers.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrimesPage3 : ContentPage
    {
        public List<PrimeBatch> Primes { get; private set; }
        PrimeNumberService _service;
        Progress<float> _progressReporter;

        public PrimesPage3()
        {
            InitializeComponent();
            _service = new PrimeNumberService();
            BindingContext = this;

            _progressReporter = new Progress<float>(value =>
            {
                progressBar.Progress = value;
            });
        }

        public PrimesPage3(int NrBatches) : this()
        {
            enNrBatches.Text = NrBatches.ToString();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Code here will run right before the screen appears
            //You want to set the Title or set the City

            //This is making the first load of data
            MainThread.BeginInvokeOnMainThread(async () => { await LoadPrimes(); });
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await LoadPrimes();
        }

        private async Task LoadPrimes()
        {
          if (!int.TryParse(enNrBatches.Text, out int nrbatches)) return;
            
            lvPrimes.IsVisible = false;
            progressBar.IsVisible = true;
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            Primes = await _service.GetPrimeBatchCountsAsync(nrbatches, null);
            OnPropertyChanged("Primes");

            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
            progressBar.IsVisible = false;
            lvPrimes.IsVisible = true;
        }
    }
}