using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;



public class testing : MonoBehaviour {

	private string dbPath;

	private void Start() {
		dbPath = "URI=file:" + Application.persistentDataPath + "/GameDatabase.db";
		//CreateSchema();
		//InsertScore("GG Meade", 3701);
		///InsertScore("US Grant", 4242);
		//InsertScore("GB McClellan", 107);
		//GetHighScores(10);
	}
	/* timesCompleted;
    public bool isRepeatable;
    public bool inProgress;*/
	public void CreateSchema() {
		using (var conn = new SqliteConnection(dbPath)) {
			conn.Open();
			using (var cmd = conn.CreateCommand()) {
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'quests' ( " +
								  "  'id' INTEGER PRIMARY KEY, " +
				                  "  'title' TEXT NOT NULL," +
				                  "  'Description' TEXT NOT NULL," +
				                  "  'QuestGiver' TEXT NOT NULL," +
				                  "  'xp' INTEGER NOT NULL," +
				                  "  'money' INTEGER NOT NULL," +
				                  "  'completed' INTEGER NOT NULL," +
				                  "  'timesCompleted' INTEGER NOT NULL," +
				                  "  'isRepeatable' INTEGER NOT NULL," +
				                  "  'inProgress' INTEGER NOT NULL" +
				                  
								  ");";

				var result = cmd.ExecuteNonQuery();
				Debug.Log("create schema: " + result);
			}
		}
	}

	public void InsertScore(string highScoreName, int score) {
		using (var conn = new SqliteConnection(dbPath)) {
			conn.Open();
			using (var cmd = conn.CreateCommand()) {
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "INSERT INTO high_score (name, score) " +
								  "VALUES (@Name, @Score);";

				cmd.Parameters.Add(new SqliteParameter {
					ParameterName = "Name",
					Value = highScoreName
				});

				cmd.Parameters.Add(new SqliteParameter {
					ParameterName = "Score",
					Value = score
				});

				var result = cmd.ExecuteNonQuery();
				Debug.Log("insert score: " + result);
			}
		}
	}

	public void GetHighScores(int limit) {
		using (var conn = new SqliteConnection(dbPath)) {
			conn.Open();
			using (var cmd = conn.CreateCommand()) {
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "SELECT * FROM high_score ORDER BY score DESC LIMIT @Count;";

				cmd.Parameters.Add(new SqliteParameter {
					ParameterName = "Count",
					Value = limit
				});

				Debug.Log("scores (begin)");
				var reader = cmd.ExecuteReader();
				while (reader.Read()) {
					var id = reader.GetInt32(0);
					var highScoreName = reader.GetString(1);
					var score = reader.GetInt32(2);
					var text = string.Format("{0}: {1} [#{2}]", highScoreName, score, id);
					Debug.Log(text);
				}
				Debug.Log("scores (end)");
			}
		}
	}
}

