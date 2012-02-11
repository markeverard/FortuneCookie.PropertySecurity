using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace FortuneCookie.PropertySecurity.Extensions
{
    public static class SecurityPrincipalExtensions
    {
        /// <summary>
        /// Determines whether the IPrincipal is in any of the specified roles.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="roles">The roles.</param>
        /// <returns>
        ///   <c>true</c> if [is in any role] [the specified principal]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInAnyRole(this IPrincipal principal, IEnumerable<string> roles)
        {
            if (principal == null || roles == null)
                return false;

            return roles.Any(principal.IsInRole);
        }

        /// <summary>
        /// Determines whether the IPrincipal's username matches any values in the list
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="users">The users.</param>
        /// <returns>
        ///   <c>true</c> if [is in user list] [the specified principal]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInUserList(this IPrincipal principal, IEnumerable<string> users)
        {
            if (principal == null || users == null)
                return false;

            return users.Any(user => user == principal.Identity.Name);
        }

        /// <summary>
        /// Determines whether the IPrincipal is in any defined roles or whether the username matches
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="usersAndRoles">The users and roles.</param>
        /// <returns>
        ///   <c>true</c> if [is in any role or user list] [the specified principal]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInAnyRoleOrUserList(this IPrincipal principal, IEnumerable<string> usersAndRoles)
        {
            return principal.IsInAnyRole(usersAndRoles) || principal.IsInUserList(usersAndRoles);
        }

    }
}