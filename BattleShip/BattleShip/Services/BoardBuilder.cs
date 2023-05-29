using BattleShip.CustomExceptions;
using BattleShip.Entities;
using BattleShip.ValueObjects;

namespace BattleShip.Services;

public class BoardBuilder
{
	private readonly IShipPositionRandomizer _shipPositionRandomizer;
	private const int MaxNmberOfAttemptsToFindPosition = 100;

	public BoardBuilder(IShipPositionRandomizer shipPositionRandomizer)
	{
		_shipPositionRandomizer = shipPositionRandomizer;
	}

	public Board Build(int size, IEnumerable<Ship> shipsToPlace)
	{
		var board = PlaceShips(size, shipsToPlace);

		return new Board(board, shipsToPlace);
	}

	private IBoardSquareTarget[,] PlaceShips(int size, IEnumerable<Ship> shipsToPlace)
	{
		var board = new IBoardSquareTarget[size, size];

		foreach (var ship in shipsToPlace)
		{
			PlaceShip(size, board, ship);
		}

		FillInTheBoardWithEmptyTargets(board);

		return board;
	}

	private void PlaceShip(int size, IBoardSquareTarget[,] board, Ship ship)
	{
		var attemptToPlaceCounter = 1;
		var placeResult = false;

		while (!placeResult)
		{
			var position = _shipPositionRandomizer.GetRandomPosition(size, ship);

			placeResult = TrySetShipOnBoard(board, ship, position);

			if (++attemptToPlaceCounter == MaxNmberOfAttemptsToFindPosition)
			{
				throw new BusinessValidationException("It was not possible to generate ships positions. Please verify board size as well as number and size of ships");
			}
		}
	}

	private bool TrySetShipOnBoard(IBoardSquareTarget[,] board, Ship ship, ShipStartingPosition position)
	{
		if (position.IsHorizontal)
		{
			return TrySetShipInDirectionHor(board, position.Row, position.Column, ship);
		}
		else
		{
			return TrySetShipInDirectionVer(board, position.Row, position.Column, ship);
		}
	}

	private bool TrySetShipInDirectionVer(IBoardSquareTarget[,] Board, int row, int col, Ship ship)
	{
		for (int i = 0; i < ship.Length; i++)
		{
			if (Board[row + i, col] != null)
			{
				return false;
			}
		}

		for (int i = 0; i < ship.Length; i++)
		{
			Board[row + i, col] = ship.Points.ElementAt(i);
		}

		return true;
	}

	private bool TrySetShipInDirectionHor(IBoardSquareTarget[,] Board, int row, int col, Ship ship)
	{
		for (int i = 0; i < ship.Length; i++)
		{
			if (Board[row, col + i] != null)
			{
				return false;
			}
		}

		for (int i = 0; i < ship.Length; i++)
		{
			Board[row, col + i] = ship.Points.ElementAt(i);
		}

		return true;
	}

	private static void FillInTheBoardWithEmptyTargets(IBoardSquareTarget[,] board)
	{
		for (int i = 0; i < board.GetLength(0); i++)
		{
			for (int j = 0; j < board.GetLength(1); j++)
			{
				if (board[i, j] == null)
				{
					board[i, j] = new EmptySquareTarget();
				}
			}
		}
	}
}
