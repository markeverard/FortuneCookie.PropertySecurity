using System.Collections.Generic;

namespace FortuneCookie.PropertySecurity.Discovery
{
    public class AuthorizedPropertyDefinition
    {
        internal AuthorizedPropertyDefinition(string propertyName, IList<string> authorizedPrincipals)
        {
            PropertyName = propertyName;
            AuthorizedPrincipals = authorizedPrincipals;
        }

        public string PropertyName { get; set; }
        public IList<string> AuthorizedPrincipals { get; set; } 
    }
}