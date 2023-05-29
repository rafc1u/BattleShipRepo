using BattleShip.Enums;

namespace BattleShip.ValueObjects;

public class ShipStartingPosition
{
	public ShipStartingPosition(StartingPositionDirection direction, int row, int column)
	{
		Row = row;
		Column = column;
		Direction = direction;
	}

	public int Row { get; }

	public int Column { get; }

	public StartingPositionDirection Direction { get; }

	public bool IsHorizontal => Direction == StartingPositionDirection.Horizontal;
}