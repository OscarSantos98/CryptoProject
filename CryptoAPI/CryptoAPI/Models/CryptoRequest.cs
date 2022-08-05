namespace CryptoAPI.Models
{
    public class CryptoRequest
    {
        // Path Params
        // '1m', '5m', '15m', '30m', '1h', '3h', '6h', '12h', '1D', '1W', '14D', '1M'
        public string TimeFrame { get; set; }
        // tBTCUSD, tETHUSD, fUSD, fBTC
        public string Symbol { get; set; }
        // "last", "hist"
        public string Section { get; set; }
        // Only required for funding candles. Enter after the symbol (trade:1m:fUSD:p30/hist).
        public string? Period { get; set; }

        // Query Params
        public Int32? limit { get; set; }
        public string? Start { get; set; }
        public string? End { get; set; }
        public Int32? sort { get; set; }

    }
}
