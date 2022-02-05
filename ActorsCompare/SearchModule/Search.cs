using System;
using System.Collections.Generic;
using System.Text;
using ActorsCompare.SearchModule.Models;

namespace ActorsCompare
{
    public class Search
    {
        private readonly ApiModule api = new ApiModule();

        public int ByName()
        {
            Console.WriteLine("Поиск персон по имени...");
            Console.WriteLine("Введите имя для поиска: ");

            string searchName = Console.ReadLine();

            ActorSearchModel actorSearchModel = api.ParseActorListByName(searchName, 1);

            int pages = 0;
            if (actorSearchModel.total == 0)
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
                    Console.WriteLine("n: "+ numItems+" id: " + i.kinopoiskId + " nameRu: " + i.nameRu + " nameEn: " + i.nameEn + " sex: " + i.sex);
                    numItems++;
                }
            }

            if (pages == 2)
            {
                ActorSearchModel actorSearchModel1 = api.ParseActorListByName(searchName, 2);
                
                foreach (var i in actorSearchModel1.items)
                {
                    actorSearchModel.items.Add(i);
                    Console.WriteLine("n: " + numItems + " id: " + i.kinopoiskId + " nameRu: " + i.nameRu + " nameEn: " + i.nameEn + " sex: " + i.sex);
                    numItems++;
                }
            }

            Console.WriteLine("Введите номер: ");
            int num = Convert.ToInt32(Console.ReadLine());

            return actorSearchModel.items[num-1].kinopoiskId;
        }

    }
}
