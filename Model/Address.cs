﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Model
{
    public class Address
    {
        [JsonProperty("id")]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [JsonProperty("cep")] 
        public string Cep { get; set; }
        [JsonProperty("city")]
        public string City { get; set; } //Herda da api Airport
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("district")]
        public string District { get; set; }
        [JsonProperty("street")]
        public string Street { get; set; }
        [JsonProperty("number")]
        public int Number { get; set; }
        [JsonProperty("complement")]
        public string Complement { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; } //Herda da api Airport
        [JsonProperty("continent")]
        public string Continent { get; set; }
    }
}
