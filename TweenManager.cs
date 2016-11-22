using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TweenEngine
{
	/**
 * A TweenManager let you pack many tweens together, and update them at once.
 * Its main interest is that it handles the pooling complexity for you if you
 * decided to enable object pooling using "Tween.setPoolEnabled()".
 *
 * <br/><br/>
 * Just add a bunch of tweens or tween groups to it and call update()
 * periodically.
 *
 * @see Tween
 * @see TweenGroup
 * @author Aurelien Ribon (aurelien.ribon@gmail.com)
 */
	public class TweenManager
	{
		// -------------------------------------------------------------------------
		// Implementation
		// -------------------------------------------------------------------------

		private readonly List<Tween> _tweens;

		/**
		 * Instantiates a new manager.
		 */
		public TweenManager() {
			this._tweens = new List<Tween>(20);
		}

		// -------------------------------------------------------------------------
		// API
		// -------------------------------------------------------------------------

		/**
		 * Adds a new tween to the manager.
		 * @param tween A tween. Does nothing if it is already present.
		 * @return The manager, for instruction chaining.
		 */
		public TweenManager Add(Tween tween) {
			_tweens.Add(tween);
			return this;
		}

		/**
		 * Adds every tween from a tween group to the manager. Note that the group
		 * will be cleared from its tweens, as says its specification. This is a
		 * mandatory operation for a better management of the memory.
		 * @param tweenGroup A tween group.
		 * @return The manager, for instruction chaining.
		 */
		public TweenManager Add(TweenGroup tweenGroup) {
			while (tweenGroup.Tweens.Count > 0)
				Add(tweenGroup.Tweens[0]);
				tweenGroup.Tweens.RemoveAt(0);
			return this;
		}

		/**
		 * Clears the manager from every tween.
		 */
		public void Clear() {
			_tweens.Clear();
		}

		/**
		 * Returns true if the manager contains any valid tween associated to the
		 * given target.
		 */
		public bool Contains(ITweenable target) {
			for (int i=0; i<_tweens.Count; i++) {
				Tween tween = _tweens[i];
				if (tween.GetTarget() == target && !tween.GetFinished())
					return true;
			}
			return false;
		}

		/**
		 * Returns true if the manager contains any valid tween associated to the
		 * given target and tween type.
		 */
		public bool Contains(ITweenable target, PositionType tweenType) {
			for (int i=0; i<_tweens.Count; i++) {
				Tween tween = _tweens[i];
				if (tween.GetTarget() == target && tween.GetTweenType() == tweenType && !tween.GetFinished())
					return true;
			}
			return false;
		}

		/**
		 * Kills every valid tween associated to the given target.
		 */
		public void Kill(ITweenable target) {
			for (int i=0; i<_tweens.Count; i++) {
				Tween tween = _tweens[i];
				if (tween.GetTarget() == target && !tween.GetFinished())
					tween.Kill();
			}
		}

		/**
		 * Kills every valid tween associated to the given target and tween type.
		 */
		public void Kill(ITweenable target, PositionType tweenType) {
			for (int i=0; i<_tweens.Count; i++) {
				Tween tween = _tweens[i];
				if (tween.GetTarget() == target && tween.GetTweenType() == tweenType && !tween.GetFinished())
					tween.Kill();
			}
		}

		/**
		 * Gets the number of tweens managed by this manager.
		 * @return The number of tweens in the manager.
		 */
		public int GetTweenCount() {
			return _tweens.Count;
		}

		/**
		 * Gets an array containing every tween in the manager.
		 * <b>Warning:</b> this method allocates an array.
		 */
		public Tween[] GetTweens() {
			return _tweens.ToArray();
		}

		/**
		 * Gets an array containing every tween in the manager dedicated to the
		 * given target.
		 * <b>Warning:</b> this method allocates an ArrayList and an array.
		 */
		public Tween[] GetTweens(ITweenable target) {
			List<Tween> selectedTweens = new List<Tween>();
			for (int i=0; i<_tweens.Count; i++) {
				Tween tween = _tweens[i];
				if (tween.GetTarget() == target && !tween.GetFinished())
					selectedTweens.Add(tween);
			}
			return selectedTweens.ToArray();
		}

		/**
		 * Gets an array containing every tween in the manager dedicated to the
		 * given target and tween type.
		 * <b>Warning:</b> this method allocates an ArrayList and an array.
		 */
		public Tween[] GetTweens(ITweenable target, PositionType tweenType) {
			List<Tween> selectedTweens = new List<Tween>();
			for (int i=0; i<_tweens.Count; i++) {
				Tween tween = _tweens[i];
				if (tween.GetTarget() == target && tween.GetTweenType() == tweenType && !tween.GetFinished())
					selectedTweens.Add(tween);
			}
			return selectedTweens.ToArray();
		}

		/**
		 * Updates every tween with the current time. Handles the tween life-cycle
		 * automatically. If a tween is finished, it will be removed from the
		 * manager.
		 */
		public void Update() {
			long currentMillis = DateTime.Now.Millisecond;

			for (int i=0; i<_tweens.Count; i++) {
				Tween tween = _tweens[i];
				if (tween.GetFinished()) {
					_tweens.RemoveAt(i);
					i -= 1;
				}
				tween.Update(currentMillis);
			}
	}	}
}

