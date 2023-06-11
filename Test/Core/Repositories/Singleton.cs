using System.Collections.Generic;

namespace Test.Core.Repositories
{
    public sealed class Singleton
    {
        public static readonly Singleton Instance = new Singleton();

        public Dictionary<string, object> repositories = new Dictionary<string, object>();

        private Singleton() { }


    }
}
