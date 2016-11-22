using System;
using System.Collections.Generic;

namespace TweenEngine
{

 /**
 * A TweenGroup can be used to group multiple tweens and to act on all of them
 * at once. Its main use lies in the sequence() method, which automatically 
 * delays the tweens so they will be executed one after the other. Another
 * option you might want to give a look is the repeat() method. It allows the
 * repetition of a whole sequence when the last running tween reaches its end.
 *
 * Note that you can reuse the same TweenGroup again and again since the pack()
 * method clears its content.
 *
 * Just like individual tweens, add the group to a TweenManager and update the
 * latter periodically.
 *
 * @see Tween
 * @see TweenManager
 * @author Aurelien Ribon (aurelien.ribon@gmail.com)
 */
	public class TweenGroup
	{
		// -------------------------------------------------------------------------
		// TweenGroup Implementation
		// -------------------------------------------------------------------------

		public List<Tween> Tweens;

		/**
		 * Creates a new TweenGroup.
		 */
		public TweenGroup() {
			Tweens = new List<Tween>(10);
		}

		// -------------------------------------------------------------------------
		// API
		// -------------------------------------------------------------------------

		/**
		 * Adds the given tweens to the group. Please note that the internal storage
		 * is cleared at the beginning of the operation.
		 * @param tweens Some tweens to group.
		 * @return The group, for instruction chaining.
		 */
		public TweenGroup Pack(Tween[] tweens) {
			this.Tweens.Clear();
			for (int i=0; i<tweens.Length; i++)
				this.Tweens.Add(tweens[i]);
			return this;
		}

		/**
		 * Modifies the delays of every tween in the group in order to sequence
		 * them one after the other.
		 * @return The group, for instruction chaining.
		 */
		public TweenGroup Sequence() {
			for (int i=1; i<Tweens.Count; i++) {
				Tween tween = Tweens[i];
				Tween previousTween = Tweens[i-1];
				tween.Delay(previousTween.GetDuration() + previousTween.GetDelay());
			}
			return this;
		}

		/**
		 * Starts every tween in the group.
		 * @return The group, for instruction chaining.
		 */
		public TweenGroup Start() {
			for (int i=0; i<Tweens.Count; i++) {
				Tweens[i].Start();
			}
			return this;
		}

		/**
		 * Convenience method to add a delay to every tween in the group.
		 * @param millis A delay, in milliseconds.
		 * @return The group, for instruction chaining.
		 */
		public TweenGroup Delay(int millis) {
			for (int i=0; i<Tweens.Count; i++) {
				Tween tween = Tweens[i];
				tween.Delay(millis);
			}
			return this;
		}

		/**
		 * Repeats the tween group for a given number of times. For infinity
		 * repeats,use Tween.INFINITY.
		 * @param count The number of repetitions.
		 * @param delayMillis A delay, in milliseconds, before every repetition.
		 * @return The group, for instruction chaining.
		 */
		public TweenGroup Repeat(int count, int delayMillis) {
			int totalDuration = ComputeDuration();
			for (int i=0; i<Tweens.Count; i++) {
				Tween tween = Tweens[i];
				int delay = totalDuration + delayMillis - (tween.GetDuration() + tween.GetDelay());
				tween.Repeat(count, delay);
			}
			return this;
		}

		// -------------------------------------------------------------------------
		// Private methods
		// -------------------------------------------------------------------------

		private int ComputeDuration() {
			int duration = 0;
			for (int i=0; i<Tweens.Count; i++) {
				Tween tween = Tweens[i];
				duration = Math.Max(duration, tween.GetDelay() + tween.GetDuration());
			}
			return duration;
		}
	}
}

