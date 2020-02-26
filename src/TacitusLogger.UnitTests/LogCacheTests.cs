using Moq;
using NUnit.Framework;
using System;
using TacitusLogger.Components.Time;
using TacitusLogger.Caching;

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class LogCacheTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Initializes_Properties_Correctly()
        {
            // Arrange
            int cacheSize = 10;
            int cacheTime = 5000;

            // Act
            InMemoryLogCache logCache = new InMemoryLogCache(cacheSize, cacheTime);

            // Assert
            Assert.IsInstanceOf<SystemTimeProvider>(logCache.TimeProvider);
            Assert.AreEqual(cacheSize, logCache.CacheSize);
            Assert.AreEqual(cacheTime, logCache.CacheTime);
            Assert.IsNotNull(logCache.LogModelCollection);
            Assert.AreEqual(cacheSize, logCache.LogModelCollection.Length);
            Assert.AreEqual(0, logCache.CurrentIndex);
        }

        [Test]
        public void Ctor_When_Called_Without_Milliseconds_Sets_Milliseconds_To_Neg1()
        {
            // Arrange
            int cacheSize = 10;

            // Act
            InMemoryLogCache logCache = new InMemoryLogCache(cacheSize);

            // Assert 
            Assert.AreEqual(-1, logCache.CacheTime);
        }

        [Test]
        public void Ctor_When_Called_With_Int32Max_Milliseconds_Does_Not_Cause_Overflow()
        {
            // Arrange
            var timeProvider = new Mock<ITimeProvider>().Object;
            int cacheSize = 10;

            // Act
            InMemoryLogCache logCache = new InMemoryLogCache(cacheSize, Int32.MaxValue);
        }

        [TestCase(int.MinValue)]
        [TestCase(-2)]
        [TestCase(-1)]
        [TestCase(0)]
        public void Ctor_When_Called_With_Invalid_Cache_Size_Throws_ArgumentException(int invalidCacheSize)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                // Act
                InMemoryLogCache logCache = new InMemoryLogCache(invalidCacheSize);
            });
        }

        [TestCase(int.MinValue)]
        [TestCase(-2)]
        [TestCase(0)]
        public void Ctor_When_Called_With_Invalid_Milliseconds_Throws_ArgumentException(int invalidMilliseconds)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                // Act
                InMemoryLogCache logCache = new InMemoryLogCache(10, invalidMilliseconds);
            });
        }

        #endregion

        #region Tests for AddToCache method

        [Test]
        public void AddToCache_When_Called_Add_LogModel_To_Cache()
        {
            // Arrange
            int cacheSize = 3;
            int cacheTime = 5000;
            InMemoryLogCache logCache = new InMemoryLogCache(cacheSize, cacheTime);
            LogModel logModel = new LogModel();

            // Act
            LogModel[] flushed = logCache.AddToCache(logModel);

            // Assert
            Assert.AreEqual(3, logCache.LogModelCollection.Length);
            Assert.AreEqual(logModel, logCache.LogModelCollection[0]);
            Assert.IsNull(logCache.LogModelCollection[1]);
            Assert.IsNull(logCache.LogModelCollection[2]);
            Assert.AreEqual(1, logCache.CurrentIndex);
            // Cache has not filled yet, that is why it should not flush the collection.
            Assert.IsNull(flushed);
        }

        [Test]
        public void AddToCache_When_Called_Until_Cache_Is_Filled_And_Once_Again_Flush_The_Cache()
        {
            // Arrange 
            int cacheSize = 4;
            InMemoryLogCache logCache = new InMemoryLogCache(cacheSize);
            LogModel logModel1 = new LogModel();
            LogModel logModel2 = new LogModel();
            LogModel logModel3 = new LogModel();
            LogModel logModel4 = new LogModel();

            // Act
            logCache.AddToCache(logModel1);
            logCache.AddToCache(logModel2);
            logCache.AddToCache(logModel3);
            LogModel[] flushedOn4thCall = logCache.AddToCache(logModel4);

            // Assert
            Assert.AreEqual(4, logCache.LogModelCollection.Length);
            // The new collection has been created with nulls
            Assert.IsNull(logCache.LogModelCollection[0]);
            Assert.IsNull(logCache.LogModelCollection[1]);
            Assert.IsNull(logCache.LogModelCollection[2]);
            Assert.IsNull(logCache.LogModelCollection[3]);
            // Current context has been reset
            Assert.AreEqual(0, logCache.CurrentIndex);
            // Cache HAS filled, that is why it should flush the collection. 
            Assert.IsNotNull(flushedOn4thCall);
            // Flushed collection should contain all added log model entities.
            Assert.AreEqual(4, flushedOn4thCall.Length);
            Assert.AreEqual(logModel1, flushedOn4thCall[0]);
            Assert.AreEqual(logModel2, flushedOn4thCall[1]);
            Assert.AreEqual(logModel3, flushedOn4thCall[2]);
            Assert.AreEqual(logModel4, flushedOn4thCall[3]);
        }

        [Test]
        public void AddToCache_When_Called_Given_That_Cache_Time_Passed_Flush_Collection_Despite_It_Is_Not_Filled_Yet()
        {
            // Arrange
            var timeProviderMock = new Mock<ITimeProvider>();
            timeProviderMock.Setup(x => x.GetLocalTime()).Returns(new DateTime(0));
            int cacheSize = 4;
            int cacheTime = 500;
            InMemoryLogCache logCache = new InMemoryLogCache(cacheSize, cacheTime);
            logCache.ResetTimeProvider(timeProviderMock.Object);
            LogModel logModel1 = new LogModel();
            LogModel logModel2 = new LogModel();
            LogModel logModel3 = new LogModel();

            // Act
            logCache.AddToCache(logModel1);
            logCache.AddToCache(logModel2);
            // Re-setup time provider to indicate that cache time has been expired.
            timeProviderMock.Setup(x => x.GetLocalTime()).Returns(new DateTime(500 * 10_000 + 1));
            LogModel[] flushedOn3rdCall = logCache.AddToCache(logModel3);

            // Assert
            Assert.NotNull(flushedOn3rdCall);
            Assert.AreEqual(4, logCache.LogModelCollection.Length);
            // The new collection has been created with nulls
            Assert.IsNull(logCache.LogModelCollection[0]);
            Assert.IsNull(logCache.LogModelCollection[1]);
            Assert.IsNull(logCache.LogModelCollection[2]);
            Assert.IsNull(logCache.LogModelCollection[3]);
            // Current context has been reset
            Assert.AreEqual(0, logCache.CurrentIndex);
            // Cache time expired, that is why it should flush the collection. 
            Assert.IsNotNull(flushedOn3rdCall);
            // Flushed collection should contain all added log model entities.
            Assert.AreEqual(3, flushedOn3rdCall.Length);
            Assert.AreEqual(logModel1, flushedOn3rdCall[0]);
            Assert.AreEqual(logModel2, flushedOn3rdCall[1]);
            Assert.AreEqual(logModel3, flushedOn3rdCall[2]);
        }

        [Test]
        public void AddToCache_When_Called_Given_That_Cache_Size_Is_One_Flush_Collection_Correctly()
        {
            // Arrange 
            int cacheSize = 1;
            InMemoryLogCache logCache = new InMemoryLogCache(cacheSize);
            LogModel logModel1 = new LogModel();

            // Act
            LogModel[] flushedOn1stCall = logCache.AddToCache(logModel1);

            // Assert
            Assert.IsNotNull(flushedOn1stCall);
            Assert.AreEqual(1, flushedOn1stCall.Length);
            Assert.AreEqual(logModel1, flushedOn1stCall[0]);
        }

        #endregion

        #region Tests for ToString method

        [Test]
        public void ToString_When_Called_Returns_String_Representation()
        {
            // Arrange
            InMemoryLogCache inMemoryLogCache = new InMemoryLogCache(10, 10000);

            // Act
            var result = inMemoryLogCache.ToString();

            // Arrange
            Assert.NotNull(result);
            Assert.IsTrue(result.Contains("TacitusLogger.Caching.InMemoryLogCache"));
            Assert.IsTrue(result.Contains("Cache size: 10"));
            Assert.IsTrue(result.Contains("Cache time (in milliseconds): 10000"));
            Assert.IsTrue(result.Contains("Time provider: TacitusLogger.Components.Time.SystemTimeProvider"));
        }

        #endregion
    }
}
