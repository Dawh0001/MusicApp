using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Model
{

    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Record> Records { get; set; }
        public static async Task<List<Genre>> LoadGenres()
        {
            List<Genre> genres = new List<Genre>();
            try
            {
                string URL = App.baseURL + "Genres";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    genres = JsonConvert.DeserializeObject<List<Genre>>(responseContent);
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
