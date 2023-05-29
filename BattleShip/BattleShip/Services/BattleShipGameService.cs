using BattleShip.CustomExceptions;
using BattleShip.Entities;
using BattleShip.Enums;
using BattleShip.ValueObjects;

namespace BattleShip.Services;

internal class BattleShipGameService
{
	private readonly int _size = Program.SquareBoardLength;

	private readonly IEnumerable<Ship> _shipConfig = new[] {
		new Ship(5),
		new Ship(4),
		new Ship(4)
	};

	public readonly Board Board;

	public BattleShipGameService(BoardBuilder boardBuilder)
	{
		Board = boardBuilder.Build(_size, _shipConfig);
	}

	public void StartGame()
	{
		Console.WriteLine("Let's make a shot. Type target for example A2 (or a2)");

		while (!Board.IsFinished())
		{
			try
			{
				var read = Console.ReadLine() ?? "";

				if (read.ToLower() == "s")
				{
					return;
				}

				var cmd = new ShotCommand(read);
				var shotResult = Board.Shot(cmd);

				Draw();

				Console.WriteLine();
				Console.WriteLine($"{read} : {shotResult}");
				Console.WriteLine();
			}
			catch (BusinessValidationException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch
			{
				Console.WriteLine("Unexpected exception. Sorry!");

				return;
			}
		}

		if (Board.IsFinished())
		{
			Console.WriteLine("Congratulations! The game is finished.");

			return;
		}
	}

	private void Draw()
	{
		var columnHeader = string.Join("", Board.ColumnLetters);

		Console.WriteLine($"   {columnHeader}");

		foreach (var row in Board.RowDigits)
		{
			Console.Write(String.Format("{0:00} ", row));

			foreach (var col in Board.ColumnLetters)
			{
				var symbol = "";

				switch (Board.GetState(new ShotCommand($"{col}{row}")))
				{
					case TargetState.NotHit:
						symbol = ".";
						break;
					case TargetState.Miss:
						symbol = "x";
						break;
					case TargetState.Hit:
						symbol = "H";
						break;
					case TargetState.Sink:
						symbol = "S";
						break;
				}

				Console.Write(symbol);
			}

			Console.WriteLine();
		}
	}
}
