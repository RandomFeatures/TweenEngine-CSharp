using System;
using System.Collections.Generic;

namespace TweenEngine
{
    /**
 * Core class of the Tween Engine. It contains many static factory methods to
 * create and instantiate new interpolations.
 *
 * @see Tweenable
 * @see TweenManager
 * @see TweenGroup
 * @author Aurelien Ribon (aurelien.ribon@gmail.com)
 */

    /*
     * This is a port of Aurelien Ribon amazing Universal Tween Engine to C#
     * I did the port back in 2012 and it has not been maintained. I found the Universal Tween Engine
     * to be very useful in my Android games. So when I started picking up C# again I decided to port/recreate it incase
     * I decided to make some more games.
     * You can find the original code here https://code.google.com/archive/p/java-universal-tween-engine/downloads
     * The author's blog page about the code here http://www.aurelienribon.com/blog/projects/universal-tween-engine/
     */

    public class Tween
{
    // -------------------------------------------------------------------------
    // Static
    // -------------------------------------------------------------------------

    /** If you need to repeat your tween for infinity, use this. */
    public static readonly int Infinity = -1;
        /** The maximum number of attributes that can be tweened in a single tween. */
        public static readonly int MaxCombinedTweens = 10;

        // -------------------------------------------------------------------------

        private static bool _isPoolEnabled;
        private static readonly PoolInstance Pool = new PoolInstance(20);
        private readonly List<ITweenCallback> _completeCallbacks;
        private readonly List<ITweenCallback> _endOfDelayCallbacks;
        private readonly List<ITweenCallback> _iterationCompleteCallbacks;
        private readonly List<ITweenCallback> _killCallbacks;

        // Misc
        private readonly float[] _localTmp = new float[MaxCombinedTweens];
        private readonly List<ITweenCallback> _poolCallbacks;

        // Callbacks
        private readonly List<ITweenCallback> _startCallbacks;
        private readonly float[] _startValues;
        private readonly float[] _targetMinusStartValues;
        private readonly float[] _targetValues;

        // Values
        private int _combinedTweenCount;
        private int _delayMillis;
        private int _durationMillis;
        private long _endDelayMillis;
        private long _endMillis;
        private long _endRepeatDelayMillis;
        private TweenEquation _equation;
        private bool _isDelayEnded;
        private bool _isEnded;
        private bool _isFinished;
        private bool _isInitialized;
        private bool _isPooled;

        // General
        private bool _isReversed;
        private bool _isStarted;
        private int _iteration;

        // Repeat
        private int _repeatCnt;
        private int _repeatDelayMillis;

        // Timings
        private long _startMillis;

        // -------------------------------------------------------------------------
        // Attributes
        // -------------------------------------------------------------------------

        // Main
        private ITweenable _target;
        private PositionType _tweenType;

        // UserData
        private object _userData;

        // -------------------------------------------------------------------------
        // Ctor
        // -------------------------------------------------------------------------

        /**
         * Instantiates a new Tween from scratch.
         * @param target The target of the interpolation.
         * @param tweenType The desired type of interpolation.
         * @param durationMillis The duration of the interpolation, in milliseconds.
         * @param equation The easing equation used during the interpolation.
         */
        public Tween(ITweenable target, PositionType tweenType, int durationMillis, TweenEquation equation)
        {
            _startValues = new float[MaxCombinedTweens];
            _targetValues = new float[MaxCombinedTweens];
            _targetMinusStartValues = new float[MaxCombinedTweens];

            _startCallbacks = new List<ITweenCallback>(3);
            _endOfDelayCallbacks = new List<ITweenCallback>(3);
            _iterationCompleteCallbacks = new List<ITweenCallback>(3);
            _completeCallbacks = new List<ITweenCallback>(3);
            _killCallbacks = new List<ITweenCallback>(3);
            _poolCallbacks = new List<ITweenCallback>(3);

            Reset();
            __build(target, tweenType, durationMillis, equation);
        }

        /**
         * Enables or disables the automatic reuse of ended tweens. Pooling prevents
         * the allocation of a new tween object when using the static constructors,
         * thus removing the need for garbage collection. Can be quite helpful on
         * slow or embedded devices.
         * <br/><br/>
         * Defaults to false.
         */
        public static void SetPoolEnabled(bool value)
        {
            _isPoolEnabled = value;
        }

        /**
         * Used for debug purpose. Gets the current number of objects that are
         * waiting in the pool.
         * @return The current size of the pool.
         */

        public static int GetPoolSize()
        {
            return Pool.Size();
        }

        /**
         * Clears every static resources and resets the static instance.
         */

