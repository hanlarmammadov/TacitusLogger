using NUnit.Framework;

namespace TacitusLogger.UnitTests.SettingProvidersTests
{
    [TestFixture]
    public class MutableSettingTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_InitialValue_When_Called_Sets_Value_To_Provided_One1()
        {
            // Act
            MutableSetting<int> setting = new MutableSetting<int>(5);

            // Assert 
            Assert.AreEqual(5, setting.Value);
        }
        [Test]
        public void Ctor_Taking_InitialValue_When_Called_Sets_Value_To_Provided_One2()
        {
            // Act
            MutableSetting<string> setting = new MutableSetting<string>("test");

            // Assert 
            Assert.AreEqual("test", setting.Value);
        }
        [Test]
        public void Ctor_Taking_InitialValue_When_Called_WithoutValue_Sets_Default1()
        {
            // Act
            MutableSetting<int> setting = new MutableSetting<int>();

            // Assert 
            Assert.AreEqual(0, setting.Value);
        }
        [Test]
        public void Ctor_Taking_InitialValue_When_Called_WithoutValue_Sets_Default2()
        {
            // Act
            MutableSetting<string> setting = new MutableSetting<string>();

            // Assert 
            Assert.AreEqual(null, setting.Value);
        }

        #endregion

        #region Implicit conversion tests

        [Test]
        public void Implicit_Conversion_From_TValue_Converts_TValue_To_MutableSettingTValue()
        {
            // Act
            MutableSetting<int> setting = 5;

            // Assert
            Assert.IsInstanceOf<MutableSetting<int>>(setting);
            Assert.AreEqual(5, setting.Value);
        }

        #endregion

        #region Tests for SetValue method

        [Test]
        public void SetValue_Taking_Initial_Value_When_Called_Resets_Value_To_Provided_One()
        {
            // Arrange
            MutableSetting<string> setting = new MutableSetting<string>("value1");

            // Act
            setting.SetValue("value2");

            // Assert 
            Assert.AreEqual("value2", setting.Value);
        }


        #endregion

        #region Tests for ToString method

        [Test]
        public void ToString_When_Called_Returns_String_Representation()
        {
            // Arrange
            MutableSetting<bool> setting = new MutableSetting<bool>(true);

            // Act
            var result = setting.ToString();

            // Arrange
            Assert.NotNull(result);
            Assert.AreEqual("MutableSetting [Value = True]", result);
        }

        #endregion
    }
}
