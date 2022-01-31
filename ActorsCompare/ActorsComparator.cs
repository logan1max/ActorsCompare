using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace ActorsCompare
{
    class ActorsComparator
    {
        public Actor firstActor;
        public Actor secondActor;

        private List<ActorMovie> firstList;
        private List<ActorMovie> secondList;

        public ActorsComparator(int firstId, int secondId)
        {
            firstActor = new Actor(firstId);
            secondActor = new Actor(secondId);
        }

        private string ApiRequest(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }
            else
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);

                httpRequest.Headers["accept"] = "application/json";
                httpRequest.Headers["X-API-KEY"] = "23dd74b2-f381-4657-9433-4ea66638f27d";

                string resultJSON;

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    resultJSON = streamReader.ReadToEnd();
                }

                Console.WriteLine(httpResponse.StatusCode);

                return resultJSON;
            }
        }

        private string GetActor(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            else
            {
                return ApiRequest("https://kinopoiskapiunofficial.tech/api/v1/staff/" + id);
            }
        }

        private string GetFilm(int? id)
        {
            if (id != null)
            {
                return ApiRequest("https://kinopoiskapiunofficial.tech/api/v2.2/films/" + (int)id);                
            }
            else
            {
                throw new ArgumentNullException(nameof(id));
            }
        }

        private List<ActorMovie> ParseActorMovie(string actJSON)
        {
            if (string.IsNullOrWhiteSpace(actJSON))
            {
                throw new ArgumentNullException(nameof(actJSON));
            }
            else
            {
                return JsonConvert.DeserializeObject<ActorJSON>(actJSON).films;
            }
        }
        
        private Film ParseFilm(string filmJSON)
        {
            return JsonConvert.DeserializeObject<Film>(filmJSON);
        }

        private List<CommonMovie> CompareMovieList()
        {            
            var temp1 = from f in firstList
                        from s in secondList
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

            List < CommonMovie > res = temp1.ToList<CommonMovie>();
            
            return res;
        }

        public void CompareActors()
        {
            string jsonFirst = GetActor(firstActor.id);
            string jsonSecond = GetActor(secondActor.id);

            firstList = ParseActorMovie(jsonFirst);
            secondList = ParseActorMovie(jsonSecond);

            List<CommonMovie> res = CompareMovieList();
            List<Film> filmList = new List<Film>();

            foreach (var t in res)
            {
                var temp = GetFilm(t.filmId);
                Film film = ParseFilm(temp);
                filmList.Add(film);
                Console.WriteLine("new: id: " + t.filmId + " role1: " + t.role1 + t.prof1 + " role2: " + t.role2 + t.prof2);
            }

            foreach (var f in filmList)
            {
                Console.WriteLine("id: " + f.kinopoiskId + " name: " + f.nameRu + " year: " + f.year);
            }    
        }
    }
}
