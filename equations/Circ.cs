using System;

namespace TweenEngine
{
	public class Circ
	{
		private sealed class TweenIn: TweenEquation {
			public TweenIn()
			{

				base.TweenName =  "Circ.IN";
			}
			public override float Compute(float t, float b, float c, float d)
			{

				return -c * ((float)Math.Sqrt(1 - (t/=d)*t) - 1) + b;
			}
		}

		private sealed class TweenOut: TweenEquation {
			public TweenOut()
			{
				TweenName =  "Circ.OUT";
			}
			public override float Compute(float t, float b, float c, float d)
			{

				return c * (float)Math.Sqrt(1 - (t=t/d-1)*t) + b;
			}
		}

		private sealed class TweenInout: TweenEquation {
			public TweenInout() 
			{
				TweenName =  "Circ.INOUT";

			}
			public override float Compute(float t, float b, float c, float d)
			{
				if ((t/=d/2) < 1) return -c/2 * ((float)Math.Sqrt(1 - t*t) - 1) + b;
				return c/2 * ((float)Math.Sqrt(1 - (t-=2)*t) + 1) + b;
			}
		}


		public static TweenEquation In = new TweenIn();
		public static TweenEquation Out = new TweenOut();
		public static TweenEquation Inout = new TweenInout();

	}
}

