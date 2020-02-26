using NUnit.Framework;
using TacitusLogger.Caching;

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class LogGroupExtensionsTests
    {
        [Test]
        public void SetLogCache_Taking_CacheSize_And_CacheTime_And_Is_Active_Flag_When_Called_Sets_Caching()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            int cacheSize = 10;
            int cacheTime = 1000;

            // Act
            LogGroupExtensions.SetLogCache(logGroup, cacheSize, cacheTime, true);

            // Assert
            Assert.IsTrue(logGroup.CachingIsActive);
            Assert.IsInstanceOf<InMemoryLogCache>(logGroup.LogCache);
        }
        [TestCase(true)]
        [TestCase(false)]
        public void SetLogCache_Taking_CacheSize_And_CacheTime_And_Is_Active_Flag_When_Called_Sets_Caching_Status_Correctly(bool isActive)
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);

            // Act
            LogGroupExtensions.SetLogCache(logGroup, 10, 1000, isActive);

            // Assert
            Assert.AreEqual(isActive, logGroup.CachingIsActive);
        }
        [Test]
        public void SetLogCache_Taking_CacheSize_And_CacheTime_And_Is_Active_Flag_When_Called_Returns_LogGroup()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);

            // Act
            var returned = LogGroupExtensions.SetLogCache(logGroup, 10, 1000, true);

            // Assert
            Assert.AreEqual(logGroup, returned);
        }
        [Test]
        public void SetLogCache_Taking_CacheSize_And_CacheTime_And_Is_Active_Flag_When_Called_Without_Cache_Time_And_Flag_Sets_Defaults()
        {
            // Arrange
            LogGroup logGroup = new LogGroup((l) => true);

            // Act
            LogGroupExtensions.SetLogCache(logGroup, 10);

            // Assert
            Assert.IsTrue(logGroup.CachingIsActive);
            Assert.AreEqual(-1, (logGroup.LogCache as InMemoryLogCache).CacheTime);
        }

        [TestCase(LogGroupStatus.Active)]
        [TestCase(LogGroupStatus.Inactive)]
        public void SetStatus_LogGroupStatus_When_Called_Sets_Const_Value_Provider_As_Log_Group_Status_Value_Provider(LogGroupStatus logGroupStatus)
        {
            // Arrange
            LogGroup logGroup = new LogGroup((l) => true); 

            // Act
            logGroup.SetStatus(logGroupStatus);

            // Assert
            Assert.IsInstanceOf<Setting<LogGroupStatus>>(logGroup.Status);
            Assert.AreEqual(logGroupStatus, logGroup.Status.Value);
        }
        [Test]
        public void SetStatus_LogGroupStatus_When_Called_Returns_Log_Group()
        {
            // Arrange
            LogGroup logGroup = new LogGroup((l) => true);

            // Act
            var returned = logGroup.SetStatus(LogGroupStatus.Inactive);

            // Assert 
            Assert.AreEqual(logGroup, returned);
        }
    }
}
