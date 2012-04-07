namespace FortuneCookie.PropertySecurity.Discovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PageTypeBuilder;
    using PageTypeBuilder.Reflection;

    public class TabLocator
    {
        private IAssemblyLocator assemblyLocator;

        public TabLocator(IAssemblyLocator assemblyLocator)
        {
            this.assemblyLocator = assemblyLocator;
        }

        public virtual IEnumerable<Tab> GetDefinedTabs()
        {
            return assemblyLocator.AssembliesWithReferenceToAssemblyOf<Tab>()
                .TypesAssignableTo<Tab>()
                .Concrete()
                .Select(t => (Tab)Activator.CreateInstance(t));
        }
    }
}
