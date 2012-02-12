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


}