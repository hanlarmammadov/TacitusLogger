using NUnit.Framework; 

namespace TacitusLogger.UnitTests.SettingProvidersTests
{
    [TestFixture]
    public class SettingBuilderTests
    {
        [Test]
        public void Variable_Taking_Initial_Value_When_Called_Returns_New_MutableSetting_Instance()
        {
            // Arrange
            SettingBuilder<string> settingBuilder = new SettingBuilder<string>();

            // Act
            MutableSetting<string> result = settingBuilder.Variable("initialValue");

            // Assert
            Assert.AreEqual("initialValue", result.Value);
        }
        [Test]
        public void Variable_Taking_Initial_Value_When_Called_Without_Value_Returns_New_MutableSetting_Instance_With_Default1()
        {
            // Arrange
            SettingBuilder<int> settingBuilder = new SettingBuilder<int>();

            // Act
            MutableSetting<int> result = settingBuilder.Variable();

            // Assert
            Assert.AreEqual(0, result.Value);
        }
        [Test]
        public void Variable_Taking_Initial_Value_When_Called_Without_Value_Returns_New_MutableSetting_Instance_With_Default2()
        {
            // Arrange
            SettingBuilder<string> settingBuilder = new SettingBuilder<string>();

            // Act
            MutableSetting<string> result = settingBuilder.Variable();

            // Assert
            Assert.AreEqual(null, result.Value);
        }
    }
}
