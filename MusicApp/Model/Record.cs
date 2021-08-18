using MusicApp.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusicApp.Model
{
    public class Record
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int YearOfRelease { get; set; }
        public Genre Genre { get; set; }
        public List<Artist> Artists { get; set; }
        public static async Task<List<Record>> LoadRecords()
        {
            try
            {
                string URL = App.baseURL + "Records";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var records = JsonConvert.DeserializeObject<List<Record>>(responseContent);
                    return records;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return new List<Record>();

        }
    }
}
