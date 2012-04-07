using EPiServer.Security;
using PageTypeBuilder;

namespace FortuneCookie.PropertySecurity.Test
{
    [Authorize(Principals = "Role1", ApplyToDefaultProperties = true)]
    public class DefaultAuthorizedPageData : TypedPageData
    {
        
    }
    
    public class NonAuthorizedTypedPageData : TypedPageData
    {
        [PageTypeProperty]
        public string Property0 { get; set; }
    }

    public class PropertyAuthorizedTypedPageData : TypedPageData
    {
        [Authorize(Principals = "Role1"), PageTypeProperty]
        public string Property1 { get; set; }

        [Authorize(Principals = "Role1, username"), PageTypeProperty]
        public string Property2 { get; set; }

        [Authorize(Principals = "Role2"), PageTypeProperty]
        public string Property3 { get; set; }
        [PageTypeProperty]
        public string Property4 { get; set; }
    }

    [Authorize(Principals = "Role1")]
    public class ClassAuthorizedFakeTypedPageData : TypedPageData
    {
        [PageTypeProperty]
        public string Property5 { get; set; }
        [PageTypeProperty]
        public string Property6 { get; set; }
    }

    [Authorize(Principals = "Role1")]
    public class InheritedAuthorizedFakeTypedPageData : PropertyAuthorizedTypedPageData
    {
        [PageTypeProperty]
        public string Property5 { get; set; }
        [PageTypeProperty]
        public string Property6 { get; set; }
    }


    public class TabAuthorizedTypedPageData : TypedPageData
    {
        [PageTypeProperty(Tab = typeof(MetaDataTab))]
        public string Property1 { get; set; }
    }

    public class PropertyGroupAuthorizedTypedPageData : TypedPageData
    {
        [PageTypePropertyGroup]
        public PropertyGroup PropertyGroup1 { get; set; }
    }

    [Authorize(Principals = "Role1")]
    public class PropertyGroupWithClassLevelRulesAuthorizedTypedPageData : TypedPageData
    {
        [PageTypePropertyGroup]
        public PropertyGroupWithoutAuthorization PropertyGroup1 { get; set; }
    }

    public class PropertyGroupWithPropertyGroupLevelAuthorizedTypedPageData : TypedPageData
    {
        [Authorize(Principals = "Role2"), PageTypePropertyGroup]
        public PropertyGroupWithoutAuthorization PropertyGroup1 { get; set; }
    }

    public class PropertyGroupWithTabAuthorizationAuthorizedTypedPageData : TypedPageData
    {
        [PageTypePropertyGroup(Tab = typeof(MetaDataTab))]
        public PropertyGroupWithoutAuthorization PropertyGroup1 { get; set; }
    }

    public class PropertyGroup : PageTypePropertyGroup
    {
        [Authorize(Principals = "Role1"), PageTypeProperty]
        public string Property1 { get; set; }
    }

    public class PropertyGroupWithoutAuthorization : PageTypePropertyGroup
    {
        [PageTypeProperty]
        public string Property1 { get; set; }
    }


    [Authorize(Principals = "Role1")]
    public class MetaDataTab : Tab
    {
        public override string Name
        {
            get { return "MetaData"; }
        }

        public override AccessLevel RequiredAccess
        {
            get { return AccessLevel.Edit; }
        }

        public override int SortIndex
        {
            get { return 100; }
        }
    }

    [Authorize(Principals = "Role2", ApplyToDefaultProperties = true)]
    public class InformationTab : Tab
    {
        public override string Name
        {
            get { return "Information"; }
        }

        public override AccessLevel RequiredAccess
        {
            get { return AccessLevel.Edit; }
        }

        public override int SortIndex
        {
            get { return 100; }
        }
    }


}