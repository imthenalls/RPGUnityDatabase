using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObj
{
	public string _id;
	public string _Name;
	public string _Money;
	public string _CurrentTown;
	public List<string> _ActiveQuests = new List<string>();
	public List<string> _CompletedQuests = new List<string>();
	public List<string> _InventorySlots = new List<string>();
	public List<string> _EquipmentSlots = new List<string>();

	public PlayerObj(string id, string name, string money, string currentTown)
	{
		_id = id;
		_Name = name;
		_Money = money;
		_CurrentTown = currentTown;
	}
}
