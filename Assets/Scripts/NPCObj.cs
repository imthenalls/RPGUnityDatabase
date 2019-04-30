using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCObj {

	public string _id;
	public List<string> _AvailableQuests;
	public List<string> _Dialogs;
	public string _HasStore;
	public string _HasQuests;
	public string _Name;

	public NPCObj(string id, string hasStore,
		string hasQuests, string name)
	{
		_id = id;
		_HasStore = hasStore;
		_HasQuests = hasQuests;
		_Name = name;
		_AvailableQuests = new List<string>();
		_Dialogs = new List<string>();
	}
}