        public static void Dispose()
        {
            _isPoolEnabled = false;
            Pool.Clear();
        }

        // -------------------------------------------------------------------------
        // Factories
        // -------------------------------------------------------------------------

        /**
         * Several options such as delays and callbacks can be added to the tween.
         * This method hides some of the internal optimizations such as object
         * reuse for convenience. However, you can control the creation of the
         * tween by using the classic constructor.
         * 
         * @param target The target of the interpolation.
         * @param tweenType The desired type of interpolation.
         * @param durationMillis The duration of the interpolation, in milliseconds.
         * @param equation The easing equation used during the interpolation.
         * @return The generated Tween.
         */
        public static Tween To(ITweenable target, PositionType tweenType, int durationMillis, TweenEquation equation)
        {
            var tween = Pool.Get();
            tween.Reset();
            tween.__build(target, tweenType, durationMillis, equation);
            tween._isPooled = _isPoolEnabled;
            return tween;
        }

        /**
         * @param target The target of the interpolation.
         * @param tweenType The desired type of interpolation.
         * @param durationMillis The duration of the interpolation, in milliseconds.
         * @param equation The easing equation used during the interpolation.
         * @return The generated Tween.
         */

        public static Tween From(ITweenable target, PositionType tweenType, int durationMillis, TweenEquation equation)
        {
            var tween = Pool.Get();
            tween.Reset();
            tween.__build(target, tweenType, durationMillis, equation);
            tween._isPooled = _isPoolEnabled;
            tween.Reverse();
            return tween;
        }

        /**
         * @param target The target of the interpolation.
         * @param tweenType The desired type of interpolation.
         * @return The generated Tween.
         */
        public static Tween Set(ITweenable target, PositionType tweenType)
        {
            var tween = Pool.Get();
            tween.Reset();
            tween.__build(target, tweenType, 0, null);
            tween._isPooled = _isPoolEnabled;
            return tween;
        }

        /**
         * @param callback The callback that will be triggered at the end of the
         * delay (if specified). A repeat behavior can be set to the tween to
         * trigger it more than once.
         * @return The generated Tween.
         */
        public static Tween Call(ITweenCallback callback)
        {
            var tween = Pool.Get();
            tween.Reset();
            tween.__build(null, PositionType.NONE, 0, null);
            tween.AddIterationCompleteCallback(callback);
            tween._isPooled = _isPoolEnabled;
            return tween;
        }

        /**
         * Starts or restart the interpolation.
         * @return The current tween for chaining instructions.
         */
        public Tween Start()
        {
            _startMillis = DateTime.Now.Millisecond;
            _endDelayMillis = _startMillis + _delayMillis;

            if ((_iteration > 0) && (_repeatDelayMillis < 0))
                _endDelayMillis = Math.Max(_endDelayMillis + _repeatDelayMillis, _startMillis);

            _endMillis = _endDelayMillis + _durationMillis;
            _endRepeatDelayMillis = Math.Max(_endMillis, _endMillis + _repeatDelayMillis);

            _isInitialized = true;
            _isStarted = true;
            _isDelayEnded = false;
            _isEnded = false;
            _isFinished = false;

            CallStartCallbacks();

            return this;
        }

        /**
         * Kills the interpolation. If pooling was enabled when this tween was
         * created, the tween will be freed, cleared, and returned to the pool. As
         * a result, you shouldn't use it anymore.
         */
        public void Kill()
        {
            _isFinished = true;
            CallKillCallbacks();
        }

        /**
         * Adds a delay to the tween.
         * @param millis The delay, in milliseconds.
         * @return The current tween for chaining instructions.
         */
        public Tween Delay(int millis)
        {
            _delayMillis += millis;
            return this;
        }

        /**
         * Sets the target value of the interpolation. If not reversed, the
         * interpolation will run from the current value to this target value.
         * @param targetValue The target value of the interpolation.
         * @return The current tween for chaining instructions.
         */
        public Tween SetTarget(float targetValue)
        {
            _targetValues[0] = targetValue;
            return this;
        }

        /**
         * Sets the target values of the interpolation. If not reversed, the
         * interpolation will run from the current values to these target values.
         * @param targetValue1 The 1st target value of the interpolation.
         * @param targetValue2 The 2nd target value of the interpolation.
         * @return The current tween for chaining instructions.
         */
        public Tween SetTarget(float targetValue1, float targetValue2)
        {
            _targetValues[0] = targetValue1;
            _targetValues[1] = targetValue2;
            return this;
        }

