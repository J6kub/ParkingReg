
using ParkingReg.Auth;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Linq;

namespace ParkingReg.Utils
{
	// initiering av database struktur. Lager Database navn, og Sjekker om tabeller eksisterer og lager dem om de ikke gjør det.
    public class SequelInit
    {
        public MySqlConnection conn;
        public string dbConnString;

        public SequelInit(string dbIP, string dbname)
        {

            string rootConnString = $"Server={dbIP};Port={Constants.DataBasePort};User ID=root;Password={Constants.DataBaseRootPassword};";
            using (var rootConn = new MySqlConnection(rootConnString))
            {
                rootConn.Open();
                using (var cmd = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS `{dbname}`;", rootConn))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            // Step 2: Initialize class-level connection to the target database
            dbConnString = $"Server={dbIP};Port={Constants.DataBasePort};Database={dbname};User ID=root;Password={Constants.DataBaseRootPassword};";
            conn = new MySqlConnection(dbConnString);

        }
        public bool TableExists(string tableName)
        {
            string query = "SHOW TABLES LIKE @tableName;";
            bool result = false;
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@tableName", tableName);
                using (var reader = cmd.ExecuteReader())
                {
                    result = reader.HasRows;
                }
            }
            return result;

        }
        
        public void InitDb(bool DoMigration) {
            SequelMigrator seq = new SequelMigrator();
            seq.Open();
            List<string> TablesToCreate = new List<string> { "Users", "WhitelistMails", "Parkings", "Vtk"};
            foreach (var tblName in TablesToCreate)
            {
                if (!TableExists(tblName))
                {
                    switch (tblName)
                    {
                        case "Users":
                            seq.CreateTable(SequelTables.Users_Table(tblName), tblName);
                            break;
                        case "WhitelistMails":
                            seq.CreateTable(SequelTables.WhitelistMails(tblName), tblName);
                            break;
                        case "Parkings":
                            seq.CreateTable(SequelTables.Parkings(tblName), tblName);
                            break;
                        case "Vtk":
                            seq.CreateTable(SequelTables.Vtk(tblName), tblName);
                            break;


                    }
                }
            }
            seq.Close();
            if (DoMigration)
            {
                seq.Migrate();
            }

        }

    }
}