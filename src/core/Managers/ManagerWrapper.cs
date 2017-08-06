namespace Linterhub.Core.Managers
{
    using Runtime;
    using System.Collections.Generic;
    using static Schema.EngineSchema.RequirementType;

    public class ManagerWrapper
    {
        private Dictionary <ManagerType, IManager> managers = new Dictionary<ManagerType, IManager>();

        public ManagerWrapper(TerminalWrapper terminal)
        {
            managers.Add(ManagerType.npm, new NpmManager(terminal));
            managers.Add(ManagerType.pip, new PipManager(terminal));
            managers.Add(ManagerType.system, new SystemManager(terminal));
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

        public void add(ManagerType manager, IManager obj)
        {
            managers.Add(manager, obj);
        }
    }
}
