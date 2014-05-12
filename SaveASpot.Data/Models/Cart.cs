using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SaveASpot.Data.Models
{
    public class Cart
    {
        public Cart()
        {
            Items = new List<CartItem>();
            Years = new List<int>();
        }

        [BsonId]
        public ObjectId CartID { get; set; }
        public string ClientIP { get; set; }
        public List<CartItem> Items { get; set; }


        public ObjectId SponsorID { get; set; }
        public List<int> Years { get; set; }
    }

    public class CartItem
    {
        public CartItem()
        {
            Qty = 1;
        }
        public ObjectId PhaseID { get; set; }
        public ObjectId SpotID { get; set; }
        public ObjectId SponsorID { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }
        public string Phase { get; set; }
        public string Sponsor { get; set; }
        public double SponsorPrice { get; set; }
    }
}
