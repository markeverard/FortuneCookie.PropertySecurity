using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EPiServer.Core;
using PageTypeBuilder.Reflection;

namespace FortuneCookie.PropertySecurity.Discovery
{
    public class AuthorizedPropertyDefinitionLocator
    {
        public virtual List<AuthorizedPropertyDefinition> GetPageTypePropertyDefinitions(PageData pageData)
        {
            var classLevelAttribute = AttributedTypesUtility.GetAttribute<AuthorizeAttribute>(pageData.GetType());

            PropertyInfo[] properties = pageData.GetType().GetPublicOrPrivateProperties();
            var authorizedPropertyDefinitions = new List<AuthorizedPropertyDefinition>();

            foreach (PropertyInfo property in properties)
            {
                AuthorizeAttribute attribute = GetAuthorizeAttribute(property);

                if (attribute != null)
                {
                    authorizedPropertyDefinitions.Add(new AuthorizedPropertyDefinition(property.Name, attribute.AuthorizedPrincipals));
                    continue;
                }

                if (classLevelAttribute != null)
                    authorizedPropertyDefinitions.Add(new AuthorizedPropertyDefinition(property.Name, classLevelAttribute.AuthorizedPrincipals));
            }

            return authorizedPropertyDefinitions;
        }

        internal AuthorizeAttribute GetAuthorizeAttribute(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes<AuthorizeAttribute>().FirstOrDefault();
        }
    }
}