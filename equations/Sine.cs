using System;

namespace TweenEngine
{
	public class Sine
	{
		private static float _pi = 3.14159265f;
		private sealed class TweenIn: TweenEquation {
			public TweenIn()
			{

				base.TweenName =  "Sine.IN";
			}
			public override float Compute(float t, float b, float c, float d)
			{
				return -c * (float)Math.Cos(t/d * (_pi/2)) + c + b;
			}
		}

		private sealed class TweenOut: TweenEquation {
			public TweenOut()
			{
				TweenName =  "Sine.OUT";
			}
			public override float Compute(float t, float b, float c, float d)
			{
				return c * (float)Math.Sin(t/d * (_pi/2)) + b;
			}
		}

		private sealed class TweenInout: TweenEquation {
			public TweenInout() 
			{
				TweenName =  "Sine.INOUT";

			}
			public override float Compute(float t, float b, float c, float d)
			{
				return -c/2 * ((float)Math.Cos(_pi*t/d) - 1) + b;
			}
		}


		public static TweenEquation In = new TweenIn();
		public static TweenEquation Out = new TweenOut();
		public static TweenEquation Inout = new TweenInout();

	}
}

