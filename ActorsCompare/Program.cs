using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using System.Text.Json;
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

    public class FilmsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                return serializer.Deserialize(reader, objectType);
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                
            }
            return new string[] { reader.Value.ToString() };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    class ActorsComparator
    {
        public Actor firstActor;
        public Actor secondActor;       

        public ActorsComparator(Actor _firstActor, Actor _secondActor)
        {
            firstActor = _firstActor;
            secondActor = _secondActor;
        }

        public string GetActor(int id)
        {
            var url = "https://kinopoiskapiunofficial.tech/api/v1/staff/" + id;

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Headers["accept"] = "application/json";
            httpRequest.Headers["X-API-KEY"] = "23dd74b2-f381-4657-9433-4ea66638f27d";

            string resultJSON = null;

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                resultJSON = streamReader.ReadToEnd();
            }

            Console.WriteLine(httpResponse.StatusCode);
            Console.WriteLine(resultJSON);

            return resultJSON;
        }

        public void ParseActor(string firstActorJSON)
        {
            dynamic data = JObject.Parse(firstActorJSON);
            

            Console.WriteLine(data);
            dynamic data1 = JsonConvert.DeserializeObject(firstActorJSON);
            Console.WriteLine(data1);
            
            ActorJSON actorJSON = JsonConvert.DeserializeObject<ActorJSON>(firstActorJSON);
            Console.WriteLine(actorJSON.nameRu);
            foreach(var item in actorJSON.films)
            {
                if (item.professionKey == "ACTOR")
                {
                    Console.WriteLine(item.filmId + " " + item.nameRu + " " + item.description + " " + item.professionKey + " " + item.rating);
                }
            }
        }

        public void CompareActors()
        {

        }
    }
    
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var firstActor = new Actor(7836);
            var secondActor = new Actor(9838);

            var comparator = new ActorsComparator(firstActor, secondActor);

            string jsonA = comparator.GetActor(firstActor.id);
            Console.WriteLine("----------------------------------------------");
            comparator.ParseActor(jsonA);
            // comparator.GetActor(secondActor.id);
        }
    }
}
