using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EmbeddedSceneSettings : ISceneSettings{


  Dictionary<string, object[]> mSlotMachineSetup =new Dictionary<string, object[]>(){

    //
    //  IntroScene
    //
    {"IntroScene", new object[]{
      typeof(IntroScene),
      null,
      null
    }},

    //
    //  lOGIN
    //
    //{"LoginScene", new object[]{
    //  typeof(Login),
    //  null,
    //  null
    //}},

    //
    //  Lobby
    //
    {"LobbyScene", new object[]{
      typeof(LobbyScene),
      null,
      null
    }},

        {"GameScene", new object[]{
      typeof(GameScene),
      null,
      null
    }},

     {"LevelListScene", new object[]{
      typeof(LevelScene),
      null,
      null
    }},


  };

  public object getSettings(string gameName, SceneSettingsType type){
    if (mSlotMachineSetup.ContainsKey(gameName)==false){
      Debug.LogError("99 - "+gameName+" not found");
      return null;
    }
    return mSlotMachineSetup[gameName][(int)type];
  }
}
