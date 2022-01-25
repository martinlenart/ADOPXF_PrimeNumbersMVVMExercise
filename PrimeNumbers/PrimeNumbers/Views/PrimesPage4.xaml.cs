using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using PrimeNumbers.Models;
using PrimeNumbers.Services;

namespace PrimeNumbers.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrimesPage4 : ContentPage
    {
        public List<PrimeBatch> Primes { get; private set; }
        PrimeNumberService _service;
        Progress<float> _progressReporter;

        public PrimesPage4()
        {
            InitializeComponent();
            _service = new PrimeNumberService();
            BindingContext = this;

            _progressReporter = new Progress<float>(value =>
            {
                progressBar.Progress = value;
            });
        }
        public PrimesPage4(int NrBatches) : this()
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

            lvPrimes.ItemsSource = await _service.GetPrimeBatchCountsAsync(nrbatches, _progressReporter);
            OnPropertyChanged("Primes");

            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
            progressBar.IsVisible = false;
            lvPrimes.IsVisible = true;
        }

        private async void lvPrimes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = (PrimeBatch)e.Item;
            var answer = await DisplayAlert("Write to disk?", 
                $"Would you like to write the {item.NrPrimes} prime numbers to disk", "Yes", "No");
            if (answer)
            {
                string userMessage=null, path=null;
                try
                {
                    path = await WriteAsync(item, $"Primes_from_{item.BatchStart}_to_{item.BatchEnd}.txt");
                    userMessage = "Write Completed";
                }
                catch (Exception ex)
                {
                    userMessage = $"Cannot write: {ex.Message}";
                }
                finally 
                {
                    await DisplayAlert(userMessage, $"Filename: {path}", "OK");
                }
            }
        }
        public async Task<string> WriteAsync(PrimeBatch batch, string filename)
        {
            List<int> primes = await _service.GetPrimesAsync(batch.BatchStart, PrimeBatch.BatchSize);
            string path = fname(filename);
            using (FileStream fs = File.Create(path))
            using (TextWriter writer = new StreamWriter(fs))
            {
                await writer.WriteLineAsync(batch.ToString());
                await writer.WriteLineAsync($"First Prime: {primes.First()}  Last Prime: {primes.Last()}");
                int nrPerLine = 50;
                for (int i = 0; i <= batch.NrPrimes; i++)
                {
                    string sPrimes = String.Join<int>(", ", primes.Take(nrPerLine));
                    await writer.WriteLineAsync(sPrimes);

                    if (primes.Count > nrPerLine)
                        primes.RemoveRange(0, nrPerLine);
                }
            }

            return path;
        }
        static string fname(string name)
        {
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            documentPath = System.IO.Path.Combine(documentPath, "AOOP2", "Examples");
            if (!Directory.Exists(documentPath)) Directory.CreateDirectory(documentPath);
            return System.IO.Path.Combine(documentPath, name);
        }
    }
}