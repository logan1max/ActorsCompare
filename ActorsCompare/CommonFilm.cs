using System;
using System.Collections.Generic;
using System.Text;

namespace ActorsCompare
{
    public class CommonFilm
    {
        public int filmId;
        public int firstId;
        public int secondId;
        public string firstHero;
        public string secondHero;
        public string rating;
        public int year;

        //public CommonFilm(int _filmId, int _firstId, int _secondId, string _firstHero, string _secondHero, string _rating)
        //{
        //    filmId = _filmId;
        //    firstId = _firstId;
        //    secondId = _secondId;
        //    firstHero = _firstHero;
        //    secondHero = _secondHero;
        //    rating = _rating;
        //}

        public CommonFilm()
        {
            filmId = 0;
            firstId = 0;
            secondId = 0;
            firstHero = null;
            secondHero = null;
            rating = null;
        }

    }
}
