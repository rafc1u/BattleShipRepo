namespace BattleShip.Extensions;

public static class StringExtensions
{
	public static int TextToNumber(this string text)
	{
		return text
			.Select(c => c - 'A' + 1)
			.Aggregate((sum, next) => sum * 26 + next) - 1;
	}
}
