using MusicApp.Model;
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
    public sealed partial class AddGenre : Page
    {
        
        public AddGenre()
        {
            this.InitializeComponent();
        }
        public async System.Threading.Tasks.Task AddGenreToDb()
        {
            try
            {
                string URL = App.baseURL + "Genres";
                HttpClient httpClient = new HttpClient();
                Genre newGenre = new Genre();
                newGenre.Name = inputName.Text;

                string jsonString = JsonConvert.SerializeObject(newGenre);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(URL, content);

                if (response.IsSuccessStatusCode)
                {
                    
                    var dialog = new MessageDialog("Your genre has been succsesfully saved");
                    await dialog.ShowAsync();
                    inputName.Text = "";
                }
                else
                {
                    var dialog = new MessageDialog("Error your genre has not been saved");
                    await dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            loadingPanel.Visibility = Visibility.Visible;
            await AddGenreToDb();
            loadingPanel.Visibility = Visibility.Collapsed;

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
