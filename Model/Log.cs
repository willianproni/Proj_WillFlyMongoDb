﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Model
{
    public class Log
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string EntityBefore { get; set; }
        public string EntityAfter { get; set; } 
        public string Operation { get; set; }
        public DateTime DataOperation { get; set; }

        public Log(string userId, string entityBefore, string entityAfter, string operation)
        {
            UserId = userId;
            EntityBefore = entityBefore;
            EntityAfter = entityAfter;
            Operation = operation;
            DataOperation = DateTime.Now;
        }
    }
}
