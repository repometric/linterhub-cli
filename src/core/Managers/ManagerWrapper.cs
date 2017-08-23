namespace Linterhub.Core.Managers
{
    using System.Linq;
    using Factory;
    using Runtime;
    using System.Collections.Generic;
    using static Schema.EngineSchema.RequirementType;

    public class ManagerWrapper
    {
        private Dictionary <ManagerType, IManager> managers = new Dictionary<ManagerType, IManager>();

        private IEngineFactory EngineFactory;

        public ManagerWrapper(TerminalWrapper terminal, IEngineFactory engineFactory)
        {
            EngineFactory = engineFactory;
            add(ManagerType.npm, new NpmManager(terminal));
            add(ManagerType.pip, new PipManager(terminal));
            add(ManagerType.system, new SystemManager(terminal));
        }

        public IManager get(ManagerType manager)
        {
            try
            {
                return managers[manager];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public IManager get(string engine)
        {
            try
            {
                return get(EngineFactory.GetSpecification(engine).Schema.Requirements.Where(x => x.Package == engine).First().Manager);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public void add(ManagerType manager, IManager obj)
        {
            managers.Add(manager, obj);
        }
    }
}
