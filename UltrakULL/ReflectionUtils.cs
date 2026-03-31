using System;
using System.Reflection;

namespace UltrakULL
{
	public class ReflectionUtils
	{
		private static readonly BindingFlags BindingFlagsFields = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		public static void SetPrivate<T, V>(T instance, Type classType, string field, V value)
		{
			classType.GetField(field, BindingFlagsFields).SetValue(instance, value);
		}

		public static V GetPrivate<T, V>(T instance, Type classType, string field)
		{
			return (V)classType.GetField(field, BindingFlagsFields).GetValue(instance);
		}
	}
}
