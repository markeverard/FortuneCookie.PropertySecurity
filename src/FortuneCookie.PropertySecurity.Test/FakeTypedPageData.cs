using PageTypeBuilder;

namespace FortuneCookie.PropertySecurity.Test
{
    [Authorize("Role1")]
    public class DefaultAuthorizedPageData : TypedPageData
    {
        
    }
    
    public class NonAuthorizedTypedPageData : TypedPageData
    {
        public string Property0 { get; set; }
    }

    public class PropertyAuthorizedTypedPageData : TypedPageData
    {
        [Authorize("Role1")]
        public string Property1 { get; set; }
        
        [Authorize("Role1, username")]
        public string Property2 { get; set; }

        [Authorize("Role2")]
        public string Property3 { get; set; }

        public string Property4 { get; set; }
    }

    [Authorize("Role1")]
    public class ClassAuthorizedFakeTypedPageData : TypedPageData
    {
        public string Property5 { get; set; }
        public string Property6 { get; set; }
    }

    [Authorize("Role1")]
    public class InheritedAuthorizedFakeTypedPageData : PropertyAuthorizedTypedPageData
    {
        public string Property5 { get; set; }
        public string Property6 { get; set; }
    }


}