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
        private Actor firstActor;
        private Actor secondActor;

        private List<Film> filmList;

        private List<CommonMovie> res;

        private ApiModule api;

        public ActorsComparator(int firstId, int secondId)
        {
            api = new ApiModule();//вот только эта строчка нужна здесь, входные параметры убери
            firstActor = api.ParseActor(firstId);//вот эти две нижние строки вынеси в метод сравнения
            secondActor = api.ParseActor(secondId);
        }

        private List<CommonMovie> CompareMovieList()//опять таки используй входные параметры, не нужно глобальных листов, этот класс это по сути сервис
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
            
            return temp1.ToList();
        }

        private void WriteResult()//в этом методе сделать входной параметр, filmlist, глобальными переменными используй только константы, сервисы и репозитории, поскольку у тебя нет вьюхи, глобальных листов быть не должно
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
                
                StringBuilder sb = new StringBuilder();// какой смысл в билдере, если ты всеравно используешь клнкатенацию? всё переделать под билдер
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

        public void CompareActors()//вообще не вижу смысла в этом методе, добавь входные параметры в виде актеров и ретурн в виде строки
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