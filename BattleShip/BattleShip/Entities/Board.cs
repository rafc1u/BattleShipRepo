using BattleShip.Enums;
using BattleShip.ValueObjects;

namespace BattleShip.Entities;

public class Board
{
	private readonly IBoardSquareTarget[,] _board;
	private readonly IEnumerable<Ship> _placedShips;
	public IEnumerable<string> ColumnLetters => Enumerable.Range(0, _board.GetLength(0)).Select(x => $"{(char)('A' + x)}");
	public IEnumerable<int> RowDigits => Enumerable.Range(1, _board.GetLength(0));

	public int Size => _board.GetLength(0);

	public Board(IBoardSquareTarget[,] board, IEnumerable<Ship> placedShips)
	{
		_board = board;
		_placedShips = placedShips;
	}

	public ShotResult Shot(ShotCommand shotCommand)
	{
		return _board[shotCommand.Row, shotCommand.Column].Shot();
	}

	public TargetState GetState(ShotCommand command)
	{
		return _board[command.Row, command.Column].State;
	}

	public bool IsFinished()
	{
		return _placedShips.All(s => s.State == TargetState.Sink);
	}
}
