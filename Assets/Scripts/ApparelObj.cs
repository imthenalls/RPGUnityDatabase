using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApparelObj {
	
	public string _id;
	public string _Name;
	public string _strength;
	public string _luck;
	public string _intelligence;
	public string _dexterity;
	public string _appType;
	public string _itemType;
	public string _price;

	public ApparelObj(string id, string name, string appType, string itemType, string price)
	{
		_id = id;
		_Name = name;
		_appType = appType;
		_itemType = itemType;
		_price = price;
	}
}
