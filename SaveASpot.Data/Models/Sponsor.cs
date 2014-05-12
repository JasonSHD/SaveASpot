using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SaveASpot.Data.Models
{
    public class Sponsor
    {
        public Sponsor()
        {
            SponsoredSpots = new List<ObjectId>();
        }

        [BsonId]
        public ObjectId SponsorID { get; set; }

        [BsonIgnore]
        public string SponsorIDToString
        {
            get
            {
                return SponsorID.ToString();
            }
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public bool ShowSponsor { get; set; }
        public int Order { get; set; }

        public List<ObjectId> SponsoredSpots { get; set; }

        public Coordinate Center { get; set; }
    }
}
