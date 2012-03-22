using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using PageTypeBuilder;
using PageTypeBuilder.Abstractions;
using PageTypeBuilder.Reflection;

namespace FortuneCookie.PropertySecurity.Discovery
{
    /// <summary>
    /// Responsible for finding properties that have had a AuthorizedAttribute placed on them
    /// </summary>
    public class AuthorizedPropertyDefinitionLocator
    {
        public AuthorizedPropertyDefinitionLocator(PageData pageData, Type pageTypeType, 
            ITabDefinitionRepository tabDefinitionRepository)
        {
            PageData = pageData;
            PageTypeType = pageTypeType;
            ClassAttribute = AttributedTypesUtility.GetAttributeFromType<AuthorizeAttribute>(PageTypeType);
            Properties = AttributedTypesUtility.GetPublicOrPrivateProperties(PageTypeType);
            _tabDefinitionRepository = tabDefinitionRepository;
        }

        protected PageData PageData { get; set; }
        protected Type PageTypeType { get; set; }
        protected AuthorizeAttribute ClassAttribute { get; set; }
        protected bool HasClassLevelAttribute { get { return ClassAttribute != null;  } }
        private PropertyInfo[] Properties { get; set; }
        private readonly ITabDefinitionRepository _tabDefinitionRepository;
        private static List<Tab> _tabs; 

        public virtual List<AuthorizedPropertyDefinition> GetAuthorizedPropertyDefinitions()
        {
            var definitions = new List<AuthorizedPropertyDefinition>();

            if (_tabs == null)
                _tabs = new TabLocator(new AppDomainAssemblyLocator()).GetDefinedTabs().ToList();

            TabDefinitionCollection tabDefinitions = _tabDefinitionRepository.List();
 
            AddPageTypePropertyDefinitions(Properties, definitions, tabDefinitions, string.Empty, null);
            AddPageTypePropertyGroupDefinitions(tabDefinitions, definitions);
            AddTabAssociatedProperyValuesToDefinitions(PageData, definitions, tabDefinitions);

            if (HasClassLevelAttribute && ClassAttribute.ApplyToDefaultProperties)
                AddDefaultPropertyValuesToDefinitions(PageData, definitions);

            return definitions;
        }

        private void AddPageTypePropertyDefinitions(IEnumerable<PropertyInfo> properties, ICollection<AuthorizedPropertyDefinition> definitions,
            TabDefinitionCollection tabDefinitions, string hierarchy, AuthorizeAttribute propertyGroupAuthorizeAttribute)
        {
            var pageTypeProperties = properties.Where(AttributedTypesUtility.PropertyHasAttribute<PageTypePropertyAttribute>).ToList();

            foreach (PropertyInfo property in pageTypeProperties)
            {
                AuthorizeAttribute attribute = AttributedTypesUtility
                                                    .GetAttributesFromProperty<AuthorizeAttribute>(property)
                                                    .FirstOrDefault();

                // if property has an authorize attribute
                if (attribute != null)
                {
                    definitions.Add(AuthorizedPropertyDefinitionFactory.Create(string.Concat(hierarchy, property.Name), attribute.GetAuthorizedPrincipals()));
                    continue;
                }

                // Add any tab authorization rules
                if (AddTabAuthorizationRules(tabDefinitions, property, definitions, hierarchy))
                    continue;

                // if property group authorize attribute has been defined then apply to property
                if (propertyGroupAuthorizeAttribute != null)
                {
                    definitions.Add(AuthorizedPropertyDefinitionFactory.Create(string.Concat(hierarchy, property.Name), propertyGroupAuthorizeAttribute.GetAuthorizedPrincipals()));
                    continue;
                }

                // if the page type class has an authorize attribute
                if (HasClassLevelAttribute)
                    definitions.Add(AuthorizedPropertyDefinitionFactory.Create(string.Concat(hierarchy, property.Name), ClassAttribute.GetAuthorizedPrincipals()));
            }
        }

        private void AddPageTypePropertyGroupDefinitions(TabDefinitionCollection tabDefinitions, ICollection<AuthorizedPropertyDefinition> definitions)
        {
            foreach (PropertyInfo property in Properties.Where(AttributedTypesUtility.PropertyHasAttribute<PageTypePropertyGroupAttribute>))
            {
                if (!property.PropertyType.IsSubclassOf(typeof(PageTypePropertyGroup)))
                    continue;

                IEnumerable<PropertyInfo> propertyGroupProperties = AttributedTypesUtility.GetPublicOrPrivateProperties(property.PropertyType)
                    .Where(AttributedTypesUtility.PropertyHasAttribute<PageTypePropertyAttribute>)
                    .ToList();

                AuthorizeAttribute authorizeAttribute = AttributedTypesUtility.GetAttributesFromProperty<AuthorizeAttribute>(property)
                    .FirstOrDefault();

                AddPageTypePropertyDefinitions(propertyGroupProperties, definitions, tabDefinitions, string.Concat(property.Name, "-"), authorizeAttribute);
            }
        }

        private bool AddTabAuthorizationRules(IEnumerable<TabDefinition> tabDefinitions, PropertyInfo property,
            ICollection<AuthorizedPropertyDefinition> definitions, string hierarchy)
        {
            TabDefinition tabDefinition = tabDefinitions
                    .FirstOrDefault(d => PageData.Property[string.Concat(hierarchy, property.Name)] != null && d.ID == PageData.Property[string.Concat(hierarchy, property.Name)].OwnerTab);

            if (tabDefinition != null)
            {
                Tab tab = _tabs.FirstOrDefault(t => string.Equals(t.Name, tabDefinition.Name));

                if (tab != null)
                {
                    AuthorizeAttribute tabClassAttribute = AttributedTypesUtility.GetAttributeFromType<AuthorizeAttribute>(tab.GetType());

                    if (tabClassAttribute != null)
                    {
                        definitions.Add(AuthorizedPropertyDefinitionFactory.Create(string.Concat(hierarchy, property.Name), tabClassAttribute.GetAuthorizedPrincipals()));
                        return true;
                    }
                }
            }

            return false;
        }
        
        private void AddTabAssociatedProperyValuesToDefinitions(PageData pageData, ICollection<AuthorizedPropertyDefinition> definitions,
            List<TabDefinition> tabDefinitions)
        {
            foreach (var property in pageData.Property)
            {
                if (definitions.Any(d => d.PropertyName == property.Name))
                    continue;

                TabDefinition tabDefinition = tabDefinitions
                    .FirstOrDefault(d => d.ID == PageData.Property[property.Name].OwnerTab);

                if (tabDefinition == null) 
                    continue;

                Tab tab = _tabs.FirstOrDefault(t => string.Equals(t.Name, tabDefinition.Name));

                if (tab == null) 
                    continue;

                AuthorizeAttribute tabClassAttribute = AttributedTypesUtility.GetAttributeFromType<AuthorizeAttribute>(tab.GetType());

                if (tabClassAttribute == null) 
                    continue;

                if (!tabClassAttribute.ApplyToDefaultProperties)
                    continue;
                
                definitions.Add(AuthorizedPropertyDefinitionFactory.Create(property.Name, tabClassAttribute.GetAuthorizedPrincipals()));
            }
        }

        private void AddDefaultPropertyValuesToDefinitions(PageData pageData, ICollection<AuthorizedPropertyDefinition> definitions)
        {
            foreach (var property in pageData.Property)
            {
                if (definitions.Any(d => d.PropertyName == property.Name))
                    continue;

                definitions.Add(AuthorizedPropertyDefinitionFactory.Create(property.Name, ClassAttribute.GetAuthorizedPrincipals()));
            }
        }

    }

}