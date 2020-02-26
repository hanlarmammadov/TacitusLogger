using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TacitusLogger.UnitTests.SettingProvidersTests
{
    [TestFixture]
    public class SettingTests
    {
        #region Implicit conversion tests

        [Test]
        public void Implicit_Conversion_From_TValue_Converts_TValue_To_SettingTValue()
        {
            // Act
            Setting<int> setting = 5;

            // Assert
            Assert.IsInstanceOf<Setting<int>>(setting);
            Assert.AreEqual(5, setting.Value);
        }
        [Test]
        public void Implicit_Conversion_From_SettingTValue_Converts_SettingTValue_To_TValue()
        {
            Setting<int> setting = 5;

            // Act
            int converted = setting;

            // Assert 
            Assert.AreEqual(5, converted);
        }

        #endregion

        #region Tests for From method

        [Test]
        public void From_When_Called_Returns_Not_Null_SettingBuilder()
        {
            // Act 
            var settingBuilder = Setting<int>.From;

            // Assert
            Assert.IsInstanceOf<SettingBuilder<int>>(settingBuilder);
        }

        #endregion

        #region Tests for Dispose method

        [Test]
        public void Dispose_When_Called_Does_Not_Throw()
        {
            // Arrange
            Setting<int> setting = 5;

            // Act
            setting.Dispose(); 
        }

        #endregion

        #region Tests for ToString method

        [Test]
        public void ToString_When_Called_Returns_String_Representation()
        {
            // Arrange
            Setting<bool> setting = true;

            // Act
            var result = setting.ToString();

            // Arrange
            Assert.NotNull(result);
            Assert.AreEqual("Setting [Value = True]", result);
        }

        #endregion
    }
}
