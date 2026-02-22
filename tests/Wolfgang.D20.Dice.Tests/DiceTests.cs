using Xunit;
// ReSharper disable RedundantArgumentDefaultValue

namespace Wolfgang.D20.Tests.Unit
{


    public class DiceTests
    {
        [Fact]
        public void Can_create_instance()
        {

            // Arrange & Act
            var dice = new Dice();
            // Assert
            Assert.NotNull(dice);
        }



        [Fact]
        public void DieCount_less_than_1_throws_ArgumentOutOfRangeException()
        {

            // Arrange & Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount:0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount:-1));
        }



        [Fact]
        public void When_not_specified_DieCount_defaults_to_1()
        {
            // Arrange & Act
            var dice = new Dice();
            
            // Assert
            Assert.Equal(1, dice.DieCount);
        }



        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        public void Can_specify_the_number_of_dice(int dieCount)
        {
            // Arrange & Act
            var dice = new Dice(dieCount: dieCount);

            // Assert
            Assert.Equal(dieCount, dice.DieCount);
        }


        [Fact]
        public void SideCount_less_than_2_throws_ArgumentOutOfRangeException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount: 1,sideCount:0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount: 1,sideCount:1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount: 1,sideCount: -1));
        }



        [Fact]
        public void When_not_specified_SideCount_defaults_to_6()
        {
            // Arrange & Act
            var dice = new Dice();

            // Assert
            Assert.Equal(6, dice.SideCount);
        }



        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        public void Can_specify_the_number_of_sides(int sideCount)
        {
            // Arrange & Act
            var dice = new Dice(sideCount: sideCount);

            // Assert
            Assert.Equal(sideCount, dice.SideCount);
        }



        [Fact]
        public void Modify_can_be_positive_or_negative()
        {
            _ = new Dice(modifier: 0);
            _ = new Dice(modifier: 1);
            _ = new Dice(modifier: -1);
            _ = new Dice(modifier: int.MinValue);
            _ = new Dice(modifier: int.MaxValue);
        }



        [Fact]
        public void When_not_specified_Modifier_defaults_to_0()
        {
            // Arrange & Act
            var dice = new Dice();

            // Assert
            Assert.Equal(0, dice.Modifier);
        }



        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        public void Can_specify_modifier_value(int modifier)
        {
            // Arrange & Act
            var dice = new Dice(modifier: modifier);

            // Assert
            Assert.Equal(modifier, dice.Modifier);
        }




        [Theory]
        [InlineData(1, 6, 0, 1)] // 1d6 + 0 = 1
        [InlineData(2, 8, 3, 5)] // 2d8 + 3 = 5
        [InlineData(2, 10, -1, 1)] // 2d10 - 1 = 1
        [InlineData(3, 4, -5, -2)] // 3d4 -5 = -2

        public void Dice_properly_shows_MinValue(int dieCount, int sideCount, int modifier, int expectedValue)
        {
            // Arrange & Act
            var sut = new Dice(dieCount, sideCount, modifier);
            // Assert
            Assert.Equal(expectedValue, sut.MinValue);
        }



        [Theory]
        [InlineData(1, 6, 0, 6)] // 1d6 + 0 = 6
        [InlineData(2, 8, 3, 19)] // 2d8 + 3 = 19
        [InlineData(2, 10, -1, 19)] // 2d10 - 1 = 19
        [InlineData(3, 4, -5, 7)] // 3d4 -5 = 7

        public void Dice_properly_shows_MaxValue(int dieCount, int sideCount, int modifier, int expectedValue)
        {
            // Arrange & Act
            var sut = new Dice(dieCount, sideCount, modifier);
            // Assert
            Assert.Equal(expectedValue, sut.MaxValue);
        }



        [Fact]
        public void Can_roll_dice()
        {
            // Arrange
            var dice = new Dice();
            // Act
            var result = dice.Roll();
            // Assert
            Assert.InRange(result, dice.MinValue, dice.MaxValue);
        }


        [Theory]
        [InlineData(1, 6, 0, "1d6")]
        [InlineData(2, 8, 3, "2d8+3")]
        [InlineData(2, 10, -1, "2d10-1")]
        [InlineData(3, 4, -5, "3d4-5")]
        public void ToString_returns_value_in_dice_notation(int dieCount,int sideCount, int modifier, string expectedValue)
        {
            // Arrange
            var dice = new Dice(dieCount, sideCount, modifier);

            // Act
            var actualValue = dice.ToString();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }



        [Fact]
        public void Two_dice_with_same_parameters_are_equal()
        {
            // Arrange
            var dice1 = new Dice(2, 6, 3);
            var dice2 = new Dice(2, 6, 3);
            // Act & Assert
            Assert.Equal(dice1, dice2);
        }


        [Fact]
        public void Two_dice_with_different_DieCount_are_not_equal()
        {
            // Arrange
            var dice1 = new Dice(1, 6, 3);
            var dice2 = new Dice(2, 6, 3);
            // Act & Assert
            Assert.NotEqual(dice1, dice2);
        }



        [Fact]
        public void Two_dice_with_different_SideCount_are_not_equal()
        {
            // Arrange
            var dice1 = new Dice(1, 6, 3);
            var dice2 = new Dice(2, 6, 3);
            // Act & Assert
            Assert.NotEqual(dice1, dice2);
        }



        [Fact]
        public void Two_dice_with_different_Modifier_are_not_equal()
        {
            // Arrange
            var dice1 = new Dice(2, 6, 3);
            var dice2 = new Dice(2, 6, 4);
            // Act & Assert
            Assert.NotEqual(dice1, dice2);
        }


        [Fact]
        public void Two_dice_with_same_parameters_are_equal_using_Equals()
        {
            // Arrange
            var dice1 = new Dice(2, 6, 3);
            var dice2 = new Dice(2, 6, 3);
            // Act & Assert
            Assert.True(dice1.Equals(dice2));
        }


        [Fact]
        public void Two_dice_with_different_parameters_are_not_equal_using_Equals()
        {
            // Arrange
            var dice1 = new Dice(2, 6, 3);
            var dice2 = new Dice(2, 6, 4);
            // Act & Assert
            Assert.False(dice1.Equals(dice2));
        }



        [Fact]
        public void Dice_can_be_cast_to_IDice()
        {
            // Arrange
            IDice dice = new Dice();
            // Act & Assert
            Assert.NotNull(dice);
            Assert.IsType<Dice>(dice);
        }


        [Fact]
        public void Dice_can_be_cast_to_IEquatable_Dice()
        {
            // Arrange
            IEquatable<Dice> dice = new Dice();
            // Act & Assert
            Assert.NotNull(dice);
            Assert.IsType<Dice>(dice);
        }


        [Fact]
        public void Two_Dice_with_the_same_parameters_have_the_same_hash_code()
        {
            // Arrange
            var dice1 = new Dice(2, 6, 3);
            var dice2 = new Dice(2, 6, 3);
            // Act
            var hash1 = dice1.GetHashCode();
            var hash2 = dice2.GetHashCode();
            // Assert
            Assert.Equal(hash1, hash2);
        }



        [Fact]
        public void Two_Dice_with_different_parameters_have_the_different_hash_codes()
        {
            // Arrange
            var dice1 = new Dice(1, 8, 2);
            var dice2 = new Dice(2, 6, 3);

            // Act
            var hash1 = dice1.GetHashCode();
            var hash2 = dice2.GetHashCode();
            
            // Assert
            Assert.NotEqual(hash1, hash2);
        }



        [Fact]
        public void EqualsDice_Null_ReturnsFalse()
        {
            var dice = new Dice(1, 6, 0);
            Assert.False(dice.Equals(null));
        }



        [Fact]
        public void EqualsDice_SameReference_ReturnsTrue()
        {
            var dice = new Dice(1, 6, 0);
            Assert.True(dice.Equals(dice));
        }



        [Fact]
        public void EqualsDice_SameValues_ReturnsTrue()
        {
            var dice1 = new Dice(2, 8, 1);
            var dice2 = new Dice(2, 8, 1);
            Assert.True(dice1.Equals(dice2));
        }



        [Fact]
        public void EqualsDice_DifferentDieCount_ReturnsFalse()
        {
            var dice1 = new Dice(1, 6, 0);
            var dice2 = new Dice(2, 6, 0);
            Assert.False(dice1.Equals(dice2));
        }



        [Fact]
        public void EqualsDice_DifferentSideCount_ReturnsFalse()
        {
            var dice1 = new Dice(1, 6, 0);
            var dice2 = new Dice(1, 8, 0);
            Assert.False(dice1.Equals(dice2));
        }



        [Fact]
        public void EqualsDice_DifferentModifier_ReturnsFalse()
        {
            var dice1 = new Dice(1, 6, 0);
            var dice2 = new Dice(1, 6, 1);
            Assert.False(dice1.Equals(dice2));
        }



        // Tests for Equals(object? obj)

        [Fact]
        public void EqualsObject_Null_ReturnsFalse()
        {
            var dice = new Dice(1, 6, 0);
            Assert.False(dice.Equals(null));
        }



        [Fact]
        public void EqualsObject_SameReference_ReturnsTrue()
        {
            var dice = new Dice(1, 6, 0);
            Assert.True(dice.Equals((object)dice));
        }



        [Fact]
        public void EqualsObject_DifferentType_ReturnsFalse()
        {
            var dice = new Dice(1, 6, 0);
            var notDice = new { DieCount = 1, SideCount = 6, Modifier = 0 };
            Assert.False(dice.Equals(notDice));
        }



        [Fact]
        public void EqualsObject_SameValues_ReturnsTrue()
        {
            var dice1 = new Dice(2, 8, 1);
            var dice2 = new Dice(2, 8, 1);
            Assert.True(dice1.Equals((object)dice2));
        }



        [Fact]
        public void EqualsObject_DifferentValues_ReturnsFalse()
        {
            var dice1 = new Dice(1, 6, 0);
            var dice2 = new Dice(2, 6, 0);
            Assert.False(dice1.Equals((object)dice2));
        }



        [Fact]
        public void EqualsObject_WhenObjectIsNull_ReturnsFalse()
        {
            var dice = new Dice(1, 6, 0);
            object? obj = null;

            var result = dice.Equals(obj);

            Assert.False(result);
        }

        

        [Theory]
        [InlineData("1d6", 1, 6, 0)]
        [InlineData("2d8+3", 2, 8, 3)]
        [InlineData("2d10-1", 2, 10, -1)]
        [InlineData("2d10-1+2", 2, 10, 1)]
        [InlineData("2d10-1-2", 2, 10, -3)]
        [InlineData("2d10+1+2", 2, 10, 3)]
        [InlineData("2d10+0", 2, 10, 0)]
        [InlineData("2d10-0", 2, 10, 0)]
        [InlineData("d10", 1, 10, 0)]
        [InlineData("d10+3", 1, 10, 3)]
        [InlineData("d6-1", 1, 6, -1)]
        public void TryParse_when_notation_is_valid_returns_new_Dice(string notation, int dieCount, int sideCount, int modifier)
        {
            var result = Dice.TryParse(notation);

            Assert.True(result.Succeeded);
            Assert.Equal(dieCount, result.Value.DieCount);
            Assert.Equal(sideCount, result.Value.SideCount);
            Assert.Equal(modifier, result.Value.Modifier);
        }



        [Theory]
        [InlineData("-1d6")] // negative die count
        [InlineData("1d-1")]
        public void TryParse_when_dice_notation_is_invalid_fails_with_error_message(string? notation)
        {
            var result = Dice.TryParse(notation);

            Assert.True(result.Failed);
            Assert.Equal("Invalid dice notation format. Value must be in XdY+Z format.", result.ErrorMessage);
        }



        [Theory]
        [InlineData("  ")]
        [InlineData(null)]
        [InlineData("")]
        public void TryParse_when_dice_notation_is_null_or_whitespace(string? notation)
        {
            var result = Dice.TryParse(notation);

            Assert.True(result.Failed);
            Assert.Equal("Value cannot be null or empty.", result.ErrorMessage);
        }



        [Theory]
        [InlineData("0d6")]
        public void TryParse_when_dice_notation_dice_count_is_less_than_1_fails_with_error_message(string notation)
        {
            var result = Dice.TryParse(notation);

            Assert.True(result.Failed);
            Assert.Equal("Die count must be greater than 0.", result.ErrorMessage);
        }



        [Theory]
        [InlineData("1d1")]
        [InlineData("1d0")]
        public void TryParse_when_dice_notation_side_count_is_less_than_2_fails_with_error_message(string notation)
        {
            var result = Dice.TryParse(notation);

            Assert.True(result.Failed);
            Assert.Equal("Side count must be greater than 1.", result.ErrorMessage);
        }



    }
}
