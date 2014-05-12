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
    public class SpotCollection
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="spots">Spots in the db</param>
        /// <param name="userSpots">User/Spot association in the db</param>
        public SpotCollection(MongoCollection<Spot> spots)
        {
            this._spots = spots;
        }

        /// <summary>
        /// Spots objects accessible from the database.
        /// </summary>
        private MongoCollection<Spot> _spots = null;
        protected MongoCollection<Spot> Spots
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
        public bool Insert(Spot spot)
        {
            if (spot.Validate())
            {
                spot.SpotID = ObjectId.GenerateNewId();
                SafeModeResult result = Spots.Insert(spot);
                return true;
            }
            else return false;
        }

        public bool InsertBatch(List<Spot> spots)
        {
            try
            {
                foreach (var spot in spots) { spot.SpotID = ObjectId.GenerateNewId(); }
                var result = Spots.InsertBatch(spots, WriteConcern.Unacknowledged);
            }
            catch { return false; }
            return true;
        }

        public bool UpdateSponsor(List<Spot> spots, ObjectId sponsorID)
        {
            try
            {
                /*List<BsonValue> ids = new List<BsonValue>();
                foreach (var spot in spots)
                {
                    ids.Add(BsonValue.Create(spot.SpotID));
                }
                var query = Query.In("SpotID", ids);
                var update = Update.Set("SponsorId", sponsorID);
                Spots.Update(query, update);*/

                foreach(var spot in spots) 
                {
                    spot.SponsorId = sponsorID;
                    Spots.Save(spot, WriteConcern.Unacknowledged);
                }
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
            Spot spot = Spots.FindOneById(id);
            FindAndModifyResult spotResult = Spots.FindAndRemove(Query.EQ("_id", id), SortBy.Ascending("_id"));

            return spotResult.Ok;
        }

        public bool DeleteByParcelId(ObjectId id)
        {
            var result = Spots.Remove(Query.EQ("ParcelId", id), WriteConcern.Acknowledged);
            return result.Ok;
        }

        public bool DeleteByPhaseID(ObjectId id)
        {
            var result = Spots.Remove(Query.EQ("PhaseID", id), WriteConcern.Acknowledged);
            return result.Ok;
        }

        /// <summary>
        /// Returns all spots from the database
        /// </summary>
        /// <returns>Returns all spots from the database</returns>
        public List<Spot> GetAllSpots()
        {
            return Spots.FindAll().ToList();
        }

        public int GetSpotCountByRegion(ObjectId phaseID, Coordinate northEast, Coordinate southWest)
        {
            var spots = (from s in Spots.AsQueryable()
                         where s.PhaseID == phaseID &&  s.SpotShape.Any(c => 
                             c.Latitude > southWest.Longitude && c.Latitude < northEast.Longitude &&
                             c.Longitude > southWest.Latitude && c.Longitude < northEast.Latitude
                         ) select s).Count();

            return spots;
        }

        /// <summary>
        /// Returns the spot by their spot id
        /// </summary>
        /// <param name="spotId">The id of the spot to retrieve</param>
        /// <returns>the spot found or null</returns>
        public Spot GetSpot(ObjectId id)
        {
            return Spots.FindOneById(id);
        }

        public Count GetSpotsInfoByPhase(ObjectId phaseID)
        {
            var spots = (from s in Spots.AsQueryable() where s.PhaseID == phaseID select s);

            Count result = new Count();
            result.Total = spots.Count();
            result.Paid = spots.Where(s => s.CustomerId != ObjectId.Empty).Count();

            return result;
        }

        public List<Spot> GetSpotsAvailableByPhase(ObjectId phaseID, int take)
        {
            var spots = (from s in Spots.AsQueryable()
                         where s.PhaseID == phaseID && s.CustomerId == null
                         select s);
            return spots.Take(take).ToList();
        }

        public List<Spot> GetSpotsByPhase(ObjectId phaseID, int take, int index, Coordinate northEast, Coordinate southWest)
        {
            if (index < 0) { index = 0; }

            var spots = (from s in Spots.AsQueryable()
                         where s.PhaseID == phaseID && s.SpotShape.Any(c =>
                             c.Latitude > southWest.Longitude && c.Latitude < northEast.Longitude &&
                             c.Longitude > southWest.Latitude && c.Longitude < northEast.Latitude
                         )
                         select s);
            return spots.Skip(index).Take(take).ToList();
        }

        public List<Spot> GetSpotsByRegion(Coordinate northEast, Coordinate southWest)
        {
            var spots = (from s in Spots.AsQueryable()
                         where s.SpotShape.Any(c =>
                             c.Longitude > southWest.Longitude && c.Longitude < northEast.Longitude &&
                             c.Latitude > southWest.Latitude && c.Latitude < northEast.Latitude
                         )
                         select s);
            return spots.ToList();
        }

        public List<Spot> CheckArea(Coordinate northEast, Coordinate southWest, List<Spot> spots)
        {
            List<Spot> results = new List<Spot>();

            foreach (var spot in spots)
            {
                var aNorth = spot.NorthEastCoordinate;
                var aSouth = spot.SouthWestCoordinate;
                var area = (aSouth.Latitude + aNorth.Latitude) * (aNorth.Longitude - aSouth.Longitude);
                var overlaparea = RectangleOverlap(aNorth, aSouth, northEast, southWest);

                if (overlaparea > area / 2) { results.Add(spot); }
            }

            return results;
        }


        public double LineOverlap(double line1a, double line1b, double line2a, double line2b) 
        {
          // assume line1a <= line1b and line2a <= line2b
          if (line1a < line2a) 
          {
            if (line1b > line2b)
              return line2b-line2a;
            else if (line1b > line2a)
              return line1b-line2a;
            else 
              return 0;
          }
          else if (line2a < line1b)
            return line2b-line1a;
          else 
            return 0;
        }


        public double RectangleOverlap(Coordinate aNorthEast, Coordinate aSouthWest, Coordinate bNorthEast, Coordinate bSouthWest) 
        {
            return LineOverlap(aNorthEast.Latitude, aSouthWest.Latitude, bNorthEast.Latitude, bSouthWest.Latitude) *
            LineOverlap(aNorthEast.Longitude, aSouthWest.Longitude, bNorthEast.Longitude, bSouthWest.Longitude);
        }


        public List<Spot> GetSpotsBySponsor(ObjectId sponsorID)
        {
            var spots = (from s in Spots.AsQueryable() where s.SponsorId == sponsorID select s);
            return spots.ToList();
        }

        public bool UpdateSpotsWithPhaseID(ObjectId phaseID, ObjectId parcelID)
        {
            var query = Query.EQ("ParcelId", parcelID);
            var update = Update.Set("PhaseID", phaseID);
            var result = Spots.Update(query, update, UpdateFlags.Multi, WriteConcern.Acknowledged);

            return result.Ok;
        }

        public bool SaveSpot(Spot spot)
        {
            var result = Spots.Save(spot, new MongoInsertOptions { WriteConcern = WriteConcern.Acknowledged });
            return true;
        }

        /// <summary>
        /// Gets a list of spots based on paging and searching
        /// </summary>
        /// <param name="take">the number of spots to return</param>
        /// <param name="page">the page of spots to display</param>
        /// <param name="search">a search term to filter on</param>
        /// <param name="total">returns a total of available spots based on the search term</param>
        /// <returns>list of spots that are found from the search</returns>        
        public List<Spot> GetSpots(int take, int page, ref int total)
        {
            if (page < 1) { page = 1; }

            var spots = (from r in Spots.AsQueryable()
                         select r);
            total = spots.Count();
            return spots.Skip((page - 1) * take).Take(take).ToList();
        }

        /// <summary>
        /// Initializes all the needed indexes for the pages to optimize search on them.
        /// </summary>
        public void InitializeIndexes()
        {
            // key options
            var options = new IndexOptionsBuilder().SetUnique(true);

            // page search key
            var keys = new IndexKeysBuilder().Ascending("Spot");
            Spots.EnsureIndex(keys, options);

            // general page search
            keys = new IndexKeysBuilder().Ascending("_id");
            Spots.EnsureIndex(keys, options);

            options.SetUnique(false);

            keys = new IndexKeysBuilder().Ascending("ParcelId");
            Spots.EnsureIndex(keys, options);

            keys = new IndexKeysBuilder().Ascending("CustomerId");
            Spots.EnsureIndex(keys, options);

            keys = new IndexKeysBuilder().Ascending("SponsorId");
            Spots.EnsureIndex(keys, options);
        }
    }
}
