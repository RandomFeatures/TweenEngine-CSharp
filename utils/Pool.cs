using System;
using System.Collections.Generic;

namespace TweenEngine
{
	public abstract class Pool<T>
	{
		private readonly List<T> _objects;

	    protected Pool (int iCapacity)
		{
			_objects = new List<T>(iCapacity);
		}

		protected abstract T GetNew ();

		public T Get()
		{
			T rtn;

			if(_objects.Count == 0)
				return GetNew();
			else
			{
				rtn = _objects[_objects.Count-1];
				_objects.RemoveAt(_objects.Count-1);
				return rtn;
			}
		}

		public void Free (T obj)
		{
			if(!_objects.Contains(obj))
				_objects.Add(obj);
		}

		public void Clear ()
		{
			_objects.Clear();
		}

		public int Size ()
		{
			return _objects.Count;
		}
	}
}

