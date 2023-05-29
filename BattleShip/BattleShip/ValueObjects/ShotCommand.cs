using BattleShip.CustomExceptions;
using BattleShip.Extensions;

namespace BattleShip.ValueObjects;

public class ShotCommand
{
	private readonly int Size = Program.SquareBoardLength;

	public int Row { get; }
	public int Column { get; }

	public ShotCommand(string input)
	{
		if (input.Length < 2 || input.Length > 3)
		{
			throw new BusinessValidationException("Incorrect value of Shot. Correct input should consist of a letter and digit. For example A2");
		}

		var col = input.Substring(0, 1).ToUpperInvariant().TextToNumber();
		var digitParsed = int.TryParse(input.Substring(1), out var row);
		row--;

		if (!digitParsed || row < 0 || col < 0 || row >= Size || col >= Size)
		{
			throw new BusinessValidationException($"Incorrect value of Shot. Correct input should be between A0 - {(char)(Convert.ToInt32('A') + Program.SquareBoardLength - 1)}{Program.SquareBoardLength}");
		}

		Row = row;
		Column = col;
	}
}
