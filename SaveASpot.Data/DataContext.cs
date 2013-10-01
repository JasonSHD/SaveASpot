using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using SaveASpot.Data.Models;
using SaveASpot.Data.Controllers;

namespace SaveASpot.Data
{
    public class DataContext
    {
        #region constructors

        public DataContext(string connectionString)
        {
            ConnectionString = connectionString;
            Author = Author ?? new Author();

        }

        public DataContext(string connectionString, Author author) : this(connectionString)
        {
            Author = author;
        }

        #endregion

        public Author Author { get; set; }

        #region database setup

        private MongoDatabase _database = null;
        public MongoDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = MongoDatabase.Create(ConnectionString);
                }
                return _database;
            }
            set
            {
                _database = value;
            }
        }

        public string ConnectionString { get; set; }

        #endregion

        #region collection names

        private const string kUsers = "Users"; 
        private const string kUsersInSites = "UsersInSites";
        private const string kRoles = "Roles";
        private const string kUsersInRoles = "UsersInRoles";
        private const string kWebEvents = "WebEvents";

        private const string kPhase = "Phase";
        private const string kParcel = "Parcel";
        private const string kSpot = "Spot";
        private const string kSponsorSpot = "SponsorSpot";
        private const string kSponsors = "Sponsors"; 
        
        #endregion

        #region collections

        private WebEventsCollection _webEvents = null;
        public WebEventsCollection WebEvents
        {
            get
            {
                if (_webEvents == null)
                {
                    _webEvents = new WebEventsCollection(Database.GetCollection<WebEvent>(kWebEvents));
                }
                return _webEvents;
            }
        }

        private UsersCollection _users = null;
        public UsersCollection Users
        {
            get
            {
                if (_users == null)
                {
                    _users = new UsersCollection(Database.GetCollection<User>(kUsers), Database.GetCollection<UsersInRoles>(kUsersInRoles));
                }
                return _users;
            }
        }

        private RolesCollection _roles = null;
        public RolesCollection Roles
        {
            get
            {
                if (_roles == null)
                {
                    _roles = new RolesCollection(Database.GetCollection<Role>(kRoles), Database.GetCollection<UsersInRoles>(kUsersInRoles));
                }
                return _roles;
            }
        }

        private UsersInRolesCollection _usersInRoles = null;
        public UsersInRolesCollection UsersInRoles
        {
            get
            {
                if (_usersInRoles == null)
                {
                    _usersInRoles = new UsersInRolesCollection(Database.GetCollection<UsersInRoles>(kUsersInRoles));
                }
                return _usersInRoles;
            }
        }

        private PhaseCollection _phases = null;
        public PhaseCollection Phases
        {
            get
            {
                if (_phases == null)
                {
                    _phases = new PhaseCollection(Database.GetCollection<Phase>(kPhase));
                }
                return _phases;
            }
        }

        private ParcelCollection _parcels = null;
        public ParcelCollection Parcels
        {
            get
            {
                if (_parcels == null)
                {
                    _parcels = new ParcelCollection(Database.GetCollection<Parcel>(kParcel));
                }
                return _parcels;
            }
        }

        private SpotCollection _spots = null;
        public SpotCollection Spots
        {
            get
            {
                if (_spots == null)
                {
                    _spots = new SpotCollection(Database.GetCollection<Spot>(kSpot));
                }
                return _spots;
            }
        }

        private SponsorSpotCollection _sponsorSpots = null;
        public SponsorSpotCollection SponsorSpots
        {
            get
            {
                if (_sponsorSpots == null)
                {
                    _sponsorSpots = new SponsorSpotCollection(Database.GetCollection<SponsorSpot>(kSponsorSpot));
                }
                return _sponsorSpots;
            }
        }

        private SponsorCollection _sponsors = null;
        public SponsorCollection Sponsors
        {
            get
            {
                if (_sponsors == null)
                {
                    _sponsors = new SponsorCollection(Database.GetCollection<Sponsor>(kSponsors));
                }
                return _sponsors;
            }
        }

        #endregion

        public MongoCollection<T> GetCollection<T>()
        {
            return Database.GetCollection<T>(typeof(T).Name);
        }
    }
}
