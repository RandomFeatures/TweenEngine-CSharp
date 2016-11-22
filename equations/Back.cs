using System;


namespace TweenEngine
{
	public class Back
	{
		private sealed class TweenIn: TweenEquation {
			public TweenIn()
			{

				base.TweenName =  "Back.IN";
			}
			public override float Compute(float t, float b, float c, float d)
			{
				float s = 1.70158f;
				return c*(t/=d)*t*((s+1)*t - s)+b;
			}
		}

		private sealed class TweenOut: TweenEquation {
			public TweenOut()
			{
				TweenName =  "Back.OUT";
			}
			public override float Compute(float t, float b, float c, float d)
			{
				float s = 1.70158f;
				return c*((t=t/d-1)*t*((s+1)*t + s) + 1)+b;
			}
		}

		private sealed class TweenInout: TweenEquation {
			public TweenInout() 
			{
				TweenName =  "Back.INOUT";

			}
			public override float Compute(float t, float b, float c, float d)
			{
				float s = 1.70158f;
				if((t/=d/2)<1) return c/2*(t*t*(((s*=(1.525f))+1)*t - s)) + b;
				return c/2*((t-=2)*t*(((s*=(1.525f))+1)*t + s) + 2) + b;
			}
		}


		public static TweenEquation In = new TweenIn();
		public static TweenEquation Out = new TweenOut();
		public static TweenEquation Inout = new TweenInout();
	}
}

