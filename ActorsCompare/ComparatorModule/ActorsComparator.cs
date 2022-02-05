using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ActorsCompare.SearchModule.Models;
using System.Threading;

namespace ActorsCompare
{
    class ActorsComparator
    {
        public Actor firstActor;
        public Actor secondActor;

        public List<Film> filmList;

        public List<CommonMovie> res;

        public ApiModule api = new ApiModule();

        public ActorsComparator(int firstId, int secondId)
        {
            firstActor = api.ParseActor(firstId);
            secondActor = api.ParseActor(secondId);
        }

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

        private void WriteResult()
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
                Console.WriteLine(sb);
            }
        }

        public void CompareActors()
        {
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