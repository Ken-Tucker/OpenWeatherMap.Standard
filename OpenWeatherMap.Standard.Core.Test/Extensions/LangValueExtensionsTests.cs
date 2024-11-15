namespace OpenWeatherMap.Standard.Core.Test.Extensions
{
    using global::OpenWeatherMap.Standard.Attributes;
    using global::OpenWeatherMap.Standard.Extensions;
    using Xunit;


    public class LangValueExtensionTests
    {
        private enum TestEnum
        {
            [LangValue("en")]
            English,
            [LangValue("fr")]
            French,
            NoLangValue
        }

        [Fact]
        public void GetStringValue_WithLangValueAttribute_ReturnsLangValue()
        {
            // Arrange
            var value = TestEnum.English;

            // Act
            var result = value.GetStringValue();

            // Assert
            Assert.Equal("en", result);
        }

        [Fact]
        public void GetStringValue_WithDifferentLangValueAttribute_ReturnsLangValue()
        {
            // Arrange
            var value = TestEnum.French;

            // Act
            var result = value.GetStringValue();

            // Assert
            Assert.Equal("fr", result);
        }

        [Fact]
        public void GetStringValue_WithoutLangValueAttribute_ReturnsEnumName()
        {
            // Arrange
            var value = TestEnum.NoLangValue;

            // Act
            var result = value.GetStringValue();

            // Assert
            Assert.Equal("NoLangValue", result);
        }
    }
}


