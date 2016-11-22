using System;

namespace TweenEngine
{
	public class Quad
	{
		private sealed class TweenIn: TweenEquation {
			public TweenIn()
			{

				base.TweenName =  "Quad.IN";
			}
			public override float Compute(float t, float b, float c, float d)
			{
				return c*(t/=d)*t + b;
			}
		}

		private sealed class TweenOut: TweenEquation {
			public TweenOut()
			{
				TweenName =  "Quad.OUT";
			}
			public override float Compute(float t, float b, float c, float d)
			{
				return -c*(t/=d)*(t-2) + b;
			}
		}

		private sealed class TweenInout: TweenEquation {
			public TweenInout() 
			{
				TweenName =  "Quad.INOUT";

			}
			public override float Compute(float t, float b, float c, float d)
			{
				if ((t/=d/2) < 1) return c/2*t*t + b;
				return -c/2 * ((--t)*(t-2) - 1) + b;
			}
		}


		public static TweenEquation In = new TweenIn();
		public static TweenEquation Out = new TweenOut();
		public static TweenEquation Inout = new TweenInout();

	}
}

