using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Destinations;
using TacitusLogger.Exceptions;
using TacitusLogger.Caching;
using TacitusLogger.Strategies.DestinationFeeding;

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class LogGroupTests
    {
        #region Ctors tests

        [Test]
        public void Ctor_Taking_Name_Rule_LogDestinations_And_LogGroupSettings_When_Called_Sets_Provided_Dependencies()
        {
            // Arrange
            string groupName = "group name";
            LogModelFunc<bool> rule = ld => true;
            var logDestination = new Mock<ILogDestination>().Object;
            var logDestinations = new List<ILogDestination>() { logDestination };
            var logGroupSettings = new LogGroupSettings();

            // Act
            LogGroup logGroup = new LogGroup(groupName, rule, logDestinations, logGroupSettings);

            // Assert
            Assert.AreEqual(groupName, logGroup.Name);
            Assert.AreEqual(rule, logGroup.Rule);
            Assert.AreEqual(1, logGroup.LogDestinations.Count);
            Assert.AreEqual(logDestination, logGroup.LogDestinations[0]);
            Assert.AreEqual(logGroupSettings, logGroup.LogGroupSettings);
        }
        [Test]
        public void Ctor_Taking_Name_Rule_LogDestinations_And_LogGroupSettings_When_Called_With_Null_Name_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act           
                LogGroup logGroup = new LogGroup(null as string, ld => true, new List<ILogDestination>(), new LogGroupSettings());
            });
            Assert.AreEqual("name", ex.ParamName);
        }
        [Test]
        public void Ctor_Taking_Name_Rule_LogDestinations_And_LogGroupSettings_When_Called_With_Null_Log_Group_Settings_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act           
                LogGroup logGroup = new LogGroup("log group name", ld => true, new List<ILogDestination>(), null as LogGroupSettings);
            });
            Assert.AreEqual("logGroupSettings", ex.ParamName);
        }
        [Test]
        public void Ctor_Taking_Name_Rule_LogDestinations_And_LogGroupSettings_When_Called_With_Null_Log_Destinations_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act           
                LogGroup logGroup = new LogGroup("log group name", ld => true, null as List<ILogDestination>, new LogGroupSettings());
            });
            Assert.AreEqual("logDestinations", ex.ParamName);
        }
        [Test]
        public void Ctor_Taking_Name_Rule_LogDestinations_And_LogGroupSettings_When_Called_With_Null_Rule_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act           
                LogGroup logGroup = new LogGroup("log group name", null as LogModelFunc<bool>, new List<ILogDestination>(), new LogGroupSettings());
            });
            Assert.AreEqual("rule", ex.ParamName);
        }
        [Test]
        public void Ctor_Taking_Name_Rule_LogDestinations_And_LogGroupSettings_When_Called_Sets_Log_Group_Status()
        {
            // Arrange 
            var status = LogGroupStatus.Inactive;

            // Act
            LogGroup logGroup = new LogGroup("group name", ld => true, new List<ILogDestination>(), new LogGroupSettings() { Status = status });

            // Assert 
            Assert.IsInstanceOf<Setting<LogGroupStatus>>(logGroup.Status);
            Assert.AreEqual(status, logGroup.Status.Value);
        }
        [Test]
        public void Ctor_Taking_Name_Rule_LogDestinations_And_LogGroupSettings_When_Called_Sets_Log_Group_Status2()
        {
            // Arrange  
            var status = LogGroupStatus.Inactive;
            var statusValueProviderMock = new Mock<Setting<LogGroupStatus>>();
            statusValueProviderMock.SetupGet(x => x.Value).Returns(status);

            // Act
            LogGroup logGroup = new LogGroup("group name", ld => true, new List<ILogDestination>(), new LogGroupSettings() { Status = statusValueProviderMock.Object });

            // Assert 
            Assert.AreEqual(statusValueProviderMock.Object, logGroup.Status);
            Assert.AreEqual(status, logGroup.Status.Value);
        }
        [Test]
        public void Ctor_Taking_Name_Rule_LogDestinations_And_LogGroupSettings_When_Called_Sets_Log_Group_Status3()
        {
            // Arrange  
            var statusValueProviderMock = new Mock<Setting<LogGroupStatus>>();

            // Act
            LogGroup logGroup = new LogGroup("group name", ld => true, new List<ILogDestination>(), new LogGroupSettings() { Status = statusValueProviderMock.Object });

            // Assert 
            Assert.AreEqual(statusValueProviderMock.Object, logGroup.Status);
        }
        [Test]
        public void Ctor_Taking_Name_Rule_LogDestinations_And_LogGroupSettings_When_Called_Caching_Is_Disabled()
        {
            // Act
            LogGroup logGroup = new LogGroup("group name", ld => true, new List<ILogDestination>(), new LogGroupSettings());

            // Assert   
            Assert.IsFalse(logGroup.CachingIsActive);
            Assert.IsNull(logGroup.LogCache);
        }
        [Test]
        public void Ctor_Taking_Name_Rule_LogDestinations_And_LogGroupSettings_When_Called_Sets_Destination_Feeding_Strategy()
        {
            // Arrange
            var settings = new LogGroupSettings()
            {
                DestinationFeeding = DestinationFeeding.FirstSuccess
            };

            // Act
            LogGroup logGroup = new LogGroup("group name", ld => true, new List<ILogDestination>(), settings);

            // Assert  
            Assert.IsInstanceOf<FirstSuccessDestinationFeedingStrategy>(logGroup.DestinationFeedingStrategy);
        }

        [Test]
        public void Ctor_Taking_Name_Rule_And_LogDestinations_When_Called_Sets_Provided_Dependencies()
        {
            // Arrange
            string groupName = "group name";
            LogModelFunc<bool> rule = ld => true;
            var logDestination = new Mock<ILogDestination>().Object;
            var logDestinations = new List<ILogDestination>() { logDestination };

            // Act
            LogGroup logGroup = new LogGroup(groupName, rule, logDestinations);

            // Assert
            Assert.AreEqual(groupName, logGroup.Name);
            Assert.AreEqual(rule, logGroup.Rule);
            Assert.AreEqual(1, logGroup.LogDestinations.Count);
            Assert.AreEqual(logDestination, logGroup.LogDestinations[0]);
        }
        [Test]
        public void Ctor_Taking_Name_Rule_And_LogDestinations_When_Called_Sets_Log_Group_Settings_To_Default()
        {
            // Act
            LogGroup logGroup = new LogGroup("group name", ld => true, new List<ILogDestination>());

            // Assert
            Assert.AreEqual(Defaults.LogGroupSettings, logGroup.LogGroupSettings);
        }
        [Test]
        public void Ctor_Taking_Name_Rule_And_LogDestinations_When_Called_With_Null_Name_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act           
                LogGroup logGroup = new LogGroup(null as string, ld => true, new List<ILogDestination>());
            });
            Assert.AreEqual("name", ex.ParamName);
        }
        [Test]
        public void Ctor_Taking_Name_Rule_And_LogDestinations_When_Called_With_Null_Log_Destinations_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act           
                LogGroup logGroup = new LogGroup("log group name", ld => true, null as List<ILogDestination>);
            });
            Assert.AreEqual("logDestinations", ex.ParamName);
        }
        [Test]
        public void Ctor_Taking_Name_Rule_And_LogDestinations_When_Called_With_Null_Rule_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act           
                LogGroup logGroup = new LogGroup("log group name", null as LogModelFunc<bool>, new List<ILogDestination>());
            });
            Assert.AreEqual("rule", ex.ParamName);
        }

        [Test]
        public void Ctor_Taking_Rule_LogDestinations_And_LogGroupSettings_When_Called_Sets_Provided_Dependencies()
        {
            // Arrange 
            LogModelFunc<bool> rule = ld => true;
            var logDestination = new Mock<ILogDestination>().Object;
            var logDestinations = new List<ILogDestination>() { logDestination };
            var logGroupSettings = new LogGroupSettings();

            // Act
            LogGroup logGroup = new LogGroup(rule, logDestinations, logGroupSettings);

            // Assert 
            Assert.AreEqual(rule, logGroup.Rule);
            Assert.AreEqual(1, logGroup.LogDestinations.Count);
            Assert.AreEqual(logDestination, logGroup.LogDestinations[0]);
            Assert.AreEqual(logGroupSettings, logGroup.LogGroupSettings);
        }
        [Test]
        public void Ctor_Taking_Rule_LogDestinations_And_LogGroupSettings_When_Called_Sets_Default_Random_Name()
        {
            // Act
            LogGroup logGroup = new LogGroup(ld => true, new List<ILogDestination>(), new LogGroupSettings());

            // Assert
            Assert.IsNotEmpty(logGroup.Name);
            Assert.AreEqual("Group_", logGroup.Name.Substring(0, 6));
        }
        [Test]
        public void Ctor_Taking_Rule_LogDestinations_And_LogGroupSettings_When_Called_With_Null_Rule_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act           
                LogGroup logGroup = new LogGroup(null as LogModelFunc<bool>, new List<ILogDestination>(), new LogGroupSettings());
            });
            Assert.AreEqual("rule", ex.ParamName);
        }
        [Test]
        public void Ctor_Taking_Rule_LogDestinations_And_LogGroupSettings_When_Called_With_Null_Log_Destinations_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act           
                LogGroup logGroup = new LogGroup(ld => true, null as List<ILogDestination>, new LogGroupSettings());
            });
            Assert.AreEqual("logDestinations", ex.ParamName);
        }
        [Test]
        public void Ctor_Taking_Rule_LogDestinations_And_LogGroupSettings_When_Called_With_Null_Log_Group_Settings_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act           
                LogGroup logGroup = new LogGroup(ld => true, new List<ILogDestination>(), null as LogGroupSettings);
            });
            Assert.AreEqual("logGroupSettings", ex.ParamName);
        }

        [Test]
        public void Ctor_Taking_Rule_LogDestinations_When_Called_Sets_Provided_Dependencies()
        {
            // Arrange 
            LogModelFunc<bool> rule = ld => true;
            var logDestination = new Mock<ILogDestination>().Object;
            var logDestinations = new List<ILogDestination>() { logDestination };

            // Act
            LogGroup logGroup = new LogGroup(rule, logDestinations);

            // Assert 
            Assert.AreEqual(rule, logGroup.Rule);
            Assert.AreEqual(1, logGroup.LogDestinations.Count);
            Assert.AreEqual(logDestination, logGroup.LogDestinations[0]);
        }
        [Test]
        public void Ctor_Taking_Rule_LogDestinations_When_Called_Sets_Default_Random_Name()
        {
            // Act
            LogGroup logGroup = new LogGroup(ld => true, new List<ILogDestination>());

            // Assert
            Assert.IsNotEmpty(logGroup.Name);
            Assert.AreEqual("Group_", logGroup.Name.Substring(0, 6));
        }
        [Test]
        public void Ctor_Taking_Rule_LogDestinations_When_Called_Sets_Default_Log_Group_Settings()
        {
            // Act
            LogGroup logGroup = new LogGroup(ld => true, new List<ILogDestination>());

            // Assert
            Assert.AreEqual(Defaults.LogGroupSettings, logGroup.LogGroupSettings);
        }
        [Test]
        public void Ctor_Taking_Rule_LogDestinations_When_Called_With_Null_Rule_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act           
                LogGroup logGroup = new LogGroup(null as LogModelFunc<bool>, new List<ILogDestination>());
            });
            Assert.AreEqual("rule", ex.ParamName);
        }
        [Test]
        public void Ctor_Taking_Rule_LogDestinations_When_Called_With_Null_Log_Destinations_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act           
                LogGroup logGroup = new LogGroup(ld => true, null as List<ILogDestination>);
            });
            Assert.AreEqual("logDestinations", ex.ParamName);
        }

        [Test]
        public void Ctor_Taking_Rule_And_LogGroupSettings_When_Called_Sets_Provided_Dependencies()
        {
            // Arrange 
            LogModelFunc<bool> rule = ld => true;
            LogGroupSettings logGroupSettings = new LogGroupSettings();

            // Act
            LogGroup logGroup = new LogGroup(rule, logGroupSettings);

            // Assert 
            Assert.AreEqual(rule, logGroup.Rule);
            Assert.AreEqual(logGroupSettings, logGroup.LogGroupSettings);
        }
        [Test]
        public void Ctor_Taking_Rule_And_LogGroupSettings_When_Called_Sets_Default_Random_Name()
        {
            // Act
            LogGroup logGroup = new LogGroup(ld => true, new LogGroupSettings());

            // Assert
            Assert.IsNotEmpty(logGroup.Name);
            Assert.AreEqual("Group_", logGroup.Name.Substring(0, 6));
        }
        [Test]
        public void Ctor_Taking_Rule_And_LogGroupSettings_When_Called_Sets_Empty_Log_Destinations()
        {
            // Act
            LogGroup logGroup = new LogGroup(ld => true, new LogGroupSettings());

            // Assert
            Assert.IsNotNull(logGroup.LogDestinations);
            Assert.AreEqual(0, logGroup.LogDestinations.Count);
        }
        [Test]
        public void Ctor_Taking_Rule_And_LogGroupSettings_When_Called_With_Null_Rule_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act           
                LogGroup logGroup = new LogGroup(null as LogModelFunc<bool>, new LogGroupSettings());
            });
            Assert.AreEqual("rule", ex.ParamName);
        }
        [Test]
        public void Ctor_Taking_Rule_And_LogGroupSettings_When_Called_With_Null_Log_Group_Settings_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act           
                LogGroup logGroup = new LogGroup(ld => true, null as LogGroupSettings);
            });
            Assert.AreEqual("logGroupSettings", ex.ParamName);
        }

        [Test]
        public void Ctor_Taking_Rule_When_Called_Sets_Provided_Dependencies()
        {
            // Arrange 
            LogModelFunc<bool> rule = ld => true;

            // Act
            LogGroup logGroup = new LogGroup(rule);

            // Assert 
            Assert.AreEqual(rule, logGroup.Rule);
        }
        [Test]
        public void Ctor_Taking_Rule_When_Called_Sets_Default_Random_Name()
        {
            // Act
            LogGroup logGroup = new LogGroup(ld => true);

            // Assert
            Assert.IsNotEmpty(logGroup.Name);
            Assert.AreEqual("Group_", logGroup.Name.Substring(0, 6));
        }
        [Test]
        public void Ctor_Taking_Rule_When_Called_Sets_Empty_Log_Destinations()
        {
            // Act
            LogGroup logGroup = new LogGroup(ld => true);

            // Assert
            Assert.IsNotNull(logGroup.LogDestinations);
            Assert.AreEqual(0, logGroup.LogDestinations.Count);
        }
        [Test]
        public void Ctor_Taking_Rule_When_Called_Sets_Default_Log_Group_Settings()
        {
            // Act
            LogGroup logGroup = new LogGroup(ld => true);

            // Assert 
            Assert.AreEqual(Defaults.LogGroupSettings, logGroup.LogGroupSettings);
        }
        [Test]
        public void Ctor_Taking_Rule_When_Called_With_Null_Rule_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act           
                LogGroup logGroup = new LogGroup(null as LogModelFunc<bool>);
            });
            Assert.AreEqual("rule", ex.ParamName);
        }

        [Test]
        public void Ctor_Taking_LogGroupSettings_When_Called_Sets_Provided_Dependencies()
        {
            // Arrange 
            LogGroupSettings logGroupSettings = new LogGroupSettings();

            // Act
            LogGroup logGroup = new LogGroup(logGroupSettings);

            // Assert 
            Assert.AreEqual(logGroupSettings, logGroup.LogGroupSettings);
        }
        [Test]
        public void Ctor_Taking_LogGroupSettings_When_Called_Sets_Default_Random_Name()
        {
            // Act
            LogGroup logGroup = new LogGroup(new LogGroupSettings());

            // Assert
            Assert.IsNotEmpty(logGroup.Name);
            Assert.AreEqual("Group_", logGroup.Name.Substring(0, 6));
        }
        [Test]
        public void Ctor_Taking_LogGroupSettings_When_Called_Sets_Empty_Log_Destinations()
        {
            // Act
            LogGroup logGroup = new LogGroup(new LogGroupSettings());

            // Assert
            Assert.IsNotNull(logGroup.LogDestinations);
            Assert.AreEqual(0, logGroup.LogDestinations.Count);
        }
        [Test]
        public void Ctor_Taking_LogGroupSettings_When_Called_Sets_Default_Rule()
        {
            // Act
            LogGroup logGroup = new LogGroup(new LogGroupSettings());

            // Assert
            Assert.IsNotNull(logGroup.Rule);
        }
        [Test]
        public void Ctor_Taking_LogGroupSettings_When_Called_With_Null_Log_Group_Settings_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act           
                LogGroup logGroup = new LogGroup(null as LogGroupSettings);
            });
            Assert.AreEqual("logGroupSettings", ex.ParamName);
        }

        [Test]
        public void Ctor_Default_When_Called_Sets_Default_Random_Name()
        {
            // Act
            LogGroup logGroup = new LogGroup();

            // Assert
            Assert.IsNotEmpty(logGroup.Name);
            Assert.AreEqual("Group_", logGroup.Name.Substring(0, 6));
        }
        [Test]
        public void Ctor_Default_When_Called_Sets_Empty_Log_Destinations()
        {
            // Act
            LogGroup logGroup = new LogGroup();

            // Assert
            Assert.IsNotNull(logGroup.LogDestinations);
            Assert.AreEqual(0, logGroup.LogDestinations.Count);
        }
        [Test]
        public void Ctor_Default_When_Called_Sets_Default_Rule()
        {
            // Act
            LogGroup logGroup = new LogGroup();

            // Assert
            Assert.IsNotNull(logGroup.Rule);
        }
        [Test]
        public void Ctor_Default_When_Called_Sets_Default_Log_Group_Settings()
        {
            // Act
            LogGroup logGroup = new LogGroup();

            // Assert
            Assert.AreEqual(Defaults.LogGroupSettings, logGroup.LogGroupSettings);
        }

        #endregion

        #region Tests for AddDestinations method

        [Test]
        public void AddDestinations_When_Called_Log_Destinations_Are_Set_To_Provided_Ones()
        {
            // Arrange 
            ILogDestination logDestination = new Mock<ILogDestination>().Object;
            LogGroup logGroup = new LogGroup((l) => true);

            // Act
            logGroup.AddDestinations(logDestination);

            // Assert   
            Assert.AreEqual(logDestination, logGroup.LogDestinations[0]);
        }

        [Test]
        public void AddDestinations_When_Called_Log_Destinations_Send_Method_Is_Not_Called()
        {
            // Arrange 
            var logDestinationMock = new Mock<ILogDestination>();
            LogGroup logGroup = new LogGroup((l) => true);

            // Act
            logGroup.AddDestinations(logDestinationMock.Object);

            // Assert   
            logDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
            logDestinationMock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), default), Times.Never);
        }

        [Test]
        public void AddDestinations_When_Called_Several_Times_Log_Destinations_Are_Set_To_Provided_Ones()
        {
            // Arrange 
            ILogDestination logDestination1 = new Mock<ILogDestination>().Object;
            ILogDestination logDestination2 = new Mock<ILogDestination>().Object;
            LogGroup logGroup = new LogGroup((l) => true);

            // Act
            logGroup.AddDestinations(logDestination1);
            logGroup.AddDestinations(logDestination2);

            // Assert   
            Assert.AreEqual(logDestination1, logGroup.LogDestinations[0]);
            Assert.AreEqual(logDestination2, logGroup.LogDestinations[1]);
        }

        [Test]
        public void AddDestinations_When_Called_With_Null_Throws_ArgumentNullException()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logGroup.AddDestinations(null);
            });
        }

        [Test]
        public void AddDestinations_When_Called_With_Empty_Array_Throws_ArgumentNullException()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logGroup.AddDestinations(new ILogDestination[0]);
            });
        }

        [Test]
        public void AddDestinations_When_Called_With_Array_Containing_Null_Throws_ArgumentNullException()
        {
            // Arrange 
            ILogDestination logDestination = new Mock<ILogDestination>().Object;
            LogGroup logGroup = new LogGroup((l) => true);

            // Assert   
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logGroup.AddDestinations(logDestination, null);
            });
        }

        [Test]
        public void AddDestinations_When_Called_Returns_Self()
        {
            // Arrange 
            ILogDestination logDestination = new Mock<ILogDestination>().Object;
            LogGroup logGroup = new LogGroup((l) => true);

            // Act
            var self = logGroup.AddDestinations(logDestination);

            // Assert
            Assert.AreEqual(logGroup, self);
        }

        #endregion

        #region Tests for SetLogCache method

        [Test]
        public void SetLogCache_Taking_Log_Cache_And_IsActive_Flag_When_Called_Sets_Provided_Data()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup();
            var logCache = new Mock<ILogCache>().Object;

            // Act
            logGroup.SetLogCache(logCache, false);

            // Assert 
            Assert.AreEqual(logCache, logGroup.LogCache);
            Assert.AreEqual(false, logGroup.CachingIsActive);
        }
        [Test]
        public void SetLogCache_Taking_Log_Cache_And_IsActive_Value_Provider_When_Called_With_Null_ILogCache_Throws_ArgumentNullException()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup();

            // Assert 
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logGroup.SetLogCache(null as ILogCache, true);
            });
        }
        [Test]
        public void SetLogCache_Taking_Log_Cache_And_IsActive_Value_Provider_When_Called_Without_IsEnabled_Sets_To_False()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup();
            var logCache = new Mock<ILogCache>().Object;

            // Act
            logGroup.SetLogCache(new Mock<ILogCache>().Object);

            // Assert
            Assert.IsTrue(logGroup.CachingIsActive);
        }

        #endregion

        #region Tests for SetStatus method

        [TestCase(LogGroupStatus.Active)]
        [TestCase(LogGroupStatus.Inactive)]
        public void SetStatus_When_Called_Sets_LogGroup_Status(LogGroupStatus logGroupStatus)
        {
            // Arrange  
            LogGroup logGroup = new LogGroup();
            var logGroupStatusValueProviderMock = new Mock<Setting<LogGroupStatus>>();
            logGroupStatusValueProviderMock.SetupGet(x => x.Value).Returns(logGroupStatus);

            // Act
            logGroup.SetStatus(logGroupStatusValueProviderMock.Object);

            // Assert 
            Assert.AreEqual(logGroupStatusValueProviderMock.Object, logGroup.Status);
            Assert.AreEqual(logGroupStatus, logGroup.Status.Value);
        }

        [Test]
        public void SetStatus_When_Called_Several_Times_Sets_LogGroup_Status()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup();
            var logGroupStatusValueProvider1Mock = new Mock<Setting<LogGroupStatus>>();
            var logGroupStatusValueProvider2Mock = new Mock<Setting<LogGroupStatus>>();

            // Act
            logGroup.SetStatus(logGroupStatusValueProvider1Mock.Object);
            Assert.AreEqual(logGroupStatusValueProvider1Mock.Object, logGroup.Status);
            logGroup.SetStatus(logGroupStatusValueProvider2Mock.Object);
            Assert.AreEqual(logGroupStatusValueProvider2Mock.Object, logGroup.Status);
        }
        [Test]
        public void SetStatus_When_Called_With_Null_Log_Group_Status_Value_Provider_Throws_ArgumentNullException()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup();

            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logGroup.SetStatus(null as Setting<LogGroupStatus>);
            });
            Assert.AreEqual("status", ex.ParamName);
        }
        #endregion

        #region Tests for Send method

        [Test]
        public void Send_When_Called_Given_That_Caching_Not_Enabled_Calls_LogDestinations_Send_Method()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup();
            // Setup log destination
            var logDestinationMock = new Mock<ILogDestination>();
            logGroup.AddDestinations(logDestinationMock.Object);
            LogModel logModel = new LogModel();

            // Act
            logGroup.Send(logModel);

            // Assert
            Assert.IsFalse(logGroup.CachingIsActive);
            logDestinationMock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0] == logModel)), Times.Once);
        }

        [Test]
        public void Send_When_Called_Given_That_Caching_Enabled_And_Not_Yet_Filled_Does_Not_Calls_LogDestinations_Send_Method()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            // Log destination mock setup
            var logDestinationMock = new Mock<ILogDestination>();
            logGroup.AddDestinations(logDestinationMock.Object);
            // Log model
            LogModel logModel = new LogModel();
            // Log cache setup
            var logCacheMock = new Mock<ILogCache>();
            logCacheMock.Setup(x => x.AddToCache(It.IsAny<LogModel>(), It.IsAny<bool>())).Returns(null as LogModel[]);
            logGroup.SetLogCache(logCacheMock.Object);

            // Act
            logGroup.Send(logModel);

            // Assert
            Assert.IsTrue(logGroup.CachingIsActive);
            // Log model was added to cache
            logCacheMock.Verify(x => x.AddToCache(logModel, false), Times.Once);
            // But was not sent to destination
            logDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
        }

        [Test]
        public void Send_When_Called_Given_That_Caching_Enabled_And_Filled_Calls_LogDestinations_Send_Method()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            // Log destination mock setup
            var logDestinationMock = new Mock<ILogDestination>();
            logGroup.AddDestinations(logDestinationMock.Object);
            // Log model and cached collection
            LogModel logModel = new LogModel();
            LogModel[] logModelCollectionCached = new LogModel[] { };
            // Log cache setup
            var logCacheMock = new Mock<ILogCache>();
            logCacheMock.Setup(x => x.AddToCache(logModel, It.IsAny<bool>())).Returns(logModelCollectionCached);
            logGroup.SetLogCache(logCacheMock.Object);

            // Act
            logGroup.Send(logModel);

            // Assert
            Assert.IsTrue(logGroup.CachingIsActive);
            // Log model was added to cache
            logCacheMock.Verify(x => x.AddToCache(logModel, false), Times.Once);
            // Cached collection was flushed and sent to destination
            logDestinationMock.Verify(x => x.Send(logModelCollectionCached), Times.Once);
        }

        [Test]
        public void Send_When_Called_Given_That_Several_Log_Destinations_Added_To_LogGroup_Calls_Each_LogDestinations_Send_Method()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            var logDestination1Mock = new Mock<ILogDestination>();
            var logDestination2Mock = new Mock<ILogDestination>();
            var logDestination3Mock = new Mock<ILogDestination>();
            logGroup.AddDestinations(logDestination1Mock.Object);
            logGroup.AddDestinations(logDestination2Mock.Object);
            logGroup.AddDestinations(logDestination3Mock.Object);
            LogModel logModel = new LogModel();

            // Act
            logGroup.Send(logModel);

            // Assert
            logDestination1Mock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0] == logModel)), Times.Once);
            logDestination2Mock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0] == logModel)), Times.Once);
            logDestination3Mock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0] == logModel)), Times.Once);
        }

        [Test]
        public void Send_When_Called_Given_That_Several_Log_Destinations_Added_To_LogGroup_And_Cache_Is_Not_Filled_Does_Not_Calls_Each_LogDestinations_Send_Method()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            // Setup and add log destinations to log group.
            var logDestination1Mock = new Mock<ILogDestination>();
            var logDestination2Mock = new Mock<ILogDestination>();
            var logDestination3Mock = new Mock<ILogDestination>();
            logGroup.AddDestinations(logDestination1Mock.Object);
            logGroup.AddDestinations(logDestination2Mock.Object);
            logGroup.AddDestinations(logDestination3Mock.Object);
            LogModel logModel = new LogModel();
            // Setup log cache.
            var logCacheMock = new Mock<ILogCache>();
            logCacheMock.Setup(x => x.AddToCache(It.IsAny<LogModel>(), It.IsAny<bool>())).Returns(null as LogModel[]);
            logGroup.SetLogCache(logCacheMock.Object);

            // Act
            logGroup.Send(logModel);

            // Assert
            // Log model was added to cache.
            logCacheMock.Verify(x => x.AddToCache(logModel, false), Times.Once);
            // But was not sent to log destinations because cache has not filled yet.
            logDestination1Mock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0] == logModel)), Times.Never);
            logDestination2Mock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0] == logModel)), Times.Never);
            logDestination3Mock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0] == logModel)), Times.Never);
        }

        [Test]
        public void Send_When_Called_Given_That_Log_Destinations_Was_Not_Added_To_LogGroup_Does_Not_Throws_An_Exception()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            LogModel logModel = new LogModel();

            // Act
            logGroup.Send(logModel);
        }

        [Test]
        public void Send_When_Called_None_Of_Log_Destinations_Async_Methods_Are_Called()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            var logDestinationMock = new Mock<ILogDestination>();
            logGroup.AddDestinations(logDestinationMock.Object);
            LogModel logModel = new LogModel();

            // Act
            logGroup.Send(logModel);

            // Assert
            logDestinationMock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void Send_When_Called_With_Null_LogModel_Throws_ArgumentNullException()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);

            // Assert
            var ex = Assert.Catch<LogGroupException>(() =>
            {
                // Act
                logGroup.Send(null as LogModel);
            });
            Assert.AreEqual("Log model cannot be null", ex.Message);
        }

        #endregion

        #region Tests for SendAsync method

        [Test]
        public async Task SendAsync_When_Called_Given_That_Caching_Not_Enabled_Calls_LogDestinations_Send_Method()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            // Setup log destination
            var logDestinationMock = new Mock<ILogDestination>();
            logGroup.AddDestinations(logDestinationMock.Object);
            LogModel logModel = new LogModel();

            // Act
            await logGroup.SendAsync(logModel);

            // Assert
            Assert.IsFalse(logGroup.CachingIsActive);
            logDestinationMock.Verify(x => x.SendAsync(It.Is<LogModel[]>(d => d[0] == logModel), default(CancellationToken)), Times.Once);
        }

        [Test]
        public async Task SendAsync_When_Called_Given_That_Caching_Enabled_And_Not_Yet_Filled_Does_Not_Calls_LogDestinations_Send_Method()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            // Log destination mock setup
            var logDestinationMock = new Mock<ILogDestination>();
            logGroup.AddDestinations(logDestinationMock.Object);
            // Log model
            LogModel logModel = new LogModel();
            // Log cache setup
            var logCacheMock = new Mock<ILogCache>();
            logCacheMock.Setup(x => x.AddToCache(It.IsAny<LogModel>(), false)).Returns(null as LogModel[]);
            logGroup.SetLogCache(logCacheMock.Object);

            // Act
            await logGroup.SendAsync(logModel);

            // Assert
            Assert.IsTrue(logGroup.CachingIsActive);
            // Log model was added to cache
            logCacheMock.Verify(x => x.AddToCache(logModel, false), Times.Once);
            // But was not sent to destination
            logDestinationMock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), default), Times.Never);
        }

        [Test]
        public async Task SendAsync_When_Called_Given_That_Caching_Enabled_And_Filled_Calls_LogDestinations_Send_Method()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            // Log destination mock setup
            var logDestinationMock = new Mock<ILogDestination>();
            logGroup.AddDestinations(logDestinationMock.Object);
            // Log model and cached collection
            LogModel logModel = new LogModel();
            LogModel[] logModelCollectionCached = new LogModel[] { };
            // Log cache setup
            var logCacheMock = new Mock<ILogCache>();
            logCacheMock.Setup(x => x.AddToCache(logModel, false)).Returns(logModelCollectionCached);
            logGroup.SetLogCache(logCacheMock.Object);

            // Act
            await logGroup.SendAsync(logModel);

            // Assert
            Assert.IsTrue(logGroup.CachingIsActive);
            // Log model was added to cache
            logCacheMock.Verify(x => x.AddToCache(logModel, false), Times.Once);
            // Cached collection was flushed and sent to destination
            logDestinationMock.Verify(x => x.SendAsync(logModelCollectionCached, default(CancellationToken)), Times.Once);
        }

        [Test]
        public async Task SendAsync_When_Called_Given_That_Several_Log_Destinations_Added_To_LogGroup_Calls_Each_LogDestinations_Send_Method()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            var logDestination1Mock = new Mock<ILogDestination>();
            var logDestination2Mock = new Mock<ILogDestination>();
            var logDestination3Mock = new Mock<ILogDestination>();
            logGroup.AddDestinations(logDestination1Mock.Object);
            logGroup.AddDestinations(logDestination2Mock.Object);
            logGroup.AddDestinations(logDestination3Mock.Object);
            LogModel logModel = new LogModel();

            // Act
            await logGroup.SendAsync(logModel);

            // Assert
            logDestination1Mock.Verify(x => x.SendAsync(It.Is<LogModel[]>(d => d[0] == logModel), default(CancellationToken)), Times.Once);
            logDestination2Mock.Verify(x => x.SendAsync(It.Is<LogModel[]>(d => d[0] == logModel), default(CancellationToken)), Times.Once);
            logDestination3Mock.Verify(x => x.SendAsync(It.Is<LogModel[]>(d => d[0] == logModel), default(CancellationToken)), Times.Once);
        }

        [Test]
        public async Task SendAsync_When_Called_Given_That_Several_Log_Destinations_Added_To_LogGroup_And_Cache_Is_Not_Filled_Does_Not_Calls_Each_LogDestinations_Send_Method()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            // Setup and add log destinations to log group.
            var logDestination1Mock = new Mock<ILogDestination>();
            var logDestination2Mock = new Mock<ILogDestination>();
            var logDestination3Mock = new Mock<ILogDestination>();
            logGroup.AddDestinations(logDestination1Mock.Object);
            logGroup.AddDestinations(logDestination2Mock.Object);
            logGroup.AddDestinations(logDestination3Mock.Object);
            LogModel logModel = new LogModel();
            // Setup log cache.
            var logCacheMock = new Mock<ILogCache>();
            logCacheMock.Setup(x => x.AddToCache(It.IsAny<LogModel>(), false)).Returns(null as LogModel[]);
            logGroup.SetLogCache(logCacheMock.Object);

            // Act
            await logGroup.SendAsync(logModel);

            // Assert
            // Log model was added to cache.
            logCacheMock.Verify(x => x.AddToCache(logModel, false), Times.Once);
            // But was not sent to log destinations because cache has not filled yet.
            logDestination1Mock.Verify(x => x.SendAsync(It.Is<LogModel[]>(d => d[0] == logModel), It.IsAny<CancellationToken>()), Times.Never);
            logDestination2Mock.Verify(x => x.SendAsync(It.Is<LogModel[]>(d => d[0] == logModel), It.IsAny<CancellationToken>()), Times.Never);
            logDestination3Mock.Verify(x => x.SendAsync(It.Is<LogModel[]>(d => d[0] == logModel), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task SendAsync_When_Called_None_Of_Log_Destinations_Sync_Methods_Are_Called()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            var logDestinationMock = new Mock<ILogDestination>();
            logGroup.AddDestinations(logDestinationMock.Object);
            LogModel logModel = new LogModel();

            // Act
            await logGroup.SendAsync(logModel);

            // Assert
            logDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
        }

        [Test]
        public void SendAsync_When_Called_With_Null_LogModel_Throws_ArgumentNullException()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);

            // Assert
            var ex = Assert.CatchAsync<LogGroupException>(async () =>
            {
                // Act
                await logGroup.SendAsync(null as LogModel);
            });
            Assert.AreEqual("Log model cannot be null", ex.Message);
        }

        [Test]
        public void SendAsync_When_Called_With_Cancelled_Cancellation_Token_Immediately_Returns_Cancelled_Task()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            var destinationFeedingStrategyMock = new Mock<DestinationFeedingStrategyBase>();
            logGroup.ResetDestinationFeedingStrategy(destinationFeedingStrategyMock.Object);
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Assert
            Assert.CatchAsync<TaskCanceledException>(async () =>
            {
                // Act
                await logGroup.SendAsync(new LogModel(), cancellationToken);
            });
            destinationFeedingStrategyMock.Verify(x => x.FeedAsync(It.IsAny<LogModel[]>(), It.IsAny<IList<ILogDestination>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region Tests for ResetDestinationFeedingStrategy method

        [Test]
        public void ResetDestinationFeedingStrategy_When_Called_Sets_New_DestinationFeedingStrategy()
        {
            //Arrange
            LogGroup logGroup = new LogGroup(x => true);
            DestinationFeedingStrategyBase newDestinationFeedingStrategy = new Mock<DestinationFeedingStrategyBase>().Object;

            //Act
            logGroup.ResetDestinationFeedingStrategy(newDestinationFeedingStrategy);

            //Assert
            Assert.AreEqual(newDestinationFeedingStrategy, logGroup.DestinationFeedingStrategy);
        }

        [Test]
        public void ResetDestinationFeedingStrategy_When_Called_With_Null_DestinationFeedingStrategyBase_Throws_ArgumentNullException()
        {
            //Arrange
            LogGroup logGroup = new LogGroup(x => true);

            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                logGroup.ResetDestinationFeedingStrategy(null as DestinationFeedingStrategyBase);
            });
        }

        #endregion

        #region Tests for Dispose method

        [Test]
        [Ignore("Not implemented")]
        public void Dispose_When_Called_Given_That_Caching_Is_Active_Log_Cache_Is_Flushed_And_Logs_Are_Saved()
        {
            // Arrange
            var logCacheMock = new Mock<ILogCache>();
            LogModel flushedLog1 = new LogModel();
            LogModel flushedLog2 = new LogModel();
            LogModel[] flushedLogs = new LogModel[3] { flushedLog1, flushedLog2, null };
            //logCacheMock.Setup(x => x.AddToCache(It.Is<LogModel>(log => { flushedLogs[3] = log; return true; }), true)).Returns(()=> { flushedLogs[3] = log; return null; });
            var destinationFeedingStrategyMock = new Mock<DestinationFeedingStrategyBase>();
            LogGroup logGroup = new LogGroup();
            logGroup.SetLogCache(logCacheMock.Object);
            logGroup.ResetDestinationFeedingStrategy(destinationFeedingStrategyMock.Object);

            // Act
            logGroup.Dispose();

            // Assert
            logCacheMock.Verify(x => x.AddToCache(It.IsNotNull<LogModel>(), true), Times.Once);
        }
        [Test]
        [Ignore("Not implemented")]
        public void Dispose_When_Called_Given_That_Caching_Is_Not_Active()
        {

        }
        [Test]
        [Ignore("Not implemented")]
        public void Dispose_When_Called_Given_That_Caching_Is_Active_But_Cache_Is_Empty()
        {

        }


        [Test]
        public void Dispose_When_Called_Calls_Log_Cache_Dispose_Method()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            var logCacheMock = new Mock<ILogCache>();
            logGroup.SetLogCache(logCacheMock.Object);

            // Act
            logGroup.Dispose();

            // Assert   
            logCacheMock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Dispose_When_Called_If_Log_Cache_Dispose_Method_Throws_Exception_Is_Swallowed()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            var logCacheMock = new Mock<ILogCache>();
            logCacheMock.Setup(x => x.Dispose()).Throws<Exception>();
            logGroup.SetLogCache(logCacheMock.Object);

            // Act
            logGroup.Dispose();

            // Assert   
            logCacheMock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Dispose_When_Called_Calls_Log_Group_Status_Value_Provider_Dispose_Method()
        {
            // Arrange  
            var LogGroupStatusValueProviderMock = new Mock<Setting<LogGroupStatus>>();
            LogGroup logGroup = new LogGroup((l) => true, new LogGroupSettings { Status = LogGroupStatusValueProviderMock.Object });

            // Act
            logGroup.Dispose();

            // Assert   
            LogGroupStatusValueProviderMock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Dispose_When_Called_If_Log_Group_Status_Value_Provider_Dispose_Method_Throws_Exception_Is_Swallowed()
        {
            // Arrange  
            var LogGroupStatusValueProviderMock = new Mock<Setting<LogGroupStatus>>();
            LogGroupStatusValueProviderMock.Setup(x => x.Dispose()).Throws<Exception>();
            LogGroup logGroup = new LogGroup((l) => true, new LogGroupSettings { Status = LogGroupStatusValueProviderMock.Object });

            // Act
            logGroup.Dispose();

            // Assert   
            LogGroupStatusValueProviderMock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Dispose_When_Called_Calls_Dispose_Method_Of_All_Log_Destinations()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            var logDestination1Mock = new Mock<ILogDestination>();
            var logDestination2Mock = new Mock<ILogDestination>();
            var logDestination3Mock = new Mock<ILogDestination>();
            logGroup.AddDestinations(logDestination1Mock.Object, logDestination2Mock.Object, logDestination3Mock.Object);

            // Act
            logGroup.Dispose();

            // Assert   
            logDestination1Mock.Verify(x => x.Dispose(), Times.Once);
            logDestination2Mock.Verify(x => x.Dispose(), Times.Once);
            logDestination3Mock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Dispose_When_Called_If_Log_Destination_Throws_The_Exception_Is_Swallowed()
        {
            // Arrange  
            LogGroup logGroup = new LogGroup((l) => true);
            var logDestination1Mock = new Mock<ILogDestination>();
            logDestination1Mock.Setup(x => x.Dispose()).Throws<Exception>();
            var logDestination2Mock = new Mock<ILogDestination>();
            logGroup.AddDestinations(logDestination1Mock.Object, logDestination2Mock.Object);

            // Act
            logGroup.Dispose();

            // Assert   
            logDestination1Mock.Verify(x => x.Dispose(), Times.Once);
            logDestination2Mock.Verify(x => x.Dispose(), Times.Once);
        }

        #endregion

        #region Tests for ToString method

        [Test]
        public void ToString_When_Called_Contains_Information_About_The_LogGroup()
        {
            // Arrange 
            LogGroup logGroup = new LogGroup();

            // Act
            var result = logGroup.ToString();

            // Assert 
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(logGroup.Name));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_All_Log_Destinations()
        {
            // Arrange 
            var logDestination1Mock = new Mock<ILogDestination>();
            var destination1Description = "destination1Description";
            logDestination1Mock.Setup(x => x.ToString()).Returns(destination1Description);

            var logDestination2Mock = new Mock<ILogDestination>();
            var destination2Description = "destination2Description";
            logDestination2Mock.Setup(x => x.ToString()).Returns(destination2Description);

            var logDestination3Mock = new Mock<ILogDestination>();
            var destination3Description = "destination3Description";
            logDestination3Mock.Setup(x => x.ToString()).Returns(destination3Description);

            LogGroup logGroup = new LogGroup();
            logGroup.AddDestinations(logDestination1Mock.Object, logDestination2Mock.Object, logDestination3Mock.Object);

            // Act
            var result = logGroup.ToString();

            // Assert
            logDestination1Mock.Verify(x => x.ToString(), Times.Once);
            logDestination2Mock.Verify(x => x.ToString(), Times.Once);
            logDestination3Mock.Verify(x => x.ToString(), Times.Once);

            Assert.IsTrue(result.Contains($"1. {destination1Description}"));
            Assert.IsTrue(result.Contains($"2. {destination2Description}"));
            Assert.IsTrue(result.Contains($"3. {destination3Description}"));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_Log_Cache()
        {
            // Arrange 
            var logCacheMock = new Mock<ILogCache>();
            var logCacheDescription = "logCacheDescription";
            logCacheMock.Setup(x => x.ToString()).Returns(logCacheDescription);
            LogGroup logGroup = new LogGroup();
            logGroup.SetLogCache(logCacheMock.Object);

            // Act
            var result = logGroup.ToString();

            // Assert
            logCacheMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(logCacheDescription));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_Destination_Feeding_Strategy()
        {
            // Arrange 
            var destinationFeedingStrategyMock = new Mock<DestinationFeedingStrategyBase>();
            var destinationFeedingStrategyDescription = "destinationFeedingStrategyDescription";
            destinationFeedingStrategyMock.Setup(x => x.ToString()).Returns(destinationFeedingStrategyDescription);
            LogGroup logGroup = new LogGroup();
            logGroup.ResetDestinationFeedingStrategy(destinationFeedingStrategyMock.Object);

            // Act
            var result = logGroup.ToString();

            // Assert
            destinationFeedingStrategyMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(destinationFeedingStrategyDescription));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_Log_Group_Status_Setting()
        {
            // Arrange 
            var logGroupStatusSettingMock = new Mock<Setting<LogGroupStatus>>();
            var logGroupStatusSettingDescription = "logGroupStatusSettingDescription";
            logGroupStatusSettingMock.Setup(x => x.ToString()).Returns(logGroupStatusSettingDescription);
            LogGroup logGroup = new LogGroup(new LogGroupSettings() { Status = logGroupStatusSettingMock.Object });

            // Act
            var result = logGroup.ToString();

            // Assert
            logGroupStatusSettingMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(logGroupStatusSettingDescription));
        }

        #endregion
    }
}
