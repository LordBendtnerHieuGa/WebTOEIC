using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebToeic.WebAppMVC.Data.Configurations;
using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.WebToeic.Data.Extension;

namespace WebToeic.WebAppMVC.Data.EFCore
{
    public class WebToeicDbContext : IdentityDbContext<User>
    {
        public WebToeicDbContext(DbContextOptions<WebToeicDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LAPTOP-GINGGKVH;Database=WEB_TOEIC_DB;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.ApplyConfiguration(new CommentGrammarConfiguration());
            modelBuilder.ApplyConfiguration(new CommentListenConfiguration());
            modelBuilder.ApplyConfiguration(new CommentReadConfiguration());
            modelBuilder.ApplyConfiguration(new CommentVocConfiguration());

            modelBuilder.ApplyConfiguration(new VocabularyConfiguration());
            modelBuilder.ApplyConfiguration(new VocabularyContentConfiguration());
            modelBuilder.ApplyConfiguration(new TestConfiguration());
            modelBuilder.ApplyConfiguration(new TestQuestionConfiguration());
            modelBuilder.ApplyConfiguration(new ReadingConfiguration());
            modelBuilder.ApplyConfiguration(new ReadingQuestionConfiguration());
            modelBuilder.ApplyConfiguration(new ListeningConfiguration());
            modelBuilder.ApplyConfiguration(new ListeningQuestionConfiguration());
            modelBuilder.ApplyConfiguration(new GrammarConfiguration());
            modelBuilder.ApplyConfiguration(new ResultConfiguration());
            modelBuilder.ApplyConfiguration(new SlideBannerConfiguration());

            /*modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles").HasKey(x => new { x.UserId, x.RoleId});
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens").HasKey(x => x.UserId);*/

            // Data Seeding
            modelBuilder.Seed();

            base.OnModelCreating(modelBuilder);
        }

        /*public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }*/

        public DbSet<CommentGrammar> Comments { get; set; }
        public DbSet<CommentListening> CommentListens { get; set; }
        public DbSet<CommentReading> CommentReadings { get; set; }
        public DbSet<CommentVocabulary> CommentVocabularies { get; set; }

        public DbSet<Vocabulary> Vocabularies { get; set; }
        public DbSet<VocabularyContent> VocabularyContents {get; set;} 
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestQuestion> TestQuestions { get; set; }
        public DbSet<Reading> Readings { get; set; }
        public DbSet<ReadingQuestion> ReadingQuestions { get; set; }
        public DbSet<Listening> Listenings { get; set; }
        public DbSet<ListeningQuestion> ListeningQuestions { get; set; }
        public DbSet<Grammar> Grammars { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<SlideBanner> SlideBanners { get; set; }
    }
}
