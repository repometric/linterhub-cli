namespace Linterhub.Engine
{
    using System;

    public class ArgAttribute : Attribute
    {
        public string Name { get; }

        public string Separator { get; }

        public bool Add { get; }

        public int Order { get; }

        public ArgAttribute(string name = null, bool add = true, string separator = " ", int order = int.MinValue)
        {
            Name = name;
            Add = add;
            Separator = separator;
            Order = order;
        }
    }
}
