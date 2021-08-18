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
    class GenrePresentation : Genre
    {
        public string RecordsString
        {
            get
            {
                string records = "";
                foreach (var record in base.Records)
                {
                    records += record.Name + ", ";
                }
                if (records.Length > 2)
                {
                    records = records.Remove(records.Length - 2);
                }
                return records;
            }
        }
        public static async Task<List<GenrePresentation>> LoadGenres()
        {
            List<GenrePresentation> genres = new List<GenrePresentation>();
            try
            {
                string URL = App.baseURL + "Genres";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    genres = JsonConvert.DeserializeObject<List<GenrePresentation>>(responseContent);
                    return genres;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return genres;
        }
    }
}
