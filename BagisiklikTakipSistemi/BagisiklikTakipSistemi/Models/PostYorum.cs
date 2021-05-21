using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BagisiklikTakipSistemi.Models
{
    public class PostYorum
    {
        public IEnumerable<PostTablosu> Deger1 { get; set; }
        public IEnumerable<YorumlarTablosu> Deger2 { get; set; }

    }
}