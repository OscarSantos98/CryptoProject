using CsvHelper;
using System.Globalization;
using TradingView.Blazor.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text;

namespace CryptoBlazor.Data
{
    public class ChartService
    {
        public readonly HttpClient _httpClient;
        public readonly AppSettings _appSettings;

        public ChartService()
        {
            _appSettings = new AppSettings();
            _httpClient = new HttpClient();
        }


        //get data from API
        public async Task<List<Ohlcv>> ProcessDataAsync(CryptoRequest cryptoRequest)
        {

            //request to the bitfinex API directly 
            var RequestLink = new StringBuilder("https://api-pub.bitfinex.com/v2/candles/trade:" +
                cryptoRequest.TimeFrame);
            RequestLink.Append(":t" + cryptoRequest.Symbol.ToUpper() + "/hist?limit="  +cryptoRequest.limit + "&sort=-1");
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, RequestLink.ToString());

            //request to the local API
            //HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, "https://localhost:44375/api/Crypto");

            string serializedRequest = JsonConvert.SerializeObject(cryptoRequest);
            httpRequestMessage.Content = new StringContent(serializedRequest);
            httpRequestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await _httpClient.SendAsync(httpRequestMessage);
            var rawJson = await response.Content.ReadAsStringAsync();

            //formatting raw json into an array with all the value
            rawJson = rawJson.Replace("[", "");
            rawJson = rawJson.Replace("]", "");
            var valuesArray = rawJson.Split(',');

            //initialize candles variables for Ohlcv class
            DateTime startDate;
            decimal open;
            decimal high;
            decimal low;
            decimal close;
            decimal volume;

            List<Ohlcv> listData = new List<Ohlcv>();
            List<Ohlcv> listDataOrdered = new List<Ohlcv>();
            
            //formatting values into a list of Ohlcv
            for (int i = 0; i < valuesArray.Length; i=i+6)
            {
                startDate = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(valuesArray[i])).UtcDateTime;        //converting milliseconds to standard UNIX timestamp
                open = System.Convert.ToDecimal(valuesArray[i + 1], CultureInfo.InvariantCulture);                  //ignoring culture info to keep "."(english) instead of "," (french) in deciaml numbers
                close = System.Convert.ToDecimal(valuesArray[i + 2], CultureInfo.InvariantCulture);
                high = System.Convert.ToDecimal(valuesArray[i + 3], CultureInfo.InvariantCulture);
                low = System.Convert.ToDecimal(valuesArray[i + 4], CultureInfo.InvariantCulture);
                volume = System.Convert.ToDecimal(valuesArray[i + 5], CultureInfo.InvariantCulture);
                var itemOhlcv= new Ohlcv(startDate, open, high, low, close, volume);                                //creating new Ohlcv item with the 6 values 
                listData.Add(itemOhlcv);
            }
            
            //ordering listData into listDataOrdered from oldest to newest value
            IEnumerable<Ohlcv> query =listData.OrderBy(Ohlcv => Ohlcv.Time);
            foreach (Ohlcv item in query)
            {
                listDataOrdered.Add(item);
            }

            return listDataOrdered;

        }
    }
}
