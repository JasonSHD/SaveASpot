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
    public class PhaseCollection
    {
        public PhaseCollection(MongoCollection<Phase> phases)
        {
            this._phases = phases;
        }

        private MongoCollection<Phase> _phases = null;
        protected MongoCollection<Phase> Phases
        {
            get
            {
                return this._phases;
            }
        }

        public Phase Insert(Phase phase)
        {
            if (phase.Validate())
            {
                phase.PhaseID = ObjectId.GenerateNewId();
                WriteConcernResult result = Phases.Insert(phase, new MongoInsertOptions{ WriteConcern = WriteConcern.Acknowledged });

                if (result.Ok) { return phase; }
            }
            return null;
        }
        public bool Delete(ObjectId id)
        {
            Phase phase = Phases.FindOneById(id);

            FindAndModifyResult phaseResult = Phases.FindAndRemove(Query.EQ("_id", id), SortBy.Ascending("_id"));

            return phaseResult.Ok;
        }

        public bool Update(Phase model)
        {
            var phase = Phases.FindOneById(model.PhaseID);

            if (phase == null) { return false; }

            phase.Active = model.Active;
            phase.Complete = model.Complete;
            phase.PhaseName = model.PhaseName;
            phase.SpotPrice = model.SpotPrice;

            WriteConcernResult result = Phases.Save(phase, new MongoInsertOptions { WriteConcern = WriteConcern.Acknowledged });
            return result.Ok;
        }

        /// <summary>
        /// Returns all roles from the database
        /// </summary>
        /// <returns>Returns all roles from the database</returns>
        public List<Phase> GetAllPhases()
        {
            return Phases.FindAll().ToList();
        }

        /// <summary>
        /// Returns the phase by their role id
        /// </summary>
        /// <param name="id">The id of the role to retrieve</param>
        /// <returns>the role found or null</returns>
        public Phase GetPhase(ObjectId id)
        {
            return Phases.FindOneById(id);
        }

        /// <summary>
        /// Gets a list of roles based on paging and searching
        /// </summary>
        /// <param name="take">the number of roles to return</param>
        /// <param name="page">the page of roles to display</param>
        /// <param name="search">a search term to filter on</param>
        /// <param name="total">returns a total of available roles based on the search term</param>
        /// <returns>list of roles that are found from the search</returns>        
        public List<Phase> GetPhases(int take, int page, string search, ref int total)
        {
            if (page < 1) { page = 1; }

            if (search == null) { search = ""; }
            else { search = search.ToLower(); }

            var phases = (from r in Phases.AsQueryable()
                         where r.PhaseName.Contains(search)
                         orderby r.PhaseName
                         select r);
            total = phases.Count();
            return phases.Skip((page - 1) * take).Take(take).ToList();
        }

        /// <summary>
        /// Checks to see if the role exists in the database
        /// </summary>
        /// <param name="role">the role to search for.</param>
        /// <returns>true if found, false if not found.</returns>
        public bool Exists(string phase)
        {
            phase = phase.ToLower();
            return Phases.AsQueryable().Any(u => u.PhaseName == phase);
        }

        /// <summary>
        /// Initializes all the needed indexes for the pages to optimize search on them.
        /// </summary>
        public void InitializeIndexes()
        {
            // key options
            var options = new IndexOptionsBuilder().SetUnique(true);

            // page search key
            var keys = new IndexKeysBuilder().Ascending("PhaseName");
            Phases.EnsureIndex(keys, options);

            // general page search
            keys = new IndexKeysBuilder().Ascending("_id");
            Phases.EnsureIndex(keys, options);
        }
    }
}
