namespace CryptoBlazor.Data
{
    public class Crypto
    {
        public decimal Mts { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Volume { get; set; }

        public Crypto()
        {

        }

        public Crypto(string a, string b, string c, string d, string e, string f)
        {
            Mts = decimal.Parse(a);
            Open =decimal.Parse(b);
            Close = decimal.Parse(c);
            High = decimal.Parse(d);
            Low = decimal.Parse(e);
            Volume = decimal.Parse(f);

        }


    }
}
