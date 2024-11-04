using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

public class DatabaseManager
{
    private static DatabaseManager instance;
    private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Directory.GetCurrentDirectory() + "\\Database.mdf;Integrated Security=True";

    private DatabaseManager() { }

    public static DatabaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DatabaseManager();
            }
            return instance;
        }
    }

    public void OpenConnection(SqlConnection connection)
    {
        if (connection.State == ConnectionState.Closed)
        {
            connection.Open();
        }
    }

    public void CloseConnection(SqlConnection connection)
    {
        if (connection.State == ConnectionState.Open)
        {
            connection.Close();
        }
    }

    public DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
    {
        DataTable dataTable = new DataTable();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Mở kết nối
                OpenConnection(connection);

                // Thêm các tham số vào SqlCommand
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    try
                    {
                        adapter.Fill(dataTable);
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("Error executing query: " + ex.Message);
                    }
                }
            }
        }
        return dataTable;
    }


    public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                OpenConnection(connection);
                try
                {
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("Error executing non-query: " + ex.Message);
                    return -1; // Indicate failure
                }
            }
        }
    }

    public object ExecuteScalar(string query, params SqlParameter[] parameters)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                OpenConnection(connection);
                try
                {
                    return command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("Error executing scalar: " + ex.Message);
                    return null; // Indicate failure
                }
            }
        }
    }

    public void ShowErrorMessage(string message)
    {
        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public void ShowSuccessMessage(string message)
    {
        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
