using System;
using System.Collections.Generic;

namespace CryptoAPI.Models
{
    public partial class Crypto
    {
        public int CryptoId { get; set; }
        public int? UserId { get; set; }
        public string? CoupleName { get; set; }
        public double? Value { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual User? User { get; set; }
    }
}
