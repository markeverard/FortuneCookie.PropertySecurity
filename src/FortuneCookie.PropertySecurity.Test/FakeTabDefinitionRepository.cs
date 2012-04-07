using System.Linq;
using EPiServer.DataAbstraction;
using PageTypeBuilder.Abstractions;

namespace FortuneCookie.PropertySecurity.Test
{
    public class FakeTabDefinitionRepository : ITabDefinitionRepository
    {

        public TabDefinition GetTabDefinition(string name)
        {
            return List().FirstOrDefault(t => string.Equals(t.Name, name));
        }

        public TabDefinitionCollection List()
        {
            TabDefinitionCollection tabDefinitionCollection = new TabDefinitionCollection
                                                                  {
                                                                      new TabDefinition
                                                                          {
                                                                              ID = 100,
                                                                              IsSystemTab = false,
                                                                              Name = "MetaData"
                                                                          },
                                                                          new TabDefinition
                                                                          {
                                                                              ID = 101,
                                                                              IsSystemTab = true,
                                                                              Name = "Information"
                                                                          }
                                                                  };
            return tabDefinitionCollection;
        }

        public void SaveTabDefinition(TabDefinition tabDefinition)
        {
        }
    }
}
