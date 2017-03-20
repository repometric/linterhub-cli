namespace Linterhub.Cli
{
    using System.Collections.Generic;

    public class ServiceLocator  
    {  
        public Dictionary<object, object> container = null; 
 
        public ServiceLocator()  
        {  
            container = new Dictionary<object, object>();  
        }

        public T Get<T>()  
        {
            return (T)container[typeof(T)]; 
        }

        public void Register<TInterface>(object implementation)
        {
            container.Add(typeof(TInterface), implementation);
        }
    }
}