        /**
         * Sets the target values of the interpolation. If not reversed, the
         * interpolation will run from the current values to these target values.
         * @param targetValue1 The 1st target value of the interpolation.
         * @param targetValue2 The 2nd target value of the interpolation.
         * @param targetValue3 The 3rd target value of the interpolation.
         * @return The current tween for chaining instructions.
         */
        public Tween SetTarget(float targetValue1, float targetValue2, float targetValue3)
        {
            _targetValues[0] = targetValue1;
            _targetValues[1] = targetValue2;
            _targetValues[2] = targetValue3;
            return this;
        }

        /**
         * Sets the target values of the interpolation. If not reversed, the
         * interpolation will run from the current values to these target values.
         * <br/><br/>
         * The other methods are convenience to avoid the allocation of an array.
         * @param targetValues The target values of the interpolation.
         * @return The current tween for chaining instructions.
         */
        public Tween SetTarget(float[] targetValues)
        {
            if (targetValues.Length > MaxCombinedTweens)
                throw new Exception("You cannot set more than " + MaxCombinedTweens + " targets.");
            Array.Copy(targetValues, 0, _targetValues, 0, targetValues.Length);
            return this;
        }

        /**
         * Adds a callback to the tween.
         * The callback is triggered when start() is called on the tween.
         * @param callback A tween callback.
         * @return The current tween for chaining instructions.
         */
        public Tween AddStartCallback(ITweenCallback callback)
        {
            _startCallbacks.Add(callback);
            return this;
        }

        /**
         * Adds a callback to the tween.
         * The callback is triggered at the end of the delay.
         * @param callback A tween callback.
         * @return The current tween for chaining instructions.
         */
        public Tween AddEndOfDelayCallback(ITweenCallback callback)
        {
            _endOfDelayCallbacks.Add(callback);
            return this;
        }

        /**
         * Adds a callback to the tween.
         * The callback is triggered on each iteration ending. If no repeat
         * behavior was specified, this callback is similar to a Types.COMPLETE
         * callback.
         * @param callback A tween callback.
         * @return The current tween for chaining instructions.
         */
        public Tween AddIterationCompleteCallback(ITweenCallback callback)
        {
            _iterationCompleteCallbacks.Add(callback);
            return this;
        }

        /**
         * Adds a callback to the tween.
         * The callback is triggered at the end of the tween.
         * @param callback A tween callback.
         * @return The current tween for chaining instructions.
         */
        public Tween AddCompleteCallback(ITweenCallback callback)
        {
            _completeCallbacks.Add(callback);
            return this;
        }

        /**
         * Adds a callback to the tween.
         * The callback is triggered if the tween is manually killed.
         * @param callback A tween callback.
         * @return The current tween for chaining instructions.
         */
        public Tween AddKillCallback(ITweenCallback callback)
        {
            _killCallbacks.Add(callback);
            return this;
        }

        /**
         * Adds a callback to the tween.
         * The callback is triggered right before a tween is sent back to the pool.
         * @param callback A tween callback.
         * @return The current tween for chaining instructions.
         */
        public Tween AddPoolCallback(ITweenCallback callback)
        {
            _poolCallbacks.Add(callback);
            return this;
        }

        /**
         * Repeats the tween for a given number of times. 
         * @param count The number of desired repetition. For infinite repetition,
         * use Tween.INFINITY, or a negative number.
         * @param millis A delay before each repetition.
         * @return The current tween for chaining instructions.
         */
        public Tween Repeat(int count, int delayMillis)
        {
            _repeatCnt = count;
            _repeatDelayMillis = delayMillis;
            return this;
        }

        /**
         * Reverse the tween. Will interpolate from target values to the
         * current values if not already reversed.
         * @return The current tween for chaining instructions.
         */
        public Tween Reverse()
        {
            _isReversed = !_isReversed;
            return this;
        }

        /**
         * Sets an object attached to this tween. It can be useful in order to
         * retrieve some data from a TweenCallback.
         * @param data Any kind of object.
         * @return The current tween for chaining instructions.
         */

        public Tween SetUserData(object data)
        {
            _userData = data;
            return this;
        }

        /**
         * Convenience method to add a single tween to a manager and avoids the
         * verbose <i>myManager.add(Tween.to(....).delay(...).start());</i>.
         * This method only makes sense for single tweens. If you use a TweenGroup,
         * the addition to the manager is automatic.
         * @param manager A TweenManager.
         * @return The current tween for chaining instructions.
         */
        public Tween AddToManager(TweenManager manager)
        {
            manager.Add(this);
            return this;
        }

