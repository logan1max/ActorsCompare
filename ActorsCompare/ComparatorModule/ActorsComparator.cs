using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ActorsCompare
{
    class ActorsComparator
    {
        public Actor firstActor;
        public Actor secondActor;

        public List<Film> filmList;

        public List<CommonMovie> res;

        public ApiModule api = new ApiModule();

        public ActorsComparator()
        {

        }

        //private string ApiRequest(string url)
        //{
        //    if (!string.IsNullOrWhiteSpace(url))
        //    {
        //        var httpRequest = (HttpWebRequest)WebRequest.Create(url);

        //        httpRequest.Headers["accept"] = "application/json";
        //        httpRequest.Headers["X-API-KEY"] = "23dd74b2-f381-4657-9433-4ea66638f27d";

        //        string resultJSON;

        //        var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            resultJSON = streamReader.ReadToEnd();
        //        }

        //        Console.WriteLine(httpResponse.StatusCode);

        //        return resultJSON;
        //    }
        //    else
        //    {
        //        throw new ArgumentNullException(nameof(url));
        //    }
        //}

        //private string GetActor(int? id)
        //{
        //    if (id != null)
        //    {
        //        Console.WriteLine("actor id: " + id);
        //        return ApiRequest("https://kinopoiskapiunofficial.tech/api/v1/staff/" + id);                
        //    }
        //    else
        //    {
        //        throw new ArgumentNullException(nameof(id));
        //    }
        //}

        //private string GetFilm(int? id)
        //{
        //    if (id != null)
        //    {
        //        Console.WriteLine("film id: " + id);
        //        return ApiRequest("https://kinopoiskapiunofficial.tech/api/v2.2/films/" + (int)id);                
        //    }
        //    else
        //    {
        //        throw new ArgumentNullException(nameof(id));
        //    }
        //}

        //private Actor ParseActor(int? id)
        //{
        //    if (id != null)
        //    {                
        //        return JsonConvert.DeserializeObject<Actor>(GetActor(id));
        //    }
        //    else
        //    {
        //        throw new ArgumentNullException(nameof(id));
        //    }
        //}
        
        //private Film ParseFilm(int? id)
        //{
        //    if (id != null)
        //    {
        //        return JsonConvert.DeserializeObject<Film>(GetFilm(id));
        //    }
        //    else
        //    {
        //        throw new ArgumentNullException(nameof(id));
        //    }
        //}

        private List<CommonMovie> CompareMovieList()
        {            
            var temp1 = from f in firstActor.films
                        from s in secondActor.films
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

            List<CommonMovie> temp2 = temp1.ToList<CommonMovie>();
            
            return temp2;
        }

        public void WriteResult()
        {
            foreach (var f in filmList)
            {
                string role1 = null;
                foreach (ActorMovie f1 in firstActor.films)
                {
                    if (f1.filmId == f.kinopoiskId)
                    {
                        role1 = f1.description;
                    }
                }

                string role2 = null;
                foreach (ActorMovie f2 in secondActor.films)
                {
                    if (f2.filmId == f.kinopoiskId)
                    {
                        role2 = f2.description;
                    }
                }
                
                StringBuilder sb = new StringBuilder();
                sb.Append("id: " + f.kinopoiskId);
                sb.Append(" name: " + f.nameRu);
                sb.Append(" year: " + f.year);
                sb.Append(" role1: " + role1.ToString());
                sb.Append(" role2: " + role2.ToString());

                if (f.ratingKinopoisk == null)
                {
                    sb.Append(" rate: no rate ");
                }
                else
                {
                    sb.Append(" rate: " + f.ratingKinopoisk);
                }

                sb.Append(" genre: ");
                foreach(Genre g in f.genres)
                {
                    if (g == f.genres.Last())
                    {
                        sb.Append(g.genre + ".");
                    }
                    else
                    {
                        sb.Append(g.genre + ", ");
                    }
                }

                Console.WriteLine(sb.ToString());              
            }
        }

        public void CompareActors()
        {
            int firstId = 7836;
            int secondId = 9838;

            firstActor = api.ParseActor(firstId);
            secondActor = api.ParseActor(secondId);

            Console.WriteLine("first: " + firstActor.nameRu);
            Console.WriteLine("second: " + secondActor.nameRu);

            res = CompareMovieList();
            filmList = new List<Film>();

            foreach (var t in res)
            {
                Film film = api.ParseFilm(t.filmId);
                filmList.Add(film);
            }

            WriteResult();
        }
    }
}