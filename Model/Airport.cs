﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Model
{
    public class Airport
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string CodeIATA { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string LoginUser { get; set; }
    }
}
