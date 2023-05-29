using BattleShip.CustomExceptions;
using BattleShip.Entities;
using BattleShip.Enums;
using BattleShip.Services;
using Moq;
using Xunit;

namespace BattleShip.UnitTests.Services;

public class ShipPositionRandomizerTests
{
	[Theory]
	[InlineData(0, 0)]
	[InlineData(0, 5)]
	[InlineData(2, 3)]
	[InlineData(5, 5)]
	[InlineData(5, 0)]
	public void GetRandomPosition_BoardOfSize10WithHorizontalDirectionOfShip_ShipMightBePlacedInAllPossibleStartingSquares(int row, int col)
	{
		int squareBoardLength = 10;

		Mock<Random> randomMock = new Mock<Random>();
		randomMock.Setup(x => x.Next(0, 2)).Returns((int)StartingPositionDirection.Horizontal);
		randomMock.Setup(x => x.Next(0, 5)).Returns(col);
		randomMock.Setup(x => x.Next(0, 10)).Returns(row);

		var shipPositionRandomizer = new ShipPositionRandomizerService(randomMock.Object);

		var startingPosition = shipPositionRandomizer.GetRandomPosition(squareBoardLength, new Ship(5));

		Assert.Equal(row, startingPosition.Row);
		Assert.Equal(col, startingPosition.Column);
		Assert.Equal(StartingPositionDirection.Horizontal, startingPosition.Direction);

		randomMock.Verify(x => x.Next(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
	}

	[Theory]
	[InlineData(0, 0)]
	[InlineData(0, 5)]
	[InlineData(5, 5)]
	[InlineData(5, 0)]
	[InlineData(4, 4)]
	public void GetRandomPosition_BoardOfSize10WithVerticalDirectionOfShip_ShipMightBePlacedInAllPossibleStartingSquares(int row, int col)
	{
		int squareBoardLength = 10;

		Mock<Random> randomMock = new Mock<Random>();
		randomMock.Setup(x => x.Next(0, 2)).Returns((int)StartingPositionDirection.Vertical);
		randomMock.Setup(x => x.Next(0, 5)).Returns(row);
		randomMock.Setup(x => x.Next(0, 10)).Returns(col);

		var shipPositionRandomizer = new ShipPositionRandomizerService(randomMock.Object);

		var startingPosition = shipPositionRandomizer.GetRandomPosition(squareBoardLength, new Ship(5));

		Assert.Equal(row, startingPosition.Row);
		Assert.Equal(col, startingPosition.Column);
		Assert.Equal(StartingPositionDirection.Vertical, startingPosition.Direction);

		randomMock.Verify(x => x.Next(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
	}

	[Fact]
	public void GetRandomPosition_ShipLongerThenBoardSize_ExceptionIsThrown()
	{
		Mock<Random> randomMock = new Mock<Random>();
		var shipPositionRandomizer = new ShipPositionRandomizerService(randomMock.Object);

		var action = () => shipPositionRandomizer.GetRandomPosition(1, new Ship(5));

		Assert.Throws<BusinessValidationException>(action);
	}
}