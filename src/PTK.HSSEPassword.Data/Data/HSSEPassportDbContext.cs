using Microsoft.EntityFrameworkCore;
using PTK.HSSEPassport.Api.Data.Dao;
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Data.Dao.Sessions;
using PTK.HSSEPassport.Api.Data.Dao.Transaction;

namespace PTK.HSSEPassport.Api.Data.Data
{
    public class HSSEPassportDbContext : DbContext
    {

        public HSSEPassportDbContext(DbContextOptions<HSSEPassportDbContext> options) : base(options)
        {
        }

        public DbSet<FileUpload> FileUploads { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        #region MasterData
        public DbSet<Pertamina> Pertamina { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Config> Config { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Element> Element { get; set; }
        public DbSet<User> User { get; set; }

        #endregion

        #region Transaction
        public DbSet<Passport> Passport { get; set; }
        public DbSet<Fraud> Fraud { get; set; }
        public DbSet<Test> Test { get; set; }
        public DbSet<TestDetail> TestDetail { get; set; }

        #endregion

        #region Sessions
        public DbSet<UserSession> UserSessions { get; set; }
        #endregion
    }
}


//add-migration "add email tabel user" -Context HSSEPassportDbContext -Output Data/Migrations
//update-database -Context HSSEPassportDbContext