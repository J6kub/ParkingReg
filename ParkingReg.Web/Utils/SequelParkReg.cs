using MySql.Data;
using MySql.Data.MySqlClient;
namespace KartverketRegister.Utils
{

    // Klasse for migrering av database
    public class SequelParkReg : SequelBase
    {
        
        public SequelParkReg(string dbIP, string dbname) : base(dbIP, dbname) { }
        public SequelParkReg() : base() { }

        
    }
}
