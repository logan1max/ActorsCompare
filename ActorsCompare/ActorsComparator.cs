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

            return resultJSON;
        }

        public List<Movie> ParseActor(string actJSON)
        {
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

        //public string[,] MovieListToArray(List<Movie> movie)
        //{
        //    string[,] str = new string[movie.Count, 2];
        //    int i = 0;

        //    foreach (var item in movie)
        //    {
        //        str[i, 0] = item.filmId.ToString();
        //        str[i, 1] = item.nameRu;
        //        i++;
        //    }

        //    return str;
        //}

        public List<CommonFilm> CompareMovieList(List<Movie> firstList, List<Movie> secondList)
        {
            List<CommonFilm> res = new List<CommonFilm>();

            List<int> firstFilmsId = new List<int>();
            List<int> secondFilmsId = new List<int>();

            var temp1 = from f in firstList
                        from s in secondList
                            //    where f.professionKey == "ACTOR" && s.professionKey == "ACTOR" && f.filmId == s.filmId
                        where f.filmId == s.filmId  
                                && !f.description.Contains("гость") 
                                && !s.description.Contains("гость")
                                && (f.description.Contains("самого себя") ^ f.professionKey == "ACTOR")
                                && (s.description.Contains("самого себя") ^ s.professionKey == "ACTOR")
                        select new CommonMovie
                        {
                            filmId = f.filmId,
                            role1 = f.description,
                            role2 = s.description,
                            prof1 = f.professionKey,
                            prof2 = s.professionKey
                        };

            temp1 = temp1.Distinct();

            foreach (var t in temp1)
            {
                Console.WriteLine("id: " + t.filmId + " role1: " + t.role1 + t.prof1 + " role2: " + t.role2 + t.prof2);
            }

            var temp2 = from f in secondList
                        select f.filmId;
            

            foreach (var i in firstList)
            {
                foreach (var j in secondList)
                {
                    if (i.filmId == j.filmId)
                    {
                        CommonFilm comFilm = new CommonFilm();
                        comFilm.filmId = i.filmId;
                        comFilm.firstId = firstActor.id;
                        comFilm.secondId = secondActor.id;
                        comFilm.firstHero = i.description;
                        comFilm.secondHero = j.description;
                        comFilm.rating = i.rating;                        
                        res.Add(comFilm);
                    }
                }
            }
            res.Distinct();
            return res;
        }

        public void CompareActors()
        {
            string jsonFirst = GetActor(firstActor.id);
            string jsonSecond = GetActor(secondActor.id);

            List<Movie> firstList = ParseActor(jsonFirst);
            List<Movie> secondList = ParseActor(jsonSecond);

            List<CommonFilm> res = CompareMovieList(firstList, secondList);

            foreach (var i in res)
            {
               // Console.WriteLine("id: " + i.filmId + " id1: " + i.firstId + " role1: " + i.firstHero + " id2: " + i.secondId + " role2: " + i.secondHero + " rate: " + i.rating);
            }
            //List<string> res = new List<string>();

            //foreach (Movie m1 in firstList)
            //{
            //    foreach (Movie m2 in secondList)
            //    {
            //        if ((m1.filmId == m2.filmId) && !res.Contains(m1.filmId.ToString()) && m1.rating != null)
            //        {
            //            res.Add(m1.filmId.ToString());
            //            Console.WriteLine(m1.filmId + " " + m1.nameRu + " " + m1.rating);
            //        }
            //    }
            //}
            //Console.WriteLine("------------------------------");
            //foreach (string s in res)
            //{
            //    Console.WriteLine(s);
            //}
            //Console.WriteLine("------------------------------");


            //string[,] firstArray = MovieListToArray(firstList);
            //string[,] secondArray = MovieListToArray(secondList);

            //for (int i = 0; i < firstList.Count; i++)
            //{
            //    for (int j = 0; j < secondList.Count; j++)
            //    {
            //        if (firstArray[i, 0] == secondArray[j, 0])
            //        {
            //            //     Console.WriteLine("id: " + firstArray[i,0] + " name: " + firstArray[i,1] );
            //        }
            //    }
            //}
        }
    }
}
