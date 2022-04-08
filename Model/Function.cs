﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Model
{
    public class Function
    {
        [BsonId]
        public string Id { get; set; }
        public string Description { get; set; }
        public List<Access> Access { get; set; }
    }
}
