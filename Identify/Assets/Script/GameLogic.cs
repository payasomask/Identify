using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestionAnswer{
  Same,
  Different
}

public class GameLogic : MonoBehaviour
{
  public static GameLogic _GameLogic = null;
  QuestionAnswer questionanswer = QuestionAnswer.Different;
  ValueRange H_range = new ValueRange() { mini = 0,max = 360 };
  ValueRange S_range = new ValueRange() { mini = 0, max = 100 };
  ValueRange V_range = new ValueRange() { mini = 60, max = 100 };

  GameDataConfig config = null;
  HSV[] HSV_arr;
  Point[,] right_point_maxtra;
  Point[,] left_point_maxtra;
  Point[] different_list;//���]�ݭn����C�⨺�N�q�o�̪�����
  List<int> H_source;
  List<int> S_source;
  List<int> V_source;
  private void Awake(){
    _GameLogic = this;
  }

  public void SetConfig(GameDataConfig dataconfig){
    config = dataconfig;
  }
  // Start is called before the first frame update
  void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public void InitLevel(float cellsize){
    GernerateHSV();
    GerneratePoint(cellsize);
    //�򥻤W��o�̴N�����������F�A���ۥ~���ϥ�
  }

  public void NextQuest(){
    //�o�̭n������C��A�]���P�@�����d�̳]�w�����|�ܥu�O�b��@���Ӥw
    UpdateHSV();
    UpdatePoint();
  }

  //�ͥX�C�⪺�˥�
  public void GernerateHSV(){
    int[] Hs = GetH_arr();
    int[] Ss = GetS_arr();
    int[] Vs = GetV_arr();

    injectHSV(Hs,Ss,Vs);
  }

  //�q�Lsource�A�����ͤ��P���C��˥�
  public void UpdateHSV(){
    int[] Hs = updateHSVsource(H_range.max, H_range.mini, H_source.ToArray());
    int[] Ss = updateHSVsource(S_range.max, S_range.mini, S_source.ToArray());
    int[] Vs = updateHSVsource(V_range.max, V_range.mini, V_source.ToArray());

    injectHSV(Hs,Ss,Vs);
  }

  void injectHSV(int[] Hs, int[] Ss, int[] Vs)
  {
    List<HSV> list = new List<HSV>();
    for (int i = 0; i < config.Color; i++)
    {
      HSV tmp = new HSV();
      tmp.H = Hs[i];
      tmp.S = Ss[i];
      tmp.V = Vs[i];
      list.Add(tmp);
    }
    HSV_arr = list.ToArray();
  }

