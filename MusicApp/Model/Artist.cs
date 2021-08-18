using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Model
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int YearOfBirth { get; set; }
        public List<Record> Records { get; set; }

        public static async Task<List<Artist>> LoadArtists()
        {
            try
            {
                string URL = App.baseURL + "Artists";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var artistList = JsonConvert.DeserializeObject<List<Artist>>(responseContent);
                    return artistList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return new List<Artist>();
        }
    }
   
}
