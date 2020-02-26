using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Components.Time;
using TacitusLogger.Contributors;
using TacitusLogger.Strategies.LogCreation;
using TacitusLogger.LogIdGenerators;
using TacitusLogger.Strategies.ExceptionHandling;
using TacitusLogger.Exceptions;
using TacitusLogger.Tests.Helpers;

namespace TacitusLogger.UnitTests.StrategyTests.LogCreation
{
    [TestFixture]
    public class StandardLogCreationStrategyTests
    {
        private void AssertThatStringEnumerationsHaveSameElements(IEnumerable<string> expected, IEnumerable<string> actual)
        {
            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_All_Dependencies_Correctly()
        {
            // Arrange 
            bool useUtcTime = true;

            // Act
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy(useUtcTime);

            // Assert 
            Assert.AreEqual(useUtcTime, standardLogCreationStrategy.UseUtcTime);
            Assert.IsInstanceOf<SystemTimeProvider>(standardLogCreationStrategy.TimeProvider);
            Assert.IsNull(standardLogCreationStrategy.LogContributors);
            Assert.IsNull(standardLogCreationStrategy.LogIdGenerator);
            Assert.IsNull(standardLogCreationStrategy.ExceptionHandlingStrategy);
        }

        [Test]
        public void Ctor_When_Called_Without_UseUtcTime_Param_Sets_UseUtcTime_To_False()
        {
            // Act
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();

            // Assert 
            Assert.IsFalse(standardLogCreationStrategy.UseUtcTime);
        }

        #endregion

        #region Tests for InitStrategy method

        [Test]
        public void InitStrategy_When_Called_Sets_All_Dependencies_Correctly()
        {
            // Arrange
            ILogIdGenerator logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributors = new Mock<IList<LogContributorBase>>().Object;
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;

            // Act
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);

            // Assert
            Assert.AreEqual(logIdGenerator, standardLogCreationStrategy.LogIdGenerator);
            Assert.AreEqual(logContributors, standardLogCreationStrategy.LogContributors);
            Assert.AreEqual(exceptionHandlingStrategy, standardLogCreationStrategy.ExceptionHandlingStrategy);
        }