  //�ᤩ�C�@�ӲyHSV
  public void GerneratePoint(float cellsize)
  {

    if (HSV_arr == null)
      return;

    //�~�P
    UtilityHelper.Shuffle(ref HSV_arr);

    right_point_maxtra = new Point[config.H, config.W];
    left_point_maxtra = new Point[config.H, config.W];

    //  if (right_point_maxtra == null)
    //    right_point_maxtra = new Point[config.H, config.W];
    //  else
    //if (right_point_maxtra.Length != config.W * config.H){
    //    //��ܰg�c���e���ܤF
    //    right_point_maxtra = new Point[config.H, config.W];
    //  }


    Vector2 offset_pivot = UtilityHelper.MazePoint(config.W * cellsize, config.H * cellsize);


    bool diff = UtilityHelper.Random01() <= config.DifferenceRate;
    //bool diff = false;

    different_list = new Point[config.H * config.W];

    for (int i = 0; i < config.H; i++)
    {
      for (int j = 0; j < config.W; j++)
      {
        int index = i * config.W + j;
        HSV hsv = null;
        try
        {
          hsv = HSV_arr[index];
        }
        catch (IndexOutOfRangeException e)
        {
          hsv = HSV_arr[UtilityHelper.Random(0, HSV_arr.Length)];
        }

        right_point_maxtra[i, j] = new Point()
          {
            X = i,
            Y = j,
            position = new Vector2(j * cellsize + cellsize * 0.5f, i * cellsize + cellsize * 0.5f)- offset_pivot,
            HSV = hsv
          };
        left_point_maxtra[i, j] = new Point(){
            X = i,
            Y = j,
            position = new Vector2(j * cellsize + cellsize * 0.5f, i * cellsize + cellsize * 0.5f) - offset_pivot,
            HSV = hsv
          };

        //�@�˪��ܴN����
        if (!diff)
          continue;

        different_list[index] = right_point_maxtra[i, j];
        //Debug.Log("Cell�y�� : [" + maze_cell_matrix[i, j].X + "�A" + maze_cell_matrix[i, j].Y + "]�� Cell position : " + maze_cell_matrix[i,j].position);
      }
    }

    if (!diff){
      //�@��
      questionanswer = QuestionAnswer.Same;
      return;
    }
    questionanswer = QuestionAnswer.Different;

    //�N�o�Ӭ~�P�A�ΨӬD���H�����I
    UtilityHelper.Shuffle(ref different_list);

    //����
    for(int i = 0; i < config.DifferencePoint; i++){
      Point tmp = different_list[i];
      //���o�s��HSV�d��
      ValueRange H = GetValueRandom(H_range.mini, H_range.max, tmp.HSV.H, config.Hn, config.Hf);
      ValueRange S = GetValueRandom(S_range.mini, S_range.max, tmp.HSV.S, config.Sn, config.Sf);
      ValueRange V = GetValueRandom(V_range.mini, V_range.max, tmp.HSV.V, config.Vn, config.Vf);
      //�ˬd���S���@�˪��C��
      bool repeatcolor = true;
      HSV new_hsv = null;
      while (repeatcolor){
        new_hsv = GetHSV(H, S, V);
        repeatcolor = repeatColor(new_hsv, right_point_maxtra);
      }
      tmp.HSV = new_hsv;
      tmp.different = true;
      left_point_maxtra[tmp.X, tmp.Y].different = true;
    }

  }
  public void UpdatePoint()
  {

    if (HSV_arr == null)
      return;

    //�~�P
    UtilityHelper.Shuffle(ref HSV_arr);


    bool diff = UtilityHelper.Random01() <= config.DifferenceRate;;

    for (int i = 0; i < config.H; i++)
    {
      for (int j = 0; j < config.W; j++)
      {
        int index = i * config.W + j;
        HSV hsv = null;
        try
        {
          hsv = HSV_arr[index];
        }
        catch (IndexOutOfRangeException e)
        {
          hsv = HSV_arr[UtilityHelper.Random(0, HSV_arr.Length)];
        }
        right_point_maxtra[i, j].HSV = hsv;
        left_point_maxtra[i, j].HSV = hsv;
        right_point_maxtra[i, j].different = false;
        left_point_maxtra[i, j].different = false;
        //�@�˪��ܴN����
        if (!diff)
          continue;

        different_list[index] = right_point_maxtra[i, j];
        //Debug.Log("Cell�y�� : [" + maze_cell_matrix[i, j].X + "�A" + maze_cell_matrix[i, j].Y + "]�� Cell position : " + maze_cell_matrix[i,j].position);
      }
    }

    if (!diff)
    {
      //�@��
      questionanswer = QuestionAnswer.Same;
      return;
    }
    questionanswer = QuestionAnswer.Different;

    //�N�o�Ӭ~�P�A�ΨӬD���H�����I
    UtilityHelper.Shuffle(ref different_list);

    //����
    for (int i = 0; i < config.DifferencePoint; i++)
    {
      Point tmp = different_list[i];
      //���o�s��HSV�d��
      ValueRange H = GetValueRandom(H_range.mini, H_range.max, tmp.HSV.H, config.Hn, config.Hf);
      ValueRange S = GetValueRandom(S_range.mini, S_range.max, tmp.HSV.S, config.Sn, config.Sf);
      ValueRange V = GetValueRandom(V_range.mini, V_range.max, tmp.HSV.V, config.Vn, config.Vf);
      //�ˬd���S���@�˪��C��
      bool repeatcolor = true;
      HSV new_hsv = null;
      while (repeatcolor)
      {
        new_hsv = GetHSV(H, S, V);
        repeatcolor = repeatColor(new_hsv, right_point_maxtra);
      }
      tmp.HSV = new_hsv;
      tmp.different = true;
      left_point_maxtra[tmp.X, tmp.Y].different = true;
    }

  }

