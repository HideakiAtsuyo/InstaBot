namespace InstagramBot
{
	public static class CallBackLog
	{
		public delegate void callbackEvent(string log);

		public static callbackEvent callbackEventHandler;
	}
}
