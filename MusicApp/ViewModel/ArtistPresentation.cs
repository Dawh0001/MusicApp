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
    class ArtistPresentation : Artist
    {
        public string RecordsString
        {
            get
            {
                string records = "";
                // creating a string with all artists
                for (int i = 0; i < base.Records.Count; i++)
                {
                    //prevents from begining and endig on ","
                    if (i != 0)
                    {
                        records += ", " + base.Records[i].Name;
                    }
                    else
                    {
                        records = base.Records[i].Name;
                    }
                }
                return records;
            }
        }

        public static async Task<List<ArtistPresentation>> LoadArtists()
        {
            try
            {
                string URL = App.baseURL + "Artists";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var artistList = JsonConvert.DeserializeObject<List<ArtistPresentation>>(responseContent);
                    return artistList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return new List<ArtistPresentation>();
        }
    }
}
