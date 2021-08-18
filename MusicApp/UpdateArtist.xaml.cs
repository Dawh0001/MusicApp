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
    public sealed partial class UpdateArtist : Page
    {

        public UpdateArtist()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            loadingPanel.Visibility = Visibility.Visible;
            cmbArtists.ItemsSource = await ArtistPresentation.LoadArtists();
            loadingPanel.Visibility = Visibility.Collapsed;
        }
        
        private async System.Threading.Tasks.Task UpdateArtistToDb()
        {
            if (cmbArtists.SelectedItem != null)
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    Artist artist = (Artist)cmbArtists.SelectedItem;
                    artist.Name = inputName.Text;
                    artist.YearOfBirth = (int)inputYearOfBirth.Value;
                    string URL = App.baseURL + "Artists/" + artist.Id;

                    string jsonString = JsonConvert.SerializeObject(artist);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var response = await httpClient.PutAsync(URL, content);

                    if (response.IsSuccessStatusCode)
                    {
                        loadingPanel.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        progRing.IsActive = false;
                        loadingText.Text = "Error the artist was not updated";

                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    progRing.IsActive = false;
                    loadingText.Text = "Error the artist was not updated";
                }
            }
            else
            {
                var dialog = new MessageDialog("You need to choose an artist");
                await dialog.ShowAsync();
            }
        }
        private async System.Threading.Tasks.Task DeleteArtistFromDb()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                Artist artist = (Artist)cmbArtists.SelectedItem;
                string URL = App.baseURL + "Artists/" + artist.Id;
                var response = await httpClient.DeleteAsync(URL);
                if (response.IsSuccessStatusCode)
                {
                    loadingPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    progRing.IsActive = false;
                    loadingText.Text = "Error the record was not deleted";

                }
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                progRing.IsActive = false;
                loadingText.Text = "Error the artist was not deleted";
            }

        }
        private void cmbArtists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Artist artist = (Artist)cmbArtists.SelectedItem;
            inputName.Text = artist.Name;
            inputYearOfBirth.Value = artist.YearOfBirth;
        }
        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            loadingText.Text = "Saving please wait";
            loadingPanel.Visibility = Visibility.Visible;
            await DeleteArtistFromDb();
            var dialog = new MessageDialog("The artist has been succsesfully deleted");
            await dialog.ShowAsync();
        }
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            loadingText.Text = "Saving please wait";
            loadingPanel.Visibility = Visibility.Visible;
            await UpdateArtistToDb();
            var dialog = new MessageDialog("Your artist has been succsesfully saved");
            await dialog.ShowAsync();
            inputName.Text = "";
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
