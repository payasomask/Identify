using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class UtilityHelper
{
  public enum MazeCorner{
    LeftBottom = 0,
    RightBottom,
    LeftTop,
    RightTop,
    SZ
   }
  public static int Random(int min, int max)
  {
    UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
    return UnityEngine.Random.Range(min, max);
  }
  public static float Random01()
  {
    UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
    return UnityEngine.Random.Range(0.0f, 1.0f);
  }

  public static float Random(float mini,float max)
  {
    UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
    return UnityEngine.Random.Range(mini, max);
  }

  /// <summary>
  ///  取出數值不重複的int arr，包含0，不包含
  /// </summary>
  /// <param name="count">需求數量</param>
  /// <param name="max">最大值</param>
  /// <returns></returns>
  public static int[] Random_arr(int count, int max)
  {
    List<int> list = new List<int>(count);
    List<int> pool = new List<int>(max);
    for(int i = 0; i < max; i++){
      pool.Add(i);
    }
    
    while(list.Count < count){
      int index = Random(0, pool.Count);
      list.Add(pool[index]);
      pool.Remove(index);
    }

    return list.ToArray();
  }

  //畢氏定理
  public static Vector2 Pythagoreantheorem(float bottom, float hight){
    float c_length = Mathf.Pow((Mathf.Pow(bottom , 2) + Mathf.Pow(hight , 2)), 0.5f);
    return new Vector2(bottom, hight).normalized * c_length;
  }

  //迷宮矩形中心點
  public static Vector2 MazePoint(float bottom, float hight){
    return Pythagoreantheorem(bottom, hight) * 0.5f;
  }
  //取得對角座標
  public static Vector2 GetDiagonalLocation(int X, int Y, int rows , int columns)
  {
    int EndY = (columns -1) - Y;
    int EndX = (rows - 1) - X;
    return new Vector2(EndX, EndY);
  }

  public static Vector2 GetMazeCorner(MazeCorner cornerlocation, int rows, int columns){
    if(cornerlocation == MazeCorner.LeftBottom){
      return new Vector2(0, 0);
    }
    else if(cornerlocation == MazeCorner.LeftTop){
      return new Vector2(rows - 1, 0);
    }
    else if(cornerlocation == MazeCorner.RightBottom){
      return new Vector2(0, columns - 1);

    }
    else if(cornerlocation == MazeCorner.RightTop){
      return new Vector2(rows - 1 , columns - 1);
    }
     return new Vector2(0, 0);
  }

  //輸入一個角落返回除了對角線角落的其他兩個角落
  //如果輸入是SZ，那會返回接近中心點的座標
  public static MazeCorner[] GetMazeCorners(MazeCorner exclusivecornerlocation, int rows, int columns)
  {
    if (exclusivecornerlocation == MazeCorner.LeftBottom)
    {
      return new MazeCorner[]{ MazeCorner.LeftTop,MazeCorner.RightBottom };
    }
    else if (exclusivecornerlocation == MazeCorner.LeftTop)
    {
      return new MazeCorner[] { MazeCorner.RightTop, MazeCorner.LeftBottom};
    }
    else if (exclusivecornerlocation == MazeCorner.RightBottom)
    {
      return new MazeCorner[] { MazeCorner.RightTop, MazeCorner.LeftBottom};

    }
    else if (exclusivecornerlocation == MazeCorner.RightTop)
    {
      return new MazeCorner[] { MazeCorner.RightBottom, MazeCorner.LeftTop};
    }
    return null;
  }

  public static float toFloat(object val){
    float ret = 0;
    if (val.GetType() == typeof(double))
    {
      ret = (float)(double)val;
    }
    else
    if (val.GetType() == typeof(float))
    {
      ret = (float)val;
    }
    else
    if (val.GetType() == typeof(int))
    {
      ret = (float)(int)val;
    }
    else
    if (val.GetType() == typeof(uint))
    {
      ret = (float)(uint)val;
    }
    else
    if (val.GetType() == typeof(System.Int64))
    {
      ret = (float)(System.Int64)val;
    }
    else
    if (val.GetType() == typeof(System.UInt64))
    {
      ret = (float)(System.UInt64)val;
    }

    return ret;
  }

  public static void Shuffle<T>(ref T[] arr)
  {
    int i = arr.Length;
    int j;
    if (i == 0){
      return;
    }
    while (--i != 0)
    {
      System.Random ran = new System.Random();
      j = ran.Next() % (i + 1);
      T tmp = arr[i];
      arr[i] = arr[j];
      arr[j] = tmp;
    }
  }


  public static float GetBiggerValue(float a ,float b){
    return a >= b ? a : b;
  }
  public static float GetSmallValue(float a, float b){
    return a < b ? a : b;
  }

  public static int GetBiggerValue(int a, int b)
  {
    return a >= b ? a : b;
  }
  public static int GetSmallValue(int a, int b)
  {
    return a < b ? a : b;
  }
}
