using UnityEngine;
using System;
using System.Collections;

// from: http://www.robertpenner.com/easing/

// Removed animation stuff - not needed for Unity.
// Fixed documentation erroneously calling param "c" Final value. If c == 0 then fuck all happens. Therefore:
// c is not the final value and the person who wrote that it is, happens to be an asshole that wasted me an hour
// figuring out why

/// <summary>
/// Animates the change in value of a double property using 
/// Robert Penner's easing equations for interpolation over a specified duration.
/// </summary>
public class CurveUtil{

  /// <summary>
  /// Linear change in value.
  /// Useful when assigning delegates to control animation. 
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double Linear( double t, double b, double c, double d )
  {
    return c * t / d + b;
  }
  
  /// <summary>
  /// Easing equation function for an exponential (2^t) easing out: 
  /// decelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double ExpoEaseOut( double t, double b, double c, double d )
  {
    return ( t == d ) ? b + c : c * ( -Math.Pow( 2, -10 * t / d ) + 1 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for an exponential (2^t) easing in: 
  /// accelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double ExpoEaseIn( double t, double b, double c, double d )
  {
    return ( t == 0 ) ? b : c * Math.Pow( 2, 10 * ( t / d - 1 ) ) + b;
  }
  
  /// <summary>
  /// Easing equation function for an exponential (2^t) easing in/out: 
  /// acceleration until halfway, then deceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double ExpoEaseInOut( double t, double b, double c, double d )
  {
    if ( t == 0 )
      return b;
    
    if ( t == d )
      return b + c;
    
    if ( ( t /= d / 2 ) < 1 )
      return c / 2 * Math.Pow( 2, 10 * ( t - 1 ) ) + b;
    
    return c / 2 * ( -Math.Pow( 2, -10 * --t ) + 2 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for an exponential (2^t) easing out/in: 
  /// deceleration until halfway, then acceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double ExpoEaseOutIn( double t, double b, double c, double d )
  {
    if ( t < d / 2 )
      return ExpoEaseOut( t * 2, b, c / 2, d );
    
    return ExpoEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
  }
  
  /// <summary>
  /// Easing equation function for a circular (sqrt(1-t^2)) easing out: 
  /// decelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double CircEaseOut( double t, double b, double c, double d )
  {
    return c * Math.Sqrt( 1 - ( t = t / d - 1 ) * t ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a circular (sqrt(1-t^2)) easing in: 
  /// accelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double CircEaseIn( double t, double b, double c, double d )
  {
    return -c * ( Math.Sqrt( 1 - ( t /= d ) * t ) - 1 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a circular (sqrt(1-t^2)) easing in/out: 
  /// acceleration until halfway, then deceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double CircEaseInOut( double t, double b, double c, double d )
  {
    if ( ( t /= d / 2 ) < 1 )
      return -c / 2 * ( Math.Sqrt( 1 - t * t ) - 1 ) + b;
    
    return c / 2 * ( Math.Sqrt( 1 - ( t -= 2 ) * t ) + 1 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a circular (sqrt(1-t^2)) easing in/out: 
  /// acceleration until halfway, then deceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double CircEaseOutIn( double t, double b, double c, double d )
  {
    if ( t < d / 2 )
      return CircEaseOut( t * 2, b, c / 2, d );
    
    return CircEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
  }
  
  /// <summary>
  /// Easing equation function for a quadratic (t^2) easing out: 
  /// decelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double QuadEaseOut( double t, double b, double c, double d )
  {
    return -c * ( t /= d ) * ( t - 2 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a quadratic (t^2) easing in: 
  /// accelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double QuadEaseIn( double t, double b, double c, double d )
  {
    return c * ( t /= d ) * t + b;
  }
  
  /// <summary>
  /// Easing equation function for a quadratic (t^2) easing in/out: 
  /// acceleration until halfway, then deceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double QuadEaseInOut( double t, double b, double c, double d )
  {
    if ( ( t /= d / 2 ) < 1 )
      return c / 2 * t * t + b;
    
    return -c / 2 * ( ( --t ) * ( t - 2 ) - 1 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a quadratic (t^2) easing out/in: 
  /// deceleration until halfway, then acceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double QuadEaseOutIn( double t, double b, double c, double d )
  {
    if ( t < d / 2 )
      return QuadEaseOut( t * 2, b, c / 2, d );
    
    return QuadEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
  }
  
  /// <summary>
  /// Easing equation function for a sinusoidal (sin(t)) easing out: 
  /// decelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double SineEaseOut( double t, double b, double c, double d )
  {
    return c * Math.Sin( t / d * ( Math.PI / 2 ) ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a sinusoidal (sin(t)) easing in: 
  /// accelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double SineEaseIn( double t, double b, double c, double d )
  {
    return -c * Math.Cos( t / d * ( Math.PI / 2 ) ) + c + b;
  }
  
  /// <summary>
  /// Easing equation function for a sinusoidal (sin(t)) easing in/out: 
  /// acceleration until halfway, then deceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double SineEaseInOut( double t, double b, double c, double d )
  {
    if ( ( t /= d / 2 ) < 1 )
      return c / 2 * ( Math.Sin( Math.PI * t / 2 ) ) + b;
    
    return -c / 2 * ( Math.Cos( Math.PI * --t / 2 ) - 2 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a sinusoidal (sin(t)) easing in/out: 
  /// deceleration until halfway, then acceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double SineEaseOutIn( double t, double b, double c, double d )
  {
    if ( t < d / 2 )
      return SineEaseOut( t * 2, b, c / 2, d );
    
    return SineEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
  }
  
  /// <summary>
  /// Easing equation function for a cubic (t^3) easing out: 
  /// decelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double CubicEaseOut( double t, double b, double c, double d )
  {
    return c * ( ( t = t / d - 1 ) * t * t + 1 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a cubic (t^3) easing in: 
  /// accelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double CubicEaseIn( double t, double b, double c, double d )
  {
    return c * ( t /= d ) * t * t + b;
  }
  
  /// <summary>
  /// Easing equation function for a cubic (t^3) easing in/out: 
  /// acceleration until halfway, then deceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double CubicEaseInOut( double t, double b, double c, double d )
  {
    if ( ( t /= d / 2 ) < 1 )
      return c / 2 * t * t * t + b;
    
    return c / 2 * ( ( t -= 2 ) * t * t + 2 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a cubic (t^3) easing out/in: 
  /// deceleration until halfway, then acceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double CubicEaseOutIn( double t, double b, double c, double d )
  {
    if ( t < d / 2 )
      return CubicEaseOut( t * 2, b, c / 2, d );
    
    return CubicEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
  }
  
  /// <summary>
  /// Easing equation function for a quartic (t^4) easing out: 
  /// decelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double QuartEaseOut( double t, double b, double c, double d )
  {
    return -c * ( ( t = t / d - 1 ) * t * t * t - 1 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a quartic (t^4) easing in: 
  /// accelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double QuartEaseIn( double t, double b, double c, double d )
  {
    return c * ( t /= d ) * t * t * t + b;
  }
  
  /// <summary>
  /// Easing equation function for a quartic (t^4) easing in/out: 
  /// acceleration until halfway, then deceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double QuartEaseInOut( double t, double b, double c, double d )
  {
    if ( ( t /= d / 2 ) < 1 )
      return c / 2 * t * t * t * t + b;
    
    return -c / 2 * ( ( t -= 2 ) * t * t * t - 2 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a quartic (t^4) easing out/in: 
  /// deceleration until halfway, then acceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double QuartEaseOutIn( double t, double b, double c, double d )
  {
    if ( t < d / 2 )
      return QuartEaseOut( t * 2, b, c / 2, d );
    
    return QuartEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
  }
  
  /// <summary>
  /// Easing equation function for a quintic (t^5) easing out: 
  /// decelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double QuintEaseOut( double t, double b, double c, double d )
  {
    return c * ( ( t = t / d - 1 ) * t * t * t * t + 1 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a quintic (t^5) easing in: 
  /// accelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double QuintEaseIn( double t, double b, double c, double d )
  {
    return c * ( t /= d ) * t * t * t * t + b;
  }
  
  /// <summary>
  /// Easing equation function for a quintic (t^5) easing in/out: 
  /// acceleration until halfway, then deceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double QuintEaseInOut( double t, double b, double c, double d )
  {
    if ( ( t /= d / 2 ) < 1 )
      return c / 2 * t * t * t * t * t + b;
    return c / 2 * ( ( t -= 2 ) * t * t * t * t + 2 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a quintic (t^5) easing in/out: 
  /// acceleration until halfway, then deceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double QuintEaseOutIn( double t, double b, double c, double d )
  {
    if ( t < d / 2 )
      return QuintEaseOut( t * 2, b, c / 2, d );
    return QuintEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
  }
  
  /// <summary>
  /// Easing equation function for an elastic (exponentially decaying sine wave) easing out: 
  /// decelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double ElasticEaseOut( double t, double b, double c, double d )
  {
    if ( ( t /= d ) == 1 )
      return b + c;
    
    double p = d * .3;
    double s = p / 4;
    
    return ( c * Math.Pow( 2, -10 * t ) * Math.Sin( ( t * d - s ) * ( 2 * Math.PI ) / p ) + c + b );
  }
  
  /// <summary>
  /// Easing equation function for an elastic (exponentially decaying sine wave) easing in: 
  /// accelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double ElasticEaseIn( double t, double b, double c, double d )
  {
    if ( ( t /= d ) == 1 )
      return b + c;
    
    double p = d * .3;
    double s = p / 4;
    
    return -( c * Math.Pow( 2, 10 * ( t -= 1 ) ) * Math.Sin( ( t * d - s ) * ( 2 * Math.PI ) / p ) ) + b;
  }
  
  /// <summary>
  /// Easing equation function for an elastic (exponentially decaying sine wave) easing in/out: 
  /// acceleration until halfway, then deceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double ElasticEaseInOut( double t, double b, double c, double d )
  {
    if ( ( t /= d / 2 ) == 2 )
      return b + c;
    
    double p = d * ( .3 * 1.5 );
    double s = p / 4;
    
    if ( t < 1 )
      return -.5 * ( c * Math.Pow( 2, 10 * ( t -= 1 ) ) * Math.Sin( ( t * d - s ) * ( 2 * Math.PI ) / p ) ) + b;
    return c * Math.Pow( 2, -10 * ( t -= 1 ) ) * Math.Sin( ( t * d - s ) * ( 2 * Math.PI ) / p ) * .5 + c + b;
  }
  
  /// <summary>
  /// Easing equation function for an elastic (exponentially decaying sine wave) easing out/in: 
  /// deceleration until halfway, then acceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double ElasticEaseOutIn( double t, double b, double c, double d )
  {
    if ( t < d / 2 )
      return ElasticEaseOut( t * 2, b, c / 2, d );
    return ElasticEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
  }
  
  /// <summary>
  /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out: 
  /// decelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double BounceEaseOut( double t, double b, double c, double d )
  {
    if ( ( t /= d ) < ( 1 / 2.75 ) )
      return c * ( 7.5625 * t * t ) + b;
    else if ( t < ( 2 / 2.75 ) )
      return c * ( 7.5625 * ( t -= ( 1.5 / 2.75 ) ) * t + .75 ) + b;
    else if ( t < ( 2.5 / 2.75 ) )
      return c * ( 7.5625 * ( t -= ( 2.25 / 2.75 ) ) * t + .9375 ) + b;
    else
      return c * ( 7.5625 * ( t -= ( 2.625 / 2.75 ) ) * t + .984375 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in: 
  /// accelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double BounceEaseIn( double t, double b, double c, double d )
  {
    return c - BounceEaseOut( d - t, 0, c, d ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out: 
  /// acceleration until halfway, then deceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double BounceEaseInOut( double t, double b, double c, double d )
  {
    if ( t < d / 2 )
      return BounceEaseIn( t * 2, 0, c, d ) * .5 + b;
    else
      return BounceEaseOut( t * 2 - d, 0, c, d ) * .5 + c * .5 + b;
  }
  
  /// <summary>
  /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out/in: 
  /// deceleration until halfway, then acceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double BounceEaseOutIn( double t, double b, double c, double d )
  {
    if ( t < d / 2 )
      return BounceEaseOut( t * 2, b, c / 2, d );
    return BounceEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
  }
  
  /// <summary>
  /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out: 
  /// decelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double BackEaseOut( double t, double b, double c, double d )
  {
    return c * ( ( t = t / d - 1 ) * t * ( ( 1.70158 + 1 ) * t + 1.70158 ) + 1 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in: 
  /// accelerating from zero velocity.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double BackEaseIn( double t, double b, double c, double d )
  {
    return c * ( t /= d ) * t * ( ( 1.70158 + 1 ) * t - 1.70158 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in/out: 
  /// acceleration until halfway, then deceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double BackEaseInOut( double t, double b, double c, double d )
  {
    double s = 1.70158;
    if ( ( t /= d / 2 ) < 1 )
      return c / 2 * ( t * t * ( ( ( s *= ( 1.525 ) ) + 1 ) * t - s ) ) + b;
    return c / 2 * ( ( t -= 2 ) * t * ( ( ( s *= ( 1.525 ) ) + 1 ) * t + s ) + 2 ) + b;
  }
  
  /// <summary>
  /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out/in: 
  /// deceleration until halfway, then acceleration.
  /// </summary>
  /// <param name="t">Current time in seconds.</param>
  /// <param name="b">Starting value.</param>
  /// <param name="c">Change in value.</param>
  /// <param name="d">Duration of animation.</param>
  /// <returns>The correct value.</returns>
  public static double BackEaseOutIn( double t, double b, double c, double d )
  {
    if ( t < d / 2 )
      return BackEaseOut( t * 2, b, c / 2, d );
    return BackEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
  }

  public static float getPolynomialFollowerDisplacement(float fTotalDisplacement, float fTotalTime, float fCurrentTime){
    float ftb =fCurrentTime/fTotalTime;
    float p1 =ftb*ftb*ftb*(10.0f*fTotalDisplacement);
    float p2 =ftb*ftb*ftb*ftb*(15.0f*fTotalDisplacement);
    float p3 =ftb*ftb*ftb*ftb*ftb*(6.0f*fTotalDisplacement);

    return p1-p2+p3;
  }

  public static float LinearToDecibel(float linear){
    float dB =-144.0f;

    if (linear != 0){
       dB = 20.0f * Mathf.Log10(linear);
    }else{
       dB = -144.0f;
    }

    return dB;
  }

}

public delegate double CurveDelegate(double t, double b, double c, double d);
public interface IAnimation{
  void initAnimation(CurveDelegate curveDelegate, float startDelay, float duration, GameObject[] target_sr, bool autoDismiss =true);
  void updateAnimation(float deltaTime);
  bool isAnimationDone();
  bool isAutoDismiss();

  void restartAnimation();
}

public class BasicAnimation{
  protected float mTimeElapsedCounter =0.0f;
  protected float mStartDelay =0.0f;
  protected float mDuration =0.0f;
  protected CurveDelegate mCurveFunc =null;
  protected bool mAutoDismiss =false;
  protected GameObject[] mTargetGO =null;
  protected bool mInited =false;
  protected bool mExtraInfoInited =false;
  protected bool mAnimationDone =false;

  public void initAnimation(CurveDelegate curveDelegate, float startDelay, float duration, GameObject[] target_sr, bool autoDismiss =true){
    mCurveFunc =curveDelegate;
    mStartDelay =startDelay;
    mDuration =duration;
    mTargetGO =target_sr;
    mAutoDismiss =autoDismiss;

    mInited =true;
  }

  public bool isAnimationDone(){
    if (mInited==false || mExtraInfoInited==false)
      return false;
    return (mAnimationDone);
  }

  public bool isAutoDismiss(){
    return mAutoDismiss;
  }

  public void restartAnimation(){
    mTimeElapsedCounter =0.0f;
    mAnimationDone =false;
  }
}

public class PositionAnimation : BasicAnimation, IAnimation{

  Vector3 mStartPt =Vector3.zero;
  Vector3 mEndPt =Vector3.zero;

  public void setupPositionVal(Vector3 startPt, Vector3 endPt){
    mStartPt =startPt;
    mEndPt =endPt;

    for (int i=0;i<mTargetGO.Length;++i)
      mTargetGO[i].gameObject.transform.localPosition =mStartPt;
        
    mExtraInfoInited =true;
  }

  public void updateAnimation(float deltaTime){
    if (mAnimationDone==true)
      return;

    mTimeElapsedCounter+=deltaTime;
    if (mTimeElapsedCounter<mStartDelay){
      for (int i=0;i<mTargetGO.Length;++i)
        mTargetGO[i].gameObject.transform.localPosition =mStartPt;
      return;
    }

    if (mTimeElapsedCounter>(mStartDelay+mDuration)){
      for (int i=0;i<mTargetGO.Length;++i)
        mTargetGO[i].gameObject.transform.localPosition =mEndPt;

      mAnimationDone =true;
      return;
    }

    float intepolatedScale =(float)mCurveFunc(mTimeElapsedCounter-mStartDelay, 0.0f, 1.0f, mDuration);
    Vector3 norm =(mEndPt-mStartPt).normalized;
    Vector3 curr =mStartPt+norm*intepolatedScale*(mEndPt-mStartPt).magnitude;

    //setup curr
    for (int i=0;i<mTargetGO.Length;++i)
      mTargetGO[i].gameObject.transform.localPosition =curr;
  }
}

public class ScaleAnimation : BasicAnimation, IAnimation{

  Vector3 mStartScale =Vector3.zero;
  Vector3 mEndScale =Vector3.zero;

  public void setupScaleVal(Vector3 startScale, Vector3 endScale){
    mStartScale =startScale;
    mEndScale =endScale;

    for (int i=0;i<mTargetGO.Length;++i)
      mTargetGO[i].gameObject.transform.localScale =mStartScale;

    mExtraInfoInited =true;
  }

  public void updateAnimation(float deltaTime){
    if (mAnimationDone==true)
      return;

    mTimeElapsedCounter+=deltaTime;
    if (mTimeElapsedCounter<mStartDelay){
      for (int i=0;i<mTargetGO.Length;++i)
        mTargetGO[i].gameObject.transform.localScale =mStartScale;
      return;
    }

    if (mTimeElapsedCounter>(mStartDelay+mDuration)){
      for (int i=0;i<mTargetGO.Length;++i)
        mTargetGO[i].gameObject.transform.localScale =mEndScale;

      mAnimationDone =true;
      return;
    }

    float intepolatedScale =(float)mCurveFunc(mTimeElapsedCounter-mStartDelay, 0.0f, 1.0f, mDuration);
    Vector3 norm =(mEndScale-mStartScale).normalized;
    Vector3 curr =mStartScale+norm*intepolatedScale*(mEndScale-mStartScale).magnitude;

    //setup curr
    for (int i=0;i<mTargetGO.Length;++i)
      mTargetGO[i].gameObject.transform.localScale =curr;
  }
}

public class QuaternionAnimation : BasicAnimation, IAnimation{
  Vector3 mStartQ =Vector3.zero;
  Vector3 mEndQ =Vector3.zero;

  public void setupQuaternionVal(Vector3 startQ, Vector3 endQ){
    mStartQ =startQ;
    mEndQ =endQ;

    for (int i=0;i<mTargetGO.Length;++i)
      mTargetGO[i].gameObject.transform.localRotation =Quaternion.Euler(mStartQ.x, mStartQ.y, mStartQ.z);

    mExtraInfoInited =true;
  }

  public void updateAnimation(float deltaTime){
    if (mAnimationDone==true)
      return;

    mTimeElapsedCounter+=deltaTime;
    if (mTimeElapsedCounter<mStartDelay){
      for (int i=0;i<mTargetGO.Length;++i)
        mTargetGO[i].gameObject.transform.localRotation =Quaternion.Euler(mStartQ.x, mStartQ.y, mStartQ.z);
      return;
    }

    if (mTimeElapsedCounter>(mStartDelay+mDuration)){
      for (int i=0;i<mTargetGO.Length;++i)
        mTargetGO[i].gameObject.transform.localRotation =Quaternion.Euler(mEndQ.x, mEndQ.y, mEndQ.z);

      mAnimationDone =true;
      return;
    }

    float intepolatedScale =(float)mCurveFunc(mTimeElapsedCounter-mStartDelay, 0.0f, 1.0f, mDuration);
    // Quaternion curr =Quaternion.Slerp(mStartQ, mEndQ, intepolatedScale);
    Vector3 norm =(mEndQ-mStartQ).normalized;
    Vector3 currVec =mStartQ+norm*intepolatedScale*(mEndQ-mStartQ).magnitude;
    Quaternion curr =Quaternion.Euler(currVec.x, currVec.y, currVec.z);

    //setup curr
    for (int i=0;i<mTargetGO.Length;++i)
      mTargetGO[i].gameObject.transform.localRotation =curr;
  }
}

public class ColorAnimation : BasicAnimation, IAnimation{
  Color mStartC;
  Color mEndC;
  SpriteRenderer[] mTargetSpriteRenderer =null;

  public void initAnimation(CurveDelegate curveDelegate, float startDelay, float duration, SpriteRenderer[] target_sr, bool autoDismiss =true){
    mCurveFunc =curveDelegate;
    mStartDelay =startDelay;
    mDuration =duration;
    mTargetSpriteRenderer =target_sr;
    mAutoDismiss =autoDismiss;

    mInited =true;
  }

  public void setupColorVal(Color startC, Color endC){
    mStartC =startC;
    mEndC =endC;

    for (int i=0;i<mTargetSpriteRenderer.Length;++i)
      mTargetSpriteRenderer[i].color =mStartC;

    mExtraInfoInited =true;
  }

  public void updateAnimation(float deltaTime){
    if (mAnimationDone==true)
      return;

    mTimeElapsedCounter+=deltaTime;
    if (mTimeElapsedCounter<mStartDelay){
      for (int i=0;i<mTargetSpriteRenderer.Length;++i)
        mTargetSpriteRenderer[i].color =mStartC;
      return;
    }

    if (mTimeElapsedCounter>(mStartDelay+mDuration)){
      for (int i=0;i<mTargetSpriteRenderer.Length;++i)
        mTargetSpriteRenderer[i].color =mEndC;

      mAnimationDone =true;
      return;
    }

    float intepolatedScale =(float)mCurveFunc(mTimeElapsedCounter-mStartDelay, 0.0f, 1.0f, mDuration);

    Vector3 endVec =new Vector3(mEndC.r, mEndC.g, mEndC.b);
    Vector3 startVec =new Vector3(mStartC.r, mStartC.g, mStartC.b);

    Vector3 norm =(endVec-startVec).normalized;
    Vector3 curr =startVec+norm*intepolatedScale*(endVec-startVec).magnitude;

    float alpha =mStartC.a+(intepolatedScale*(mEndC.a-mStartC.a));

    //setup curr
    for (int i=0;i<mTargetSpriteRenderer.Length;++i)
      mTargetSpriteRenderer[i].color =new Color(curr.x, curr.y, curr.z, alpha);
  }
}
