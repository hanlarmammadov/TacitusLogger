using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TacitusLogger.Transformers;

namespace TacitusLogger.UnitTests.TransformerTests
{
    [TestFixture]
    public class ManualTransformerTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_Action_And_Name_When_Called_Sets_Transformer_Action_And_Name()
        {
            // Arrange
            string name = "some name";
            Action<LogModel> transformerAction = m => { };

            // Act
            ManualTransformer manualTransformer = new ManualTransformer(transformerAction, name);

            // Assert
            Assert.AreEqual(transformerAction, manualTransformer.TransformerAction);
            Assert.AreEqual(name, manualTransformer.Name);
        }
        [Test]
        public void Ctor_Taking_Action_And_Name_When_Called_Without_Name_Sets_Default_Name()
        {
            // Arrange 

            // Act
            ManualTransformer manualTransformer = new ManualTransformer(m => { });

            // Assert 
            Assert.AreEqual("Manual transformer", manualTransformer.Name);
        }
        [Test]
        public void Ctor_Taking_Action_And_Name_When_Called_With_Null_Transformer_Action_Throws_ArgumentNullException()
        {
            // Arrange
            Action<LogModel> transformerAction = m => { };

            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                ManualTransformer manualTransformer = new ManualTransformer(null as Action<LogModel>, "name");
            });
            Assert.AreEqual("transformerAction", ex.ParamName);
        }
        [Test]
        public void Ctor_Taking_Action_And_Name_When_Called_With_Null_Name_Throws_ArgumentNullException()
        {
            // Arrange
            Action<LogModel> transformerAction = m => { };

            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                ManualTransformer manualTransformer = new ManualTransformer(m => { }, null as string);
            });
            Assert.AreEqual("name", ex.ParamName);
        }

        #endregion

        #region Tests for Transform method

        [Test]
        public void Transform_When_Called_Calls_Transformer_Action_And_Passes_Log_Model_To_It()
        {
            // Arrange
            LogModel logModelPassedToAction = null;
            Action<LogModel> transformerAction = m =>
            {
                logModelPassedToAction = m;
            };
            ManualTransformer manualTransformer = new ManualTransformer(transformerAction);
            LogModel logModel = new LogModel();

            // Act
            manualTransformer.Transform(logModel);

            // Assert
            Assert.AreEqual(logModel, logModelPassedToAction);
        }
        [Test]
        public void Transform_When_Called_With_Null_Log_Model_Calls_Transformer_Action_And_Passes_Null_To_It()
        {
            // Arrange
            int timesActionIsCalledWithNull = 0;
            Action<LogModel> transformerAction = m =>
            {
                if (m == null)
                    timesActionIsCalledWithNull++;
            };
            ManualTransformer manualTransformer = new ManualTransformer(transformerAction);

            // Act
            manualTransformer.Transform(null as LogModel);

            // Assert
            Assert.AreEqual(1, timesActionIsCalledWithNull);
        }

        #endregion 
    }
}
