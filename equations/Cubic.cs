using System;

namespace TweenEngine
{
	public class Cubic
	{
		private sealed class TweenIn: TweenEquation {
			public TweenIn()
			{

				base.TweenName =  "Cubic.IN";
			}
			public override float Compute(float t, float b, float c, float d)
			{
				return c*(t/=d)*t*t + b;
			}
		}

		private sealed class TweenOut: TweenEquation {
			public TweenOut()
			{
				TweenName =  "Cubic.OUT";
			}
			public override float Compute(float t, float b, float c, float d)
			{
				return c*((t=t/d-1)*t*t + 1) + b;
			}
		}

		private sealed class TweenInout: TweenEquation {
			public TweenInout() 
			{
				TweenName =  "Cubic.INOUT";

			}
			public override float Compute(float t, float b, float c, float d)
			{
				if ((t/=d/2) < 1) return c/2*t*t*t + b;
				return c/2*((t-=2)*t*t + 2) + b;
			}
		}


		public static TweenEquation In = new TweenIn();
		public static TweenEquation Out = new TweenOut();
		public static TweenEquation Inout = new TweenInout();

	}
}

