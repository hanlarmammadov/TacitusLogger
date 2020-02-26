using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Destinations;
using TacitusLogger.Strategies.DestinationFeeding;
using System.Linq;
using TacitusLogger.Exceptions;
using TacitusLogger.Tests.Helpers;

namespace TacitusLogger.UnitTests.StrategyTests.DestinationFeeding
{
    [TestFixture]
    public class GreedyDestinationFeedingStrategyTests
    {
        #region Tests for Feed method

        [Test]
        public void Feed_When_Called_Sends_Logs_Collection_To_All_Specified_Destinations()
        {
            //Arrange
            LogModel[] logs = new LogModel[]
            {
                Samples.LogModels.Standard(),
                Samples.LogModels.Standard(),
            };
            var destination1Mock = new Mock<ILogDestination>();
            var destination2Mock = new Mock<ILogDestination>();
            var destination3Mock = new Mock<ILogDestination>();
            List<ILogDestination> destinations = new List<ILogDestination>()
            {
                destination1Mock.Object,
                destination2Mock.Object,
                destination3Mock.Object,
            };
            GreedyDestinationFeedingStrategy greedyDestinationFeedingStrategy = new GreedyDestinationFeedingStrategy();

            //Act
            greedyDestinationFeedingStrategy.Feed(logs, destinations);

            //Assert
            destination1Mock.Verify(x => x.Send(logs), Times.Once);
            destination2Mock.Verify(x => x.Send(logs), Times.Once);
            destination3Mock.Verify(x => x.Send(logs), Times.Once);
        }
        [Test]
        public void Feed_When_Called_Never_Calls_Async_Write_Methods_Of_Destinations()
        {
            //Arrange
            LogModel[] logs = new LogModel[]
            {
                Samples.LogModels.Standard(),
                Samples.LogModels.Standard(),
            };
            var destination1Mock = new Mock<ILogDestination>();
            List<ILogDestination> destinations = new List<ILogDestination>()
            {
                destination1Mock.Object,
            };
            GreedyDestinationFeedingStrategy greedyDestinationFeedingStrategy = new GreedyDestinationFeedingStrategy();

            //Act
            greedyDestinationFeedingStrategy.Feed(logs, destinations);

            //Assert
            destination1Mock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        [Test]
        public void Feed_When_Called_And_One_Destination_Throws_It_Does_Not_Prevent_Other_Destinations_From_Being_Fet_And_Feed_Throws_AggregateException()
        {
            //Arrange
            LogModel[] logs = new LogModel[]
            {
                Samples.LogModels.Standard(),
                Samples.LogModels.Standard(),
            };
            var exceptionFromFirstDestination = new Exception("Exception from first log destination");
            var exceptionFromSecondDestination = new Exception("Exception from second log destination");
            var exceptionFromThirdDestination = new Exception("Exception from third log destination");
            var destination1Mock = new Mock<ILogDestination>();
            destination1Mock.Setup(x => x.Send(It.IsAny<LogModel[]>())).Throws(exceptionFromFirstDestination);
            var destination2Mock = new Mock<ILogDestination>();
            destination2Mock.Setup(x => x.Send(It.IsAny<LogModel[]>())).Throws(exceptionFromSecondDestination);
            var destination3Mock = new Mock<ILogDestination>();
            destination3Mock.Setup(x => x.Send(It.IsAny<LogModel[]>())).Throws(exceptionFromThirdDestination);
            List<ILogDestination> destinations = new List<ILogDestination>()
            {
                destination1Mock.Object,
                destination2Mock.Object,
                destination3Mock.Object,
            };
            GreedyDestinationFeedingStrategy greedyDestinationFeedingStrategy = new GreedyDestinationFeedingStrategy();

            // Assert that AggregateException is thrown.
            DestinationFeedingException destinationFeedingException = Assert.Catch<DestinationFeedingException>(() =>
            {
                //Act
                greedyDestinationFeedingStrategy.Feed(logs, destinations);
            });
            Assert.IsInstanceOf<AggregateException>(destinationFeedingException.InnerException);
            var aggregateException = (AggregateException)destinationFeedingException.InnerException;
            // Assert that all thrown exceptions are in the aggregate exception. 
            Assert.AreEqual(3, aggregateException.InnerExceptions.Count);
            Assert.Contains(exceptionFromFirstDestination, aggregateException.InnerExceptions);
            Assert.Contains(exceptionFromSecondDestination, aggregateException.InnerExceptions);
            Assert.Contains(exceptionFromThirdDestination, aggregateException.InnerExceptions);
            // Assert that methods of all three log destinations are called
            destination1Mock.Verify(x => x.Send(logs), Times.Once);
            destination2Mock.Verify(x => x.Send(logs), Times.Once);
            destination3Mock.Verify(x => x.Send(logs), Times.Once);
        }

        #endregion

        #region Tests for FeedAsync method

        [Test]
        public async Task FeedAsync_When_Called_Sends_Logs_Collection_To_All_Specified_Destinations()
        {
            //Arrange
            LogModel[] logs = new LogModel[]
            {
                Samples.LogModels.Standard(),
                Samples.LogModels.Standard(),
            };
            var destination1Mock = new Mock<ILogDestination>();
            var destination2Mock = new Mock<ILogDestination>();
            var destination3Mock = new Mock<ILogDestination>();
            List<ILogDestination> destinations = new List<ILogDestination>()
            {
                destination1Mock.Object,
                destination2Mock.Object,
                destination3Mock.Object,
            };
            GreedyDestinationFeedingStrategy greedyDestinationFeedingStrategy = new GreedyDestinationFeedingStrategy();
            CancellationToken cancellationToken = new CancellationToken();

            //Act
            await greedyDestinationFeedingStrategy.FeedAsync(logs, destinations);

            //Assert
            destination1Mock.Verify(x => x.SendAsync(logs, cancellationToken), Times.Once);
            destination2Mock.Verify(x => x.SendAsync(logs, cancellationToken), Times.Once);
            destination3Mock.Verify(x => x.SendAsync(logs, cancellationToken), Times.Once);
        }
        [Test]
        public async Task FeedAsync_When_Called_Never_Calls_Sync_Write_Methods_Of_Destinations()
        {
            //Arrange
            LogModel[] logs = new LogModel[]
            {
                Samples.LogModels.Standard(),
                Samples.LogModels.Standard(),
            };
            var destination1Mock = new Mock<ILogDestination>();
            List<ILogDestination> destinations = new List<ILogDestination>()
            {
                destination1Mock.Object,
            };
            GreedyDestinationFeedingStrategy greedyDestinationFeedingStrategy = new GreedyDestinationFeedingStrategy();

            //Act
            await greedyDestinationFeedingStrategy.FeedAsync(logs, destinations);

            //Assert
            destination1Mock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
        }
        [Test]
        public void FeedAsync_When_Called_And_One_Destination_Throws_It_Does_Not_Prevent_Other_Destinations_From_Being_Fet_And_FeedAsync_Throws_AggregateException()
        {
            //Arrange
            LogModel[] logs = new LogModel[]
            {
                Samples.LogModels.Standard(),
                Samples.LogModels.Standard(),
            };
            var exceptionFromFirstDestination = new Exception("Exception from first log destination");
            var exceptionFromSecondDestination = new Exception("Exception from second log destination");
            var exceptionFromThirdDestination = new Exception("Exception from third log destination");
            var destination1Mock = new Mock<ILogDestination>();
            destination1Mock.Setup(x => x.SendAsync(It.IsAny<LogModel[]>(), default)).ThrowsAsync(exceptionFromFirstDestination);
            var destination2Mock = new Mock<ILogDestination>();
            destination2Mock.Setup(x => x.SendAsync(It.IsAny<LogModel[]>(), default)).ThrowsAsync(exceptionFromSecondDestination);
            var destination3Mock = new Mock<ILogDestination>();
            destination3Mock.Setup(x => x.SendAsync(It.IsAny<LogModel[]>(), default)).ThrowsAsync(exceptionFromThirdDestination);
            List<ILogDestination> destinations = new List<ILogDestination>()
            {
                destination1Mock.Object,
                destination2Mock.Object,
                destination3Mock.Object,
            };
            GreedyDestinationFeedingStrategy greedyDestinationFeedingStrategy = new GreedyDestinationFeedingStrategy();

            // Assert that AggregateException is thrown.
            DestinationFeedingException destinationFeedingException = Assert.CatchAsync<DestinationFeedingException>(async () =>
            {
                //Act
                await greedyDestinationFeedingStrategy.FeedAsync(logs, destinations);
            });
            Assert.IsInstanceOf<AggregateException>(destinationFeedingException.InnerException);
            var aggregateException = (AggregateException)destinationFeedingException.InnerException;
            // Assert that all thrown exceptions are in the aggregate exception. 
            Assert.AreEqual(3, aggregateException.InnerExceptions.Count);
            Assert.Contains(exceptionFromFirstDestination, aggregateException.InnerExceptions);
            Assert.Contains(exceptionFromSecondDestination, aggregateException.InnerExceptions);
            Assert.Contains(exceptionFromThirdDestination, aggregateException.InnerExceptions);
            // Assert that methods of all three log destinations are called
            destination1Mock.Verify(x => x.SendAsync(logs, default), Times.Once);
            destination2Mock.Verify(x => x.SendAsync(logs, default), Times.Once);
            destination3Mock.Verify(x => x.SendAsync(logs, default), Times.Once);
        }
        [Test]
        public void FeedAsync_When_Called_With_Cancelled_Cancellation_Immediately_Returns_Cancelled_Task()
        {
            //Arrange 
            GreedyDestinationFeedingStrategy greedyDestinationFeedingStrategy = new GreedyDestinationFeedingStrategy();
            CancellationToken cancellationToken = new CancellationToken(canceled: true);
            var destinationMocks = new List<Mock<ILogDestination>>() { new Mock<ILogDestination>(), new Mock<ILogDestination>() };

            Assert.CatchAsync<TaskCanceledException>(async () =>
            {
                //Act
                await greedyDestinationFeedingStrategy.FeedAsync(new LogModel[] { Samples.LogModels.Standard() }, destinationMocks.Select(x => x.Object).ToList(), cancellationToken);
            });
            foreach (var mock in destinationMocks)
                mock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion
    }
}