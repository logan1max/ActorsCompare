using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using ActorsCompare.Models;


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
                httpRequest.Headers["X-API-KEY"] = GetAppKey("apiKey");

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
                throw new Exception("URL is empty"); ;
            }
        }

        private string GetAppKey(string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[key];
            }
            else
            {
                throw new Exception("Key is empty");
            }

        }

        private string GetActor(int? id)
        {
            if (id != null)
            {
                Console.WriteLine("actor id: " + id);
                return ApiRequest(GetAppKey("actorLink") + id);//вынести в конфиг (прочитай про app.config)
            }
            else
            {
                throw new Exception("Id is empty");
            }
        }

        private string GetFilm(int? id)
        {
            if (id != null)
            {
                Console.Write("film id: " + id + " ");
                return ApiRequest(GetAppKey("filmLink") + (int)id);//ссылка в конфиг
            }
            else
            {
                throw new Exception("Id is empty");
            }
        }

        private string GetActorByKeyword(string name, int? page)
        {
            if (!string.IsNullOrWhiteSpace(name) && page != null)
            {
                Console.WriteLine(GetAppKey("searchActorLink") + name + "&page=" + page);
                return ApiRequest("https://kinopoiskapiunofficial.tech/api/v1/persons?name=" + name + "&page=" + page);//ссылка в конфиг
            }
            else
            {
                throw new Exception("Name or page is empty");// заменить на обычный эксепшн и добавить сообщение
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
                throw new Exception("Id is empty");// заменить на обычный эксепшн и добавить сообщение
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
                throw new Exception("Id is empty");// заменить на обычный эксепшн и добавить сообщение
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
                throw new Exception("Name or page is empty");// заменить на обычный эксепшн и добавить сообщение
            }
        }
    }
}