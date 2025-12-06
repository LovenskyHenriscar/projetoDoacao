using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;

namespace ProjetoDoacao.Persistencia
{
    public class ConexaoDB
    {
        private readonly string _connectionString;

        public ConexaoDB(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new ArgumentNullException(nameof(configuration), "Connection string não pode ser nula");
        }

        public SqliteConnection Conexao()
        {
            var conexao = new SqliteConnection(_connectionString);
            conexao.Open();
            return conexao;
        }
    }
}
