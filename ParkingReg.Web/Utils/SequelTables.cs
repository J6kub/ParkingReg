namespace ParkingReg.Utils
{
    //SQL-skript for opprettelse av tabeller i databasen
    public static class SequelTables
    {
        public static string Users_Table(string tableName)
        {
            return @$"
            CREATE TABLE {tableName} (
                UserId        INT AUTO_INCREMENT PRIMARY KEY,
                Name          VARCHAR(100) NOT NULL,
                LastName      VARCHAR(100) NOT NULL,
                FirstName     VARCHAR(100) NOT NULL,
                UserType      ENUM('User','Admin','Employee') NOT NULL DEFAULT 'User',
                
                CreatedAt     DATETIME DEFAULT CURRENT_TIMESTAMP,

                NormalizedName     VARCHAR(100),
                PasswordHash       VARCHAR(500),
                Email              VARCHAR(255),
                NormalizedEmail    VARCHAR(255),
                SecurityStamp      VARCHAR(100),
                ConcurrencyStamp   VARCHAR(100),
                UserName           VARCHAR(100)
            );";
        }

        public static string WhitelistMails(string tableName)
        {
            return @$"
            CREATE TABLE {tableName} (
                Id          INT AUTO_INCREMENT PRIMARY KEY,
                Email       VARCHAR(200) NOT NULL UNIQUE,
                Address     VARCHAR(200),
                Name        VARCHAR(100),
                Active      BOOLEAN DEFAULT TRUE
                
            );";
        }

        public static string Parkings(string tableName)
        {
            return @$"
            CREATE TABLE {tableName} (
                Id          INT AUTO_INCREMENT PRIMARY KEY,
                Regnr       VARCHAR(10) NOT NULL,
                EmailId     INT,
                FromDate    DATETIME NOT NULL,
                ToDate      DATETIME NOT NULL,
                Active      BOOLEAN DEFAULT TRUE,
                FOREIGN KEY (EmailId) REFERENCES WhitelistMails(Id) ON DELETE SET NULL
            );";
        }

        public static string Vtk(string tableName)
        {
            return @$"
            CREATE TABLE {tableName} (
                Id          INT AUTO_INCREMENT PRIMARY KEY,
                Emailid     INT,
                Token       VARCHAR(30) NOT NULL UNIQUE,
                Valid       BOOLEAN DEFAULT TRUE,

                FOREIGN KEY (Emailid) REFERENCES WhitelistMails(Id) ON DELETE SET NULL
            );";
        }

        public static string Notifications_Table(string tableName)
        {
            return @$"
            CREATE TABLE {tableName} (
                NotificationId INT AUTO_INCREMENT PRIMARY KEY,
                UserId INT NOT NULL,
                MarkerId INT NULL,
                Message VARCHAR(300) NOT NULL,
                Date DATETIME DEFAULT CURRENT_TIMESTAMP,
                IsRead BOOLEAN DEFAULT FALSE,
                Type ENUM('Info', 'Warning', 'ReviewAssigned', 'MarkerAccepted', 'MarkerRejected') DEFAULT 'Info',

                FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
            );";
        }
    }
}
