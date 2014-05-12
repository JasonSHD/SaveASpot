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
    public class SponsorSpotCollection
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="spots">Spots in the db</param>
        /// <param name="userSpots">User/Spot association in the db</param>
        public SponsorSpotCollection(MongoCollection<SponsorSpot> spots)
        {
            this._spots = spots;
        }

        /// <summary>
        /// Spots objects accessible from the database.
        /// </summary>
        private MongoCollection<SponsorSpot> _spots = null;
        protected MongoCollection<SponsorSpot> Spots
        {
            get
            {
                return this._spots;
            }
        }

        /// <summary>
        /// Inserts a new spot into the db.
        /// </summary>
        /// <param name="spot">The spot to add.</param>
        /// <returns>true if the spot is valid and successfully added.</returns>
        public bool Insert(SponsorSpot spot)
        {
            spot.SpotID = ObjectId.GenerateNewId();
            var result = Spots.Insert(spot, WriteConcern.Acknowledged);
            return result.Ok;
        }

        public bool InsertBatch(List<SponsorSpot> spots)
        {
            try
            {
                foreach (var spot in spots) { spot.SpotID = ObjectId.GenerateNewId(); }
                var result = Spots.InsertBatch(spots, WriteConcern.Unacknowledged);
            }
            catch { return false; }
            return true;
        }

        // <summary>
        /// Deletes the spot with the specified id
        /// </summary>
        /// <param name="spotId">Id of the spot to delete</param>
        /// <returns>Result of the operation, true if was ok</returns>
        public bool Delete(ObjectId id)
        {
            FindAndModifyResult spotResult = Spots.FindAndRemove(Query.EQ("_id", id), SortBy.Ascending("_id"));
            return spotResult.Ok;
        }

        public bool DeleteByPhaseID(ObjectId id)
        {
            var result = Spots.Remove(Query.EQ("PhaseID", id), WriteConcern.Acknowledged);
            return result.Ok;
        }

        public bool DeleteAll()
        {
            var result = Spots.RemoveAll(WriteConcern.Acknowledged);
            return result.Ok;
        }

        /// <summary>
        /// Returns all spots from the database
        /// </summary>
        /// <returns>Returns all spots from the database</returns>
        public List<SponsorSpot> GetAllSpots()
        {
            return Spots.FindAll().ToList();
        }

        /// <summary>
        /// Returns the spot by their spot id
        /// </summary>
        /// <param name="spotId">The id of the spot to retrieve</param>
        /// <returns>the spot found or null</returns>
        public SponsorSpot GetSpot(ObjectId id)
        {
            return Spots.FindOneById(id);
        }

        public Count GetSpotsInfoByPhase(ObjectId phaseID)
        {
            var spots = (from s in Spots.AsQueryable() where s.PhaseID == phaseID select s);

            Count result = new Count();
            result.Total = spots.Count();
            result.Paid = (from s in Spots.AsQueryable() where s.SponsorID != ObjectId.Empty select s).Count();
            //result.Paid = spots.Where(s => s.CustomerId != ObjectId.Empty).Count();

            return result;
        }

        public List<SponsorSpot> GetSpotsByPhase(ObjectId phaseID, int take, int index)
        {
            if (index < 0) { index = 0; }

            var spots = (from s in Spots.AsQueryable() where s.PhaseID == phaseID select s);
            return spots.Skip(index).Take(take).ToList();
        }

        public int GetCountByPhase(ObjectId phaseID)
        {
            var spots = (from s in Spots.AsQueryable() where s.PhaseID == phaseID select s);
            return spots.Count();
        }



        public List<SponsorSpot> GetSpotsBySponsors(ObjectId sponsorID)
        {
            var spots = (from s in Spots.AsQueryable() where s.SponsorID == sponsorID select s);
            return spots.ToList();
        }

        public List<ObjectId> GetAllSponsorIDsByPhase(ObjectId phaseID)
        {
            var spots = (from s in Spots.AsQueryable() where s.PhaseID == phaseID select s.SponsorID);
            return spots.Distinct().ToList();
        }

        public bool SaveSpot(SponsorSpot spot)
        {
            var result = Spots.Save(spot, new MongoInsertOptions { WriteConcern = WriteConcern.Acknowledged });
            return result.Ok;
        }

        /// <summary>
        /// Initializes all the needed indexes for the pages to optimize search on them.
        /// </summary>
        public void InitializeIndexes()
        {
            // key options
            var options = new IndexOptionsBuilder().SetUnique(true);

            // page search key
            //var keys = new IndexKeysBuilder().Ascending("Spot");
            //Spots.EnsureIndex(keys, options);

            // general page search
            var keys = new IndexKeysBuilder().Ascending("_id");
            Spots.EnsureIndex(keys, options);

            options.SetUnique(false);

            keys = new IndexKeysBuilder().Ascending("PhaseID");
            Spots.EnsureIndex(keys, options);
        }
    }
}
