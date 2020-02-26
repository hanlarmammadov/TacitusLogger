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
    public class FirstSuccessDestinationFeedingStrategyTests
    {
        #region Tests for Feed method

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
            FirstSuccessDestinationFeedingStrategy firstSuccessDestinationFeedingStrategy = new FirstSuccessDestinationFeedingStrategy();

            //Act
            firstSuccessDestinationFeedingStrategy.Feed(logs, destinations);

            //Assert
            destination1Mock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void Feed_When_Called__Calls_Destinations_Until_One_Returns_Successfully_Or_Destinations_Got_Exhausted_Then_All_Thrown_Exceptions_Are_Rethrown_In_AggregateException()
        {
            //Arrange
            LogModel[] logs = new LogModel[]
            {
                Samples.LogModels.Standard(),
                Samples.LogModels.Standard(),
            };
            var exceptionFromFirstDestination = new Exception("Exception from first log destination");
            var destination1Mock = new Mock<ILogDestination>();
            // Only the first destination is configured to throw.
            destination1Mock.Setup(x => x.Send(It.IsAny<LogModel[]>())).Throws(exceptionFromFirstDestination);
            var destination2Mock = new Mock<ILogDestination>();
            var destination3Mock = new Mock<ILogDestination>();
            List<ILogDestination> destinations = new List<ILogDestination>()
            {
                destination1Mock.Object,
                destination2Mock.Object,
                destination3Mock.Object,
            };
            FirstSuccessDestinationFeedingStrategy firstSuccessDestinationFeedingStrategy = new FirstSuccessDestinationFeedingStrategy();

            // Assert that AggregateException is thrown.
            DestinationFeedingException destinationFeedingException = Assert.Catch<DestinationFeedingException>(() =>
            {
                //Act
                firstSuccessDestinationFeedingStrategy.Feed(logs, destinations);
            });
            Assert.IsInstanceOf<AggregateException>(destinationFeedingException.InnerException);
            var aggregateException = (AggregateException)destinationFeedingException.InnerException;
            // Assert that all thrown exceptions are in the aggregate exception. 
            Assert.AreEqual(1, aggregateException.InnerExceptions.Count);
            Assert.Contains(exceptionFromFirstDestination, aggregateException.InnerExceptions);
            // Assert that methods was called till first success.
            destination1Mock.Verify(x => x.Send(logs), Times.Once);
            destination2Mock.Verify(x => x.Send(logs), Times.Once);
            destination3Mock.Verify(x => x.Send(logs), Times.Never);
        }

        [Test]
        public void Feed_When_Called_And_No_Exception_Is_Thrown_Sends_Logs_Collection_To_First_Destination_And_Return()
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
            FirstSuccessDestinationFeedingStrategy firstSuccessDestinationFeedingStrategy = new FirstSuccessDestinationFeedingStrategy();

            //Act
            firstSuccessDestinationFeedingStrategy.Feed(logs, destinations);

            //Assert
            destination1Mock.Verify(x => x.Send(logs), Times.Once);
            destination2Mock.Verify(x => x.Send(logs), Times.Never);
            destination3Mock.Verify(x => x.Send(logs), Times.Never);
        }

        [Test]
        public void Feed_When_Called_Given_That_All_Destinations_Throw_Exhaust_The_List_And_Rethrow_Exceptions_In_AggregateException()
        {
            //Arrange
            LogModel[] logs = new LogModel[]
            {
                Samples.LogModels.Standard(),
                Samples.LogModels.Standard(),
            };
            // All three destinations are configured to throw.
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
            FirstSuccessDestinationFeedingStrategy firstSuccessDestinationFeedingStrategy = new FirstSuccessDestinationFeedingStrategy();

            // Assert that AggregateException is thrown.
            DestinationFeedingException destinationFeedingException = Assert.Catch<DestinationFeedingException>(() =>
            {
                //Act
                firstSuccessDestinationFeedingStrategy.Feed(logs, destinations);
            });
            Assert.IsInstanceOf<AggregateException>(destinationFeedingException.InnerException);
            var aggregateException = (AggregateException)destinationFeedingException.InnerException;
            // Assert that all thrown exceptions are in the aggregate exception. 
            Assert.AreEqual(3, aggregateException.InnerExceptions.Count);
            Assert.Contains(exceptionFromFirstDestination, aggregateException.InnerExceptions);
            Assert.Contains(exceptionFromSecondDestination, aggregateException.InnerExceptions);
            Assert.Contains(exceptionFromThirdDestination, aggregateException.InnerExceptions);
            // Assert that methods was called till first success.
            destination1Mock.Verify(x => x.Send(logs), Times.Once);
            destination2Mock.Verify(x => x.Send(logs), Times.Once);
            destination3Mock.Verify(x => x.Send(logs), Times.Once);
        }

        [Test]
        public void Feed_When_Called_Given_That_Destinations_List_Is_Empty_Returns_Without_Throwing()
        {
            //Arrange
            LogModel[] logs = new LogModel[]
            {
                Samples.LogModels.Standard(),
                Samples.LogModels.Standard(),
            };
            FirstSuccessDestinationFeedingStrategy firstSuccessDestinationFeedingStrategy = new FirstSuccessDestinationFeedingStrategy();

            //Act
            firstSuccessDestinationFeedingStrategy.Feed(logs, new List<ILogDestination>() { });
        }

        #endregion

        #region Tests for FeedAsync method

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
            FirstSuccessDestinationFeedingStrategy firstSuccessDestinationFeedingStrategy = new FirstSuccessDestinationFeedingStrategy();

            //Act
            await firstSuccessDestinationFeedingStrategy.FeedAsync(logs, destinations);

            //Assert
            destination1Mock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
        }

        [Test]
        public void FeedAsync_When_Called__Calls_Destinations_Until_One_Returns_Successfully_Or_Destinations_Got_Exhausted_Then_All_Thrown_Exceptions_Are_Rethrown_In_AggregateException()
        {
            //Arrange
            LogModel[] logs = new LogModel[]
            {
                Samples.LogModels.Standard(),
                Samples.LogModels.Standard(),
            };
            var exceptionFromFirstDestination = new Exception("Exception from first log destination");
            var destination1Mock = new Mock<ILogDestination>();
            // Only the first destination is configured to throw.
            destination1Mock.Setup(x => x.SendAsync(It.IsAny<LogModel[]>(), default)).ThrowsAsync(exceptionFromFirstDestination);
            var destination2Mock = new Mock<ILogDestination>();
            var destination3Mock = new Mock<ILogDestination>();
            List<ILogDestination> destinations = new List<ILogDestination>()
            {
                destination1Mock.Object,
                destination2Mock.Object,
                destination3Mock.Object,
            };
            FirstSuccessDestinationFeedingStrategy firstSuccessDestinationFeedingStrategy = new FirstSuccessDestinationFeedingStrategy();

            // Assert that AggregateException is thrown.
            DestinationFeedingException destinationFeedingException = Assert.CatchAsync<DestinationFeedingException>(async () =>
            {
                //Act
                await firstSuccessDestinationFeedingStrategy.FeedAsync(logs, destinations);
            });
            Assert.IsInstanceOf<AggregateException>(destinationFeedingException.InnerException);
            var aggregateException = (AggregateException)destinationFeedingException.InnerException;
            // Assert that all thrown exceptions are in the aggregate exception. 
            Assert.AreEqual(1, aggregateException.InnerExceptions.Count);
            Assert.Contains(exceptionFromFirstDestination, aggregateException.InnerExceptions);
            // Assert that methods was called till first success.
            destination1Mock.Verify(x => x.SendAsync(logs, default), Times.Once);
            destination2Mock.Verify(x => x.SendAsync(logs, default), Times.Once);
            destination3Mock.Verify(x => x.SendAsync(logs, default), Times.Never);
        }

        [Test]
        public async Task FeedAsync_When_Called_And_No_Exception_Is_Thrown_Sends_Logs_Collection_To_First_Destination_And_Return()
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
            FirstSuccessDestinationFeedingStrategy firstSuccessDestinationFeedingStrategy = new FirstSuccessDestinationFeedingStrategy();

            //Act
            await firstSuccessDestinationFeedingStrategy.FeedAsync(logs, destinations);

            //Assert
            destination1Mock.Verify(x => x.SendAsync(logs, default), Times.Once);
            destination2Mock.Verify(x => x.SendAsync(logs, default), Times.Never);
            destination3Mock.Verify(x => x.SendAsync(logs, default), Times.Never);
        }

        [Test]
        public void FeedAsync_When_Called_Given_That_All_Destinations_Throw_Exhaust_The_List_And_Rethrow_Exceptions_In_AggregateException()
        {
            //Arrange
            LogModel[] logs = new LogModel[]
            {
                Samples.LogModels.Standard(),
                Samples.LogModels.Standard(),
            };
            // All three destinations are configured to throw.
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
            FirstSuccessDestinationFeedingStrategy firstSuccessDestinationFeedingStrategy = new FirstSuccessDestinationFeedingStrategy();

            // Assert that AggregateException is thrown.
            DestinationFeedingException destinationFeedingException = Assert.CatchAsync<DestinationFeedingException>(async () =>
            {
                //Act
                await firstSuccessDestinationFeedingStrategy.FeedAsync(logs, destinations);
            });
            Assert.IsInstanceOf<AggregateException>(destinationFeedingException.InnerException);
            var aggregateException = (AggregateException)destinationFeedingException.InnerException;
            // Assert that all thrown exceptions are in the aggregate exception. 
            Assert.AreEqual(3, aggregateException.InnerExceptions.Count);
            Assert.Contains(exceptionFromFirstDestination, aggregateException.InnerExceptions);
            Assert.Contains(exceptionFromSecondDestination, aggregateException.InnerExceptions);
            Assert.Contains(exceptionFromThirdDestination, aggregateException.InnerExceptions);
            // Assert that methods was called till first success.
            destination1Mock.Verify(x => x.SendAsync(logs, default), Times.Once);
            destination2Mock.Verify(x => x.SendAsync(logs, default), Times.Once);
            destination3Mock.Verify(x => x.SendAsync(logs, default), Times.Once);
        }

        [Test]
        public async Task FeedAsync_When_Called_Given_That_Destinations_List_Is_Empty_Returns_Without_Throwing()
        {
            //Arrange
            LogModel[] logs = new LogModel[]
            {
                Samples.LogModels.Standard(),
                Samples.LogModels.Standard(),
            };
            FirstSuccessDestinationFeedingStrategy firstSuccessDestinationFeedingStrategy = new FirstSuccessDestinationFeedingStrategy();

            //Act
            await firstSuccessDestinationFeedingStrategy.FeedAsync(logs, new List<ILogDestination>() { });
        }

        [Test]
        public void FeedAsync_When_Called_With_Cancelled_Cancellation_Immediately_Returns_Cancelled_Task()
        {
            //Arrange 
            FirstSuccessDestinationFeedingStrategy firstSuccessDestinationFeedingStrategy = new FirstSuccessDestinationFeedingStrategy();
            CancellationToken cancellationToken = new CancellationToken(canceled: true);
            var destinationMocks = new List<Mock<ILogDestination>>() { new Mock<ILogDestination>(), new Mock<ILogDestination>() };

            Assert.CatchAsync<TaskCanceledException>(async () =>
            {
                //Act
                await firstSuccessDestinationFeedingStrategy.FeedAsync(new LogModel[] { Samples.LogModels.Standard() }, destinationMocks.Select(x => x.Object).ToList(), cancellationToken);
            });
            foreach (var mock in destinationMocks)
                mock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion
    }
}