        /**
         * Gets the tween target.
         * @return The tween target.
         */
        public ITweenable GetTarget()
        {
            return _target;
        }

        /**
         * Gets the tween type.
         * @return The tween type.
         */
        public PositionType GetTweenType()
        {
            return _tweenType;
        }

        /**
         * Gets the tween easing equation.
         * @return The tween easing equation.
         */
        public TweenEquation GetEquation()
        {
            return _equation;
        }

        /**
         * Gets the tween target values.
         * @return The tween target values.
         */
        public float[] GetTargetValues()
        {
            return _targetValues;
        }

        /**
         * Gets the tween duration.
         * @return The tween duration.
         */
        public int GetDuration()
        {
            return _durationMillis;
        }

        /**
         * Gets the tween delay.
         * @return The tween delay.
         */
        public int GetDelay()
        {
            return _delayMillis;
        }

        /**
         * Gets the number of combined tweens.
         * @return The number of combined tweens.
         */
        public int GetCombinedTweenCount()
        {
            return _combinedTweenCount;
        }

        /**
         * Getsthe total number of repetitions.
         * @return The total number of repetitions.
         */
        public int GetRepeatCount()
        {
            return _repeatCnt;
        }

        /**
         * Gets the delay before each repetition.
         * @return The delay before each repetition.
         */
        public int GetRepeatDelay()
        {
            return _repeatDelayMillis;
        }

        /**
         * Gets the number of remaining iterations.
         * @return The number of remaining iterations.
         */
        public int GetRemainingIterationCount()
        {
            return _repeatCnt - _iteration;
        }

        /**
         * Returns true if the tween is finished (i.e. if the tween has reached
         * its end or has been killed). If this is the case and tween pooling is
         * enabled, the tween should no longer been used, since it will be reset
         * and returned to the pool.
         * @return True if the tween is finished.
         */
        public bool GetFinished()
        {
            return _isFinished;
        }

        /**
         * Gets the attached user data, or null if none.
         * @return The attached user data.
         */
        public object GetUserData()
        {
            return _userData;
        }

        /**
         * Updates the tween state. Using this method can be unsafe if tween
         * pooling was first enabled. <b>The recommanded behavior is to use a
         * TweenManager instead.</b>
         * @param currentMillis The current milliseconds. You would generally
         * want to use <i>System.currentMillis()</i> and pass the result to
         * every unsafeUpdate call.
         */
        public void UnsafeUpdate(long currentMillis)
        {
            Update(currentMillis);
        }

        // -------------------------------------------------------------------------
        // Update engine
        // -------------------------------------------------------------------------

        public void Update(long currentMillis)
        {
            // Is the tween valid ?
            CheckForValidity();

            // Are we started ?
            if (!_isStarted)
                return;

            // Shall we repeat ?
            if (CheckForRepetition(currentMillis))
                return;

            // Is the tween ended ?
            if (_isEnded)
                return;

            // Wait for the end of the delay then either grab the start or end
            // values if it is the first iteration, or restart from those values
            // if the animation is replaying.
            if (CheckForEndOfDelay(currentMillis))
                return;

            // Test for the end of the tween. If true, set the target values to
            // their final values (to avoid precision loss when moving fast), and
            // call the callbacks.
            if (CheckForEndOfTween(currentMillis))
                return;

            // New values computation
            UpdateTarget(currentMillis);
        }

        private bool CheckForValidity()
        {
            if (_isFinished && _isPooled && _isInitialized)
            {
                CallPoolCallbacks();
                Reset();
                Pool.Free(this);
                return true;
            }
            if (_isFinished)
            {
                return true;
            }
            return false;
        }

        private bool CheckForRepetition(long currentMillis)
        {
            if (ShouldRepeat() && (currentMillis >= _endRepeatDelayMillis))
            {
                _iteration += 1;
                Start();
                return true;
            }
            return false;
        }

        private bool CheckForEndOfDelay(long currentMillis)
        {
            if (!_isDelayEnded && (currentMillis >= _endDelayMillis))
            {
                _isDelayEnded = true;

                if ((_iteration > 0) && (_target != null))
                {
                    _target.OnTweenUpdated(_tweenType, _startValues);
                }
                else if (_target != null)
                {
                    _target.GetTweenValues(_tweenType, _startValues);
                    for (var i = 0; i < _combinedTweenCount; i++)
                        _targetMinusStartValues[i] = _targetValues[i] - _startValues[i];
                }

                CallDelayEndedCallbacks();
            }
            else if (!_isDelayEnded)
            {
                return true;
            }
            return false;
        }

