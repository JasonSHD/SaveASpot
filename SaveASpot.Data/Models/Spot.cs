using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SaveASpot.Data.Models
{
    public class Spot
    {
        public Spot()
        {
            SpotShape = new List<Coordinate>();
        }

        [BsonId]
        public ObjectId SpotID { get; set; }

        public string SpotLength { get; set; }
        public string SpotPrice { get; set; }
        public string SpotArea { get; set; }

        public ObjectId ParcelId { get; set; }
        public ObjectId CustomerId { get; set; }
        public ObjectId SponsorId { get; set; }
        public ObjectId PhaseID { get; set; }

        public List<Coordinate> SpotShape { get; set; }

        public bool Validate()
        {
            return true;
        }
    }
}
