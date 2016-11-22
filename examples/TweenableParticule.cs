using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TweenEngine;



namespace TweenEngine.examples
{
 /*
 * 
 * The Tweenable interface lets you interpolate any attribute from any object.
 * Just implement it as you want and let the engine do the interpolation for
 * you.
 * 
 * The following code snippet presents an example of implementation for tweening
 * a Particule class. This Particule class is supposed to only define a position
 * with an "x" and an "y" field.
 * 
 * 
 * The implementation is done with the Composition Pattern. It allows us to let
 * the Particule class untouched (that way, even third-party classes can be
 * tweened !).
 *  * 
 */

    public class Particule
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class TweenableParticule : ITweenable
    {
      

        // The following lines define the different possible tween types.
        // It's up to you to define what you need :-)
        public static int X = 1;
        public static int Y = 2;
        public static int XY = 3;

        // Composition pattern
        private Particule target;

        // Constructor
        public TweenableParticule(Particule particule)
        {
            this.target = particule;
        }

        public int getTweenValues(PositionType tweenType, float[] returnValues)
        {
            switch (tweenType)
            {
                case PositionType.X:
                    returnValues[0] = target.X;
                    return 1;
                case PositionType.Y:
                    returnValues[0] = target.Y;
                    return 1;
                case PositionType.XY:
                    returnValues[0] = target.X;
                    returnValues[1] = target.Y;
                    return 2;
                default:
                    return 0;
            }
        }

        public void onTweenUpdated(PositionType tweenType, float[] newValues)
        {
            switch (tweenType)
            {
                case PositionType.X:
                    target.X = newValues[0];
                    break;
                case PositionType.Y:
                    target.Y = newValues[1];
                    break;
                case PositionType.XY:
                    target.X = newValues[0];
                    target.Y = newValues[1];
                    break;
            }
        }

        public int GetTweenValues(PositionType tweenType, float[] returnValues)
        {
            throw new NotImplementedException();
        }

        public void OnTweenUpdated(PositionType tweenType, float[] newValues)
        {
            throw new NotImplementedException();
        }
    }
}