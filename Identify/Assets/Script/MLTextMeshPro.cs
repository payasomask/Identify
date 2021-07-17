using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class MLTextMeshPro : MonoBehaviour
{
  //有些東西不需要初始化string
  [SerializeField]
  bool inti_stringkey = true;
  // Start is called before the first frame update
  [SerializeField]
  string stringkey;
    void Awake(){
    TextMeshPro t = gameObject.GetComponent<TextMeshPro>();
      FontManager._FontManager.getFont(t);
    if (inti_stringkey)
      t.text = JsonLoader._JsonLoader.GetString(stringkey);
    }
}
