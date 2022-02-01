using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using ActorsCompare.SearchModule.Models;

namespace ActorsCompare
{
    public class ApiModule
    {
        private string ApiRequest(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
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
            else
            {
                throw new ArgumentNullException(nameof(url));
            }
        }

        private string GetActor(int? id)
        {
            if (id != null)
            {
                Console.WriteLine("actor id: " + id);
                return ApiRequest("https://kinopoiskapiunofficial.tech/api/v1/staff/" + id);
            }
            else
            {
                throw new ArgumentNullException(nameof(id));
            }
        }

        private string GetFilm(int? id)
        {
            if (id != null)
            {
                Console.WriteLine("film id: " + id);
                return ApiRequest("https://kinopoiskapiunofficial.tech/api/v2.2/films/" + (int)id);
            }
            else
            {
                throw new ArgumentNullException(nameof(id));
            }
        }

        private string GetActorByKeyword(string name, int? page)
        {
            if (!string.IsNullOrWhiteSpace(name) && page != null)
            {
                return ApiRequest("https://kinopoiskapiunofficial.tech/api/v1/persons?name=" + name + "&page=" + page);
            }
            else
            {
                throw new ArgumentNullException(nameof(name));
            }
        }

        public Actor ParseActor(int? id)
        {
            if (id != null)
            {
                return JsonConvert.DeserializeObject<Actor>(GetActor(id));
            }
            else
            {
                throw new ArgumentNullException(nameof(id));
            }
        }

        public Film ParseFilm(int? id)
        {
            if (id != null)
            {
                return JsonConvert.DeserializeObject<Film>(GetFilm(id));
            }
            else
            {
                throw new ArgumentNullException(nameof(id));
            }
        }

        public ActorSearchModel ParseActorListByName(string name, int? page)
        {
            if (!string.IsNullOrWhiteSpace(name) && page != null)
            {
                return JsonConvert.DeserializeObject<ActorSearchModel>(GetActorByKeyword(name, page));
            }
            else
            {
                throw new ArgumentNullException(nameof(name));
            }
        }
    }
}