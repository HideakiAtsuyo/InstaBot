using System.Collections.Generic;

namespace InstagramBot
{
	public static class CallBackTime
	{
		public delegate void callbackEvent(Dictionary<string, int> next);

		public static callbackEvent callbackEventHandler;
	}
}
