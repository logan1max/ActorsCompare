using System;
using System.Collections.Generic;
using System.Text;
using ActorsCompare.Models;

namespace ActorsCompare
{
    public class SearchModule
    {
        private readonly ApiModule api = new ApiModule();

        public int ByName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                //      string searchName = Console.ReadLine();// никакого считывания здесь, этот метод уже должен принимать строку

                ActorSearchModel actorSearchModel = api.ParseActorListByName(name, 1);

                int pages = 0;
                if (actorSearchModel.total == 0)//это костыль из ифов, подумай как от него избавиться (почитай на тему избавления от ифов спомощью ооп)
                {
                    Console.WriteLine("Персоны не найдены...");
                    pages = 0;
                }
                else if (actorSearchModel.total <= 50)
                {
                    Console.WriteLine("Найдено " + actorSearchModel.total + " персон...");
                    pages = 1;
                }
                else if (actorSearchModel.total < 100)
                {
                    Console.WriteLine("Найдено " + actorSearchModel.total + " персон...");
                    pages = 2;
                }
                else if (actorSearchModel.total > 100)
                {
                    Console.WriteLine("Найдено более 100 персон...");
                    pages = 2;
                }

                int numItems = 1;

                if (pages > 0)
                {
                    foreach (var i in actorSearchModel.items)
                    {
                        Console.WriteLine("n: " + numItems + " id: " + i.kinopoiskId + " nameRu: " + i.nameRu + " nameEn: " + i.nameEn + " sex: " + i.sex);
                        numItems++;
                    }
                }

                if (pages == 2)// не понимаю зачем?
                {
                    ActorSearchModel actorSearchModel1 = api.ParseActorListByName(name, 2);

                    foreach (var i in actorSearchModel1.items)
                    {
                        actorSearchModel.items.Add(i);
                        Console.WriteLine("n: " + numItems + " id: " + i.kinopoiskId + " nameRu: " + i.nameRu + " nameEn: " + i.nameEn + " sex: " + i.sex);
                        numItems++;
                    }
                }

                Console.WriteLine("Введите номер: ");
                int num = Convert.ToInt32(Console.ReadLine());//подумай как от этого избавиться

                return actorSearchModel.items[num - 1].kinopoiskId;
            }
            else
            {
                throw new Exception("Wrong name");
            }
        }
    }
}
