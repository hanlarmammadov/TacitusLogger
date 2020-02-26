using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using TacitusLogger.Components.Json;
using TacitusLogger.Components.TemplateResolving.PlaceholderResolvers;

namespace TacitusLogger.UnitTests.PlaceholderResolverTests
{
    [TestFixture]
    public class LogItemsPlaceholderResolverTests
    {
        #region Test for Ctor

        [Test]
        public void Ctor_When_Called_Sets_Json_Serializer_Settings()
        {
            // Arrange           
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            var jsonSerializerFacade = new Mock<IJsonSerializerFacade>().Object;

            // Act
            LogItemsPlaceholderResolver logItemsPlaceholderResolver = new LogItemsPlaceholderResolver(ref jsonSerializerFacade, jsonSerializerSettings);

            // Assert
            Assert.AreEqual(jsonSerializerSettings, logItemsPlaceholderResolver.JsonSerializerSettings);
            Assert.AreEqual(jsonSerializerFacade, logItemsPlaceholderResolver.JsonSerializerFacade);
        }

        [Test]
        public void Ctor_When_Called_With_Null_Json_Serializer_Settings_Throws_ArgumentNullException()
        {
            var jsonSerializerFacade = new Mock<IJsonSerializerFacade>().Object;

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                LogItemsPlaceholderResolver logItemsPlaceholderResolver = new LogItemsPlaceholderResolver(ref jsonSerializerFacade, null);
            });
        }

        [Test]
        public void Ctor_When_Called_With_Null_Json_Serializer_Facade_Throws_ArgumentNullException()
        {
            IJsonSerializerFacade nullJsonSerializerFacade = null;

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                LogItemsPlaceholderResolver logItemsPlaceholderResolver = new LogItemsPlaceholderResolver(ref nullJsonSerializerFacade, new JsonSerializerSettings());
            });
        }

        #endregion

        #region Tests for Resolve method

        [Test]
        public void Resolve_When_Called_Given_That_Items_List_Is_Empty_Resolves_Placeholder_Correctly()
        {
            // Arrange 
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            IJsonSerializerFacade jsonSerializerFacade = new NewtonsoftJsonSerializerFacade();
            LogItemsPlaceholderResolver logItemsPlaceholderResolver = new LogItemsPlaceholderResolver(ref jsonSerializerFacade, jsonSerializerSettings);
            LogItem[] logItems = new LogItem[0];
            string initialString = $"$LogItems";

            // Act
            logItemsPlaceholderResolver.Resolve(new LogModel() { LogItems = logItems }, ref initialString);

            // Assert 
            Assert.AreEqual("", initialString);
        }

        [Test]
        public void Resolve_When_Called_Given_That_Items_List_Is_Null_Resolves_Placeholder_Correctly()
        {
            // Arrange 
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            IJsonSerializerFacade jsonSerializerFacade = new NewtonsoftJsonSerializerFacade();
            LogItemsPlaceholderResolver logItemsPlaceholderResolver = new LogItemsPlaceholderResolver(ref jsonSerializerFacade, jsonSerializerSettings);
            string initialString = $"$LogItems";

            // Act
            logItemsPlaceholderResolver.Resolve(new LogModel() { LogItems = null }, ref initialString);

            // Assert 
            Assert.AreEqual("", initialString);
        }

        [Test]
        public void Resolve_When_Called_Given_That_Items_Contain_One_Item_With_Object_Value_Resolves_Placeholder_Correctly()
        {
            // Arrange 
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            IJsonSerializerFacade jsonSerializerFacade = new NewtonsoftJsonSerializerFacade();
            LogItemsPlaceholderResolver logItemsPlaceholderResolver = new LogItemsPlaceholderResolver(ref jsonSerializerFacade, jsonSerializerSettings);
            LogItem[] logItems = new LogItem[]
            {
                new LogItem("Item", new { Name1 = "Value1", Name2 = "Value2"})
            };
            string initialString = $"$LogItems";

            // Act
            logItemsPlaceholderResolver.Resolve(new LogModel() { LogItems = logItems }, ref initialString);

            // Assert
            string expected = $"Item: {JsonConvert.SerializeObject(logItems[0].Value)}";
            Assert.AreEqual(expected, initialString);
        }

        [Test]
        public void Resolve_When_Called_Given_That_Items_Contain_One_Item_With_String_Value_Resolves_Placeholder_Correctly()
        {
            // Arrange 
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            IJsonSerializerFacade jsonSerializerFacade = new NewtonsoftJsonSerializerFacade();
            LogItemsPlaceholderResolver logItemsPlaceholderResolver = new LogItemsPlaceholderResolver(ref jsonSerializerFacade, jsonSerializerSettings);
            LogItem[] logItems = new LogItem[]
            {
                new LogItem("Item", "Item value")
            };
            string initialString = $"$LogItems";

            // Act
            logItemsPlaceholderResolver.Resolve(new LogModel() { LogItems = logItems }, ref initialString);

            // Assert
            string expected = $"Item: Item value";
            Assert.AreEqual(expected, initialString);
        }

        [Test]
        public void Resolve_When_Called_Given_That_Items_Contain_One_Item_With_Null_Value_Resolves_Placeholder_Correctly()
        {
            // Arrange 
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            IJsonSerializerFacade jsonSerializerFacade = new NewtonsoftJsonSerializerFacade();
            LogItemsPlaceholderResolver logItemsPlaceholderResolver = new LogItemsPlaceholderResolver(ref jsonSerializerFacade, jsonSerializerSettings);
            LogItem[] logItems = new LogItem[]
            {
                new LogItem("Item", null)
            };
            string initialString = $"$LogItems";

            // Act
            logItemsPlaceholderResolver.Resolve(new LogModel() { LogItems = logItems }, ref initialString);

            // Assert
            string expected = $"Item: null";
            Assert.AreEqual(expected, initialString);
        }

        [Test]
        public void Resolve_When_Called_Given_That_Items_Contain_One_Item_With_Null_Name_Resolves_Placeholder_Correctly()
        {
            // Arrange 
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            IJsonSerializerFacade jsonSerializerFacade = new NewtonsoftJsonSerializerFacade();
            LogItemsPlaceholderResolver logItemsPlaceholderResolver = new LogItemsPlaceholderResolver(ref jsonSerializerFacade, jsonSerializerSettings);
            LogItem[] logItems = new LogItem[]
            {
                new LogItem(null, "Item value")
            };
            string initialString = $"$LogItems";

            // Act
            logItemsPlaceholderResolver.Resolve(new LogModel() { LogItems = logItems }, ref initialString);

            // Assert
            string expected = $"Item: Item value";
            Assert.AreEqual(expected, initialString);
        }

        [Test]
        public void Resolve_When_Called_Given_That_Items_Contain_One_Item_With_Null_Name_And_Value_Resolves_Placeholder_Correctly()
        {
            // Arrange 
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            IJsonSerializerFacade jsonSerializerFacade = new NewtonsoftJsonSerializerFacade();
            LogItemsPlaceholderResolver logItemsPlaceholderResolver = new LogItemsPlaceholderResolver(ref jsonSerializerFacade, jsonSerializerSettings);
            LogItem[] logItems = new LogItem[]
            {
                new LogItem(null, null)
            };
            string initialString = $"$LogItems";

            // Act
            logItemsPlaceholderResolver.Resolve(new LogModel() { LogItems = logItems }, ref initialString);

            // Assert
            string expected = $"Item: null";
            Assert.AreEqual(expected, initialString);
        }

        [Test]
        public void Resolve_When_Called_Given_That_Items_Contain_Several_Items_With_Object_Values_Resolves_Placeholder_Correctly()
        {
            // Arrange 
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            IJsonSerializerFacade jsonSerializerFacade = new NewtonsoftJsonSerializerFacade();
            LogItemsPlaceholderResolver logItemsPlaceholderResolver = new LogItemsPlaceholderResolver(ref jsonSerializerFacade, jsonSerializerSettings);
            LogItem[] logItems = new LogItem[]
            {
                new LogItem("Item1", new { Name11 = "Value11", Name12 = "Value12"}),
                new LogItem("Item2", new { Name21 = "Value21", Name22 = "Value22"})
            };
            string initialString = $"$LogItems";

            // Act
            logItemsPlaceholderResolver.Resolve(new LogModel() { LogItems = logItems }, ref initialString);

            // Assert
            string expected = $"Item1: {JsonConvert.SerializeObject(logItems[0].Value)}{Environment.NewLine}Item2: {JsonConvert.SerializeObject(logItems[1].Value)}";
            Assert.AreEqual(expected, initialString);
        }

        [Test]
        public void Resolve_When_Called_Given_That_Items_Contain_Several_Items_With_Mixed_Object_And_String_Values_Resolves_Placeholder_Correctly()
        {
            // Arrange 
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            IJsonSerializerFacade jsonSerializerFacade = new NewtonsoftJsonSerializerFacade();
            LogItemsPlaceholderResolver logItemsPlaceholderResolver = new LogItemsPlaceholderResolver(ref jsonSerializerFacade, jsonSerializerSettings);
            LogItem[] logItems = new LogItem[]
            {
                new LogItem("Item1", new { Name11 = "Value11", Name12 = "Value12"}),
                new LogItem("Item2", "String value"),
                new LogItem("Item3", new { Name31 = "Value31", Name32 = "Value32"})
            };
            string initialString = $"$LogItems";

            // Act
            logItemsPlaceholderResolver.Resolve(new LogModel() { LogItems = logItems }, ref initialString);

            // Assert
            string expected = $"Item1: {JsonConvert.SerializeObject(logItems[0].Value)}{Environment.NewLine}Item2: String value{Environment.NewLine}Item3: {JsonConvert.SerializeObject(logItems[2].Value)}";
            Assert.AreEqual(expected, initialString);
        }

        [Test]
        public void Resolve_When_Called_Given_That_Template_Contains_Several_Same_Placeholders()
        {
            // Arrange 
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            IJsonSerializerFacade jsonSerializerFacade = new NewtonsoftJsonSerializerFacade();
            LogItemsPlaceholderResolver logItemsPlaceholderResolver = new LogItemsPlaceholderResolver(ref jsonSerializerFacade, jsonSerializerSettings);
            LogItem[] logItems = new LogItem[]
            {
                new LogItem("Item1", new { Name11 = "Value11", Name12 = "Value12"}),
                new LogItem("Item2", "String value"),
                new LogItem("Item3", new { Name31 = "Value31", Name32 = "Value32"})
            };
            string initialString = $"qwerty$LogItems$LogItemsqwerty";

            // Act
            logItemsPlaceholderResolver.Resolve(new LogModel() { LogItems = logItems }, ref initialString);

            // Assert
            string expected = $"Item1: {JsonConvert.SerializeObject(logItems[0].Value)}{Environment.NewLine}Item2: String value{Environment.NewLine}Item3: {JsonConvert.SerializeObject(logItems[2].Value)}";
            Assert.AreEqual($"qwerty{expected}{expected}qwerty", initialString);
        }

        [Test]
        public void Resolve_When_Called_Given_That_Template_Contains_Other_Symbols()
        {
            // Arrange 
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            IJsonSerializerFacade jsonSerializerFacade = new NewtonsoftJsonSerializerFacade();
            LogItemsPlaceholderResolver logItemsPlaceholderResolver = new LogItemsPlaceholderResolver(ref jsonSerializerFacade, jsonSerializerSettings);
            LogItem[] logItems = new LogItem[]
            {
                new LogItem("Item2", "String value"),
            };
            string initialString = $"qwertyLogItems";

            // Act
            logItemsPlaceholderResolver.Resolve(new LogModel() { LogItems = logItems }, ref initialString);

            // Assert
            string expected = "qwertyLogItems";
            Assert.AreEqual(expected , initialString);
        }

        #endregion
    }
}
