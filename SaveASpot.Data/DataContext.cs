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

        #endregion

        public MongoCollection<T> GetCollection<T>()
        {
            return Database.GetCollection<T>(typeof(T).Name);
        }
    }
}
