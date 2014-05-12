using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaveASpot.Data.Models;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace SaveASpot.Data.Controllers
{
    public class CartCollection
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="roles">Roles in the db</param>
        /// <param name="userRoles">User/Role association in the db</param>
        public CartCollection(MongoCollection<Cart> carts)
        {
            this._carts = carts;
        }

        /// <summary>
        /// Roles objects accessible from the database.
        /// </summary>
        private MongoCollection<Cart> _carts = null;
        protected MongoCollection<Cart> Carts
        {
            get
            {
                return this._carts;
            }
        }

        /// <summary>
        /// Inserts a new role into the db.
        /// </summary>
        /// <param name="role">The role to add.</param>
        /// <returns>true if the role is valid and successfully added.</returns>
        public bool AddItem(string clientIP, CartItem item, ref Cart cart)
        {
            // first check to see if the cart doesn't currently exist
            var db = (from c in Carts.AsQueryable() where c.ClientIP == clientIP select c);

            // retrieve or create cart
            if (db.Count() > 0)
            {
                cart = db.First();
            }
            else
            {
                cart = new Cart() { ClientIP = clientIP, CartID = ObjectId.GenerateNewId(), Items = new List<CartItem>() };
            }

            // set the item
            bool found = false;
            foreach (var itm in cart.Items)
            {
                if (itm.SpotID == item.SpotID)
                {
                    found = true;
                    itm.Qty += item.Qty;
                    if (item.Qty == 0)
                    {
                        cart.Items.Remove(itm);
                    }
                    break;
                }
            }

            if (!found)
            {
                cart.Items.Add(item);
            }

            // now save
            var result = Carts.Save(cart, WriteConcern.Acknowledged);

            return result.Ok;
        }

        /// <summary>
        /// Returns the role by their role id
        /// </summary>
        /// <param name="roleId">The id of the role to retrieve</param>
        /// <returns>the role found or null</returns>
        public Cart GetCart(ObjectId id, string clientIP)
        {
            var db = (from c in Carts.AsQueryable() where c.ClientIP == clientIP select c);
            return (db.Count() > 0) ? db.First() : null;
        }

        /// <summary>
        /// Returns the role by their role id
        /// </summary>
        /// <param name="roleId">The id of the role to retrieve</param>
        /// <returns>the role found or null</returns>
        public bool DeleteCart(ObjectId id, string clientIP)
        {
            var db = (from c in Carts.AsQueryable() where c.ClientIP == clientIP select c).First(); 
            FindAndModifyResult spotResult = Carts.FindAndRemove(Query.EQ("_id", db.CartID), SortBy.Ascending("_id"));
            return spotResult.Ok;
        }

        /// <summary>
        /// Initializes all the needed indexes for the pages to optimize search on them.
        /// </summary>
        /*public void InitializeIndexes()
        {
            // key options
            var options = new IndexOptionsBuilder().SetUnique(true);

            // page search key
            var keys = new IndexKeysBuilder().Ascending("ParcelName");
            Parcels.EnsureIndex(keys, options);

            // general page search
            keys = new IndexKeysBuilder().Ascending("_id");
            Parcels.EnsureIndex(keys, options);
        }*/
    }
}
