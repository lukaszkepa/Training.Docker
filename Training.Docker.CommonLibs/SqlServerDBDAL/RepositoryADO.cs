using System;
using System.Data;
using System.Data.SqlClient;

namespace Training.Docker.CommonLibs.SqlServerDBDAL
{
    public class RepositoryADO : IDisposable
    {
        private string _connectionString = String.Empty;
        private SqlConnection _conn = null;
        private bool _isInstantiatedCorrectly = false;
        private bool _isDisposed = false;
        public RepositoryADO(string connectionString)
        {
            this._connectionString = connectionString;
            this._conn = new SqlConnection();
            this._conn.ConnectionString = this._connectionString;
            try
            {
                this._conn.Open();
                if (this._conn.State == ConnectionState.Open)
                    this._isInstantiatedCorrectly = true;
            }
            catch
            {}
        }

        public void InsertCustomerOrderAggregatedData(string customerName, DateTime orderPlacementDate, Decimal totalPrice)
        {
            if ((this._isInstantiatedCorrectly) && (!this._isDisposed))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = this._conn;
                    cmd.CommandText = "insert into OrdersAggregatedData (CustomerName, OrderPlacementDate, TotalPrice) values (@CustomerName, @OrderPlacementDate, @TotalPrice)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar).Value = customerName;
                    cmd.Parameters.Add("@OrderPlacementDate", SqlDbType.DateTime2).Value = orderPlacementDate;
                    cmd.Parameters.Add("@TotalPrice", SqlDbType.Money).Value = totalPrice;
                    cmd.ExecuteNonQuery();
                }
                catch
                {}
            }
        }

        ~RepositoryADO()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected void Dispose(bool isExpliciteInvoked)
        {
            if (!this._isDisposed)
            {
                try
                {
                    if ((this._conn != null) && (this._conn.State == ConnectionState.Open))
                        this._conn.Close();
                }
                catch
                {
                    this._conn = null;
                }

                if (isExpliciteInvoked)
                    GC.SuppressFinalize(this);

                this._isDisposed = true;
            }
        }        
    }
}