using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CountryImageCurrency
{
    public static class ConnectString
    {
        public static SqlConnection ConnectLocal()
        {
            String LocalDb = "Server=.;Database=EmakroDb;Integrated Security = True; MultipleActiveResultSets = True";
                        
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = LocalDb;

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            return connection;

        }
    }
}
