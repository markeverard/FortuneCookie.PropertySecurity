using System.Linq;
using FortuneCookie.PropertySecurity.Discovery;
using NUnit.Framework;

namespace FortuneCookie.PropertySecurity.Test
{
    [TestFixture]
    public class AuthorizedPropertyDefinitionLocator_Test
    {
        private AuthorizedPropertyDefinitionLocator _locator;
        private ClassAuthorizedFakeTypedPageData _classLevel;
        private PropertyAuthorizedTypedPageData _propertyLevel;
        private NonAuthorizedTypedPageData _noLevel;
        private InheritedAuthorizedFakeTypedPageData _inheritedLevel;
        private DefaultAuthorizedPageData _defaultLevel;
        private int _defaultPropertyCount;

        [SetUp] 
        public void Setup()
        {
            _locator = new AuthorizedPropertyDefinitionLocator();
            _classLevel = new ClassAuthorizedFakeTypedPageData();
            _propertyLevel = new PropertyAuthorizedTypedPageData();
            _noLevel = new NonAuthorizedTypedPageData();
            _inheritedLevel = new InheritedAuthorizedFakeTypedPageData();
            _defaultLevel = new DefaultAuthorizedPageData();
            _defaultPropertyCount = 42;
        }

        [Test]
        public void Locator_Should_Discover_FortyTwo_Properties_From_ClassLevelDefaultPageData()
        {
            var actualDefinitionList = _locator.GetPageTypePropertyDefinitions(_defaultLevel);
            Assert.IsTrue(actualDefinitionList.Count == 42);
        }

        [Test]
        public void Locator_Should_Not_Discover_Definitions_From_NonAuthorizedTypedPageData()
        {
            var actualDefinitionList = _locator.GetPageTypePropertyDefinitions(_noLevel);
            Assert.IsTrue(!actualDefinitionList.Any());
        }

        [Test]
        public void Locator_Should_Discover_Two_Definitions_From_ClassAuthorizedFakeTypedPageData()
        {
            var actualDefinitionList = _locator.GetPageTypePropertyDefinitions(_classLevel);
            Assert.IsTrue(actualDefinitionList.Count == 2 + _defaultPropertyCount);
        }

        [Test]
        public void Locator_Should_Discover_Three_Definitions_From_PropertyAuthorizedTypedPageData()
        {
            var actualDefinitionList = _locator.GetPageTypePropertyDefinitions(_propertyLevel);
            Assert.IsTrue(actualDefinitionList.Count == 3);
        }

        [Test]
        public void Locator_Should_Discover_Six_Definitions_From_InheritedAuthorizedTypedPageData()
        {
            var actualDefinitionList = _locator.GetPageTypePropertyDefinitions(_inheritedLevel);
            Assert.IsTrue(actualDefinitionList.Count == 6 + _defaultPropertyCount);
        }

        [Test]
        public void Located_PropertyAttribute_Should_Override_Class_Attribute()
        {
            var actualDefinitionList = _locator.GetPageTypePropertyDefinitions(_inheritedLevel);
            var propertyThreeDefinition = actualDefinitionList.Single(d => d.PropertyName == "Property3");
            Assert.IsTrue(propertyThreeDefinition.AuthorizedPrincipals.Contains("Role2"));
        }

    }
}