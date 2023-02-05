using System;
namespace getakka_tutorial
{
	public sealed class CollectionTimeout
	{
		public static CollectionTimeout Instance { get; } = new CollectionTimeout();
		private CollectionTimeout() { }
	}
}