  bool sameColor(HSV a, HSV b){
    return a.H == b.H && a.S == b.S && a.V == b.V;
  }
  bool repeatColor(HSV targetHSV, Point [,] source){
    foreach(var v in source){
      if(sameColor(targetHSV, v.HSV)){
        return true;
      }
    }

    return false;
  }

  HSV GetHSV(ValueRange H, ValueRange S, ValueRange V){
    return new HSV()
    {
      H = UtilityHelper.Random(H.mini, H.max + 1),
      S = UtilityHelper.Random(S.mini, S.max + 1),
      V = UtilityHelper.Random(V.mini, V.max + 1)
    };
  }

  ValueRange GetValueRandom(int mini,int max, int base_value,int n, int f){
    //���o�d��}�C
    List<ValueRange> list = new List<ValueRange>(2);

    int valuemini = base_value + n;
    int valuemax = base_value + f;
    if (valuemini < mini || valuemax > max){
      //�N���ĥ�
    }
    else
      list.Add(new ValueRange() { mini = valuemini, max = valuemax });

    valuemax = base_value - n;
    valuemini = base_value - f;
    if (valuemini < mini || valuemax > max){
      //�N���ĥ�
    }
    else
      list.Add(new ValueRange() { mini = valuemini, max = valuemax });

    int pick = UtilityHelper.Random(0, list.Count);
    return list[pick];
  }


  public class ValueRange{
    public int mini;
    public int max;
  }

  int[] GetH_arr(){
    int arr_count = config.Color;

    int mini_H = H_range.mini;
    int max_H = H_range.max;

    H_source = new List<int>(arr_count);
    H_source.Add(0);
    while(H_source.Count < arr_count){
      BuildHSVsource(ref H_source, config.Hdn,config.Hdf);
    }
    return updateHSVsource(max_H, mini_H, H_source.ToArray());
  }

  int[] updateHSVsource(int maxlimit,int minilimit, int[] source_arr){

    //�H���n�]�t�W���n+1
    int random_max_value = maxlimit - source_arr[source_arr.Length - 1] + 1;

    int base_value = UtilityHelper.Random(minilimit, random_max_value);
    int[] target = new int[source_arr.Length];
    for (int i = 0; i < source_arr.Length; i++){
      target[i] = base_value + source_arr[i];
    }

    return target;
  }

  int[] GetS_arr()
  {
    int arr_count = config.Color;
    int mini_S = S_range.mini;
    int max_S = S_range.max;
    S_source = new List<int>(arr_count);
    S_source.Add(0);
    while (S_source.Count < arr_count)
    {
      BuildHSVsource(ref S_source, config.Sdn, config.Sdf);
    }

    return updateHSVsource(max_S, mini_S, S_source.ToArray());

  }

  int[] GetV_arr()
  {
    int arr_count = config.Color;
    int mini_V = V_range.mini;
    int max_V = V_range.max;
    V_source = new List<int>(arr_count);
    V_source.Add(0);
    while (V_source.Count < arr_count)
    {
      BuildHSVsource(ref V_source, config.Vdn, config.Vdf);
    }

    return updateHSVsource(max_V, mini_V, V_source.ToArray());
  }

  void BuildHSVsource(ref List<int> list,int mini, int max)
  {
    int base_value = list[list.Count - 1];
    int minivalue = base_value + mini;

    //�]�����]�tmaxivalue �A �ҥH+1
    int maxivalue = base_value + max+1;
    list.Add(UtilityHelper.Random(minivalue, maxivalue + 1));
  }

  public class Point {
    public int X, Y;
    public Vector2 position;
    public HSV HSV;
    public bool different = false;
  }
  public class HSV{
    public int H, S, V;
  }

  public Point[,] GetRightPoints(){
    return right_point_maxtra;
  }

  public Point[,] GetLeftPoints(){
    return left_point_maxtra;
  }

  public bool Correct(QuestionAnswer playeranswer){
    return playeranswer == questionanswer;
  }

  public string GetAnswer()
  {
    return questionanswer.ToString();
  }
}
