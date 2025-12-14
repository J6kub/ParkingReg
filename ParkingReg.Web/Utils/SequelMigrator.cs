using MySql.Data;
using MySql.Data.MySqlClient;
namespace KartverketRegister.Utils
{

    // Klasse for migrering av database
    public class SequelMigrator : SequelBase
    {
        
        public SequelMigrator(string dbIP, string dbname) : base(dbIP, dbname) { }
        public SequelMigrator() : base() { }

        public void Migrate()
        {
            conn.Open();

            using (var transaction = conn.BeginTransaction())
            {
                Console.WriteLine("[SequelMigrator] Started Migration Transaction");
                try
                {
                    SetForeingKeyCheckTransaction(0, transaction);

                    DropTableTransaction("Users_Copy", transaction);
                    DropTableTransaction("WhitelistMails_Copy", transaction);
                    DropTableTransaction("Parkings_Copy", transaction);

                    CreateTableTransaction(SequelTables.Users_Table("Users_Copy"), "Users_Copy", transaction);
                    CreateTableTransaction(SequelTables.WhitelistMails("WhitelistMails_Copy"), "WhitelistMails_Copy", transaction);
                    CreateTableTransaction(SequelTables.Parkings("Parkings_Copy"), "Parkings_Copy", transaction);

                    CopyTableDataBulkTransaction("Users", "Users_Copy", transaction);
                    CopyTableDataBulkTransaction("WhitelistMails", "WhitelistMails_Copy", transaction);
                    CopyTableDataBulkTransaction("Parkings", "Parkings_Copy", transaction);

                    DropTableTransaction("Users", transaction);
                    DropTableTransaction("WhitelistMails", transaction);
                    DropTableTransaction("Parkings", transaction);

                    CreateTableTransaction(SequelTables.Users_Table("Users"), "Users", transaction);
                    CreateTableTransaction(SequelTables.WhitelistMails("WhitelistMails"), "WhitelistMails", transaction);
                    CreateTableTransaction(SequelTables.Parkings("Parkings"), "Parkings", transaction);

                    CopyTableDataBulkTransaction("Users_Copy", "Users", transaction);
                    CopyTableDataBulkTransaction("WhitelistMails_Copy", "WhitelistMails", transaction);
                    CopyTableDataBulkTransaction("Parkings_Copy", "Parkings", transaction);

                    DropTableTransaction("Users_Copy", transaction);
                    DropTableTransaction("WhitelistMails_Copy", transaction);
                    DropTableTransaction("Parkings_Copy", transaction);

                    SetForeingKeyCheckTransaction(1, transaction);

                    // Commit the transaction
                    transaction.Commit();
                }
                catch
                {
                    // Rollback if anything fails
                    Console.WriteLine("[SequelMigrator] Failed Migration Transaction");
                    transaction.Rollback();
                    Console.WriteLine("[SequelMigrator] Transaction Rolled Back");
                    throw;
                }
                finally
                {
                    Console.WriteLine("[SequelMigrator] Completed Migration Transaction");
                    conn.Close();
                }
            }
        }
        private List<string> GetTableColumns(string tableName)
        {
            var columns = new List<string>();

            try
            {

                string query = @"
            SELECT COLUMN_NAME 
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @TableName
            ORDER BY ORDINAL_POSITION;";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TableName", tableName);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columns.Add(reader.GetString(0)); // Use ordinal 0 for COLUMN_NAME
                        }
                    }
                }
            } finally
            {

            }
            

            return columns;
        }
        
        public void CreateTable(string SQL_Table, string tableName)
        {
            using (var cmd = new MySqlCommand(SQL_Table, conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine($"[SequelMigrator] Created {tableName}");
            }
        }
        public void DropTable(string tableName)
        {
            string sqling = $"DROP TABLE IF EXISTS `{tableName}`;";
            using (var cmd = new MySqlCommand(sqling, conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine($"[SequelMigrator] Deleted {tableName}");
            }
        }
        public void SetForeingKeyCheck(int boolean) {
            string sqling = $"SET FOREIGN_KEY_CHECKS={boolean};";
            using (var cmd = new MySqlCommand(sqling, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
        public void CreateTableTransaction(string SQL_Table, string tableName, MySqlTransaction transaction)
        {
            using (var cmd = new MySqlCommand(SQL_Table, conn, transaction))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine($"[SequelMigrator] Created {tableName}");
            }
        }

        public void DropTableTransaction(string tableName, MySqlTransaction transaction)
        {
            string sqling = $"DROP TABLE IF EXISTS `{tableName}`;";
            using (var cmd = new MySqlCommand(sqling, conn, transaction))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine($"[SequelMigrator] Deleted {tableName}");
            }
        }

        public void SetForeingKeyCheckTransaction(int boolean, MySqlTransaction transaction)
        {
            string sqling = $"SET FOREIGN_KEY_CHECKS={boolean};";
            using (var cmd = new MySqlCommand(sqling, conn, transaction))
            {
                cmd.ExecuteNonQuery();
            }
        }
        public void CopyTableDataBulkTransaction(string oldTable, string newTable, MySqlTransaction transaction)
        {
            var newColumns = GetTableColumns(newTable);
            var oldColumns = GetTableColumns(oldTable);
            var commonColumns = oldColumns.FindAll(c => newColumns.Contains(c));

            if (!commonColumns.Any())
                throw new InvalidOperationException("No common columns found between tables.");

            string sqlColumns = string.Join(", ", commonColumns.Select(c => $"`{c}`"));
            string sql = $"INSERT INTO `{newTable}` ({sqlColumns}) SELECT {sqlColumns} FROM `{oldTable}`";

            using (var cmd = new MySqlCommand(sql, conn, transaction))
            {
                cmd.ExecuteNonQuery();
            }
        }

    }
}
