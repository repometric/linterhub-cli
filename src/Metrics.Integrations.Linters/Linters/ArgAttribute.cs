namespace Metrics.Integrations.Linters
{
    using System;

    public class ArgAttribute : Attribute
    {
        public string Name { get; }

        public string ValueSeparator { get; }

        public bool AddValue { get; }

        public int Order { get; }

        public ArgAttribute(string name = null, bool addValue = true, string valueSeparator = " ", int order = int.MinValue)
        {
            Name = name;
            AddValue = addValue;
            ValueSeparator = valueSeparator;
            Order = order;
        }
    }
}
