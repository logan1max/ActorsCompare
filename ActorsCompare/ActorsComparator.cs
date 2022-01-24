using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;

namespace ActorsCompare
{
    class ActorsComparator
    {
        private const int V = 2;
        public Actor firstActor;
        public Actor secondActor;
        public int[] firstMovies;
        public int[] secondMovies;

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
            // Console.WriteLine(resultJSON);

            return resultJSON;
        }

        public List<Movie> ParseActor(string actJSON)
        {
            dynamic data = JObject.Parse(actJSON);

            Console.WriteLine(data);
            dynamic data1 = JsonConvert.DeserializeObject(actJSON);
            Console.WriteLine(data1);

            ActorJSON actorJSON = JsonConvert.DeserializeObject<ActorJSON>(actJSON);

            Console.WriteLine(actorJSON.nameRu);
            foreach (var item in actorJSON.films)
            {
                if (item.professionKey == "ACTOR")
                {

                    Console.WriteLine(item.filmId + " " + item.nameRu + " " + item.description + " " + item.professionKey + " " + item.rating);
                }
            }

            return actorJSON.films;
        }

        public string[,] MovieListToArray(List<Movie> movie)
        {
            string[,] str = new string[movie.Count, 2];
            int i = 0;

            foreach (var item in movie)
            {
                str[i, 0] = item.filmId.ToString();
                str[i, 1] = item.nameRu;
                i++;
            }

            return str;
        }

        public void CompareMovieList(List<Movie> firstList, List<Movie> secondList)
        {
            foreach (var i in firstList)
            {
                foreach (var j in secondList)
                {
                    if (i.filmId == j.filmId)
                    {

                    }
                }
            }
        }

        public void CompareActors()
        {
            string jsonFirst = GetActor(firstActor.id);
            string jsonSecond = GetActor(secondActor.id);

            List<Movie> firstList = ParseActor(jsonFirst);
            List<Movie> secondList = ParseActor(jsonSecond);

            List<string> res = new List<string>();

            foreach (Movie m1 in firstList)
            {
                foreach (Movie m2 in secondList)
                {
                    if ((m1.filmId == m2.filmId) && !res.Contains(m1.filmId.ToString()) && m1.rating != null)
                    {
                        res.Add(m1.filmId.ToString());
                        Console.WriteLine(m1.filmId + " " + m1.nameRu + " " + m1.rating);
                    }
                }
            }
            Console.WriteLine("------------------------------");
            foreach (string s in res)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("------------------------------");


            string[,] firstArray = MovieListToArray(firstList);
            string[,] secondArray = MovieListToArray(secondList);

            for (int i = 0; i < firstList.Count; i++)
            {
                for (int j = 0; j < secondList.Count; j++)
                {
                    if (firstArray[i, 0] == secondArray[j, 0])
                    {
                        //     Console.WriteLine("id: " + firstArray[i,0] + " name: " + firstArray[i,1] );
                    }
                }
            }
        }
    }
}
