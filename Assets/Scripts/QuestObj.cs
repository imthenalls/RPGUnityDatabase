
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class QuestObj  
{
	public string _id;
	public string _QuestTitle;
	public string _Description;
	public string _QuestGiver;
	public string _xp;
	public string _Money;
	public string _Completed;
	public string _TimesCompleted;
	public string _IsRepeatable;
	public string _InProgress;
	public string _NpcId;
	public List<string> _Rewards = new List<string>();
	public List<string> _PreReqIds = new List<string>();

	public QuestObj(string id, string questTitle, string description, string questGiver, string xp, 
		string money, string completed, string timesCompleted, string isRepeatable, string inProgress)
	{
		_id = id;
		_QuestTitle = questTitle;
		_Description = description;
		_QuestGiver = questGiver;
		_xp = xp;
		_Money = money;
		_Completed = completed;
		_TimesCompleted = timesCompleted;
		_IsRepeatable = isRepeatable;
		_InProgress = inProgress;
	}
}

