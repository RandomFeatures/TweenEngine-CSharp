using System;

namespace TweenEngine
{
	public class Expo
	{
		private sealed class TweenIn: TweenEquation {
			public TweenIn()
			{

				base.TweenName =  "Expo.IN";
			}
			public override float Compute(float t, float b, float c, float d)
			{

				return (t==0) ? b : c * (float)Math.Pow(2, 10 * (t/d - 1)) + b;
			}
		}

		private sealed class TweenOut: TweenEquation {
			public TweenOut()
			{
				TweenName =  "Expo.OUT";
			}
			public override float Compute(float t, float b, float c, float d)
			{
				return (t==d) ? b+c : c * (-(float)Math.Pow(2, -10 * t/d) + 1) + b;
			}
		}

		private sealed class TweenInout: TweenEquation {
			public TweenInout() 
			{
				TweenName =  "Expo.INOUT";

			}
			public override float Compute(float t, float b, float c, float d)
			{
				if (t==0) return b;
				if (t==d) return b+c;
				if ((t/=d/2) < 1) return c/2 * (float)Math.Pow(2, 10 * (t - 1)) + b;
				return c/2 * (-(float)Math.Pow(2, -10 * --t) + 2) + b;
			}
		}


		public static TweenEquation In = new TweenIn();
		public static TweenEquation Out = new TweenOut();
		public static TweenEquation Inout = new TweenInout();
	}
}

