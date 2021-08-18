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
using System.Threading.Tasks;
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
    public sealed partial class UpdateGenre : Page
    {

        public UpdateGenre()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            loadingPanel.Visibility = Visibility.Visible;
            cmbGenres.ItemsSource = await GenrePresentation.LoadGenres();
            loadingPanel.Visibility = Visibility.Collapsed;

        }        
        private async System.Threading.Tasks.Task UpdateGenreToDb()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                Genre genre = (Genre)cmbGenres.SelectedItem;
                genre.Name = inputName.Text;
                string URL = App.baseURL + "Genres/" + genre.Id;

                string jsonString = JsonConvert.SerializeObject(genre);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(URL, content);

                if (response.IsSuccessStatusCode)
                {
                    loadingPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    progRing.IsActive = false;
                    loadingText.Text = "Error the genre was not updated";

                }
                loadingPanel.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                progRing.IsActive = false;
                loadingText.Text = "Error the genre was not updated";
            }
            loadingPanel.Visibility = Visibility.Collapsed;
        }
        private async System.Threading.Tasks.Task DeleteGenreFromDb()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                Genre genre = (Genre)cmbGenres.SelectedItem;
                string URL = App.baseURL + "Genres/" + genre.Id;
                var response = await httpClient.DeleteAsync(URL);
                if (response.IsSuccessStatusCode)
                {
                    loadingPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    progRing.IsActive = false;
                    loadingText.Text = "Error the genre was not deleted";

                }
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                progRing.IsActive = false;
                loadingText.Text = "Error the genre was not deleted";
            }

        }
        private void cmbGenres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GenrePresentation genre = (GenrePresentation)cmbGenres.SelectedItem;
            inputName.Text = genre.Name;
            records.Text = genre.RecordsString;
        }
        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            loadingText.Text = "Saving please wait";
            loadingPanel.Visibility = Visibility.Visible;
            await DeleteGenreFromDb();
            loadingPanel.Visibility = Visibility.Collapsed;
            var dialog = new MessageDialog("The genre has been succsesfully deleted");
            await dialog.ShowAsync();
        }
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            loadingText.Text = "Saving please wait";
            loadingPanel.Visibility = Visibility.Visible;
            await UpdateGenreToDb();
            loadingPanel.Visibility = Visibility.Collapsed;
            var dialog = new MessageDialog("The genre has been succsesfully updated");
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
