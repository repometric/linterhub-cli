namespace Linterhub.Cli.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class ServiceLocator  
    {  
        public Dictionary<System.Type, object> container = null; 
 
        public ServiceLocator()  
        {  
            container = new Dictionary<System.Type, object>();  
        }

        public T Get<T>()  
        {
            return (T)container.First(x => typeof(T).IsAssignableFrom(x.Key)).Value;
        }

        public void Register<TInterface>(object implementation)
        {
            container.Add(typeof(TInterface), implementation);
        }
    }
}