using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TacitusLogger.Builders;
using TacitusLogger.Transformers;

namespace TacitusLogger.UnitTests.BuildersTests
{
    [TestFixture]
    public class LogTransformersBuilderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_Build_Callback()
        {
            //Arrange
            Func<IList<LogTransformerBase>, ILoggerBuilder> buildCallback = list => null;

            //Act
            LogTransformersBuilder logTransformersBuilder = new LogTransformersBuilder(buildCallback);

            //Assert
            Assert.AreEqual(buildCallback, logTransformersBuilder.BuildCallback);
        }
        [Test]
        public void Ctor_When_Called_Initializes_LogTransformersList_With_Empty_List()
        {
            //Act
            LogTransformersBuilder logTransformersBuilder = new LogTransformersBuilder(list => null);

            //Assert
            Assert.NotNull(logTransformersBuilder.LogTransformers);
            Assert.AreEqual(0, logTransformersBuilder.LogTransformers.Count);
        }

        #endregion

        #region Tests for Custom method
        
        [Test]
        public void Custom_When_Called_Changes_Status_And_Adds_Log_Transformer_To_Transformers_List()
        {
            //Arrange
            LogTransformersBuilder logTransformersBuilder = new LogTransformersBuilder(list => null);
            var logTransformer = new Mock<LogTransformerBase>("name1").Object;
            var isActive = new Mock<Setting<bool>>().Object;

            //Act
            logTransformersBuilder.Custom(logTransformer, isActive);

            //Assert 
            Assert.AreEqual(isActive, logTransformer.IsActive);
            Assert.AreEqual(1, logTransformersBuilder.LogTransformers.Count);
            Assert.AreEqual(logTransformer, logTransformersBuilder.LogTransformers[0]);
        }
        [Test]
        public void Custom_When_Called_Several_Times_Adds_Log_Transformer_To_Log_Transformers_List()
        {
            //Arrange
            LogTransformersBuilder logTransformersBuilder = new LogTransformersBuilder(list => null);
            var logTransformer1 = new Mock<LogTransformerBase>("name1").Object;
            var logTransformer2 = new Mock<LogTransformerBase>("name2").Object;

            //Act
            logTransformersBuilder.Custom(logTransformer1, new Mock<Setting<bool>>().Object);
            logTransformersBuilder.Custom(logTransformer2, new Mock<Setting<bool>>().Object);

            //Assert 
            Assert.AreEqual(2, logTransformersBuilder.LogTransformers.Count);
            Assert.AreEqual(logTransformer1, logTransformersBuilder.LogTransformers[0]);
            Assert.AreEqual(logTransformer2, logTransformersBuilder.LogTransformers[1]);
        }
        [Test]
        public void Custom_When_Called_Returns_LogTransformersBuilder()
        {
            // Arrange
            LogTransformersBuilder logTransformersBuilder = new LogTransformersBuilder(list => null);
            var logTransformer = new Mock<LogTransformerBase>("name1").Object;

            // Act
            var returnedLogTransformersBuilder = logTransformersBuilder.Custom(new Mock<LogTransformerBase>("name1").Object, new Mock<Setting<bool>>().Object);

            // Assert 
            Assert.AreEqual(logTransformersBuilder, returnedLogTransformersBuilder);
        }
        [Test]
        public void Custom_When_Called_With_Null_LogTransformer_Throws_ArgumentNullException()
        {
            // Arrange
            LogTransformersBuilder logTransformersBuilder = new LogTransformersBuilder(list => null);

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                var returnedLogTransformersBuilder = logTransformersBuilder.Custom(null as LogTransformerBase, new Mock<Setting<bool>>().Object);
            });
        }

        #endregion

        #region Tests for BuildTransformers method

        [Test]
        public void BuildTransformers_When_Called_Calls_BuildCallback_And_Returns_It_Result()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = new Mock<ILoggerBuilder>().Object;
            LogTransformersBuilder logTransformersBuilder = new LogTransformersBuilder(list => loggerBuilder);

            // Act
            var returnedLoggerBuilder = logTransformersBuilder.BuildTransformers();

            // Assert 
            Assert.AreEqual(loggerBuilder, returnedLoggerBuilder);
        }
        [Test]
        public void BuildTransformers_When_Called_Sends_Log_Transformers_List_To_BuildCallback()
        {
            // Arrange
            IList<LogTransformerBase> logTransformers = null;
            ILoggerBuilder loggerBuilder = new Mock<ILoggerBuilder>().Object;
            LogTransformersBuilder logTransformersBuilder = new LogTransformersBuilder(list => { logTransformers = list; return null; });
            var logTransformer = new Mock<LogTransformerBase>("name1").Object;
            logTransformersBuilder.Custom(logTransformer, new Mock<Setting<bool>>().Object);

            // Act
            logTransformersBuilder.BuildTransformers();

            // Assert 
            Assert.NotNull(logTransformers.Count);
            Assert.AreEqual(1, logTransformers.Count);
            Assert.AreEqual(logTransformer, logTransformers[0]);
        }

        #endregion
    }
}
