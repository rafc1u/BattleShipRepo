using System.Collections.ObjectModel;
using BattleShip.Enums;
using BattleShip.ValueObjects;

namespace BattleShip.Entities;

public class Ship
{
	private List<ShipSquareTarget> _points = new List<ShipSquareTarget>();

	public ReadOnlyCollection<ShipSquareTarget> Points => _points.AsReadOnly();

	public int Length => _points.Count;

	private bool _isHit;

	private bool _isSank;


	public TargetState State
	{
		get
		{
			if (!_isHit)
			{
				return TargetState.NotHit;
			}

			if (_isSank)
			{
				return TargetState.Sink;
			}

			return TargetState.Hit;
		}
	}


	public Ship(int size)
	{
		if (size < 1)
		{
			throw new ArgumentException("Size of the ships can not be smaller then 1");
		}

		for (int i = 0; i < size; i++)
		{
			_points.Add(new ShipSquareTarget(this));
		}
	}

	public ShotResult Shot(ShipSquareTarget point, TargetState state)
	{
		if (state != TargetState.NotHit)
		{
			return ShotResult.AlreadyHit;
		}

		_isHit = true;

		var result = _points
			.Where(p => p != point)
			.All(p => p.State == TargetState.Hit) ? ShotResult.Sink : ShotResult.Hit;

		if (result == ShotResult.Sink)
		{
			_isSank = true;
		}

		return result;
	}
}
