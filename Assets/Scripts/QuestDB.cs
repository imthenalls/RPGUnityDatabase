using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;
using Mono.Data.Sqlite;

public class QuestDB : SqliteHelper {
	private const string Tag = "nalls: QuestDB:\t";
	
	private const string TABLE_NAME = "quests";
	private const string TABLE_NAME_REWARDS = "quest_rewards";
	private const string TABLE_NAME_PREREQS = "quest_prereqs";
	private const string KEY_QUESTID = "quest_id";
	private const string KEY_ID = "id";
	private const string KEY_TITLE = "title";
	private const string KEY_DESCRIPTION = "description";
	private const string KEY_NPCID = "npc_id";
	private const string KEY_XP = "xp";
	private const string KEY_MONEY = "money";
	private const string KEY_COMPLETED = "completed";
	private const string KEY_TIMESCOMPLETED = "timesCompleted";
	private const string KEY_ISREPEATABLE = "isRepeatable";
	private const string KEY_INPROGRESS = "inProgress";

	private string[] COLUMNS = new string[] {KEY_ID, KEY_TITLE, KEY_DESCRIPTION, KEY_NPCID,
		KEY_XP,KEY_MONEY,KEY_COMPLETED,KEY_TIMESCOMPLETED,KEY_ISREPEATABLE,KEY_INPROGRESS};

	public QuestDB() : base()
	{

	}
	public QuestObj getQuestFromDB(string id)
	{
		IDataReader reader = getDataByString(id);
		QuestObj quest = null;
		while (reader.Read())
		{
			 QuestObj entity = new QuestObj(reader[0].ToString(), 
				reader[1].ToString(), 
				reader[2].ToString(),
				reader[3].ToString(), 
				reader[4].ToString(), 
				reader[5].ToString(), 
				reader[6].ToString(), 
				reader[7].ToString(), 
				reader[8].ToString(), 
				reader[9].ToString());
			quest = entity;
		}
		if (quest == null) return null;
		reader.Close();

		reader = getRewards(quest._id);
		while (reader.Read())
		{
			quest._Rewards.Add(reader[0].ToString());
		}
		reader.Close();

		reader = getPreReqs(quest._id);
		while (reader.Read())
		{
			quest._PreReqIds.Add(reader[0].ToString());
		}
		reader.Close();

		return quest;
	}
	
	public void addDataToPreReqs(string id, string prereqid )
	{
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandType = CommandType.Text;
		dbcmd.CommandText =
			"INSERT INTO " + TABLE_NAME_PREREQS
			               + " ( "
			               + KEY_ID + ", "
			               + KEY_QUESTID + " ) "

			               + "VALUES (@id, @quest_id)";
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "id",
			Value = prereqid});		
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "quest_id",
			Value = id});
		dbcmd.ExecuteNonQuery();
	}
	
	public void addDataToRewards(string quest_id, string item_id)
	{
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandType = CommandType.Text;
		dbcmd.CommandText =
			"INSERT INTO " + TABLE_NAME_REWARDS
			               + " ( "
			               + KEY_ID + ", "
			               + KEY_QUESTID + " ) "

			               + "VALUES (@id, @quest_id)";
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "id",
			Value = item_id});		
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "quest_id",
			Value = quest_id});
		dbcmd.ExecuteNonQuery();
	}
	
	public void addData(QuestObj quest)
	{
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandText =
			"INSERT INTO " + TABLE_NAME
			+ " ( "
			+ KEY_ID + ", "
			+ KEY_TITLE + ", "
			+ KEY_DESCRIPTION + ", "
			+ KEY_NPCID + ", "
			+ KEY_XP + ", "
			+ KEY_MONEY + ", "
			+ KEY_COMPLETED + ", "
			+ KEY_TIMESCOMPLETED + ", "
			+ KEY_ISREPEATABLE + ", "
			+ KEY_INPROGRESS + " ) "

			+ "VALUES (@id, @title, @description, @giver, @xp, @money, @completed, @timesCompleted, @isRepeatable, @inProgress)";

		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "id",
			Value = quest._id});
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "title",
			Value = quest._QuestTitle});
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "description",
			Value = quest._Description});
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "giver",
			Value = quest._QuestGiver});
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "xp",
			Value = quest._xp});
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "money",
			Value = quest._Money});
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "completed",
			Value = quest._Completed});
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "timesCompleted",
			Value = quest._TimesCompleted});
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "isRepeatable",
			Value = quest._IsRepeatable});
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "inProgress",
			Value = quest._InProgress});
			
		dbcmd.ExecuteNonQuery();
	}

	public override IDataReader getDataById(int id)
	{
			return base.getDataById(id);
	}

	public IDataReader getRewards(string id)
	{
		Debug.Log(Tag + "Getting Quest Rewards from : " + id);
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandType = CommandType.Text;
		dbcmd.CommandText =
			"SELECT distinct quest_rewards.id"  +
			" FROM " + TABLE_NAME_REWARDS + " INNER JOIN "+TABLE_NAME +
			" ON " +TABLE_NAME_REWARDS+"."+KEY_QUESTID + " = @id";
		Debug.Log(dbcmd.CommandText);
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "id",
			Value = id});
		return dbcmd.ExecuteReader();
	}

	public IDataReader getPreReqs(string id)
	{
		Debug.Log(Tag + "Getting Quest PreReqs from : " + id);
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandType = CommandType.Text;
		dbcmd.CommandText =
			"SELECT distinct quest_prereqs.id"  +
			" FROM " + TABLE_NAME_PREREQS + " INNER JOIN "+TABLE_NAME +
			" ON " +TABLE_NAME_PREREQS+"."+KEY_QUESTID + " = @id";
		Debug.Log(dbcmd.CommandText);
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "id",
			Value = id});
		return dbcmd.ExecuteReader();
	}
	
	public override IDataReader getDataByString(string id)
	{
		Debug.Log(Tag + "Getting Quest: " + id);
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandType = CommandType.Text;
		dbcmd.CommandText =
			"SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = @id";
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "id",
			Value = id});	
		return dbcmd.ExecuteReader();
	}

	public override void deleteDataByString(string id)
	{
		Debug.Log(Tag + "Deleting Quest: " + id);
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandType = CommandType.Text;
		dbcmd.CommandText =
			"DELETE FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = @id";
		dbcmd.Parameters.Add(new SqliteParameter {
			ParameterName = "id",
			Value = id});	
		dbcmd.ExecuteNonQuery();
	}
	public override void deleteAllData()
	{
		Debug.Log(Tag + "Deleting Table");
		base.deleteAllData(TABLE_NAME);
	}

	public override IDataReader getAllData()
	{
		return base.getAllData(TABLE_NAME);
	}

}
