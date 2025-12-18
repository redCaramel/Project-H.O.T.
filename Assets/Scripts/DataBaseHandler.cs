using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System;
using TMPro;
public class DataBaseHandler : MonoBehaviour
{
    public int usingType;
    int temp;
    public GameObject text;
    void Start()
    {
        HandelSQL();
    }
    void Update()
    {
        if (temp != usingType)
        {
            HandelSQL();
        }
    }
    private void HandelSQL()
    {
        temp = usingType;
        string dbname = "/testSQL.db";
        string connectionStr = "URI=file:" + Application.streamingAssetsPath + dbname;
        IDbConnection dbConnection = new SqliteConnection(connectionStr);
        dbConnection.Open();

        if (usingType == 0) scoreBoard(dbConnection, "misson1", "점");
        else if (usingType == 1) scoreBoard(dbConnection, "misson2", "점");
        else if (usingType == 2) scoreBoard(dbConnection, "misson3", "초", true);
        else if (usingType == 3) scoreBoard(dbConnection, "speed", "초", true);
        else if (usingType == 4)
        {
            if(ResultShower.instance.playerScoreInt == -1)
            {
                ResultShower.instance.Fail();
                scoreBoard(dbConnection, "misson1", "점");
            }
            else
            {
                int idx = resultInt(dbConnection, "misson1", ResultShower.instance.playerScoreInt);
                if (idx > 0)
                {
                    ResultShower.instance.success();
                    addToScoreInt(connectionStr, "misson1", ResultShower.instance.playerScoreInt, ResultShower.instance.playerName, idx);
                    scoreBoard(dbConnection, "misson1", "점", false, idx);
                }
                else
                {
                    ResultShower.instance.notBoard();
                    scoreBoard(dbConnection, "misson1", "점");
                }
            }
        }
        else if (usingType == 5)
        {
            if(ResultShower.instance.playerScoreInt == -1)
            {
                ResultShower.instance.Fail();
                scoreBoard(dbConnection, "misson2", "점");
            }
            else
            {
                int idx = resultInt(dbConnection, "misson2", ResultShower.instance.playerScoreInt);
                if (idx > 0)
                {
                    ResultShower.instance.success();
                    addToScoreInt(connectionStr, "misson2", ResultShower.instance.playerScoreInt, ResultShower.instance.playerName, idx);
                    scoreBoard(dbConnection, "misson2", "점", false, idx);
                }
                else
                {
                    ResultShower.instance.notBoard();
                    scoreBoard(dbConnection, "misson2", "점");
                }
            }
        }
        else if (usingType == 6)
        {
            if(ResultShower.instance.playerScoreFloat == -1)
            {
                ResultShower.instance.Fail();
                scoreBoard(dbConnection, "misson3", "초");
            }
            else
            {
                int idx = resultFloat(dbConnection, "misson3", ResultShower.instance.playerScoreFloat);
                if (idx > 0)
                {
                    ResultShower.instance.success();
                    addToScoreFloat(connectionStr, "misson3", ResultShower.instance.playerScoreFloat, ResultShower.instance.playerName, idx);
                    scoreBoard(dbConnection, "misson3", "초", true, idx);
                }
                else
                {
                    ResultShower.instance.notBoard();
                    scoreBoard(dbConnection, "misson3", "초", true);
                }
            }
        }
        else if (usingType == 7)
        {
            if(ResultShower.instance.playerScoreFloat == -1)
            {
                ResultShower.instance.Fail();
                scoreBoard(dbConnection, "speed", "초");
            }
            else
            {
                int idx = resultFloat(dbConnection, "speed", ResultShower.instance.playerScoreFloat);
                if (idx > 0)
                {
                    ResultShower.instance.success();
                    addToScoreFloat(connectionStr, "speed", ResultShower.instance.playerScoreFloat, ResultShower.instance.playerName, idx);
                    scoreBoard(dbConnection, "speed", "초", true, idx);
                }
                else
                {
                    ResultShower.instance.notBoard();
                    scoreBoard(dbConnection, "speed", "초", true);
                }
            }
        }
    }

