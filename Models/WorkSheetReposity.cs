using Dapper;
using Andyyy.Models;
using System.Data;

namespace Andyyy.Models
{
    public class WorkSheetReposity
    {
        private readonly DapperContext _context;
        public WorkSheetReposity(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkSheet>> GetWorkSheets()
        {
            var query = "SELECT * FROM WorkSheet";
            using (var connection = _context.CreateConnection())
            {
                var worksheets = await connection.QueryAsync<WorkSheet>(query);
                return worksheets.ToList();
            }
        }

        public async Task<WorkSheet> GetWorkSheet(int id)
        {
            var query = "SELECT * FROM WorkSheet WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                var company = await connection.QuerySingleOrDefaultAsync<WorkSheet>(query, new { id });
                return company;
            }
        }

        public async Task<WorkSheet> CreateWorkSheet(WorkSheet worksheet)
        {
            var query = "INSERT INTO WorkSheet (Name, CreateDateTime, UpDateTime) VALUES (@Name, @CreateDateTime, @UpDateTime)" +
                "SELECT CAST(SCOPE_IDENTITY() AS int)";
            var parameters = new DynamicParameters();
            parameters.Add("Name", worksheet.Name, DbType.String);
            parameters.Add("CreateDateTime", worksheet.CreateDateTime, DbType.DateTime);
            parameters.Add("UpDateTime", worksheet.UpDateTime, DbType.DateTime);
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
                var createdWorkSheet = new WorkSheet
                {
                    Id = id,
                    Name = worksheet.Name,
                    CreateDateTime = worksheet.CreateDateTime,
                    UpDateTime = worksheet.UpDateTime
                };
                return createdWorkSheet;
            }
        }

        public async Task UpdateWorkSheet(int id, WorkSheet worksheet)
        {
            var query = "UPDATE WorkSheet SET Name = @Name, CreateDateTime = @CreateDateTime, UpDateTime = @UpDateTime WHERE Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Name", worksheet.Name, DbType.String);
            parameters.Add("CreateDateTime", worksheet.CreateDateTime, DbType.DateTime);
            parameters.Add("UpDateTime", worksheet.UpDateTime, DbType.DateTime);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
        public async Task DeleteWorkSheet(int id)
        {
            var query = "DELETE FROM WorkSheet WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }
    }
}