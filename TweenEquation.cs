using System;

namespace TweenEngine
{
	/**
	 * Base class for every easing equations. You can create your own equations 
	 * and directly use them in the tween static methods by deriving from this 
	 * class
	 */ 

	public abstract class TweenEquation
	{
		public TweenEquation()
		{

		}
		protected String TweenName;
		/**
		 * Computes the next value of the interpolation
		 * @param t Current time, in seconds
		 * @param b Initial value
		 * @param c Offset to the inital value
		 * @param d Total duration, in seconds
		 */
		public abstract float Compute(float t, float b, float c, float d);



		/**
		 * Returns true if the given string is the name of the equation (the name
		 * is returned in the ToString() method, don't forget to override it).
		 * This method is usually used to save/ load a tween to/from a text file
		 */
		public bool IsValueOf (String str)
		{
			return str.Equals(TweenName);
		}
	}
}

