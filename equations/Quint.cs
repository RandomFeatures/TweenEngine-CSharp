using System;

namespace TweenEngine
{
	public class Quint
	{
		private sealed class TweenIn: TweenEquation {
			public TweenIn()
			{

				base.TweenName =  "Quint.IN";
			}
			public override float Compute(float t, float b, float c, float d)
			{
				return c*(t/=d)*t*t*t*t + b;
			}
		}

		private sealed class TweenOut: TweenEquation {
			public TweenOut()
			{
				TweenName =  "Quint.OUT";
			}
			public override float Compute(float t, float b, float c, float d)
			{
				return c*((t=t/d-1)*t*t*t*t + 1) + b;
			}
		}

		private sealed class TweenInout: TweenEquation {
			public TweenInout() 
			{
				TweenName =  "Quint.INOUT";

			}
			public override float Compute(float t, float b, float c, float d)
			{
				if ((t/=d/2) < 1) return c/2*t*t*t*t*t + b;
				return c/2*((t-=2)*t*t*t*t + 2) + b;
			}
		}


		public static TweenEquation In = new TweenIn();
		public static TweenEquation Out = new TweenOut();
		public static TweenEquation Inout = new TweenInout();

	}
}

