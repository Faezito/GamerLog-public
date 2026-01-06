using Z1.Model.Email;
using Z3.DataAccess.Database;

namespace Z3.DataAccess
{
    public interface IEmailDataAccess
    {
        Task<EmailConfig> Obter(int id);
    }
    public class EmailDataAccess : IEmailDataAccess
    {
        private readonly IDapper _dapper;

        public EmailDataAccess(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<EmailConfig> Obter(int id)
        {
            try
            {
                string sql = @"
SELECT [ID]
      ,[Server]
      ,[Port]
      ,[UseSSL]
      ,[UseStartTls]
      ,[FromName]
      ,[Username]
      ,[Password]
FROM [dbo].[ServicosEmail] WITH(NOLOCK)
WHERE id = @id
";
                var obj = new
                {
                    id = id
                };

                return await _dapper.QuerySingleOrDefaultAsync<EmailConfig>(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}