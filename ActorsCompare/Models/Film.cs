using System;
using System.Collections.Generic;
using System.Text;

namespace ActorsCompare
{
    public class Film
    {
        public int kinopoiskId { get; set; }
        public string nameRu { get; set; }
        public string ratingKinopoisk { get; set; }
        public int ratingKinopoiskVoteCount { get; set; } 
        public string type { get; set; }
        public List<Genre> genres { get; set; }
        public int year { get; set; }
    }
}
