using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaveASpot.Data.Models;
using MongoDB.Bson;
using ClipperLib;

namespace SaveASpot.Areas.API.Controllers
{
    public class PhaseController : SaveASpot.Controllers.ApplicationController
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult All()
        {
            var phases = new List<Phase>();
            bool success = true;
            try
            {
                phases = Context.Phases.GetAllPhases();

                foreach (var phase in phases)
                {
                    phase.Parcels = Context.Parcels.GetParcelsByPhaseID(phase.PhaseID);
                }            
            }
            catch (Exception exp)
            {
                success = false;
            }

            return Json(new { success = success, results = phases });
        }

        public JsonResult Sponsors()
        {
            var sponsors = new List<Sponsor>();
            bool success = true;
            try
            {
                // get the sponsors
                sponsors = Context.Sponsors.GetAllSponsors();

                foreach (var sponsor in sponsors)
                {
                    var spots = Context.SponsorSpots.GetSpotsBySponsors(sponsor.SponsorID);
                    if (spots.Count > 0)
                    {
                        sponsor.Center = getCenter(sponsor, spots);
                    }
                    else { sponsor.ShowSponsor = false; }
                }

                sponsors = sponsors.Where(s => s.ShowSponsor).ToList();
            }
            catch (Exception exp)
            {
                success = false;
            }

            return Json(new { success = success, results = (from s in sponsors where s.Center != null select new { s.Center, s.Description, s.ImageUrl, s.Name, SponsorID = s.SponsorIDToString, s.Url }).ToList() });
        }

        public JsonResult GetSponsorsByPhase(ObjectId id)
        {
            List<Sponsor> sponsors = new List<Sponsor>();
            bool success = true;

            try
            {
                var sponsorIDs = Context.SponsorSpots.GetAllSponsorIDsByPhase(id);
                sponsors = Context.Sponsors.GetSponsors(sponsorIDs);
            }
            catch(Exception exp)  
            { 
                success = false; 
            }

            return Json(new { success = success, results = sponsors });
        }

        private Coordinate getCenter(Sponsor sponsor, List<SponsorSpot> spots)
        {
            Coordinate coordinate = new Coordinate { Latitude = 0, Longitude = 0 };

            if (sponsor.Center == null)
            {
                var count = 0;

                foreach (var spot in spots)
                {
                    foreach (var coord in spot.SpotShape)
                    {
                        count++;
                        coordinate.Latitude += coord.Latitude;
                        coordinate.Longitude += coord.Longitude;
                    }
                }

                if (count > 0)
                {
                    coordinate.Latitude = coordinate.Latitude / count;
                    coordinate.Longitude = coordinate.Longitude / count;
                }

                sponsor.Center = coordinate;
                Context.Sponsors.Update(sponsor);
            }
            else { coordinate = sponsor.Center; }

            return coordinate;
        }
    }
}
