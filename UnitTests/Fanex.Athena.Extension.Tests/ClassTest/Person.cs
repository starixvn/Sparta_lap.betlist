namespace Fanex.Athena.Extension.Tests.ClassTest
{
    using System.Collections.Generic;

    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string NickName { get; set; }

        public IDictionary<int, string> Children { get; set; }
    }
}