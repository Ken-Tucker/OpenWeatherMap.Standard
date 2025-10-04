namespace OpenWeatherMap.Standard.Core.Test.Extensions
{
    using global::OpenWeatherMap.Standard.Attributes;
    using global::OpenWeatherMap.Standard.Extensions;
    using Xunit;


    public class IsoValueExtensionTests
    {
        private enum TestEnum
        {
            [IsoValue("en")]
            English,
            [IsoValue("fr")]
            French,
            NoLangValue
        }

        [Fact]
        public void GetStringValue_WithIsoValueAttribute_ReturnsIsoValue()
        {
            // Arrange
            var value = TestEnum.English;

            // Act
            var result = value.GetStringValue();

            // Assert
            Assert.Equal("en", result);
        }

        [Fact]
        public void GetStringValue_WithDifferentIsoValueAttribute_ReturnsIsoValue()
        {
            // Arrange
            var value = TestEnum.French;

            // Act
            var result = value.GetStringValue();

            // Assert
            Assert.Equal("fr", result);
        }

        [Fact]
        public void GetStringValue_WithoutIsoValueAttribute_ReturnsEnumName()
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


