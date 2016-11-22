using System;

namespace TweenEngine
{
	public class Bounce
	{
		private sealed class TweenIn: TweenEquation {
			public TweenIn()
			{

				base.TweenName =  "Bounce.IN";
			}
			public override float Compute(float t, float b, float c, float d)
			{
				return c - Out.Compute(d-t, 0, c, d) + b;
			}
		}

		private sealed class TweenOut: TweenEquation {
			public TweenOut()
			{
				TweenName =  "Bounce.OUT";
			}
			public override float Compute(float t, float b, float c, float d)
			{
				if ((t/=d) < (1/2.75)) {
					return c*(7.5625f*t*t) + b;
				} else if (t < (2/2.75)) {
					return c*(7.5625f*(t-=(1.5f/2.75f))*t + .75f) + b;
				} else if (t < (2.5/2.75)) {
					return c*(7.5625f*(t-=(2.25f/2.75f))*t + .9375f) + b;
				} else {
					return c*(7.5625f*(t-=(2.625f/2.75f))*t + .984375f) + b;
				}
			}
		}

		private sealed class TweenInout: TweenEquation {
			public TweenInout() 
			{
				TweenName =  "Bounce.INOUT";

			}
			public override float Compute(float t, float b, float c, float d)
			{
				if (t < d/2) return In.Compute(t*2, 0, c, d) * .5f + b;
				else return Out.Compute(t*2-d, 0, c, d) * .5f + c*.5f + b;
			}
		}


		public static TweenEquation In = new TweenIn();
		public static TweenEquation Out = new TweenOut();
		public static TweenEquation Inout = new TweenInout();
	}
}

