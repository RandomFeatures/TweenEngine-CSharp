using System;

namespace TweenEngine
{
	public class Elastic
	{
		private static float _pi = 3.14159265f;
		private sealed class TweenIn: TweenEquation {
			public TweenIn()
			{

				base.TweenName =  "Elastic.IN";
			}
			public override float Compute(float t, float b, float c, float d)
			{
				if (t==0) return b;  if ((t/=d)==1) return b+c;
				float p = d*.3f;
				float a = c;
				float s = p/4;
				return -(a*(float)Math.Pow(2,10*(t-=1)) * (float)Math.Sin( (t*d-s)*(2*_pi)/p )) + b;
			}
		}

		private sealed class TweenOut: TweenEquation {
			public TweenOut()
			{
				TweenName =  "Elastic.OUT";
			}
			public override float Compute(float t, float b, float c, float d)
			{
				if (t==0) return b;  if ((t/=d)==1) return b+c;
				float p = d*.3f;
				float a = c;
				float s = p/4;
				return (a*(float)Math.Pow(2,-10*t) * (float)Math.Sin( (t*d-s)*(2*_pi)/p ) + c + b);
			}
		}

		private sealed class TweenInout: TweenEquation {
			public TweenInout() 
			{
				TweenName =  "Elastic.INOUT";

			}
			public override float Compute(float t, float b, float c, float d)
			{
				if (t==0) return b;  if ((t/=d/2)==2) return b+c;
				float p = d*(.3f*1.5f);
				float a = c;
				float s = p/4;
				if (t < 1) return -.5f*(a*(float)Math.Pow(2,10*(t-=1)) * (float)Math.Sin( (t*d-s)*(2*_pi)/p )) + b;
				return a*(float)Math.Pow(2,-10*(t-=1)) * (float)Math.Sin( (t*d-s)*(2*_pi)/p )*.5f + c + b;
			}
		}


		public static TweenEquation In = new TweenIn();
		public static TweenEquation Out = new TweenOut();
		public static TweenEquation Inout = new TweenInout();

	}
}