        [Test]
        public void InitStrategy_When_Called_With_Null_ILogIdGenerator_Throws_ArgumentNullException()
        {
            // Arrange  
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            IList<LogContributorBase> logContributors = new Mock<IList<LogContributorBase>>().Object;
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            ILogIdGenerator nullLogIdGenerator = null;
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                standardLogCreationStrategy.InitStrategy(nullLogIdGenerator, logContributors, exceptionHandlingStrategy);
            });
        }

        [Test]
        public void InitStrategy_When_Called_With_Null_ILogContributor_List_Throws_ArgumentNullException()
        {
            // Arrange 
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            ILogIdGenerator logIdGenerator = new Mock<ILogIdGenerator>().Object;
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            IList<LogContributorBase> nullLogContributors = null;

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                standardLogCreationStrategy.InitStrategy(logIdGenerator, nullLogContributors, exceptionHandlingStrategy);
            });
        }

        [Test]
        public void InitStrategy_When_Called_With_Null_Exception_Handling_Strategy_Sets_Null_As_Strategy()
        {
            // Arrange
            ILogIdGenerator logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributors = new Mock<IList<LogContributorBase>>().Object;
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = null;

            // Act
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);

            // Assert 
            Assert.IsNull(standardLogCreationStrategy.ExceptionHandlingStrategy);
        }

        #endregion

        #region Tests for CreateLogModel and CreateLogModelAsync methods

        [TestCase(true)]
        [TestCase(false)]
        public void CreateLogModel_When_Called_With_According_Time_Settings_Calls_Correct_TimeProvider_Method(bool useUtcTime)
        {
            // Arrange
            // Create and instance of StandardLogCreationStrategy. 
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy(useUtcTime);
            ILogIdGenerator logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributors = new Mock<IList<LogContributorBase>>().Object;
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;

            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);
            //Reset the time provider.
            var timeProviderMock = new Mock<ITimeProvider>();
            standardLogCreationStrategy.ResetTimeProvider(timeProviderMock.Object);
            Log log = Samples.Logs.Standard();

            // Act
            standardLogCreationStrategy.CreateLogModel(log, "source");

            // Assert
            if (useUtcTime)
            {
                timeProviderMock.Verify(x => x.GetLocalTime(), Times.Never);
                timeProviderMock.Verify(x => x.GetUtcTime(), Times.Once);
            }
            else
            {
                timeProviderMock.Verify(x => x.GetLocalTime(), Times.Once);
                timeProviderMock.Verify(x => x.GetUtcTime(), Times.Never);
            }
        }
        [Test]
        public void CreateLogModel_When_Called_Assign_LogModel_Id_Using_Provided_ILogIdGenerator()
        {
            // Arrange 
            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            logIdGeneratorMock.Setup(x => x.Generate(It.IsAny<LogModel>())).Returns("logId");
            var logIdGenerator = logIdGeneratorMock.Object;
            IList<LogContributorBase> logContributors = new List<LogContributorBase>();
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);
            Log log = Samples.Logs.Standard();

            // Act
            LogModel logModel = standardLogCreationStrategy.CreateLogModel(log, "source");

            // Assert
            Assert.AreEqual("logId", logModel.LogId);
            logIdGeneratorMock.Verify(x => x.Generate(It.IsAny<LogModel>()), Times.Once);
        }
        [Test]
        public void CreateLogModel_When_Called_Every_Active_LogContributor_Method_Called_And_Result_Added_To_LogItems_List()
        {
            // Arrange  
            IList<LogContributorBase> logContributors = new List<LogContributorBase>();
            // 
            var logContributor1Mock = new Mock<LogContributorBase>("name1");
            LogItem logItem1 = new LogItem("Item1", new KeyValuePair<string, string>[1]);
            logContributor1Mock.Setup(x => x.ProduceLogItem()).Returns(logItem1);
            logContributors.Add(logContributor1Mock.Object);
            //
            var inactiveLogContributor2Mock = new Mock<LogContributorBase>("name2");
            inactiveLogContributor2Mock.Object.SetActive(false);
            logContributors.Add(inactiveLogContributor2Mock.Object);
            //
            var logContributor3Mock = new Mock<LogContributorBase>("name3");
            LogItem logItem3 = new LogItem("Item3", new KeyValuePair<string, string>[1]);
            logContributor3Mock.Setup(x => x.ProduceLogItem()).Returns(logItem3);
            logContributors.Add(logContributor3Mock.Object);
            //
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);
            Log log = Samples.Logs.Standard();

            // Act
            LogModel logModel = standardLogCreationStrategy.CreateLogModel(log, "source");

            // Assert
            // Methods are called for all active log contributors.
            logContributor1Mock.Verify(x => x.ProduceLogItem(), Times.Once);
            inactiveLogContributor2Mock.Verify(x => x.ProduceLogItem(), Times.Never);
            logContributor3Mock.Verify(x => x.ProduceLogItem(), Times.Once);
            // Log attachments are assigned correctly.
            Assert.AreEqual(2, logModel.LogItems.Length);
            Assert.AreEqual(logItem1, logModel.LogItems[0]);
            Assert.AreEqual(logItem3, logModel.LogItems[1]);
        }
        [Test]
        public void CreateLogModel_When_Called_Sets_Provided_Params_To_LogModel_Properties_Correctly()
        {
            // Arrange  
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributors = new List<LogContributorBase>();
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);
            var source = "Source";
            Log log = Samples.Logs.Standard();

            // Act
            LogModel logModel = standardLogCreationStrategy.CreateLogModel(log, source);

            // Assert
            Assert.AreEqual(source, logModel.Source);
            Assert.AreEqual(log.Context, logModel.Context);
            AssertThatStringEnumerationsHaveSameElements(log.Tags, logModel.Tags);
            Assert.AreEqual(log.Type, logModel.LogType);
            Assert.AreEqual(log.Description, logModel.Description);
            Assert.AreEqual(log.Items, logModel.LogItems);
        }
        [Test]
        public void CreateLogModel_When_Called_With_Null_Source_Returns_LogModel_With_Null_Source()
        {
            // Arrange  
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributor = new List<LogContributorBase>();
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributor, exceptionHandlingStrategy);

            // Act
            LogModel logModel = standardLogCreationStrategy.CreateLogModel(Samples.Logs.Standard(), null);

            // Assert
            Assert.IsNull(logModel.Source);
        }
        [Test]
        public void CreateLogModel_When_Called_With_Null_Context_Returns_LogModel_With_Null_Context()
        {
            // Arrange  
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributor = new List<LogContributorBase>();
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributor, exceptionHandlingStrategy);
            Log log = Log.Critical("description").From(context: null);

            // Act
            LogModel logModel = standardLogCreationStrategy.CreateLogModel(log, "source");

            // Assert
            Assert.IsNull(logModel.Context);
        }
        [Test]
        public void CreateLogModel_When_Called_With_Null_Description_Returns_LogModel_With_Null_Description()
        {
            // Arrange  
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributor = new List<LogContributorBase>();
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributor, exceptionHandlingStrategy);
            Log log = Log.Critical(description: null);

            // Act
            LogModel logModel = standardLogCreationStrategy.CreateLogModel(log, "source");

            // Assert
            Assert.IsNull(logModel.Description);
        }
        [Test]
        public void CreateLogModelAsync_When_Called_With_Cancelled_Cancellation_Token_Immediately_Returns_Cancelled_Task()
        {
            // Arrange  
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            var logIdGenerator = logIdGeneratorMock.Object;
            IList<LogContributorBase> logContributors = new List<LogContributorBase>();
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);
            CancellationToken cancellationToken = new CancellationToken(canceled: true);
            Log log = Samples.Logs.Standard();

            // Assert 
            Assert.CatchAsync<TaskCanceledException>(async () =>
            {
                // Act
                await standardLogCreationStrategy.CreateLogModelAsync(log, "source", cancellationToken);
            });
            logIdGeneratorMock.Verify(x => x.GenerateAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        [Test]
        public void CreateLogModel_When_Called_Given_That_Log_Contributor_Throws_An_Exception_Uses_Strategy_For_Exception_Handling()
        {
            // Arrange 
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributors = new List<LogContributorBase>();
            var logContributorMock = new Mock<LogContributorBase>("Contributor1");
            Exception ex = new Exception();
            logContributorMock.Setup(x => x.ProduceLogItem()).Throws(ex);
            logContributors.Add(logContributorMock.Object);

            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var exceptionHandlingStrategyMock = new Mock<ExceptionHandlingStrategyBase>();
            exceptionHandlingStrategyMock.SetupGet(x => x.ShouldRethrow).Returns(false);
            var exceptionHandlingStrategy = exceptionHandlingStrategyMock.Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);

            // Act
            LogModel logModel = standardLogCreationStrategy.CreateLogModel(new Log(), "source");

            // Assert
            exceptionHandlingStrategyMock.Verify(x => x.HandleException(ex, "Log contributor: Contributor1"), Times.Once);
            exceptionHandlingStrategyMock.VerifyGet(x => x.ShouldRethrow, Times.Once);
        }
        [Test]
        public void CreateLogModel_When_Called_Given_That_Log_Contributor_Throws_An_Exception_And_ShouldRethrow_Is_Set_True_Rethrows_The_Exception()
        {
            // Arrange 
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributors = new List<LogContributorBase>();
            var logContributorMock = new Mock<LogContributorBase>("Contributor1");
            var ex = new Mock<Exception>().Object;
            logContributorMock.Setup(x => x.ProduceLogItem()).Throws(ex);
            logContributors.Add(logContributorMock.Object);

            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var exceptionHandlingStrategyMock = new Mock<ExceptionHandlingStrategyBase>();
            exceptionHandlingStrategyMock.SetupGet(x => x.ShouldRethrow).Returns(true);
            var exceptionHandlingStrategy = exceptionHandlingStrategyMock.Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);

            // Assert
            Assert.Catch<LogCreationException>(() =>
            {
                // Act
                LogModel logModel = standardLogCreationStrategy.CreateLogModel(new Log(), "source");
            });
            exceptionHandlingStrategyMock.Verify(x => x.HandleException(ex, "Log contributor: Contributor1"), Times.Once);
            exceptionHandlingStrategyMock.VerifyGet(x => x.ShouldRethrow, Times.Once);
        }
        [Test]
        public void CreateLogModel_When_Called_Given_That_Should_Rethrow_Is_False_And_One_Of_Log_Contributors_Throws_An_Exception_Other_Contributors_Are_Called()
        {
            // Arrange 
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributors = new List<LogContributorBase>();
            var logContributor1Mock = new Mock<LogContributorBase>("Contributor1");
            Exception ex = new Exception();
            logContributor1Mock.Setup(x => x.ProduceLogItem()).Throws(ex);
            logContributors.Add(logContributor1Mock.Object);

            var logContributor2Mock = new Mock<LogContributorBase>("Contributor2");
            logContributors.Add(logContributor2Mock.Object);

            var logContributor3Mock = new Mock<LogContributorBase>("Contributor3");
            logContributors.Add(logContributor3Mock.Object);

            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var exceptionHandlingStrategyMock = new Mock<ExceptionHandlingStrategyBase>();
            exceptionHandlingStrategyMock.SetupGet(x => x.ShouldRethrow).Returns(false);
            var exceptionHandlingStrategy = exceptionHandlingStrategyMock.Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);

            // Act
            LogModel logModel = standardLogCreationStrategy.CreateLogModel(new Log(), "source");

            // Assert
            logContributor1Mock.Verify(x => x.ProduceLogItem(), Times.Once);
            logContributor2Mock.Verify(x => x.ProduceLogItem(), Times.Once);
            logContributor3Mock.Verify(x => x.ProduceLogItem(), Times.Once);
        }
        [Test]
        public void CreateLogModel_When_Called_Async_Methods_Of_Contributors_Are_Never_Called()
        {
            // Arrange  
            IList<LogContributorBase> logContributors = new List<LogContributorBase>();
            // 
            var logContributor1Mock = new Mock<LogContributorBase>("name1");
            logContributors.Add(logContributor1Mock.Object);
            //
            var logContributor2Mock = new Mock<LogContributorBase>("name2");
            logContributors.Add(logContributor2Mock.Object);
            //
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);

            // Act
            LogModel logModel = standardLogCreationStrategy.CreateLogModel(new Log(), "source");

            // Assert 
            logContributor1Mock.Verify(x => x.ProduceLogItemAsync(It.IsAny<CancellationToken>()), Times.Never);
            logContributor2Mock.Verify(x => x.ProduceLogItemAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region Tests for CreateLogModelAsync method

        [Test]
        public async Task CreateLogModelAsync_When_Called_With_Null_Source_Returns_LogModel_With_Null_Source()
        {
            // Arrange  
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributor = new List<LogContributorBase>();
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributor, exceptionHandlingStrategy);

            // Act 
            LogModel logModelAsync = await standardLogCreationStrategy.CreateLogModelAsync(Samples.Logs.Standard(), null);

            // Assert 
            Assert.IsNull(logModelAsync.Source);
        }
        [Test]
        public async Task CreateLogModelAsync_When_Called_With_Null_Context_Returns_LogModel_With_Null_Context()
        {
            // Arrange  
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributor = new List<LogContributorBase>();
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributor, exceptionHandlingStrategy);
            Log log = Log.Critical("description").From(context: null);

            // Act 
            LogModel logModelAsync = await standardLogCreationStrategy.CreateLogModelAsync(log, "source");

            // Assert
            Assert.IsNull(logModelAsync.Context);
        }
        [Test]
        public async Task CreateLogModelAsync_When_Called_With_Null_Description_Returns_LogModel_With_Null_Description()
        {
            // Arrange  
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributor = new List<LogContributorBase>();
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributor, exceptionHandlingStrategy);
            Log log = Log.Critical(description: null);

            // Act
            LogModel logModelAsync = await standardLogCreationStrategy.CreateLogModelAsync(log, "source");

            // Assert
            Assert.IsNull(logModelAsync.Description);
        }
        [Test]
        public async Task CreateLogModelAsync_When_Called_Assign_LogModel_Id_Using_Provided_ILogIdGenerator()
        {
            // Arrange 
            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            logIdGeneratorMock.Setup(x => x.GenerateAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ReturnsAsync("logId");
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var logIdGenerator = logIdGeneratorMock.Object;
            IList<LogContributorBase> logContributors = new List<LogContributorBase>();
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);
            Log log = Samples.Logs.Standard();

            // Act
            LogModel logModel = await standardLogCreationStrategy.CreateLogModelAsync(log, "source");

            // Assert
            Assert.AreEqual("logId", logModel.LogId);
            logIdGeneratorMock.Verify(x => x.GenerateAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task CreateLogModelAsync_When_Called_Every_Active_LogContributor_Method_Called_And_Result_Added_To_LogItems_List()
        {
            // Arrange  
            IList<LogContributorBase> logContributors = new List<LogContributorBase>();
            // 
            var logContributor1Mock = new Mock<LogContributorBase>("name1");
            LogItem logItem1 = new LogItem("Item1", new KeyValuePair<string, string>[1]);
            logContributor1Mock.Setup(x => x.ProduceLogItemAsync(It.IsAny<CancellationToken>())).ReturnsAsync(logItem1);
            logContributors.Add(logContributor1Mock.Object);
            //
            var inactiveLogContributor2Mock = new Mock<LogContributorBase>("name2");
            inactiveLogContributor2Mock.Object.SetActive(false);
            logContributors.Add(inactiveLogContributor2Mock.Object);
            //
            var logContributor3Mock = new Mock<LogContributorBase>("name3");
            LogItem logItem3 = new LogItem("Item3", new KeyValuePair<string, string>[1]);
            logContributor3Mock.Setup(x => x.ProduceLogItemAsync(It.IsAny<CancellationToken>())).ReturnsAsync(logItem3);
            logContributors.Add(logContributor3Mock.Object);
            //
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);
            Log log = Samples.Logs.Standard();

            // Act
            LogModel logModel = await standardLogCreationStrategy.CreateLogModelAsync(log, "source");

            // Assert
            // Async methods are called only for active contributors.
            logContributor1Mock.Verify(x => x.ProduceLogItemAsync(It.IsAny<CancellationToken>()), Times.Once);
            inactiveLogContributor2Mock.Verify(x => x.ProduceLogItemAsync(It.IsAny<CancellationToken>()), Times.Never);
            logContributor3Mock.Verify(x => x.ProduceLogItemAsync(It.IsAny<CancellationToken>()), Times.Once);

            // Log attachments are assigned correctly.
            Assert.AreEqual(2, logModel.LogItems.Length);
            Assert.AreEqual(logItem1, logModel.LogItems[0]);
            Assert.AreEqual(logItem3, logModel.LogItems[1]);
        }
        [Test]
        public async Task CreateLogModelAsync_When_Called_Sync_Methods_Of_Contributors_Are_Never_Called()
        {
            // Arrange  
            IList<LogContributorBase> logContributors = new List<LogContributorBase>();
            // 
            var logContributor1Mock = new Mock<LogContributorBase>("name1");
            logContributors.Add(logContributor1Mock.Object);
            //
            var logContributor2Mock = new Mock<LogContributorBase>("name2");
            logContributors.Add(logContributor2Mock.Object);
            //
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);

            // Act
            LogModel logModel = await standardLogCreationStrategy.CreateLogModelAsync(new Log(), "source");

            // Assert 
            logContributor1Mock.Verify(x => x.ProduceLogItem(), Times.Never);
            logContributor2Mock.Verify(x => x.ProduceLogItem(), Times.Never);
        }
        [Test]
        public async Task CreateLogModelAsync_When_Called_Passes_Cancellation_Token_To_Async_Methods_Of_Contributors()
        {
            // Arrange  
            IList<LogContributorBase> logContributors = new List<LogContributorBase>();
            // 
            var logContributor1Mock = new Mock<LogContributorBase>("name1");
            logContributors.Add(logContributor1Mock.Object);
            //
            var logContributor2Mock = new Mock<LogContributorBase>("name2");
            logContributors.Add(logContributor2Mock.Object);
            //
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            LogModel logModel = await standardLogCreationStrategy.CreateLogModelAsync(new Log(), "source", cancellationToken);

            // Assert 
            logContributor1Mock.Verify(x => x.ProduceLogItemAsync(cancellationToken), Times.Once);
            logContributor2Mock.Verify(x => x.ProduceLogItemAsync(cancellationToken), Times.Once);
        }
        [Test]
        public async Task CreateLogModelAsync_When_Called_Sets_Provided_Params_To_LogModel_Properties_Correctly()
        {
            // Arrange  
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributor = new List<LogContributorBase>();
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributor, exceptionHandlingStrategy);
            var source = "source";
            Log log = Samples.Logs.Standard();

            // Act
            LogModel logModel = await standardLogCreationStrategy.CreateLogModelAsync(log, source);

            // Assert
            Assert.AreEqual(source, logModel.Source);
            Assert.AreEqual(log.Context, logModel.Context);
            AssertThatStringEnumerationsHaveSameElements(log.Tags, logModel.Tags);
            Assert.AreEqual(log.Type, logModel.LogType);
            Assert.AreEqual(log.Description, logModel.Description);
            Assert.AreEqual(log.Items, logModel.LogItems);
        }
        [Test]
        public async Task CreateLogModelAsync_When_Called_Given_That_Log_Contributor_Throws_An_Exception_Uses_Strategy_For_Exception_Handling()
        {
            // Arrange 
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributors = new List<LogContributorBase>();
            var logContributorMock = new Mock<LogContributorBase>("Contributor1");
            Exception ex = new Exception();
            logContributorMock.Setup(x => x.ProduceLogItemAsync(It.IsAny<CancellationToken>())).Throws(ex);
            logContributors.Add(logContributorMock.Object);

            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var exceptionHandlingStrategyMock = new Mock<ExceptionHandlingStrategyBase>();
            exceptionHandlingStrategyMock.SetupGet(x => x.ShouldRethrow).Returns(false);
            var exceptionHandlingStrategy = exceptionHandlingStrategyMock.Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);

            // Act
            LogModel logModel = await standardLogCreationStrategy.CreateLogModelAsync(new Log(), "source");

            // Assert
            exceptionHandlingStrategyMock.Verify(x => x.HandleExceptionAsync(ex, "Log contributor: Contributor1", default(CancellationToken)), Times.Once);
            exceptionHandlingStrategyMock.VerifyGet(x => x.ShouldRethrow, Times.Once);
        }
        [Test]
        public async Task CreateLogModelAsync_When_Called_Given_That_Should_Rethrow_Is_False_And_One_Of_Log_Contributors_Throws_An_Exception_Other_Contributors_Are_Called()
        {
            // Arrange 
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributors = new List<LogContributorBase>();
            var logContributor1Mock = new Mock<LogContributorBase>("Contributor1");
            Exception ex = new Exception();
            logContributor1Mock.Setup(x => x.ProduceLogItemAsync(It.IsAny<CancellationToken>())).Throws(ex);
            logContributors.Add(logContributor1Mock.Object);

            var logContributor2Mock = new Mock<LogContributorBase>("Contributor2");
            logContributors.Add(logContributor2Mock.Object);

            var logContributor3Mock = new Mock<LogContributorBase>("Contributor3");
            logContributors.Add(logContributor3Mock.Object);

            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var exceptionHandlingStrategyMock = new Mock<ExceptionHandlingStrategyBase>();
            exceptionHandlingStrategyMock.SetupGet(x => x.ShouldRethrow).Returns(false);
            var exceptionHandlingStrategy = exceptionHandlingStrategyMock.Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);

            // Act
            LogModel logModel = await standardLogCreationStrategy.CreateLogModelAsync(new Log(), "source");

            // Assert
            logContributor1Mock.Verify(x => x.ProduceLogItemAsync(default(CancellationToken)), Times.Once);
            logContributor2Mock.Verify(x => x.ProduceLogItemAsync(default(CancellationToken)), Times.Once);
            logContributor3Mock.Verify(x => x.ProduceLogItemAsync(default(CancellationToken)), Times.Once);
        }
        [TestCase(true)]
        [TestCase(false)]
        public async Task CreateLogModelAsync_When_Called_With_According_Time_Settings_Calls_Correct_TimeProvider_Method(bool useUtcTime)
        {
            // Arrange
            // Create and instance of StandardLogCreationStrategy. 
            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy(useUtcTime);
            ILogIdGenerator logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributors = new Mock<IList<LogContributorBase>>().Object;
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;

            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);
            //Reset the time provider.
            var timeProviderMock = new Mock<ITimeProvider>();
            standardLogCreationStrategy.ResetTimeProvider(timeProviderMock.Object);
            Log log = Samples.Logs.Standard();

            // Act 
            await standardLogCreationStrategy.CreateLogModelAsync(log, "source");

            // Assert
            if (useUtcTime)
            {
                timeProviderMock.Verify(x => x.GetLocalTime(), Times.Never);
                timeProviderMock.Verify(x => x.GetUtcTime(), Times.Once);
            }
            else
            {
                timeProviderMock.Verify(x => x.GetLocalTime(), Times.Once);
                timeProviderMock.Verify(x => x.GetUtcTime(), Times.Never);
            }
        }
        [Test]
        public void CreateLogModelAsync_When_Called_Given_That_Log_Contributor_Throws_An_Exception_And_ShouldRethrow_Is_Set_True_Rethrows_The_Exception()
        {
            // Arrange 
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributors = new List<LogContributorBase>();
            var logContributorMock = new Mock<LogContributorBase>("Contributor1");
            var ex = new Mock<Exception>().Object;
            logContributorMock.Setup(x => x.ProduceLogItemAsync(It.IsAny<CancellationToken>())).Throws(ex);
            logContributors.Add(logContributorMock.Object);

            StandardLogCreationStrategy standardLogCreationStrategy = new StandardLogCreationStrategy();
            var exceptionHandlingStrategyMock = new Mock<ExceptionHandlingStrategyBase>();
            exceptionHandlingStrategyMock.SetupGet(x => x.ShouldRethrow).Returns(true);
            var exceptionHandlingStrategy = exceptionHandlingStrategyMock.Object;
            standardLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);

            // Assert
            Assert.CatchAsync<LogCreationException>(async () =>
            {
                // Act
                LogModel logModel = await standardLogCreationStrategy.CreateLogModelAsync(new Log(), "source");
            });
            exceptionHandlingStrategyMock.Verify(x => x.HandleExceptionAsync(ex, "Log contributor: Contributor1", default(CancellationToken)), Times.Once);
            exceptionHandlingStrategyMock.VerifyGet(x => x.ShouldRethrow, Times.Once);
        }

        #endregion
    }
}
