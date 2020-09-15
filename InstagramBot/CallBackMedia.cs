namespace InstagramBot
{
	public static class CallBackMedia
	{
		public delegate void callbackEvent(dynamic media, string type);

		public static callbackEvent callbackEventHandler;
	}
}
