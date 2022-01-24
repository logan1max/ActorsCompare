using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;

namespace ActorsCompare
{
    class Actor
    {
        public int id;
        public string name;

        public Actor(int _id)
        {
            id = _id;
        }
    }

    public class Movie
    {
        public int filmId { get; set; }
        public string nameRu { get; set; }
        public string rating { get; set; }
        public string description { get; set; }
        public string professionKey { get; set; }
    }
    public class ActorJSON
    {
        public string nameRu { get; set; }
        public List<Movie> films { get; set; }

    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var firstActor = new Actor(7836);
            var secondActor = new Actor(9838);

            var comparator = new ActorsComparator(firstActor, secondActor);

            comparator.CompareActors();
            /*
            string jsonA = comparator.GetActor(firstActor.id);
            string jsonB = comparator.GetActor(secondActor.id);
            Console.WriteLine("----------------------------------------------");
            comparator.ParseActor(jsonA);
            comparator.ParseActor(jsonB);
            // comparator.GetActor(secondActor.id);
            */
        }
    }
}
