using DiagNexusApi.Model;
using MySqlConnector;
using System.Threading.Tasks;

namespace DiagNexusApi.Data
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            _connectionString = "We will put connection string here";
        }
        
        public async Task<bool> VerifyUserAsync(string username, string password)
        {
            const string query = @"SELECT COUNT(*) FROM user 
                                   WHERE Name = @username AND Password = @password AND IsActive = 1";

            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            var result = (long)await command.ExecuteScalarAsync();
            return result > 0;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            const string query = "SELECT * FROM user WHERE IsActive = 1";
            var users = new List<User>();
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = new MySqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var user = new User
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Password = reader.GetString("Password"),
                    Email = reader.GetString("Email"),
                    IsActive = reader.GetBoolean("IsActive"),
                    Role = reader.GetString("Role"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt")
                };
                users.Add(user);
            }
            return users;
        }

        // Get all reports
        public async Task<List<Report>> GetAllReportsAsync()
        {
            const string query = @"SELECT ReportId, UserId, Name, Path, UpdatedBy, UpdatedDate, IsActive, PatientName FROM report";
            var reports = new List<Report>();
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = new MySqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var report = new Report
                {
                    ReportId = reader.GetInt32("ReportId"),
                    UserId = reader.GetInt32("UserId"),
                    Name = reader.GetString("Name"),
                    Path = reader.GetString("Path"),
                    UpdatedBy = reader.GetInt32("UpdatedBy"),
                    UpdatedDate = reader.GetDateTime("UpdatedDate"),
                    IsActive = reader.GetBoolean("IsActive"),
                    PatientName = reader.GetString("PatientName")
                };
                reports.Add(report);
            }
            return reports;
        }

        // Get report(s) by user id
        public async Task<List<Report>> GetReportsByUserAsync(int userId)
        {
            const string query = @"SELECT ReportId, UserId, Name, Path, UpdatedBy, UpdatedDate, IsActive, PatientName 
                                   FROM report WHERE UserId = @userId";
            var reports = new List<Report>();
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var report = new Report
                {
                    ReportId = reader.GetInt32("ReportId"),
                    UserId = reader.GetInt32("UserId"),
                    Name = reader.GetString("Name"),
                    Path = reader.GetString("Path"),
                    UpdatedBy = reader.GetInt32("UpdatedBy"),
                    UpdatedDate = reader.GetDateTime("UpdatedDate"),
                    IsActive = reader.GetBoolean("IsActive"),
                    PatientName = reader.GetString("PatientName")
                };
                reports.Add(report);
            }
            return reports;
        }

        // Get report by report id
        public async Task<Report?> GetReportByIdAsync(int reportId)
        {
            const string query = @"SELECT ReportId, UserId, Name, Path, UpdatedBy, UpdatedDate, IsActive, PatientName 
                                   FROM report WHERE ReportId = @reportId";
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@reportId", reportId);
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Report
                {
                    ReportId = reader.GetInt32("ReportId"),
                    UserId = reader.GetInt32("UserId"),
                    Name = reader.GetString("Name"),
                    Path = reader.GetString("Path"),
                    UpdatedBy = reader.GetInt32("UpdatedBy"),
                    UpdatedDate = reader.GetDateTime("UpdatedDate"),
                    IsActive = reader.GetBoolean("IsActive"),
                    PatientName = reader.GetString("PatientName")
                };
            }
            return null;
        }

        // Add a new report
        public async Task<int> AddReportAsync(Report report)
        {
            const string query = @"INSERT INTO report 
                (UserId, Name, Path, UpdatedBy, UpdatedDate, IsActive, PatientName)
                VALUES (@UserId, @Name, @Path, @UpdatedBy, @UpdatedDate, @IsActive, @PatientName);
                SELECT LAST_INSERT_ID();";

            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", report.UserId);
            command.Parameters.AddWithValue("@Name", report.Name);
            command.Parameters.AddWithValue("@Path", report.Path);
            command.Parameters.AddWithValue("@UpdatedBy", report.UpdatedBy);
            command.Parameters.AddWithValue("@UpdatedDate", report.UpdatedDate);
            command.Parameters.AddWithValue("@IsActive", report.IsActive);
            command.Parameters.AddWithValue("@PatientName", report.PatientName);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

    }
}