    private void scoreBoard(IDbConnection dbConnection, string tableName, string type, bool isFloat = false, int idx = -1)
    {
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "SELECT * FROM " + tableName;
        IDataReader dataReader = dbCommand.ExecuteReader();
        string inp = "";
        while (dataReader.Read())
        {
            int No = dataReader.GetInt32(0);    
            string name = dataReader.GetString(1);
            if (isFloat)
            {
                float score = dataReader.GetFloat(2);
                inp += No + " - " + name + " " + score.ToString("n2") + type;
                if(idx==No)
                {
                    inp += "  <- New Record!";
                }
                inp += "\n";
            }
            else
            {
                int score = dataReader.GetInt32(2);
                inp += No + " - " + name + " " + score + type ;
                if(idx==No)
                {
                    inp += "  <- New Record!";
                }
                inp += "\n";
            }
        }
        text.GetComponent<TextMeshPro>().text = inp;
        dataReader.Close();
    }
    private int resultInt(IDbConnection dbConnection, string tableName, int score)
    {
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "SELECT * FROM " + tableName;
        IDataReader dataReader = dbCommand.ExecuteReader();
        int N = 0;
        while (dataReader.Read())
        {
            N = N + 1;
            int No = dataReader.GetInt32(0);
            string name = dataReader.GetString(1);
            int sc = dataReader.GetInt32(2);
            if (sc < score)
            {
                dataReader.Close();
                return No;
            }

        }
        
        dataReader.Close();
        if(N < 10) return N+1;
        return 0;
    }
    private int resultFloat(IDbConnection dbConnection, string tableName, float score)
    {
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "SELECT * FROM " + tableName;
        IDataReader dataReader = dbCommand.ExecuteReader();
        int N = 0;
        while (dataReader.Read())
        {
            N = N + 1;
            int No = dataReader.GetInt32(0);
            string name = dataReader.GetString(1);
            float sc = dataReader.GetFloat(2);
            if (sc > score)
            {
                dataReader.Close();
                return No;
            }
        }
        dataReader.Close();
        if(N < 10) return N+1;
        return 0;
    }
    private void addToScoreInt(string path, string tableName, int score, string name, int idx)
{
    using (IDbConnection connection = new SqliteConnection(path))
    {
        connection.Open();

        using (IDbCommand cmd = connection.CreateCommand())
        {
            // 1. 기존 No 밀기
            cmd.CommandText = $"UPDATE {tableName} SET No = No + 1 WHERE No >= @insertNo;";
            cmd.Parameters.Add(new SqliteParameter("@insertNo", idx));
            cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();

            // 2. 새 랭킹 삽입
            cmd.CommandText = $"INSERT INTO {tableName} (No, name, score) VALUES (@no, @name, @score);";
            cmd.Parameters.Add(new SqliteParameter("@no", idx));
            cmd.Parameters.Add(new SqliteParameter("@name", name));
            cmd.Parameters.Add(new SqliteParameter("@score", score));
            cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();

            // 3. 정렬된 데이터 다시 불러오기
            cmd.CommandText = $"SELECT name, score FROM {tableName} ORDER BY score DESC;";
            IDataReader reader = cmd.ExecuteReader();

            // 데이터를 메모리로 복사
            var list = new List<(string name, int score)>();
            while (reader.Read())
            {
                list.Add((reader.GetString(0), reader.GetInt32(1)));
            }
            reader.Close();

            // 4. 전체 No를 1부터 다시 배정
            cmd.CommandText = $"DELETE FROM {tableName};";
            cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                cmd.CommandText = $"INSERT INTO {tableName} (No, name, score) VALUES (@no, @name, @score);";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqliteParameter("@no", i + 1));
                cmd.Parameters.Add(new SqliteParameter("@name", list[i].name));
                cmd.Parameters.Add(new SqliteParameter("@score", list[i].score));
                cmd.ExecuteNonQuery();
            }
        }

        connection.Close();
    }
}
private void addToScoreFloat(string path, string tableName, float score, string name, int idx)
{
    using (IDbConnection connection = new SqliteConnection(path))
    {
        connection.Open();

        using (IDbCommand cmd = connection.CreateCommand())
        {
            // 1. 기존 No 밀기
            cmd.CommandText = $"UPDATE {tableName} SET No = No + 1 WHERE No >= @insertNo;";
            cmd.Parameters.Add(new SqliteParameter("@insertNo", idx));
            cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();

            // 2. 새 랭킹 삽입
            cmd.CommandText = $"INSERT INTO {tableName} (No, name, score) VALUES (@no, @name, @score);";
            cmd.Parameters.Add(new SqliteParameter("@no", idx));
            cmd.Parameters.Add(new SqliteParameter("@name", name));
            cmd.Parameters.Add(new SqliteParameter("@score", score));
            cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();

            // 3. 정렬된 데이터 다시 불러오기
            cmd.CommandText = $"SELECT name, score FROM {tableName} ORDER BY score ASC;";
            IDataReader reader = cmd.ExecuteReader();

            // 데이터를 메모리로 복사
            var list = new List<(string name, float score)>();
            while (reader.Read())
            {
                list.Add((reader.GetString(0), reader.GetFloat(1)));
            }
            reader.Close();

            // 4. 전체 No를 1부터 다시 배정
            cmd.CommandText = $"DELETE FROM {tableName};";
            cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                cmd.CommandText = $"INSERT INTO {tableName} (No, name, score) VALUES (@no, @name, @score);";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqliteParameter("@no", i + 1));
                cmd.Parameters.Add(new SqliteParameter("@name", list[i].name));
                cmd.Parameters.Add(new SqliteParameter("@score", list[i].score));
                cmd.ExecuteNonQuery();
            }
        }

        connection.Close();
    }
}
    public void switchType(int type)
    {
        usingType = type;
    }
}
