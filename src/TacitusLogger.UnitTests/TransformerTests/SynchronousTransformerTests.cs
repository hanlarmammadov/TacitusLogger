using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Transformers;

namespace TacitusLogger.UnitTests.TransformerTests
{
    [TestFixture]
    public class SynchronousTransformerTests
    {
        public class TestTransformer : SynchronousTransformerBase
        {
            private readonly Action<LogModel> _testAction;

            public TestTransformer(Action<LogModel> testAction, string name = "Test")
                : base(name)
            {
                _testAction = testAction;
            }

            public override void Transform(LogModel logModel)
            {
                _testAction(logModel);
            }
        }

        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_Name()
        {
            // Arrange
            string name = "some name";

            // Act        
            var transformer = new TestTransformer(m => { }, name);

            // Assert
            Assert.AreEqual(name, transformer.Name);
        }
        [Test]
        public void Ctor_When_Called_With_Null_Name_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act        
                var transformer = new TestTransformer(m => { }, null as string);
            });
            Assert.AreEqual("name", ex.ParamName);
        }

        #endregion

        #region Tests for TransformAsync method

        [Test]
        public async Task TransformAsync_When_Called_Calls_Transform_And_Passes_Log_Model_To_It()
        {
            // Arrange
            LogModel logModelPassedToTransformMethod = null;
            var transformer = new TestTransformer(m =>
            {
                logModelPassedToTransformMethod = m;
            });
            LogModel logModel = new LogModel();

            // Act
            await transformer.TransformAsync(logModel);

            // Assert
            Assert.AreEqual(logModel, logModelPassedToTransformMethod);
        }
        [Test]
        public void TransformAsync_When_Called_With_Cancelled_Cancellation_Token_Returns_Cancelled_Task()
        {
            // Arrange 
            int timesTransformMethodCalled = 0;
            var transformer = new TestTransformer(m =>
            {
                timesTransformMethodCalled++;
            });
            LogModel logModel = new LogModel();
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Assert
            Assert.CatchAsync<TaskCanceledException>(async () =>
            {
                await transformer.TransformAsync(logModel, cancellationToken);
            });
            Assert.AreEqual(0, timesTransformMethodCalled);
        }

        #endregion

    }
}
