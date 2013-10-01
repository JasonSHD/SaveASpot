using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SaveASpot.Data.Models;

namespace SaveASpot.Areas.Settings.Models
{
    public class PhaseDetail
    {
        public PhaseDetail()
        {
        }

        public Phase Phase { get; set; }

        public Count SpotInfo { get; set; }
        public Count SponsorInfo { get; set; }

        public long SponsorCount { get; set; }
        public long SponsorPaid { get; set; }
        public int SponsorPercentage
        {
            get
            {
                return Convert.ToInt32((SponsorPaid / SponsorCount) * 100);
            }
        }
    }
}