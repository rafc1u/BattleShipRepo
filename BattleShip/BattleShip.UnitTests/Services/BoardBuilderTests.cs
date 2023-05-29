using BattleShip.CustomExceptions;
using BattleShip.Entities;
using BattleShip.Enums;
using BattleShip.Services;
using BattleShip.ValueObjects;
using Moq;
using Xunit;

namespace BattleShip.UnitTests.Services;

public class BoardBuilderTests
{
	[Theory]
	[InlineData(StartingPositionDirection.Vertical, 0, 0, "A1", "A2", "A3", "A4", "A5")]
	[InlineData(StartingPositionDirection.Vertical, 5, 0, "A6", "A7", "A8", "A9", "A10")]
	[InlineData(StartingPositionDirection.Horizontal, 9, 5, "F10", "G10", "H10", "I10", "J10")]
	public void Build_PlaceSingleShipOnBoard_ShipPlacedOnTheDrawnPosition(StartingPositionDirection direction, int row, int col, params string[] shots)
	{
		var size = 10;

		var shipPositionRandomizer = new Mock<IShipPositionRandomizer>();

		shipPositionRandomizer
			.Setup(s => s.GetRandomPosition(It.IsAny<int>(), It.IsAny<Ship>()))
			.Returns(new ShipStartingPosition(direction, row, col));

		var boardBuilder = new BoardBuilder(shipPositionRandomizer.Object);

		var board = boardBuilder.Build(size, new[] { new Ship(5) });

		Assert.False(board.IsFinished());

		for (int i = 0; i < shots.Length - 1; i++)
		{
			Assert.Equal(ShotResult.Hit, board.Shot(new ShotCommand(shots[i])));
		}

		Assert.Equal(ShotResult.Sink, board.Shot(new ShotCommand(shots.Last())));
		Assert.True(board.IsFinished());

		shipPositionRandomizer.Verify(s => s.GetRandomPosition(It.IsAny<int>(), It.IsAny<Ship>()), Times.Once);
	}

	[Theory]
	[InlineData(StartingPositionDirection.Horizontal, 0, 0)]
	[InlineData(StartingPositionDirection.Horizontal, 0, 1)]
	[InlineData(StartingPositionDirection.Horizontal, 0, 4)]
	[InlineData(StartingPositionDirection.Vertical, 0, 0)]
	[InlineData(StartingPositionDirection.Vertical, 0, 2)]
	[InlineData(StartingPositionDirection.Vertical, 0, 4)]
	public void Build_PlaceTwoShipsWhichIntersectsAfterFirstDrawn_AnotherDrawIsPerformedToFindNewPosition(StartingPositionDirection direction, int row, int col)
	{
		var shipPositionRandomizer = new Mock<IShipPositionRandomizer>();

		shipPositionRandomizer
			.SetupSequence(s => s.GetRandomPosition(It.IsAny<int>(), It.IsAny<Ship>()))
			.Returns(new ShipStartingPosition(StartingPositionDirection.Horizontal, 0, 0))
			.Returns(new ShipStartingPosition(direction, row, col))
			.Returns(new ShipStartingPosition(StartingPositionDirection.Horizontal, 0, 5));

		var boardBuilder = new BoardBuilder(shipPositionRandomizer.Object);

		var board = boardBuilder.Build(10, new[] { new Ship(5), new Ship(5) });

		Assert.False(board.IsFinished());

		Assert.Equal(ShotResult.Hit, board.Shot(new ShotCommand("A1")));
		Assert.Equal(ShotResult.Hit, board.Shot(new ShotCommand("B1")));
		Assert.Equal(ShotResult.Hit, board.Shot(new ShotCommand("C1")));
		Assert.Equal(ShotResult.Hit, board.Shot(new ShotCommand("D1")));
		Assert.Equal(ShotResult.Sink, board.Shot(new ShotCommand("E1")));

		Assert.False(board.IsFinished());

		Assert.Equal(ShotResult.Hit, board.Shot(new ShotCommand("F1")));
		Assert.Equal(ShotResult.Hit, board.Shot(new ShotCommand("G1")));
		Assert.Equal(ShotResult.Hit, board.Shot(new ShotCommand("H1")));
		Assert.Equal(ShotResult.Hit, board.Shot(new ShotCommand("I1")));
		Assert.Equal(ShotResult.Sink, board.Shot(new ShotCommand("J1")));

		Assert.True(board.IsFinished());

		shipPositionRandomizer.Verify(s => s.GetRandomPosition(It.IsAny<int>(), It.IsAny<Ship>()), Times.Exactly(3));
	}

	[Fact]
	public void Build_PlaceTwoShipsWithSecondShipOverlapingFirstOneAllTheTIme_ExceptionIsThrownBecauseOfPotentialInfiniteLoop()
	{
		var shipPositionRandomizer = new Mock<IShipPositionRandomizer>();

		shipPositionRandomizer
			.Setup(s => s.GetRandomPosition(It.IsAny<int>(), It.IsAny<Ship>()))
			.Returns(new ShipStartingPosition(StartingPositionDirection.Horizontal, 0, 0));

		var boardBuilder = new BoardBuilder(shipPositionRandomizer.Object);

		var action = () => boardBuilder.Build(10, new[] { new Ship(5), new Ship(5) });

		Assert.Throws<BusinessValidationException>(action);

		shipPositionRandomizer.Verify(s => s.GetRandomPosition(It.IsAny<int>(), It.IsAny<Ship>()), Times.Exactly(100));
	}
}
