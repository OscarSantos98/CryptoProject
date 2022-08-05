using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingView.Blazor.Models
{
    public class Ohlcv
    {
        public DateTime Time { get; set; }
        public decimal Open {  get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }

        public Ohlcv()
        {

        }

        public Ohlcv(DateTime time, decimal open, decimal high, decimal low, decimal close, decimal vol)
        {
            Time = time;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = vol;
        }
    }



}
