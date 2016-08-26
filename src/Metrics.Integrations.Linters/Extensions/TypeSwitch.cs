namespace Metrics.Integrations.Extensions
{
    using System;

    public static class TypeSwitch
    {
        public static Switch<TSource> On<TSource>(TSource value)
        {
            return new Switch<TSource>(value);
        }

        public sealed class Switch<TSource>
        {
            private readonly TSource value;
            private bool handled = false;

            internal Switch(TSource value)
            {
                this.value = value;
            }

            public Switch<TSource> Case<TTarget>(Action<TTarget> action)
                where TTarget : TSource
            {
                if (!handled && value is TTarget)
                {
                    action((TTarget)value);
                    handled = true;
                }

                return this;
            }

            public void Default(Action<TSource> action)
            {
                if (!handled)
                {
                    action(value);
                }
            }
        }
    }
}
