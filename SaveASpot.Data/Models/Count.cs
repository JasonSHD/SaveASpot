using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveASpot.Data.Models
{
    public class Count
    {
        public int Total { get; set; }
        public int Paid { get; set; }

        public int Percentage
        {
            get
            {
                var total = Total == 0 ? 1 : Total;
                var calc = (float)Paid / (float)total;

                return Convert.ToInt32(calc * 100);
            }
        }
    }
}
