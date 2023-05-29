using BattleShip.Enums;

namespace BattleShip.ValueObjects;

public interface IBoardSquareTarget
{
	public ShotResult Shot();
	public TargetState State { get; }
}
