using System.Drawing;
using BattleShip.Services;

namespace BattleShip;

internal class Program
{
	public const int SquareBoardLength = 10;

	private static void Main(string[] args)
	{
		Console.WriteLine("Hello in one-sided battleship.");
		Console.WriteLine("You can stop game anytime by entering s command and hitting enter");
		Console.WriteLine("");

		var boardBuilder = new BoardBuilder(new ShipPositionRandomizerService(new Random()));
		var gameService = new BattleShipGameService(boardBuilder);

		gameService.StartGame();

		Console.WriteLine("Press enter to exit");

		Console.ReadLine();
	}
}