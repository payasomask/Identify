using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ml_string{
	string default_val;
	Dictionary<string, string> lang_mapping_val =new Dictionary<string, string>();

	public void setVal(string lang, string val){
		if (lang_mapping_val.ContainsKey(lang)==false){
			lang_mapping_val.Add(lang, val);
		}else{
			lang_mapping_val[lang] =val;
		}
	}

	public static implicit operator string(ml_string ps){
		if (ps == null){
			Debug.Log("143 - ml_string == null..return stringEmpty ");
			return "";
    }
		string curr_lan =PlayerPrefs.GetString("Language", "US");
		if (ps.lang_mapping_val.ContainsKey(curr_lan)){
			return ps.lang_mapping_val[curr_lan];
		}
		return ps.default_val;
	}
	
	public static implicit operator ml_string(string s){
		ml_string n =new ml_string();
		n.default_val =s;
		return n;
	}

	public void addLang(Dictionary<string, object> raw_dic, string base_var, string[] lang_opt){
		for (var i=0;i<lang_opt.Length;++i){
			if (raw_dic.ContainsKey(base_var+"_"+lang_opt[i]) && ((string)raw_dic[base_var+"_"+lang_opt[i]]).Length>0){
				setVal(lang_opt[i], (string)raw_dic[base_var+"_"+lang_opt[i]]);
			}
		}
	}

	public void addLang(Dictionary<string, object> raw_dic, string base_var, List<string> lang_opt){
		for (var i=0;i<lang_opt.Count;++i){
      // Ray : 部分的raw_dic.Key 並不會帶有base_var，只有"US"、"TW"
      if (raw_dic.ContainsKey(lang_opt[i]) && ((string)raw_dic[lang_opt[i]]).Length>0){
				setVal(lang_opt[i], (string)raw_dic[lang_opt[i]]);
			}
			if (raw_dic.ContainsKey(base_var+"_"+lang_opt[i]) && ((string)raw_dic[base_var+"_"+lang_opt[i]]).Length>0){
				setVal(lang_opt[i], (string)raw_dic[base_var+"_"+lang_opt[i]]);
			}		
			if (raw_dic.ContainsKey(base_var + "_" + lang_opt[i]))
			{
				setVal(lang_opt[i], (string)raw_dic[base_var + "_" + lang_opt[i]]);
			}
		}
	}

}