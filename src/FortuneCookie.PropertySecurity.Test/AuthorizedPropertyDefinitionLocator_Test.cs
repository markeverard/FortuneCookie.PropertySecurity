using System;
using System.Linq;
using System.Reflection;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using FortuneCookie.PropertySecurity.Discovery;
using NUnit.Framework;
using PageTypeBuilder;

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
  
        [SetUp] 
        public void Setup()
        {
            _classLevel = new ClassAuthorizedFakeTypedPageData();
            _propertyLevel = new PropertyAuthorizedTypedPageData();
            _noLevel = new NonAuthorizedTypedPageData();
            _inheritedLevel = new InheritedAuthorizedFakeTypedPageData();
        }

        [Test]
        public void Locator_Should_Not_Discover_Definitions_From_NonAuthorizedTypedPageData()
        {
            _locator = new AuthorizedPropertyDefinitionLocator(_noLevel, _noLevel.GetType(), new FakeTabDefinitionRepository());
            var actualDefinitionList = _locator.GetAuthorizedPropertyDefinitions();
            Assert.IsTrue(!actualDefinitionList.Any());
        }

        [Test]
        public void Locator_Should_Discover_Two_Definitions_From_ClassAuthorizedFakeTypedPageData()
        {
            _locator = new AuthorizedPropertyDefinitionLocator(_classLevel, _classLevel.GetType(), new FakeTabDefinitionRepository());
            var actualDefinitionList = _locator.GetAuthorizedPropertyDefinitions();
            Assert.IsTrue(actualDefinitionList.Count == 2);
        }

        [Test]
        public void Locator_Should_Discover_Three_Definitions_From_PropertyAuthorizedTypedPageData()
        {
            _locator = new AuthorizedPropertyDefinitionLocator(_propertyLevel, _propertyLevel.GetType(), new FakeTabDefinitionRepository());
            var actualDefinitionList = _locator.GetAuthorizedPropertyDefinitions();
            Assert.IsTrue(actualDefinitionList.Count == 3);
        }

        [Test]
        public void Locator_Should_Discover_Six_Definitions_From_InheritedAuthorizedTypedPageData()
        {
            _locator = new AuthorizedPropertyDefinitionLocator(_inheritedLevel, _inheritedLevel.GetType(), new FakeTabDefinitionRepository());
            var actualDefinitionList = _locator.GetAuthorizedPropertyDefinitions();
            Assert.IsTrue(actualDefinitionList.Count == 6);
        }

        [Test]
        public void Located_PropertyAttribute_Should_Override_Class_Attribute()
        {
            _locator = new AuthorizedPropertyDefinitionLocator(_inheritedLevel, _inheritedLevel.GetType(), new FakeTabDefinitionRepository());
            var actualDefinitionList = _locator.GetAuthorizedPropertyDefinitions();
            var propertyThreeDefinition = actualDefinitionList.Single(d => d.PropertyName == "Property3");
            Assert.IsTrue(propertyThreeDefinition.AuthorizedPrincipals.Contains("Role2"));
        }

        #region Tab related tests

        [Test]
        public void Locator_Should_Discover_Two_Definitions_From_TabAuthorizedTypedPageData()
        {
            TabAuthorizedTypedPageData typedPageData = new TabAuthorizedTypedPageData();

            TabDefinitionCollection tabDefinitions = new FakeTabDefinitionRepository().List();
            typedPageData.Property.Add(new PropertyString
                                           {
                                               Name = "PageName",
                                               OwnerTab = tabDefinitions.First(t => t.Name == "Information").ID
                                           });

            SetupTabBasedPropertiesAndPropertyGroupPageDataProperties(typedPageData);
            _locator = new AuthorizedPropertyDefinitionLocator(typedPageData, typedPageData.GetType(), new FakeTabDefinitionRepository());
            var actualDefinitionList = _locator.GetAuthorizedPropertyDefinitions();
            Assert.IsTrue(actualDefinitionList.Count == 2);
            var propertyOneDefinition = actualDefinitionList.Single(d => d.PropertyName == "Property1");
            Assert.IsTrue(propertyOneDefinition.AuthorizedPrincipals.Contains("Role1"));
            var pageCategoryDefinition = actualDefinitionList.Single(d => d.PropertyName == "PageName");
            Assert.IsTrue(pageCategoryDefinition.AuthorizedPrincipals.Contains("Role2"));
        }

        #endregion Tab related tests

        #region Property group related tests

        [Test]
        public void Locator_Should_Discover_One_Definition_From_PropertyGroupAuthorizedTypedPageData()
        {
            PropertyGroupAuthorizedTypedPageData typedPageData = new PropertyGroupAuthorizedTypedPageData();
            SetupTabBasedPropertiesAndPropertyGroupPageDataProperties(typedPageData);
            _locator = new AuthorizedPropertyDefinitionLocator(typedPageData, typedPageData.GetType(), new FakeTabDefinitionRepository());
            var actualDefinitionList = _locator.GetAuthorizedPropertyDefinitions();
            Assert.IsTrue(actualDefinitionList.Count == 1);
            var propertyOneDefinition = actualDefinitionList.Single(d => d.PropertyName == "PropertyGroup1-Property1");
            Assert.IsTrue(propertyOneDefinition.AuthorizedPrincipals.Contains("Role1"));
        }

        [Test]
        public void Locator_Should_Discover_One_Definition_From_PropertyGroupWithClassLevelRulesAuthorizedTypedPageData()
        {
            var typedPageData = new PropertyGroupWithClassLevelRulesAuthorizedTypedPageData();
            SetupTabBasedPropertiesAndPropertyGroupPageDataProperties(typedPageData);

            _locator = new AuthorizedPropertyDefinitionLocator(typedPageData, typedPageData.GetType(), new FakeTabDefinitionRepository());
            
            var actualDefinitionList = _locator.GetAuthorizedPropertyDefinitions();
            Assert.IsTrue(actualDefinitionList.Count == 1);
            
            //var propertyOneDefinition = actualDefinitionList.Single(d => d.PropertyName == "PropertyGroup1-Property1");
            //Assert.IsTrue(propertyOneDefinition.AuthorizedPrincipals.Contains("Role1"));
        }

        [Test]
        public void Locator_Should_Discover_One_Definition_From_PropertyGroupWithPropertyGroupLevelAuthorizedTypedPageData()
        {
            PropertyGroupWithPropertyGroupLevelAuthorizedTypedPageData typedPageData = new PropertyGroupWithPropertyGroupLevelAuthorizedTypedPageData();
            SetupTabBasedPropertiesAndPropertyGroupPageDataProperties(typedPageData);
            _locator = new AuthorizedPropertyDefinitionLocator(typedPageData, typedPageData.GetType(), new FakeTabDefinitionRepository());
            var actualDefinitionList = _locator.GetAuthorizedPropertyDefinitions();
            Assert.IsTrue(actualDefinitionList.Count == 1);
            var propertyOneDefinition = actualDefinitionList.Single(d => d.PropertyName == "PropertyGroup1-Property1");
            Assert.IsTrue(propertyOneDefinition.AuthorizedPrincipals.Contains("Role2"));
        }

        [Test]
        public void Locator_Should_Discover_One_Definition_From_PropertyGroupWithTabAuthorizationAuthorizedTypedPageData()
        {
            PropertyGroupWithTabAuthorizationAuthorizedTypedPageData typedPageData = new PropertyGroupWithTabAuthorizationAuthorizedTypedPageData();
            SetupTabBasedPropertiesAndPropertyGroupPageDataProperties(typedPageData);
            _locator = new AuthorizedPropertyDefinitionLocator(typedPageData, typedPageData.GetType(), new FakeTabDefinitionRepository());
            var actualDefinitionList = _locator.GetAuthorizedPropertyDefinitions();
            Assert.IsTrue(actualDefinitionList.Count == 1);
            var propertyOneDefinition = actualDefinitionList.Single(d => d.PropertyName == "PropertyGroup1-Property1");
            Assert.IsTrue(propertyOneDefinition.AuthorizedPrincipals.Contains("Role1"));   
        }

        #endregion Property group related tests

        private void SetupTabBasedPropertiesAndPropertyGroupPageDataProperties(TypedPageData typedPageData)
        {
            TabDefinitionCollection tabDefinitions = new FakeTabDefinitionRepository().List();
            Type type = typedPageData.GetType();

            foreach (PropertyInfo property in AttributedTypesUtility.GetPublicOrPrivateProperties(type)
                .Where(AttributedTypesUtility.PropertyHasAttribute<PageTypePropertyAttribute>))
            {
                int tabDefinitionId = -1;
                PageTypePropertyAttribute pageTypePropertyAttribute = AttributedTypesUtility.GetAttributesFromProperty<PageTypePropertyAttribute>(property).First();
                
                if (pageTypePropertyAttribute.Tab != null)
                {
                    Tab tab = Activator.CreateInstance(pageTypePropertyAttribute.Tab) as Tab;
                    TabDefinition tabDefinition = tabDefinitions.FirstOrDefault(t => t.Name == tab.Name);

                    if (tabDefinition != null)
                        tabDefinitionId = tabDefinition.ID;
                }

                typedPageData.Property.Add(new PropertyString { Name = property.Name, OwnerTab = tabDefinitionId });
            }

            foreach (PropertyInfo propertyGroupProperty in AttributedTypesUtility.GetPublicOrPrivateProperties(type)
                .Where(AttributedTypesUtility.PropertyHasAttribute<PageTypePropertyGroupAttribute>))
            {
                foreach (PropertyInfo property in AttributedTypesUtility.GetPublicOrPrivateProperties(propertyGroupProperty.PropertyType)
                    .Where(AttributedTypesUtility.PropertyHasAttribute<PageTypePropertyAttribute>))
                    {
                        int tabDefinitionId = -1;
                        PageTypePropertyGroupAttribute pageTypePropertyGroupAttribute = AttributedTypesUtility.GetAttributesFromProperty<PageTypePropertyGroupAttribute>(propertyGroupProperty).First();

                        if (pageTypePropertyGroupAttribute.Tab != null)
                        {
                            Tab tab = Activator.CreateInstance(pageTypePropertyGroupAttribute.Tab) as Tab;
                            TabDefinition tabDefinition = tabDefinitions.FirstOrDefault(t => t.Name == tab.Name);

                            if (tabDefinition != null)
                                tabDefinitionId = tabDefinition.ID;
                        }

                        typedPageData.Property.Add(new PropertyString
                                                       {
                                                           Name = string.Format("{0}-{1}", propertyGroupProperty.Name, property.Name),
                                                           OwnerTab = tabDefinitionId
                                                       });
                    }
            }
        }

    }
}