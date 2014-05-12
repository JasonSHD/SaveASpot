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
    public class SponsorCollection
    {
        public SponsorCollection(MongoCollection<Sponsor> sponsors)
        {
            this._sponsors = sponsors;
        }

        private MongoCollection<Sponsor> _sponsors = null;
        protected MongoCollection<Sponsor> Sponsors
        {
            get
            {
                return this._sponsors;
            }
        }

        public Sponsor Insert(Sponsor sponsor)
        {
            sponsor.SponsorID = ObjectId.GenerateNewId();
            WriteConcernResult result = Sponsors.Insert(sponsor, new MongoInsertOptions { WriteConcern = WriteConcern.Acknowledged });

            if (result.Ok) { return sponsor; }
            return null;
        }
        public bool Delete(ObjectId id)
        {
            FindAndModifyResult phaseResult = Sponsors.FindAndRemove(Query.EQ("_id", id), SortBy.Ascending("_id"));
            return phaseResult.Ok;
        }

        public bool Update(Sponsor model)
        {
            var sponsor = Sponsors.FindOneById(model.SponsorID);

            if (sponsor == null) { return false; }

            sponsor.Description = model.Description;
            sponsor.ImageUrl = model.ImageUrl;
            sponsor.Name = model.Name;
            sponsor.Url = model.Url;
            sponsor.ShowSponsor = model.ShowSponsor;
            sponsor.Order = model.Order;
            sponsor.Center = model.Center;

            WriteConcernResult result = Sponsors.Save(sponsor, new MongoInsertOptions { WriteConcern = WriteConcern.Acknowledged });
            return result.Ok;
        }

        /// <summary>
        /// Returns all roles from the database
        /// </summary>
        /// <returns>Returns all roles from the database</returns>
        public List<Sponsor> GetAllSponsors()
        {
            return Sponsors.FindAll().ToList();
        }

        /// <summary>
        /// Returns the phase by their role id
        /// </summary>
        /// <param name="id">The id of the role to retrieve</param>
        /// <returns>the role found or null</returns>
        public Sponsor GetSponsor(ObjectId id)
        {
            return Sponsors.FindOneById(id);
        }

        public List<Sponsor> GetSponsors(List<ObjectId> ids)
        {
            var sponsors = (from s in Sponsors.AsQueryable() where ids.Contains(s.SponsorID) orderby s.Name select s);
            return sponsors.ToList();
        }

        public List<Sponsor> GetActiveSponsors()
        {
            var sponsors = (from s in Sponsors.AsQueryable() where s.ShowSponsor orderby s.Name select s);
            return sponsors.ToList();
        }

        /// <summary>
        /// Gets a list of roles based on paging and searching
        /// </summary>
        /// <param name="take">the number of roles to return</param>
        /// <param name="page">the page of roles to display</param>
        /// <param name="search">a search term to filter on</param>
        /// <param name="total">returns a total of available roles based on the search term</param>
        /// <returns>list of roles that are found from the search</returns>        
        public List<Sponsor> GetSponsors(int take, int page, string search, ref int total)
        {
            if (page < 1) { page = 1; }

            if (search == null) { search = ""; }
            else { search = search.ToLower(); }

            var sponsors = (from r in Sponsors.AsQueryable()
                         where r.Name.ToLower().Contains(search)
                         orderby r.Name
                         select r);
            total = sponsors.Count();
            return sponsors.Skip((page - 1) * take).Take(take).ToList();
        }

        /// <summary>
        /// Checks to see if the role exists in the database
        /// </summary>
        /// <param name="role">the role to search for.</param>
        /// <returns>true if found, false if not found.</returns>
        public bool Exists(string sponsor)
        {
            sponsor = sponsor.ToLower();
            return Sponsors.AsQueryable().Any(u => u.Name.ToLower() == sponsor);
        }

        /// <summary>
        /// Initializes all the needed indexes for the pages to optimize search on them.
        /// </summary>
        public void InitializeIndexes()
        {
            // key options
            var options = new IndexOptionsBuilder().SetUnique(true);

            // page search key
            var keys = new IndexKeysBuilder().Ascending("Name");
            Sponsors.EnsureIndex(keys, options);

            // general page search
            keys = new IndexKeysBuilder().Ascending("_id");
            Sponsors.EnsureIndex(keys, options);
        }
    }
}
