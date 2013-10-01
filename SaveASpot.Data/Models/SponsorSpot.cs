using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SaveASpot.Data.Models
{
    public class SponsorSpot
    {
        public SponsorSpot()
        {
            SpotShape = new List<Coordinate>();
        }

        [BsonId]
        public ObjectId SpotID { get; set; }
        public ObjectId PhaseID { get; set; }

        public List<Coordinate> SpotShape { get; set; }
    }
}
