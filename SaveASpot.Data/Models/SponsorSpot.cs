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
            Taken = false;
        }

        [BsonId]
        public ObjectId SpotID { get; set; }
        public ObjectId PhaseID { get; set; }

        public bool Taken { get; set; }
        public ObjectId SponsorID { get; set; }

        [BsonIgnore]
        public string SpotIDString { get { return SpotID.ToString(); } }
        [BsonIgnore]
        public string PhaseIDString { get { return PhaseID.ToString(); } }
        [BsonIgnore]
        public string SponsorIDString { get { return SponsorID.ToString(); } }

        public List<Coordinate> SpotShape { get; set; }
    }
}
