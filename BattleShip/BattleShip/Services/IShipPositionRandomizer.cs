using BattleShip.Entities;
using BattleShip.ValueObjects;

namespace BattleShip.Services;

public interface IShipPositionRandomizer
{
	ShipStartingPosition GetRandomPosition(int size, Ship ship);
}