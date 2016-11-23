# TweenEngine-CSharp

This is a port of Aurelien Ribon amazing Universal Tween Engine to C#
I did the port back in 2012 and it has not been maintained. I found the Universal Tween Engine to be very useful in my Android games. 
So when I started picking up C# again I decided to port/recreate it just in case I decided to make more games.

## Original Code

* [Java Code](https://code.google.com/archive/p/java-universal-tween-engine/downloads) - You can find the original code here
* [Author's Blog](http://www.aurelienribon.com/blog/projects/universal-tween-engine/) - The author's blog page about the code here

### Examples

You can find these examples in the /examples folder


The following example will move the target horizontal position from its current value to x = 200 and y = 300, during 500ms, but only after a delay of
1000ms.The transition will also be repeated 2 times(the starting position is registered at the end of the delay, so the animation will automatically
restart from this registered position).
```
Tween.To(myObject, PositionType.XY, 500, Quad.Inout).SetTarget(200).Delay(1000).Repeat(2, 0).Start();
```

You need to set the target values of the interpolation by using one of the ".SetTarget()" methods.The interpolation will run from the current values(retrieved after the delay, if any) to these target values.
```
Tween.From(myObject, PositionType.XY, 500, Quad.Inout).SetTarget(200).Delay(1000).Repeat(2, 0).Start();
```

The following example will move the target horizontal position from its current location to x = 200, then from x = 200 to x = 100, and finally from
x = 100 to x = 200, but this last transition will only occur 1000ms after the previous one. Notice the ".sequence()" method call, if it has not been called, the 3 tweens would have started together, and not one after the other.
```
new TweenGroup().Pack(new Tween[]
	{
		Tween.To(myObject, PositionType.X, 500, Quad.Inout).SetTarget(200),
		Tween.To(myObject, PositionType.X, 500, Quad.Inout).SetTarget(100),
		Tween.To(myObject, PositionType.X, 500, Quad.Inout).SetTarget(200).Delay(1000)
	}
).Sequence().Start();
```
