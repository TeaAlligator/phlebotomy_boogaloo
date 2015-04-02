
namespace Assets.Code.Messaging.Messages
{
	public class ScoreChangedMessage : IMessage 
	{
		public int NewMistakes;
		public int NewNotMistakes;
	}
}
