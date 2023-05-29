using BattleShip.CustomExceptions;
using BattleShip.Entities;
using BattleShip.Enums;
using BattleShip.ValueObjects;

namespace BattleShip.Services;

public class ShipPositionRandomizerService : IShipPositionRandomizer
{
	private readonly Random _random;

	public ShipPositionRandomizerService(Random random)
	{
		_random = random;
	}

	public ShipStartingPosition GetRandomPosition(int boardSizeLength, Ship ship)
	{
		if (ship.Length > boardSizeLength)
		{
			throw new BusinessValidationException("Such a long ship can not be placed on specified board");
		}

		var shipDirection = _random.Next((int)StartingPositionDirection.Horizontal, (int)StartingPositionDirection.Vertical + 1);
		var shortenedStartPosition = _random.Next(0, boardSizeLength - ship.Length);
		var startPosition = _random.Next(0, boardSizeLength);

		if (shipDirection == (int)StartingPositionDirection.Horizontal)
		{
			return new ShipStartingPosition((StartingPositionDirection)shipDirection, startPosition, shortenedStartPosition);
		}

		return new ShipStartingPosition((StartingPositionDirection)shipDirection, shortenedStartPosition, startPosition);
	}
}