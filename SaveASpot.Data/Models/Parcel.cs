using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SaveASpot.Data.Models
{
    public class Parcel
    {
        public Parcel()
        {
            ParcelShape = new List<Coordinate>();
        }

        [BsonId]
        public ObjectId ParcelID { get; set; }
        public ObjectId PhaseId { get; set; }

        public string ParcelName { get; set; }
        public string ParcelLength { get; set; }
        public string ParcelAcres { get; set; }
        public string ParcelArea { get; set; }

        public List<Coordinate> ParcelShape { get; set; }

        public bool Validate()
        {
            return true;
        }
    }
}
