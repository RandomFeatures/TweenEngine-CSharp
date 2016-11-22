using System;

namespace TweenEngine
{
	public interface ITweenCallback
	{
		void TweenEventOccured(TweenTypes eventType, Tween tween);
	}
}

