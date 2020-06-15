﻿using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SQLite;
using Tarscord.Persistence.Helpers;

namespace Tarscord.Persistence
{
    public class DatabaseConnection : IDatabaseConnection
    {
        public IDbConnection Connection { get; }

        public DatabaseConnection(IConfigurationRoot configuration)
        {
            string dbPath = configuration["database-file"];

            Connection = new SQLiteConnection($"Data Source={dbPath}");

            // If the database file exists, don't create a new one
            if (System.IO.File.Exists(dbPath)) return;

            OpenAndSetupDatabase();
        }

        private void OpenAndSetupDatabase()
        {
            Connection.Open();

            string setupQuery = DatabaseSetupQueries.GetSetupQuery();
            Connection.Execute(setupQuery);
        }
    }
}