using System;

namespace FortuneCookie.PropertySecurity
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class AuthorizeAttribute : Attribute
    {
        public AuthorizeAttribute(string principals)
        {
            if (string.IsNullOrEmpty(principals))
            {
                AuthorizedPrincipals = new[] { string.Empty };
                return;
            }

            AuthorizedPrincipals = principals.Split(',');
        }

        public string[] AuthorizedPrincipals { get; private set; }
    }
}