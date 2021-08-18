using MusicApp.Model;
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
    public sealed partial class AddArtist : Page
    {
        
        public AddArtist()
        {
            this.InitializeComponent();
        }
        public async System.Threading.Tasks.Task AddArtistToDb()
        {
            try
            {
                string URL = App.baseURL + "Artists";
                HttpClient httpClient = new HttpClient();
                Artist newArtist = new Artist();
                newArtist.Name = inputName.Text;
                newArtist.YearOfBirth = (int)inputYearOfBirth.Value;

                string jsonString = JsonConvert.SerializeObject(newArtist);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(URL, content);

                if (response.IsSuccessStatusCode)
                {                    
                    var dialog = new MessageDialog("Your artist has been succsesfully saved");
                    await dialog.ShowAsync();
                    inputName.Text = "";
                    inputYearOfBirth.Value = 0;
                }
                else
                {
                    var dialog = new MessageDialog("Error your artist has not been saved");
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
            await AddArtistToDb();
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
