using System;
using System.Collections.Generic;
using System.Text;

namespace ActorsCompare.SearchModule.Models
{
    public class ActorSearchModel
    {
        public int total { get; set; }
        public List<ActorItem> items { get; set; }
    }
}
