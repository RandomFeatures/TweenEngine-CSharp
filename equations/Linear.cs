using System;

namespace TweenEngine
{
	public class Linear
	{
		private sealed class TweenInout: TweenEquation {
			public TweenInout() 
			{
				TweenName =  "Linear.INOUT";

			}
			public override float Compute(float t, float b, float c, float d)
			{
				return c * t/d + b;
			}
		}

		public static TweenEquation Inout = new TweenInout();

	}
}

