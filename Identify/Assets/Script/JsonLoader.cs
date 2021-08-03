using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonLoader : MonoBehaviour
{

  public static JsonLoader _JsonLoader = null;

  Dictionary<string, ml_string> string_dic = new Dictionary<string, ml_string>();
  string[] available_language_arr;
  Dictionary<string, GameDataConfig> data_dic = new Dictionary<string, GameDataConfig>();

  private void Awake(){
    _JsonLoader = this;
  }

  private void Start(){
  }

  public void Init(){
    //start init

    Dictionary<string,object> jsontable = (Dictionary<string, object>)MiniJSON.Json.Deserialize(getJson());
    Dictionary<string, object> tablesheets = (Dictionary<string, object>)jsontable["N01_Identify_Setup.xlsx"];

    //parse lang_sheet
    Dictionary<string, object> Lang_sheets = (Dictionary<string, object>)tablesheets["Language"];
    Dictionary<string, object> langdata = (Dictionary<string, object>)Lang_sheets["1"];
    PlayerPrefsManager._PlayerPrefsManager.Language = (string)langdata["Now"];
    List<object> lang_list = (List<object>)langdata["Language"];
    List<string> tmp_list = new List<string>(lang_list.Count);
    for (int i = 0; i < lang_list.Count; i++){
      string lang = (string)lang_list[i];
      tmp_list.Add(lang);
    }
    available_language_arr = tmp_list.ToArray();

    //parse string_sheet
    Dictionary<string, object> String_sheets = (Dictionary<string, object>) tablesheets["String"];
    foreach(var v in String_sheets){
      Dictionary<string, object> string_value = (Dictionary<string, object>)v.Value;
      ml_string tmp = new ml_string();
      string string_key = v.Key;
      tmp.addLang(string_value, "String", available_language_arr);
      string_dic.Add(string_key, tmp);
    }

    //parse data_sheet
    Dictionary<string, object> Data_sheets = (Dictionary<string, object>)tablesheets["Data"];
    foreach (var v in Data_sheets)
    {
      string data_key = v.Key;
      Dictionary<string, object> data_value = (Dictionary<string, object>)v.Value;
      GameDataConfig tmp = new GameDataConfig();
      tmp.W = (int)(System.Int64)data_value["W"];
      tmp.H = (int)(System.Int64)data_value["H"];
      tmp.Color = (int)(System.Int64)data_value["Color"];
      tmp.Hdn = (int)(System.Int64)data_value["Hdn"];
      tmp.Hdf = (int)(System.Int64)data_value["Hdf"];
      tmp.Sdn = (int)(System.Int64)data_value["Sdn"];
      tmp.Sdf = (int)(System.Int64)data_value["Sdf"];
      tmp.Vdn = (int)(System.Int64)data_value["Vdn"];
      tmp.Vdf = (int)(System.Int64)data_value["Vdf"];
      tmp.Amount = (int)(System.Int64)data_value["Amount"];
      tmp.DifferenceRate = toFloat(data_value["DifferenceRate"]);
      tmp.DifferencePoint = (int)(System.Int64)data_value["DifferencePoint"];
      tmp.Hn = (int)(System.Int64)data_value["Hn"];
      tmp.Hf = (int)(System.Int64)data_value["Hf"];
      tmp.Sn = (int)(System.Int64)data_value["Sn"];
      tmp.Sf = (int)(System.Int64)data_value["Sf"];
      tmp.Vn = (int)(System.Int64)data_value["Vn"];
      tmp.Vf = (int)(System.Int64)data_value["Vf"];
      tmp.OneStarRightRate = toFloat(data_value["1StarRightRate"]);
      tmp.OneStarUesTime = toFloat(data_value["1StarUseTime"]);
      tmp.TwoStarRightRate = toFloat(data_value["2StarRightRate"]);
      tmp.TwoStarUseTime = toFloat(data_value["2StarUseTime"]);
      tmp.ThreeStarRightRate = toFloat(data_value["3StarRightRate"]);
      tmp.ThreeStarUseTime = toFloat(data_value["3StarUseTime"]);
      tmp.Time = toFloat(data_value["Time"]);
      tmp.LevelTime = tmp.Amount * tmp.Time;
      data_dic.Add(data_key, tmp);
    }



    Debug.Log("562 - Loaded Json");
  }



  string getJson(){
    string json_text;
#if UNITY_ANDROID || UNITY_EDITOR
    TextAsset t = (TextAsset)Resources.Load("N01_Identify_Setup.xlsx");
    json_text = t.text;
#elif UNITY_STANDALONE_WIN
    string file_path = Path.Combine(Application.streamingAssetsPath, "N01_Identify_Setup.xlsx.json");
    return File.ReadAllText(file_path);
#endif

    return json_text;
  }

  public string GetString(string key){
    if (string_dic.ContainsKey(key)){
      return string_dic[key];
    }

    return "536 - missing_string";
  }

  public GameDataConfig GetDataconfig(string key)
  {
    if (data_dic.ContainsKey(key)){
      return data_dic[key];
    }

    return null;
  }

  public int gettotallevelcount() {
    return data_dic.Count;
  }

  public float GetTimeRate(string level,float usetime){
    return usetime / GetDataconfig(level).LevelTime;
  }

  //public string GetTimeRateString(string level, float usetime)
  //{
  //  return string.Format("{0:F2}", (GetTimeRate(level, usetime) * 100.0f));
  //}

  public float GetCorrectRate(string level, int correct){
    return (float)correct / GetDataconfig(level).Amount ;
  }
  //public string GetCorrectRateString(string level, int correct)
  //{
  //  return string.Format("{0:F2}", (GetCorrectRate(level, correct) * 100.0f).ToString());
  //}
  public int GetStar(string level,float usetime , int correct)
  {
    float timerate = GetTimeRate(level,usetime);
    float correctrate = GetCorrectRate(level,correct);

    GameDataConfig config = GetDataconfig(level);

    int timestar = 0;
    int correctstar = 0;

    if (timerate < config.OneStarUesTime){
      timestar++;
      if(timerate < config.TwoStarUseTime){
        timestar++;
        if(timerate < config.ThreeStarUseTime){
          timestar++;
        }
      }
    }

    if (correctrate >= config.OneStarRightRate){
      correctstar++;
      if(correctrate >= config.TwoStarRightRate){
        correctstar++;
        if(correctrate >= config.ThreeStarRightRate){
          correctstar++;
        }
      }
    }

    return UtilityHelper.GetSmallValue(timestar, correctstar);
  }

  public string Star_string(string formatid, int star){

    string rich_format = GetString(formatid);

    int maxstar = 3;

    string star_string = string.Empty;

    for (int i = 0; i < maxstar; i++)
    {
      if (i < star)
        star_string += "★";
      else
        star_string += "☆";
    }

    return rich_format + star_string;
  }


  public static float toFloat(object val)
  {
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

}

public class GameDataConfig
{
  public int W;
  public int H;
  public int Color;
  public int Hdn;
  public int Hdf;
  public int Sdn;
  public int Sdf;
  public int Vdn;
  public int Vdf;
  public int Amount;
  public float LevelTime;//該關總時間 = Time * Amount
  public float Time;
  public float DifferenceRate;
  public int DifferencePoint;
  public int Hn;
  public int Hf;
  public int Sn;
  public int Sf;
  public int Vn;
  public int Vf;
  public float OneStarRightRate;
  public float OneStarUesTime;
  public float TwoStarRightRate;
  public float TwoStarUseTime;
  public float ThreeStarRightRate;
  public float ThreeStarUseTime;
}
public class GameRecord{
  public string level;
  public float time;
  public int correct;
  public int star;
}


