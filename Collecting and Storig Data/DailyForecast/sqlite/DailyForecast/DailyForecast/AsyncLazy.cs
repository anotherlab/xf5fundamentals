using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DailyForecast
{
	/// <summary>Helper class to provide an asynch friendly of initializing a resource right that the point it is needed.</summary>
	/// This makes it easy to use from a viewmodel
	/// For more information on this class:
	/// https://devblogs.microsoft.com/pfxteam/asynclazyt/
	/// https://blog.stephencleary.com/2012/08/asynchronous-lazy-initialization.html
	public class AsyncLazy<T>
	{
		readonly Lazy<Task<T>> instance;

		public AsyncLazy(Func<T> factory)
		{
			instance = new Lazy<Task<T>>(() => Task.Run(factory));
		}

		public AsyncLazy(Func<Task<T>> factory)
		{
			instance = new Lazy<Task<T>>(() => Task.Run(factory));
		}

		public TaskAwaiter<T> GetAwaiter()
		{
			return instance.Value.GetAwaiter();
		}
	}
}
