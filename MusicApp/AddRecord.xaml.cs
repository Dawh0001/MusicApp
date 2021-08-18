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
    public sealed partial class AddRecord : Page
    {
        List<Artist> selectedArtists = new List<Artist>();
        string selectedArtistsString = "";


        public AddRecord()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            loadingPanel.Visibility = Visibility.Visible;
            cmbArtists.ItemsSource = await Artist.LoadArtists();
            cmbGenres.ItemsSource = await Genre.LoadGenres();
            loadingPanel.Visibility = Visibility.Collapsed;
        }

        private async System.Threading.Tasks.Task AddRecordToDb()
        {
            if (cmbArtists.SelectedItem!=null && cmbGenres.SelectedItem!=null)
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    Record record = new Record();
                    record.Name = inputName.Text;
                    record.YearOfRelease = (int)inputYearOfRelease.Value;
                    if (cmbGenres.SelectedItem != null)
                    {
                        Genre genre = (Genre)cmbGenres.SelectedItem;
                        record.Genre = genre;
                    }
                    else
                    {
                        var dialog = new MessageDialog("Please select a genre");
                        await dialog.ShowAsync();
                        selectedArtists.Clear();
                        return;
                    }
                    record.Artists = selectedArtists;
                    string URL = App.baseURL + "Records";

                    string jsonString = JsonConvert.SerializeObject(record);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(URL, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var dialog = new MessageDialog("Your record has been succsesfully saved");
                        await dialog.ShowAsync();
                        selectedArtists.Clear();
                    }
                    else
                    {
                        var dialog = new MessageDialog("Error your record not been saved");
                        await dialog.ShowAsync();

                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
            else
            {
                var dialog = new MessageDialog("You need to select a genre and one artist");
                await dialog.ShowAsync();
            }

        }
        
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            loadingPanel.Visibility = Visibility.Visible;
            await AddRecordToDb();
            loadingPanel.Visibility = Visibility.Collapsed;


        }
        private void cmbArtists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Artist artist = (Artist)cmbArtists.SelectedItem;
            Artist artistFromList = selectedArtists.Find(a => a.Id == artist.Id);
            if (artistFromList == null)
            {
                selectedArtists.Add(artist);
                artistString.Text +=  artist.Name + ", ";
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