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
    public class ParcelCollection
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="roles">Roles in the db</param>
        /// <param name="userRoles">User/Role association in the db</param>
        public ParcelCollection(MongoCollection<Parcel> parcels)
        {
            this._parcels = parcels;
        }

        /// <summary>
        /// Roles objects accessible from the database.
        /// </summary>
        private MongoCollection<Parcel> _parcels = null;
        protected MongoCollection<Parcel> Parcels
        {
            get
            {
                return this._parcels;
            }
        }

        /// <summary>
        /// Inserts a new role into the db.
        /// </summary>
        /// <param name="role">The role to add.</param>
        /// <returns>true if the role is valid and successfully added.</returns>
        public bool Insert(Parcel parcel)
        {
            if (parcel.Validate())
            {
                parcel.ParcelID = ObjectId.GenerateNewId();
                SafeModeResult result = Parcels.Insert(parcel);

                return true;
            }
            else return false;
        }

        public bool InsertBatch(List<Parcel> parcels)
        {
            try {
                foreach(var parcel in parcels) { parcel.ParcelID = ObjectId.GenerateNewId(); }
                var result = Parcels.InsertBatch(parcels, WriteConcern.Unacknowledged);
            }
            catch{ return false; }
            return true;
        }

        // <summary>
        /// Deletes the role with the specified id
        /// </summary>
        /// <param name="roleId">Id of the role to delete</param>
        /// <returns>Result of the operation, true if was ok</returns>
        public bool Delete(ObjectId id)
        {
            Parcel parcel = Parcels.FindOneById(id);
            FindAndModifyResult parcelResult = Parcels.FindAndRemove(Query.EQ("_id", id), SortBy.Ascending("_id"));

            return parcelResult.Ok;
        }

        public bool DeleteByPhaseID(ObjectId id)
        {
            WriteConcernResult result = Parcels.Remove(Query.EQ("PhaseId", id), WriteConcern.Acknowledged);
            return result.Ok;
        }

        /// <summary>
        /// Returns all roles from the database
        /// </summary>
        /// <returns>Returns all roles from the database</returns>
        public List<Parcel> GetAllParcels()
        {
            return Parcels.FindAll().ToList();
        }

        public List<Parcel> GetParcelsByPhaseID(ObjectId id)
        {
            var parcels = (from p in Parcels.AsQueryable() where p.PhaseId == id select p);
            return parcels.Count() == 0 ? null : parcels.ToList();
        }

        /// <summary>
        /// Returns the role by their role id
        /// </summary>
        /// <param name="roleId">The id of the role to retrieve</param>
        /// <returns>the role found or null</returns>
        public Parcel GetParcel(ObjectId id)
        {
            return Parcels.FindOneById(id);
        }

        /// <summary>
        /// Returns the role by their name
        /// </summary>
        /// <param name="name">The name of the role to search on</param>
        /// <returns>the role found or null</returns>
        public Parcel GetParcelByName(string name)
        {
            var parcel = (from r in Parcels.AsQueryable() where r.ParcelName == name.ToLower() select r);
            return parcel.Count() == 0 ? null : parcel.First();
        }

        /// <summary>
        /// Searches roles by their name and returns a list
        /// </summary>
        /// <param name="search">The search string to search by</param>
        /// <returns>list of roles that match the search parameter</returns>
        public List<Parcel> SearchParcelByName(string search)
        {
            search = search.ToLower();
            var parcels = (from r in Parcels.AsQueryable() where r.ParcelName.StartsWith(search) select r);
            return parcels.ToList();
        }

        /// <summary>
        /// Gets a list of roles based on paging and searching
        /// </summary>
        /// <param name="take">the number of roles to return</param>
        /// <param name="page">the page of roles to display</param>
        /// <param name="search">a search term to filter on</param>
        /// <param name="total">returns a total of available roles based on the search term</param>
        /// <returns>list of roles that are found from the search</returns>        
        public List<Parcel> GetParcels(int take, int page, string search, ref int total)
        {
            if (page < 1) { page = 1; }

            if (search == null) { search = ""; }
            else { search = search.ToLower(); }

            var parcels = (from r in Parcels.AsQueryable()
                         where r.ParcelName.Contains(search)
                           orderby r.ParcelName
                         select r);
            total = parcels.Count();
            return parcels.Skip((page - 1) * take).Take(take).ToList();
        }

        /// <summary>
        /// Checks to see if the role exists in the database
        /// </summary>
        /// <param name="role">the role to search for.</param>
        /// <returns>true if found, false if not found.</returns>
        public bool Exists(string name)
        {
            name = name.ToLower();
            return Parcels.AsQueryable().Any(u => u.ParcelName == name);
        }

        /// <summary>
        /// Initializes all the needed indexes for the pages to optimize search on them.
        /// </summary>
        public void InitializeIndexes()
        {
            // key options
            var options = new IndexOptionsBuilder().SetUnique(true);

            // page search key
            var keys = new IndexKeysBuilder().Ascending("ParcelName");
            Parcels.EnsureIndex(keys, options);

            // general page search
            keys = new IndexKeysBuilder().Ascending("_id");
            Parcels.EnsureIndex(keys, options);
        }
    }
}
