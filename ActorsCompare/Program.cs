using System;
using System.IO;
using System.Net;
using System.Net.Http;

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
    class ActorsComparator
    {
        public Actor firstActor;
        public Actor secondActor;       


        public ActorsComparator(Actor _firstActor, Actor _secondActor)
        {
            firstActor = _firstActor;
            secondActor = _secondActor;
        }

        public void GetActor(int id)
        {
            var url = "https://kinopoiskapiunofficial.tech/api/v1/staff/" + id;

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Headers["accept"] = "application/json";
            httpRequest.Headers["X-API-KEY"] = "23dd74b2-f381-4657-9433-4ea66638f27d";

            string result = null;

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            Console.WriteLine(httpResponse.StatusCode);
            Console.WriteLine(result);
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

            comparator.GetActor(firstActor.id);
            Console.WriteLine("----------------------------------------------");
            comparator.GetActor(secondActor.id);
        }
    }
}
