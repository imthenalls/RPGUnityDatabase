using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;
using Mono.Data.Sqlite;
public class NpcDB : SqliteHelper {
	private const string Tag = "nalls: NpcDB:\t";
	
	private const string TABLE_NAME = "npcs";
	private const string KEY_ID = "id";
	private const string KEY_NPCNAME = "name";
	private const string KEY_HASQUESTS = "HasQuests";
	private const string KEY_HASSTORE = "HasStore";
	
	public NpcDB() : base()
	{

	}
	public NPCObj getNpcFromDB(string id)
	{
		NPCObj npc = null;
		IDataReader reader = getDataByString(id);
		while (reader.Read())
		{
			NPCObj entity = new NPCObj(reader[0].ToString(), 
				reader[1].ToString(), 
				reader[2].ToString(),
				reader[3].ToString() 
				);
			npc = entity;
		}
		reader.Close();
		if(npc == null) return null;

		reader = getDialogs(npc._id);
		while (reader.Read())
		{
			npc._Dialogs.Add(reader[0].ToString());
		}
		reader.Close();

		reader = getQuests(npc._id);
		while (reader.Read())
		{
			npc._AvailableQuests.Add(reader[0].ToString());
		}
		reader.Close();

		return npc;
	}

	public IDataReader getDialogs(string id)
	{
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandType = CommandType.Text;

		dbcmd.CommandText = "SELECT distinct dialogs.id" +
		                    " FROM dialogs INNER JOIN "+ TABLE_NAME +
		                    " ON dialogs.npc_id = @id";
		dbcmd.Parameters.Add(new SqliteParameter {
				ParameterName = "id",
				Value = id});	
		return dbcmd.ExecuteReader();
	}

	public IDataReader getQuests(string id)
	{
		IDbCommand dbcmd= getDbCommand();
		dbcmd.CommandType = CommandType.Text;
		dbcmd.CommandText = "SELECT distinct quests.id"+
							" FROM quests INNER JOIN "+ TABLE_NAME +
							" ON quests.npc_id = @id";
		Debug.Log(dbcmd.CommandText);
		dbcmd.Parameters.Add(new SqliteParameter {
				ParameterName = "id",
				Value = id});	
		return dbcmd.ExecuteReader();
	}	
	
	public override IDataReader getAllData()
	{
		return base.getAllData(TABLE_NAME);
	}
	
	public override IDataReader getDataByString(string id)
	{
		Debug.Log(Tag + "Getting npc: " + id);
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
		Debug.Log(Tag + "Deleting npc: " + id);
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandType = CommandType.Text;
		dbcmd.CommandText =
			"DELETE FROM " + TABLE_NAME + " WHERE " + KEY_ID  + " = @id";
		dbcmd.Parameters.Add(new SqliteParameter {
				ParameterName = "id",
				Value = id});	
		dbcmd.ExecuteNonQuery();
	}

	public override void deleteDataById(int id)
	{
		base.deleteDataById(id);
	}

	public override void deleteAllData()
	{
		Debug.Log(Tag + "Deleting Table");

		base.deleteAllData(TABLE_NAME);
	}

	
}
