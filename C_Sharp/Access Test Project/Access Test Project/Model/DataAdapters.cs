using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Access_Test_Project.Model
{
    public static class DataAdapters{

        public static OleDbDataAdapter UserDataAdapter(OleDbConnection connection)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT UserID, UserName, Mail, Password FROM Users", connection);

            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            // Create the Insert, Update and Delete commands.
            adapter.InsertCommand = new OleDbCommand(
                "INSERT INTO Users ([UserID], [UserName], [Mail], [Password]) " +
                "VALUES (?, ?, ?, ?)");
            adapter.InsertCommand.Connection = connection;

            adapter.UpdateCommand = new OleDbCommand(
                "UPDATE Users SET [UserID] = ?, [UserName] = ?, [Mail] = ?, [Password] = ? " + 
                "WHERE [UserID] = ?");
            adapter.UpdateCommand.Connection = connection;

            adapter.DeleteCommand = new OleDbCommand(
                "DELETE FROM Users WHERE [UserID] = ?");
            adapter.DeleteCommand.Connection = connection;

            // Create the parameters.
            adapter.InsertCommand.Parameters.Add("@UserID",
                OleDbType.Char, 5, "UserID");
            adapter.InsertCommand.Parameters.Add("@UserName",
                OleDbType.VarChar, 40, "UserName");
            adapter.InsertCommand.Parameters.Add("@Mail",
                OleDbType.VarChar, 40, "Mail");
            adapter.InsertCommand.Parameters.Add("@Password",
                OleDbType.VarChar, 40, "Password");

            adapter.UpdateCommand.Parameters.Add("@UserID",
                OleDbType.Char, 5, "UserID");
            adapter.UpdateCommand.Parameters.Add("@UserName",
                OleDbType.VarChar, 40, "UserName");
            adapter.UpdateCommand.Parameters.Add("@Mail",
                OleDbType.VarChar, 40, "Mail");
            adapter.UpdateCommand.Parameters.Add("@Password",
                OleDbType.VarChar, 40, "Password");
            adapter.UpdateCommand.Parameters.Add("@oldUserID",
                OleDbType.Char, 5, "UserID").SourceVersion =
                DataRowVersion.Original;

            adapter.DeleteCommand.Parameters.Add("@UserID",
                OleDbType.Char, 5, "UserID").SourceVersion =
                DataRowVersion.Original;

            return adapter;
        }

        public static OleDbDataAdapter NewAdapter(OleDbConnection connection, string tableName, string indexName, List<Tuple<string, OleDbType, int>> fieldNames)
        {
            string selectCommandText = "SELECT " + indexName;
            string insertCommandText = "INSERT INTO " + tableName + " ([" + indexName;
            string updateCommandText = "UPDATE " + tableName + " SET [" + indexName + "] = ?";
            string deleteCommandText = "DELETE FROM " + tableName + " WHERE [" + indexName + "] = ?";
            string createCommandText = "CREATE TABLE ["+tableName+"]( ["+indexName+"] "+OleDbType.Integer;
            foreach (Tuple<string, OleDbType, int> fieldName in fieldNames)
            {
                selectCommandText += ", " + fieldName.Item1;
                insertCommandText += "], [" + fieldName.Item1;
                updateCommandText += ", [" + fieldName.Item1 + "] = ?";
                createCommandText += ", [" + fieldName.Item1 + "] " + fieldName.Item2;
            }
            selectCommandText += " FROM " + tableName;
            insertCommandText += "]) VALUES (?";
            updateCommandText += " WHERE [" + indexName + "] = ?";
            createCommandText += " )";
            foreach (Tuple<string, OleDbType, int> fieldName in fieldNames)
            {
                insertCommandText += ", ?";
            }
            insertCommandText += ")";

            OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommandText, connection);

            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            // Create the Insert, Update and Delete commands.
            adapter.InsertCommand = new OleDbCommand(insertCommandText);
            adapter.InsertCommand.Connection = connection;

            adapter.UpdateCommand = new OleDbCommand(updateCommandText);
            adapter.UpdateCommand.Connection = connection;

            adapter.DeleteCommand = new OleDbCommand(deleteCommandText);
            adapter.DeleteCommand.Connection = connection;

            

            // Create the parameters.
            adapter.InsertCommand.Parameters.Add("@"+indexName,
                OleDbType.Integer, 5, indexName);

            adapter.UpdateCommand.Parameters.Add("@" +indexName,
                OleDbType.Integer, 5, indexName);

            foreach (Tuple<string, OleDbType, int> fieldName in fieldNames)
            {
                adapter.InsertCommand.Parameters.Add("@" + fieldName.Item1,
                fieldName.Item2, fieldName.Item3, fieldName.Item1);
                adapter.UpdateCommand.Parameters.Add("@" + fieldName.Item1,
                fieldName.Item2, fieldName.Item3, fieldName.Item1);
            }

            adapter.UpdateCommand.Parameters.Add("@old"+indexName,
                OleDbType.Integer, 5, indexName).SourceVersion =
                DataRowVersion.Original;

            adapter.DeleteCommand.Parameters.Add("@"+indexName,
                OleDbType.Integer, 5, indexName).SourceVersion =
                DataRowVersion.Original;

            return adapter;
        }
    }
}
