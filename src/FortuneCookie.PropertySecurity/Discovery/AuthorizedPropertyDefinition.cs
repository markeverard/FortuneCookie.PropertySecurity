using System.Collections.Generic;

namespace FortuneCookie.PropertySecurity.Discovery
{
    public class AuthorizedPropertyDefinition
    {
        public AuthorizedPropertyDefinition(string propertyName, IList<string> authorizedPrincipals)
        {
            PropertyName = propertyName;
            AuthorizedPrincipals = authorizedPrincipals;
        }

        public string PropertyName { get; set; }
        public IList<string> AuthorizedPrincipals { get; set; } 
    }
}