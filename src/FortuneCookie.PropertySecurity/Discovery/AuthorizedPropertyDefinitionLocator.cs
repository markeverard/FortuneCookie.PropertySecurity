using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EPiServer.Core;
using PageTypeBuilder;

namespace FortuneCookie.PropertySecurity.Discovery
{
    /// <summary>
    /// Responsible for finding properties that have had a AuthorizedAttribute placed on them
    /// </summary>
    public class AuthorizedPropertyDefinitionLocator
    {
        public AuthorizedPropertyDefinitionLocator(PageData pageData, Type pageTypeType)
        {
            PageData = pageData;
            PageTypeType = pageTypeType;
            ClassAttribute = AttributedTypesUtility.GetAttributeFromType<AuthorizeAttribute>(PageTypeType);
            Properties = AttributedTypesUtility.GetPublicOrPrivateProperties(PageTypeType);
        }

        protected PageData PageData { get; set; }
        protected Type PageTypeType { get; set; }
        protected AuthorizeAttribute ClassAttribute { get; set; }
        protected bool HasClassLevelAttribute { get { return ClassAttribute != null;  } }
        private PropertyInfo[] Properties { get; set; }

        public virtual List<AuthorizedPropertyDefinition> GetAuthorizedPropertyDefinitions()
        {
            var definitions = new List<AuthorizedPropertyDefinition>();
            
            foreach (PropertyInfo property in Properties.Where(AttributedTypesUtility.PropertyHasAttribute<PageTypePropertyAttribute>))
            {
                AuthorizeAttribute attribute = AttributedTypesUtility
                                                    .GetAttributesFromProperty<AuthorizeAttribute>(property)
                                                    .FirstOrDefault();
                if (attribute != null)
                {
                    definitions.Add(AuthorizedPropertyDefinitionFactory.Create(property.Name, attribute.GetAuthorizedPrincipals()));
                    continue;
                }

                if (HasClassLevelAttribute)
                    definitions.Add(AuthorizedPropertyDefinitionFactory.Create(property.Name, ClassAttribute.GetAuthorizedPrincipals()));
            }

            if (HasClassLevelAttribute && ClassAttribute.ApplyToDefaultProperties)
                AddDefaultPropertyValuesToDefinitions(PageData, definitions);

            return definitions;
        }

        private void AddDefaultPropertyValuesToDefinitions(PageData pageData, List<AuthorizedPropertyDefinition> definitions)
        {
            foreach (var property in pageData.Property)
            {
                if (definitions.FirstOrDefault(d => d.PropertyName == property.Name) != null)
                    continue;

                definitions.Add(AuthorizedPropertyDefinitionFactory.Create(property.Name, ClassAttribute.GetAuthorizedPrincipals()));
            }
        }

        private static bool DefinitionsContainsPropertyName(IEnumerable<AuthorizedPropertyDefinition> definitions, string propertyName)
        {
            return definitions.FirstOrDefault(d => d.PropertyName == propertyName) != null;
        }
    }

}