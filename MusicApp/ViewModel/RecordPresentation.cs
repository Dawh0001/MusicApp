using MusicApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.ViewModel
{
    class RecordPresentation : Record
    {
        public string ArtistsString
        {
            get
            {
                string artists = "";
                foreach (var artist in Artists)
                {
                    artists += artist.Name + ", ";
                }
                if (artists.Length > 2)
                {
                    artists = artists.Remove(artists.Length - 2);
                }
                return artists;
            }
        }
        public static async Task<List<RecordPresentation>> LoadRecords()
        {
            try
            {
                string URL = App.baseURL + "Records";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var records = JsonConvert.DeserializeObject<List<RecordPresentation>>(responseContent);
                    return records;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return new List<RecordPresentation>();

        }
    }
}
