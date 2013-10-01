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
        }

        [BsonId]
        public ObjectId CartID { get; set; }
        public List<CartItem> Items { get; set; }


        public ObjectId SponsorID { get; set; }
    }

    public class CartItem
    {
        public ObjectId SpotID { get; set; }
        public int Price { get; set; }
        public string Phase { get; set; }
    }
}
