using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SaveASpot.Data.Models;

namespace SaveASpot.Areas.Settings.Models
{
    public class SponsorDetail
    {
        public SponsorDetail()
        {
        }

        public Sponsor Sponsor { get; set; }

        public Count SpotInfo { get; set; }
        public Count SponsorInfo { get; set; }
    }
}