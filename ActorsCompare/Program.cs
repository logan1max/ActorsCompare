using System;

namespace ActorsCompare
{
    class Actor
    {
        public int id;
        public string name;

        Actor()
        {

        }
    }
    class ActorsComparator
    {
        public Actor firstActor;
        public Actor secondActor;

        public ActorsComparator(Actor _firstActor, Actor _secondActor)
        {
            firstActor = _firstActor;
            secondActor = _secondActor;
        }
        public void CompareActors()
        {

        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            

        }
    }
}
