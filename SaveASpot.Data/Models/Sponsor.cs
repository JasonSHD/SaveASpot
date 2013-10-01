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

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }

        public List<ObjectId> SponsoredSpots { get; set; }
    }
}
