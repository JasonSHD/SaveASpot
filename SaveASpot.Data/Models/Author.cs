using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace SaveASpot.Data.Models
{
    public class Author
    {
        public ObjectId AuthorID { get; set; }
        public string Name { get; set; }

        public bool Validate()
        {
            return (AuthorID != default(ObjectId)) && (Name.Length > 0);
        }
    }
}
