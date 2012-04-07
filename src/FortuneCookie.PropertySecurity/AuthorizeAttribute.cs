using System;

namespace FortuneCookie.PropertySecurity
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class AuthorizeAttribute : Attribute
    {
        public string Principals;

        public string[] GetAuthorizedPrincipals()
        {
            if (string.IsNullOrEmpty(Principals))
                throw new ArgumentException("Principal username or role must be set");

            return Principals.Split(',');
        }
    }
}