using System.Web;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.UI.Edit;
using FortuneCookie.PropertySecurity.Discovery;
using FortuneCookie.PropertySecurity.Extensions;

namespace FortuneCookie.PropertySecurity
{
    /// <summary>
    /// An <see cref="EPiServer.Framework.IInitializableModule" /> enabling hooking into EPiServer events.
    /// </summary>
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class PropertySecurityInitializer : IInitializableModule
    {
        /// <summary>
        /// <para>Initializes the Module and attaches custom eventhandlers.</para>
        /// <para>Attaches a <see cref="System.EventHandler" /> for the EPiServer.Web.UrlSegment.
        /// CreatedUrlSegment event.</para>
        /// </summary>
        /// <param name="context">The current
        /// <see cref="EPiServer.Framework.Initialization.InitializationEngine"/></param>
        public void Initialize(InitializationEngine context)
        {
            EditPanel.LoadedPage += EditPanel_LoadedPage;
        }

        private void EditPanel_LoadedPage(EditPanel sender, LoadedPageEventArgs e)
        {
            var typedPage = PageTypeBuilder.PageTypeResolver.Instance.ConvertToTyped(e.Page);
          
            var authorisedPropertyDefinitions = new AuthorizedPropertyDefinitionLocator().GetPageTypePropertyDefinitions(typedPage);
            
            var currentUser = HttpContext.Current.User;
            if (currentUser == null)
                return;

            foreach (var definition in authorisedPropertyDefinitions)
                e.Page.Property[definition.PropertyName].DisplayEditUI = currentUser.IsInAnyRole(definition.AuthorizedPrincipals);
        }

        /// <summary>
        /// Perform pre-loading tasks.
        /// </summary>
        public void Preload(string[] parameters)
        {
        }

        /// <summary>
        /// <para>Uninitializes the Module.</para>
        /// <para>Detaches the <see cref="System.EventHandler" /> for the EPiServer.Web.UrlSegment.
        /// CreatedUrlSegment event added during Initialization.</para>
        /// </summary>
        /// <param name="context">The current
        /// <see cref="EPiServer.Framework.Initialization.InitializationEngine"/></param>
        public void Uninitialize(InitializationEngine context)
        {
            EditPanel.LoadedPage -= EditPanel_LoadedPage;
        }
    }
}

