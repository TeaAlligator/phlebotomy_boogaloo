using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Messaging.Messages
{
	public class StateChangedMessage : IMessage
	{
		public Type TargetStateType;
	}
}
