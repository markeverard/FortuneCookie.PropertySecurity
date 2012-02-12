using System.Diagnostics;
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
            _classLevel = new ClassAuthorizedFakeTypedPageData();
            _propertyLevel = new PropertyAuthorizedTypedPageData();
            _noLevel = new NonAuthorizedTypedPageData();
            _inheritedLevel = new InheritedAuthorizedFakeTypedPageData();
            _defaultLevel = new DefaultAuthorizedPageData();
            _defaultPropertyCount = 42;
        }

        [Test]
        public void Locator_Should_Not_Discover_Definitions_From_NonAuthorizedTypedPageData()
        {
            _locator = new AuthorizedPropertyDefinitionLocator(_noLevel, _noLevel.GetType());
            var actualDefinitionList = _locator.GetAuthorizedPropertyDefinitions();
            Assert.IsTrue(!actualDefinitionList.Any());
        }

        [Test]
        public void Locator_Should_Discover_Two_Definitions_From_ClassAuthorizedFakeTypedPageData()
        {
            _locator = new AuthorizedPropertyDefinitionLocator(_classLevel, _classLevel.GetType());
            var actualDefinitionList = _locator.GetAuthorizedPropertyDefinitions();
            Assert.IsTrue(actualDefinitionList.Count == 2);
        }

        [Test]
        public void Locator_Should_Discover_Three_Definitions_From_PropertyAuthorizedTypedPageData()
        {
            _locator = new AuthorizedPropertyDefinitionLocator(_propertyLevel, _propertyLevel.GetType());
            var actualDefinitionList = _locator.GetAuthorizedPropertyDefinitions();
            Assert.IsTrue(actualDefinitionList.Count == 3);
        }

        [Test]
        public void Locator_Should_Discover_Six_Definitions_From_InheritedAuthorizedTypedPageData()
        {
            _locator = new AuthorizedPropertyDefinitionLocator(_inheritedLevel, _inheritedLevel.GetType());
            var actualDefinitionList = _locator.GetAuthorizedPropertyDefinitions();
            Assert.IsTrue(actualDefinitionList.Count == 6);
        }

        [Test]
        public void Located_PropertyAttribute_Should_Override_Class_Attribute()
        {
            _locator = new AuthorizedPropertyDefinitionLocator(_inheritedLevel, _inheritedLevel.GetType());
            var actualDefinitionList = _locator.GetAuthorizedPropertyDefinitions();
            var propertyThreeDefinition = actualDefinitionList.Single(d => d.PropertyName == "Property3");
            Assert.IsTrue(propertyThreeDefinition.AuthorizedPrincipals.Contains("Role2"));
        }

        [Test]
        public void Locater_Should_Discover__DefaultProperties_If_From_DefaultAuthorizedPageData()
        {
            _locator = new AuthorizedPropertyDefinitionLocator(_defaultLevel, _defaultLevel.GetType());
            var actualDefinitionList = _locator.GetAuthorizedPropertyDefinitions();
            Assert.IsTrue(actualDefinitionList.Count == 1);
        }


    }
}