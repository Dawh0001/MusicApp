using MusicApp.Model;
using MusicApp.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UpdateRecord : Page
    {
        List<Artist> selectedArtists = new List<Artist>();
        string selectedArtistsString = "";

        

        public UpdateRecord()
        {
            this.InitializeComponent();            
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            loadingPanel.Visibility = Visibility.Visible;
            cmbGenres.ItemsSource = await GenrePresentation.LoadGenres();
            cmbRecords.ItemsSource = await RecordPresentation.LoadRecords();
            cmbArtists.ItemsSource = await Artist.LoadArtists();
            loadingPanel.Visibility = Visibility.Collapsed;
        } 
        private async System.Threading.Tasks.Task UpdateRecordToDb()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                RecordPresentation recordPresentation = (RecordPresentation)cmbRecords.SelectedItem;
                Record record = new Record();
                record.Id = recordPresentation.Id;
                record.Name = inputName.Text;
                record.YearOfRelease = (int)inputYearOfRelease.Value;
                if (cmbGenres.SelectedItem != null)
                {
                    record.Genre = (Genre)cmbGenres.SelectedItem;
                }
                else
                {
                    record.Genre = recordPresentation.Genre;
                }
                record.Artists = selectedArtists;
                string URL = App.baseURL + "Records/" + record.Id;

                string jsonString = JsonConvert.SerializeObject(record);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(URL, content);

                if (response.IsSuccessStatusCode)
                {
                    loadingPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    progRing.IsActive = false;
                    loadingText.Text = "Error the record was not updated";

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                progRing.IsActive = false;
                loadingText.Text = "Error the record was not updated";
            }
            loadingPanel.Visibility = Visibility.Collapsed;
        }
        private async System.Threading.Tasks.Task DeleteRecordFromDb()
        {
            HttpClient httpClient = new HttpClient();
            RecordPresentation recordPresentation = (RecordPresentation)cmbRecords.SelectedItem;
            string URL = App.baseURL + "Records/" + recordPresentation.Id;
            var response = await httpClient.DeleteAsync(URL);
            if (response.IsSuccessStatusCode)
            {
                this.Frame.Navigate(typeof(UpdateRecord));
            }
            else
            {
                progRing.IsActive = false;
                loadingText.Text = "Error the record was not deleted";

            }
        }
        private void cmbRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Artist> a = new List<Artist>();
            RecordPresentation record = (RecordPresentation)cmbRecords.SelectedItem;
            inputName.Text = record.Name;
            inputYearOfRelease.Value = record.YearOfRelease;
            cmbGenres.Text = record.Genre.Name;
            cmbGenres.PlaceholderText = record.Genre.Name;
            cmbGenres.SelectedItem = record.Genre.Id;
            artistString.Text = record.ArtistsString;
            a = record.Artists;
            selectedArtists = a;
        }

        private void cmbGenres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Genre genre = (Genre)cmbGenres.SelectedItem;
            cmbGenres.Text = genre.Name;
        }   

        private void cmbArtists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Artist artist = (Artist)cmbArtists.SelectedItem;
            Artist artistFromList = selectedArtists.Find(a => a.Id == artist.Id);
            if (artistFromList==null)
            {
                selectedArtists.Add(artist);
                artistString.Text += artist.Name + ", ";
            }
            else
            {
                //removes the artist if it gets picked twice
                selectedArtists.Remove(artistFromList);
                string stringToRemove1 = ", " + artist.Name;
                string stringToRemove2 = artist.Name + ", ";
                string stringToRemove3 = artist.Name;
                if (artistString.Text.Contains(stringToRemove1))
                {
                    artistString.Text = artistString.Text.Replace(stringToRemove1, "");
                }
                else if (artistString.Text.Contains(stringToRemove2)) 
                {
                    artistString.Text = artistString.Text.Replace(stringToRemove2, "");
                }
                else if (artistString.Text.Contains(stringToRemove3))
                {
                    artistString.Text = artistString.Text.Replace(stringToRemove3, "");
                }
            }

        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            loadingText.Text = "Saving please wait";
            loadingPanel.Visibility = Visibility.Visible;
            await DeleteRecordFromDb();
            var dialog = new MessageDialog("The record has been succsesfully deleted");
            await dialog.ShowAsync();
        }
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            loadingText.Text = "Saving please wait";
            loadingPanel.Visibility = Visibility.Visible;
            await UpdateRecordToDb();
            var dialog = new MessageDialog("The record has been succsesfully updated");
            await dialog.ShowAsync();
        }
        #region Navigation

        private void mnuViewRecords_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void mnuViewArtists_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ViewArtists));
        }

        private void mnuViewGenres_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ViewGenres));
        }
        private void mnuUpdateRecord_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UpdateRecord));
        }
        private void mnuUpdateArtist_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UpdateArtist));
        }
        private void mnuUpdateGenre_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UpdateGenre));
        }
        private void mnuAddRecord_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddRecord));
        }
        private void mnuAddArtist_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddArtist));
        }
        private void mnuAddGenre_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddGenre));
        }
        #endregion

        
    }
}
