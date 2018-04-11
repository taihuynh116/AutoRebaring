namespace AutoRebaring.Database.BIM_PORTAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.SqlClient;
    using System.Collections.Generic;

    public partial class BIM_PORTALDbContext:DbContext
    {
        string ConnectionString = "data source=118.69.224.199,1444;initial catalog=BIM_PORTAL;persist security info=True;user id=taiht;password=Skarner116!;MultipleActiveResultSets=True;";
        string Procedure = "SP_TaiHT_getAllProject";
        public BIM_PORTALDbContext() : base("data source=118.69.224.199,1444;initial catalog=BIM_PORTAL;persist security info=True;user id=taiht;password=Skarner116!;MultipleActiveResultSets=True;App=EntityFramework")
        {
            Projects = new List<Project>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {   //open connection
                connection.Open();
                string query = "exec " + Procedure;
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection = connection;
                command.CommandText = query;

                System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Projects.Add(new Project(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                }
                connection.Close();
            }
        }
        public int Login(string username, string password, int projectId)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password),
                new SqlParameter("@InProject_Id", projectId)
            };
            return Database.SqlQuery<int>("Sp_TaiHT_Account_LoginEncrypted @Username, @Password, @InProject_Id", sqlParams).SingleOrDefault();
        }
        public List<Project> Projects { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
