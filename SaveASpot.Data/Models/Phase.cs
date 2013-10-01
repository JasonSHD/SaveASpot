using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SaveASpot.Data.Models
{
    public class Phase
    {
        public Phase()
        {
            Parcels = new List<Parcel>();
            Active = false;
            Complete = false;
        }

        [BsonId]
        public ObjectId PhaseID { get; set; }
        public string PhaseName { get; set; }
        public int SpotPrice { get; set; }
        public bool Active { get; set; }
        public bool Complete { get; set; }

        [BsonIgnore]
        public string ID
        {
            get
            {
                return PhaseID.ToString();
            }
        }

        [BsonIgnore]
        public List<Parcel> Parcels { get; set; }

        public bool Validate()
        {
            return true;
        }
    }
}
