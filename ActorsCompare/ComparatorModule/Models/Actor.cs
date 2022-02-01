using System;
using System.Collections.Generic;
using System.Text;

namespace ActorsCompare
{
    public class Actor
    {
        public int personId { get; set; }
        public string nameRu { get; set; }
        public List<ActorMovie> films { get; set; }
    }
}
