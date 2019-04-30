using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;
using Mono.Data.Sqlite;
public class PlayerDB : SqliteHelper {

	private const string Tag = "nalls: PlayerDB:\t";
	private const string TABLE_NAME = "players";
	private const string KEY_ID = "id";
	private const string KEY_PLAYERNAME = "name";
	private const string KEY_MONEY = "money";
	private const string KEY_CURRENTTOWN = "currentTown";
	
	private const string TABLE_NAMEAQ = "active_quests";
	private const string TABLE_NAMECQ = "completed_quests";	
	private const string KEY_QUESTID = "quest_id";
	private const string TABLE_NAMEISLOT = "inventory_slots";	
	private const string TABLE_NAMEESLOT = "equipment_slots";	
	private const string KEY_SLOTID = "slot_id";
	private const string KEY_ITEMID = "item_id";
	private const string KEY_PLAYERID = "player_id";

	public PlayerObj getPlayerFromDB(string id)
	{
		IDataReader reader = getDataByString(id);
		PlayerObj player = null;
		while (reader.Read())
		{
			PlayerObj entity = new PlayerObj(reader[0].ToString(), 
				reader[1].ToString(), 
				reader[2].ToString(), 
				reader[3].ToString());
			player = entity;
		}
		reader.Close();
		if (player == null) return null;
		
		reader = getActiveQuests(id);
		while (reader.Read())
		{
			player._ActiveQuests.Add(reader[0].ToString());
		}
		reader.Close();
		
		reader = getCompletedQuests(id);
		while (reader.Read())
		{
			player._CompletedQuests.Add(reader[0].ToString());
		}
		reader.Close();
		
		reader = getInventorySlots(id);
		while (reader.Read())
		{
			player._InventorySlots.Add(reader[1].ToString());
		}
		reader.Close();
				
		reader = getEquipmentSlots(id);
		while (reader.Read())
		{
			player._EquipmentSlots.Add(reader[1].ToString());
		}
		reader.Close();
		
		return player;
	}
	
	public IDataReader getEquipmentSlots(string id)
	{
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandType = CommandType.Text;
		dbcmd.CommandText = "SELECT distinct " +KEY_SLOTID+","+KEY_ITEMID + " " +
		                    "FROM "+TABLE_NAMEESLOT+" INNER JOIN "+TABLE_NAME + " " +
		                    "ON "+TABLE_NAMEESLOT+"."+KEY_PLAYERID+" = @id " +
		                    "ORDER BY "+KEY_SLOTID+" ASC";
		dbcmd.Parameters.Add(new SqliteParameter {
				ParameterName = "id",
				Value = id});		
		return  dbcmd.ExecuteReader();
	}
	
	public IDataReader getInventorySlots(string id)
	{
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandType = CommandType.Text;
		dbcmd.CommandText = "SELECT distinct " +KEY_SLOTID+","+KEY_ITEMID + " " +
		                    "FROM "+TABLE_NAMEISLOT+" INNER JOIN "+TABLE_NAME + " " +
		                    "ON "+TABLE_NAMEISLOT+"."+KEY_PLAYERID+" = @id " +
							"ORDER BY "+KEY_SLOTID+" ASC";
		dbcmd.Parameters.Add(new SqliteParameter {
				ParameterName = "id",
				Value = id});
		return  dbcmd.ExecuteReader();
	}
	
	public IDataReader getCompletedQuests(string id)
	{
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandType = CommandType.Text;
		dbcmd.CommandText = "SELECT distinct " + KEY_QUESTID + " " +
		                    "FROM "+TABLE_NAMECQ+" INNER JOIN "+TABLE_NAME+ " " +
		                    "ON "+TABLE_NAMECQ+"."+KEY_PLAYERID+" = @id";
		dbcmd.Parameters.Add(new SqliteParameter {
				ParameterName = "id",
				Value = id});			
		return  dbcmd.ExecuteReader();
	}

	public IDataReader getActiveQuests(string id)
	{
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandType = CommandType.Text;
		dbcmd.CommandText = "SELECT distinct " +  KEY_QUESTID + " " +
		                    "FROM "+TABLE_NAMEAQ+ " INNER JOIN " + TABLE_NAME + " " +
							"ON "+ TABLE_NAMEAQ+"."+KEY_PLAYERID+" = @id";
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
		Debug.Log(Tag + "Getting player: " + id);
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
		Debug.Log(Tag + "Deleting player: " + id);
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandType = CommandType.Text;
		dbcmd.CommandText =
			"DELETE FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = @id";
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
