using System.Collections.Generic;

namespace FortuneCookie.PropertySecurity.Discovery
{
    /// <summary>
    /// Creates instances of AuthorizedPropertyDefinition
    /// </summary>
    public static class AuthorizedPropertyDefinitionFactory
    {
        /// <summary>
        /// Creates the specified property name taking account of default properties which are prefixed by Page.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="authorizedPrincipals">The authorized principals.</param>
        /// <returns></returns>
        public static AuthorizedPropertyDefinition Create(string propertyName, IList<string> authorizedPrincipals)
        {
            return new AuthorizedPropertyDefinition(propertyName, authorizedPrincipals);
        }
    }
}