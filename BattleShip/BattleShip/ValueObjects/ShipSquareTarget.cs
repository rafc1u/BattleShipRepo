using BattleShip.Entities;
using BattleShip.Enums;

namespace BattleShip.ValueObjects;

public class ShipSquareTarget : IBoardSquareTarget
{
	private TargetState _state;
	public TargetState State
	{
		get
		{
			return _state == TargetState.NotHit ? TargetState.NotHit : _ship.State;
		}
	}

	private Ship _ship;

	public ShipSquareTarget(Ship ship)
	{
		_ship = ship;
	}

	public ShotResult Shot()
	{
		if (State != TargetState.NotHit)
		{
			return ShotResult.AlreadyHit;
		}

		var shipRresult = _ship.Shot(this, State);

		if (shipRresult == ShotResult.Sink)
		{
			_state = TargetState.Sink;
		}
		else
		{
			_state = TargetState.Hit;
		}

		return shipRresult;
	}
}
