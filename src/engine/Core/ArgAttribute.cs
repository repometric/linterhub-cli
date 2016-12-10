namespace Linterhub.Engine
{
    using System;

    public class ArgAttribute : Attribute
    {
        public string Name { get; }

        public string Separator { get; }

        public bool Add { get; }

        public int Order { get; }

        public ArgAttribute(string name = null, bool add = true, string separator = " ", int order = 0)
        {
            Name = name;
            Add = add;
            Separator = separator;
            Order = order;
        }
    }
}
