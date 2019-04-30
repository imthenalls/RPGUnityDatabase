using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;
using Mono.Data.Sqlite;

public class ItemDB : SqliteHelper {

	private const string Tag = "nalls: ApparelDB:\t";
	private const string TABLE_APPARELS = "apparels";
	
	private const string KEY_ID = "id";
	private const string KEY_NAME = "name";
	private const string KEY_APPTYPE = "appType";
	private const string KEY_ITEMTYPE = "itemType";
	private const string KEY_PRICE = "price";

	public ApparelObj getApparelFromDB(string id)
	{
		IDataReader reader = getDataByString(id);
		ApparelObj apparel = null;
		while (reader.Read())
		{
			ApparelObj entity = new ApparelObj(reader[0].ToString(), 
				reader[1].ToString(), 
				reader[2].ToString(),
				reader[3].ToString(), 
				reader[4].ToString());
			apparel = entity;
		}
		reader.Close();
		return apparel;
	}
	
	public override IDataReader getAllData()
	{
		return base.getAllData(TABLE_APPARELS);
	}
	public override IDataReader getDataByString(string id)
	{
		Debug.Log(Tag + "Getting npc: " + id);
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandType = CommandType.Text;
		dbcmd.CommandText =
			"SELECT * FROM " + TABLE_APPARELS + " WHERE " + KEY_ID + " = @id";
		dbcmd.Parameters.Add(new SqliteParameter {
				ParameterName = "id",
				Value = id});	
		return dbcmd.ExecuteReader();
	}
	public override void deleteDataByString(string id)
	{
		Debug.Log(Tag + "Deleting apparel: " + id);
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandType = CommandType.Text;
		dbcmd.CommandText =
			"DELETE FROM " + TABLE_APPARELS + " WHERE " + KEY_ID + " = @id";
		dbcmd.Parameters.Add(new SqliteParameter {
				ParameterName = "id",
				Value = id});		
		dbcmd.ExecuteNonQuery();
	}
	
	public override void deleteAllData()
	{
		Debug.Log(Tag + "Deleting Table");
		base.deleteAllData(TABLE_APPARELS);
	}
}
