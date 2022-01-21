using System;

namespace ActorsCompare
{
    class Actor
    {
        public int id;
        public string name;

        Actor(int _id)
        {
            id = _id;
        }
    }
    class ActorsComparator
    {
        public Actor firstActor;
        public Actor secondActor;

        private void getActor(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://kinopoiskapiunofficial.tech/api/v1/staff/" + id.ToString()))
                {
                    request.Headers.TryAddWithoutValidation("accept", "application/json");
                    request.Headers.TryAddWithoutValidation("X-API-KEY", "23dd74b2-f381-4657-9433-4ea66638f27d");

                    var response = await httpClient.SendAsync(request);

                    Console.WriteLine();
                }
            }
        }
        public ActorsComparator(Actor _firstActor, Actor _secondActor)
        {
            firstActor = _firstActor;
            secondActor = _secondActor;
        }
        public void CompareActors()
        {

        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("https://kinopoiskapiunofficial.tech/api/v1/staff/7836" + "123");

        }
    }
}
