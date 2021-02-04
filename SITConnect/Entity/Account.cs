using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SITConnect.Entity
{
    public class Account
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string IV { get; set; }
        public string Key { get; set; }
        public string CCNumber { get; set; }
        public string CCExpiryDate { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LockedFrom { get; set; }
        public string OldPassword1 { get; set; }
        public string OldSalt1 { get; set; }
        public string OldPassword2 { get; set; }
        public string OldSalt2 { get; set; }
        public DateTime PasswordAge { get; set; }

        public Account()
        {

        }

        public Account(string id, string fName, string lName, string email, string dob, string password, string salt, string iv, string key, string ccNum, string ccExp, int failedLogins, DateTime? lockedFrom, string oldPass1, string oldSalt1, string oldPass2, string oldSalt2, DateTime passAge)
        {
            Id = id;
            FirstName = fName;
            LastName = lName;
            Email = email;
            DateOfBirth = dob;
            PasswordHash = password;
            PasswordSalt = salt;
            IV = iv;
            Key = key;
            CCNumber = ccNum;
            CCExpiryDate = ccExp;
            FailedLoginAttempts = failedLogins;
            LockedFrom = lockedFrom;
            OldPassword1 = oldPass1;
            OldSalt1 = oldSalt1;
            OldPassword2 = oldPass2;
            OldSalt2 = oldSalt2;
            PasswordAge = passAge;
        }

        public static void Create(Account user)
        {
            string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);

            string sql = 
                "INSERT INTO Account VALUES (@Id, @FirstName, @LastName, @Email, @DateOfBirth, @PasswordHash, @PasswordSalt ,@IV, @Key, @CCNumber, @CCExpiryDate, @FailedLoginAttempts, @LockedFrom, @PasswordAge) " +
                "INSERT INTO PasswordHistory (Id) VALUES (@Id)";

            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Id", Guid.Parse(user.Id));
            command.Parameters.AddWithValue("@FirstName", user.FirstName);
            command.Parameters.AddWithValue("@LastName", user.LastName);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
            command.Parameters.AddWithValue("@IV", user.IV);
            command.Parameters.AddWithValue("@Key", user.Key);
            command.Parameters.AddWithValue("@CCNumber", user.CCNumber);
            command.Parameters.AddWithValue("@CCExpiryDate", user.CCExpiryDate);
            command.Parameters.AddWithValue("@FailedLoginAttempts", user.FailedLoginAttempts);
            command.Parameters.AddWithValue("@LockedFrom", DBNull.Value);
            command.Parameters.AddWithValue("@PasswordAge", user.PasswordAge);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static Account RetrieveById(string id)
        {
            string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);

            string sql = "SELECT * FROM Account acc INNER JOIN PasswordHistory ph ON acc.Id = ph.Id WHERE acc.Id = @Id ";
            SqlDataAdapter command = new SqlDataAdapter(sql, connection);
            command.SelectCommand.Parameters.AddWithValue("@Id", Guid.Parse(id));

            DataSet ds = new DataSet();

            command.Fill(ds);

            Account account = null;
            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];

                // Account
                string fName = row["FirstName"].ToString();
                string lName = row["LastName"].ToString();
                string email = row["email"].ToString();
                string dob = row["DateOfBirth"].ToString();
                string password = row["PasswordHash"].ToString();
                string salt = row["PasswordSalt"].ToString();
                string iv = row["IV"].ToString();
                string key = row["Key"].ToString();
                string ccNum = row["CCNumber"].ToString();
                string ccExp = row["CCExpiryDate"].ToString();
                int failedLogins = Convert.ToInt32(row["FailedLoginAttempts"].ToString());
                DateTime? lockedFrom = null;
                if (!string.IsNullOrEmpty(row["LockedFrom"].ToString()))
                    lockedFrom = Convert.ToDateTime(row["LockedFrom"].ToString());
                DateTime passAge = Convert.ToDateTime(row["PasswordAge"].ToString());

                // Password History
                string oldPass1 = null;
                if (!string.IsNullOrEmpty(row["OldPassword1"].ToString()))
                    oldPass1 = row["OldPassword1"].ToString();
                string oldSalt1 = null;
                if (!string.IsNullOrEmpty(row["OldSalt1"].ToString()))
                    oldSalt1 = row["OldSalt1"].ToString();
                string oldPass2 = null;
                if (!string.IsNullOrEmpty(row["OldPassword2"].ToString()))
                    oldPass2 = row["OldPassword2"].ToString();
                string oldSalt2 = null;
                if (!string.IsNullOrEmpty(row["OldSalt2"].ToString()))
                    oldSalt2 = row["OldSalt2"].ToString();

                account = new Account(id, fName, lName, email, dob, password, salt, iv, key, ccNum, ccExp, failedLogins, lockedFrom, oldPass1, oldSalt1, oldPass2, oldSalt2, passAge);
            }
            return account;
        }

        public static Account RetrieveByEmail(string email)
        {
            string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);

            string sql = "SELECT * FROM Account acc INNER JOIN PasswordHistory ph ON acc.Id = ph.Id WHERE acc.Email = @Email ";
            SqlDataAdapter command = new SqlDataAdapter(sql, connection);
            command.SelectCommand.Parameters.AddWithValue("@Email", email);

            DataSet ds = new DataSet();

            command.Fill(ds);

            Account account = null;
            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];

                // Account
                string id = row["Id"].ToString();
                string fName = row["FirstName"].ToString();
                string lName = row["LastName"].ToString();
                string dob = row["DateOfBirth"].ToString();
                string password = row["PasswordHash"].ToString();
                string salt = row["PasswordSalt"].ToString();
                string iv = row["IV"].ToString();
                string key = row["Key"].ToString();
                string ccNum = row["CCNumber"].ToString();
                string ccExp = row["CCExpiryDate"].ToString();
                int failedLogins = Convert.ToInt32(row["FailedLoginAttempts"].ToString());
                DateTime? lockedFrom = null;
                if (!string.IsNullOrEmpty(row["LockedFrom"].ToString()))
                    lockedFrom = Convert.ToDateTime(row["LockedFrom"].ToString());
                DateTime passAge = Convert.ToDateTime(row["PasswordAge"].ToString());

                // Password History
                string oldPass1 = null;
                if (!string.IsNullOrEmpty(row["OldPassword1"].ToString()))
                    oldPass1 = row["OldPassword1"].ToString();
                string oldSalt1 = null;
                if (!string.IsNullOrEmpty(row["OldSalt1"].ToString()))
                    oldSalt1 = row["OldSalt1"].ToString();
                string oldPass2 = null;
                if (!string.IsNullOrEmpty(row["OldPassword2"].ToString()))
                    oldPass2 = row["OldPassword2"].ToString();
                string oldSalt2 = null;
                if (!string.IsNullOrEmpty(row["OldSalt2"].ToString()))
                    oldSalt2 = row["OldSalt2"].ToString();

                account = new Account(id, fName, lName, email, dob, password, salt, iv, key, ccNum, ccExp, failedLogins, lockedFrom, oldPass1, oldSalt1, oldPass2, oldSalt2, passAge);
            }
            return account;
        }

        public static void Update(Account user)
        {
            string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);

            string sql =
                "UPDATE Account SET PasswordHash = @PasswordHash, PasswordSalt = @PasswordSalt, FailedLoginAttempts = @FailedLoginAttempts, LockedFrom = @LockedFrom, PasswordAge = @PasswordAge WHERE Id=@Id " +
                "UPDATE PasswordHistory SET OldPassword1 = @OldPassword1, OldSalt1 = @OldSalt1, OldPassword2 = @OldPassword2, OldSalt2 = @OldSalt2 WHERE Id=@Id";
            SqlCommand command = new SqlCommand(sql, connection);


            command.Parameters.AddWithValue("@Id", Guid.Parse(user.Id));

            // Account
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
            command.Parameters.AddWithValue("@FailedLoginAttempts", user.FailedLoginAttempts);
            if (!string.IsNullOrEmpty(user.LockedFrom.ToString()))
                command.Parameters.AddWithValue("@LockedFrom", user.LockedFrom);
            else
                command.Parameters.AddWithValue("@LockedFrom", DBNull.Value);
            command.Parameters.AddWithValue("@PasswordAge", user.PasswordAge);

            // PasswordHistory
            if (!string.IsNullOrEmpty(user.OldPassword1))
                command.Parameters.AddWithValue("@OldPassword1", user.OldPassword1);
            else
                command.Parameters.AddWithValue("@OldPassword1", DBNull.Value);
            if (!string.IsNullOrEmpty(user.OldSalt1))
                command.Parameters.AddWithValue("@OldSalt1", user.OldSalt1);
            else
                command.Parameters.AddWithValue("@OldSalt1", DBNull.Value);
            if (!string.IsNullOrEmpty(user.OldPassword2))
                command.Parameters.AddWithValue("@OldPassword2", user.OldPassword2);
            else
                command.Parameters.AddWithValue("@OldPassword2", DBNull.Value);
            if (!string.IsNullOrEmpty(user.OldSalt2))
                command.Parameters.AddWithValue("@OldSalt2", user.OldSalt2);
            else
                command.Parameters.AddWithValue("@OldSalt2", DBNull.Value);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

    }
}