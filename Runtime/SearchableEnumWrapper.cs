using System;

namespace JuiceProtocol.BeyondTooling
{
	[Serializable]
	public class SearchableEnumWrapper<T> where T : Enum
	{
		[SearchableEnum]
		public T value;

		public SearchableEnumWrapper( T value )
		{
			this.value = value;
		}

		public override bool Equals( object obj )
		{
			return obj is SearchableEnumWrapper<T> other && value.Equals( other.value );
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}

		public static implicit operator T( SearchableEnumWrapper<T> wrapper ) => wrapper.value;
		public static implicit operator SearchableEnumWrapper<T>( T value ) => new( value );
	}
}