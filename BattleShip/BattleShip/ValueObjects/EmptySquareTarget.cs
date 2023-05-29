using BattleShip.Enums;

namespace BattleShip.ValueObjects;

public class EmptySquareTarget : IBoardSquareTarget
{
	public TargetState State { get; private set; }

	public ShotResult Shot()
	{
		if (State == TargetState.NotHit)
		{
			State = TargetState.Miss;

			return ShotResult.Miss;
		}


		return ShotResult.AlreadyMiss;
	}
}