using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TweenEngine.examples
{
    public class MovableObject
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
    /*
     * The common way to create a Tween is by using one of the static constructor,
     * like:
     *
     * -- Tween.to(...);<br/>
     * -- Tween.from(...);<br/>
     * -- Tween.set(...);<br/>
     * -- Tween.call(...);
     */
    public class Program
    {
        public static void Main(string[] args)
        {
            var myObject = new TweenableParticule(new Particule());
            /*
            * The following example will move the target horizontal position from its
            * current value to x = 200 and y = 300, during 500ms, but only after a delay of
            * 1000ms.The transition will also be repeated 2 times(the starting position
            * is registered at the end of the delay, so the animation will automatically
            * restart from this registered position).
            */
            Tween.To(myObject, PositionType.XY, 500, Quad.Inout).SetTarget(200).Delay(1000).Repeat(2, 0).Start();
            /*
            *You need to set the target values of the interpolation by using one
            * of the ".target()" methods.The interpolation will run from the current
            *values(retrieved after the delay, if any) to these target values.
            */
            Tween.From(myObject, PositionType.XY, 500, Quad.Inout).SetTarget(200).Delay(1000).Repeat(2, 0).Start();


            /*
             * The following example will move the target horizontal position from its
             * current location to x = 200, then from x = 200 to x = 100, and finally from
             * x = 100 to x = 200, but this last transition will only occur 1000ms after the
             * previous one.Notice the ".sequence()" method call, if it has not been
             * called, the 3 tweens would have started together, and not one after the
             * other.
             */
            new TweenGroup().Pack(new Tween[]
                {
                    Tween.To(myObject, PositionType.X, 500, Quad.Inout).SetTarget(200),
                    Tween.To(myObject, PositionType.X, 500, Quad.Inout).SetTarget(100),
                    Tween.To(myObject, PositionType.X, 500, Quad.Inout).SetTarget(200).Delay(1000)
                }
            ).Sequence().Start();
        }
    }
}