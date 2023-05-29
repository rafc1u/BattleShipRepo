namespace BattleShip.CustomExceptions;

public class BusinessValidationException : Exception
{
	public BusinessValidationException(string message) : base(message) { }
}