        private bool CheckForEndOfTween(long currentMillis)
        {
            if (!_isEnded && (currentMillis >= _endMillis))
            {
                _isEnded = true;

                if (_target != null)
                {
                    for (var i = 0; i < _combinedTweenCount; i++)
                        _localTmp[i] = _startValues[i] + _targetMinusStartValues[i];
                    _target.OnTweenUpdated(_tweenType, _localTmp);
                }

                if (ShouldRepeat())
                {
                    CallIterationCompleteCallbacks();
                }
                else
                {
                    _isFinished = true;
                    CallIterationCompleteCallbacks();
                    CallCompleteCallbacks();
                }

                return true;
            }
            return false;
        }

        private void UpdateTarget(long currentMillis)
        {
            if (_target != null)
            {
                for (var i = 0; i < _combinedTweenCount; i++)
                    _localTmp[i] = _equation.Compute(
                        currentMillis - _endDelayMillis,
                        _isReversed ? _targetValues[i] : _startValues[i],
                        _isReversed ? -_targetMinusStartValues[i] : +_targetMinusStartValues[i],
                        _durationMillis);
                _target.OnTweenUpdated(_tweenType, _localTmp);
            }
        }

        // -------------------------------------------------------------------------
        // Expert features
        // -------------------------------------------------------------------------

        /**
         * <b>Advanced use.</b>
         * <br/>Rebuilds a tween from the current one. May be used if you want to
         * build your own pool system. You should call __reset() before.
         */

        public void __build(ITweenable target, PositionType tweenType, int durationMillis, TweenEquation equation)
        {
            Reset();

            _isInitialized = true;

            _target = target;
            _tweenType = tweenType;
            _durationMillis = durationMillis;
            _equation = equation;

            if (target != null)
            {
                _combinedTweenCount = target.GetTweenValues(tweenType, _localTmp);
                if ((_combinedTweenCount < 1) || (_combinedTweenCount > MaxCombinedTweens))
                    throw new Exception("Min combined tweens = 1, max = " + MaxCombinedTweens);
            }
        }

        // -------------------------------------------------------------------------
        // Helpers
        // -------------------------------------------------------------------------

        private void Reset()
        {
            _target = null;
            _tweenType = PositionType.NONE;
            _equation = null;

            _isReversed = false;
            _isInitialized = false;
            _isPooled = false;

            _combinedTweenCount = 0;

            _delayMillis = 0;
            _isStarted = false;
            _isDelayEnded = false;
            _isEnded = false;
            _isFinished = true;

            _completeCallbacks.Clear();
            _iterationCompleteCallbacks.Clear();
            _killCallbacks.Clear();
            _poolCallbacks.Clear();
            _startCallbacks.Clear();
            _endOfDelayCallbacks.Clear();

            _repeatCnt = 0;
            _iteration = 0;
            _repeatDelayMillis = 0;

            _userData = null;
        }

        private bool ShouldRepeat()
        {
            return (_repeatCnt < 0) || (_iteration < _repeatCnt);
        }

        private void CallStartCallbacks()
        {
            for (var i = _startCallbacks.Count - 1; i >= 0; i--)
                _startCallbacks[i].TweenEventOccured(TweenTypes.Start, this);
        }

        private void CallDelayEndedCallbacks()
        {
            for (var i = _endOfDelayCallbacks.Count - 1; i >= 0; i--)
                _endOfDelayCallbacks[i].TweenEventOccured(TweenTypes.EndOfDelay, this);
        }

        private void CallIterationCompleteCallbacks()
        {
            for (var i = _iterationCompleteCallbacks.Count - 1; i >= 0; i--)
                _iterationCompleteCallbacks[i].TweenEventOccured(TweenTypes.IterationComplete, this);
        }

        private void CallCompleteCallbacks()
        {
            for (var i = _completeCallbacks.Count - 1; i >= 0; i--)
                _completeCallbacks[i].TweenEventOccured(TweenTypes.Complete, this);
        }

        private void CallKillCallbacks()
        {
            for (var i = _killCallbacks.Count - 1; i >= 0; i--)
                _killCallbacks[i].TweenEventOccured(TweenTypes.Kill, this);
        }

        private void CallPoolCallbacks()
        {
            for (var i = _poolCallbacks.Count - 1; i >= 0; i--)
                _poolCallbacks[i].TweenEventOccured(TweenTypes.Pool, this);
        }

        private class PoolInstance : Pool<Tween>
        {
            public PoolInstance(int iCapacity) : base(iCapacity)
            {
            }

            protected override Tween GetNew()
            {
                return new Tween(null, PositionType.NONE, 0, null);
            }
        }
    }
}