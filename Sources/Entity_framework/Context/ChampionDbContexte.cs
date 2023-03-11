using Entity_framework.DataBase;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Entity_framework
{
    public class ChampionsDbContexte : DbContext
    {
        public DbSet<ChampionDB> ChampionsSet { get; set; }
        public DbSet<RuneDB> RunesSet { get; set; }
        public DbSet<SkillDB> SkillSet { get; set; }
        public DbSet<SkinDB> SkinsSet { get; set; }
        public DbSet<CategoryDicDB> CategoryRunePageSet { get; set; }

        public ChampionsDbContexte()
        { }

        public ChampionsDbContexte(DbContextOptions<ChampionsDbContexte> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=../Entity_framework/Entity_framework.LolDB.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SkinDB>().ToTable("SkinsSet");
            modelBuilder.Entity<ChampionDB>().ToTable("ChampionsSet");
            modelBuilder.Entity<RuneDB>().ToTable("RunesSet");
            modelBuilder.Entity<SkillDB>().ToTable("SkillSet");
            modelBuilder.Entity<CategoryDicDB>().ToTable("CategoryDicSet");

            //Contrainte
            //modelBuilder.Entity<SkinDB>().HasKey(e => e.Id);
            modelBuilder.Entity<ChampionDB>().HasMany(champ => champ.Skins).WithOne(skin => skin.Champion).HasForeignKey(skin => skin.ChampionForeignKey);
            modelBuilder.Entity<ChampionDB>().HasIndex(champ => champ.Name).IsUnique();
            //modelBuilder.Entity<ChampionDB>().HasMany(champ => champ.Skins).WithMany(skin => skin.Champion);
            modelBuilder.Entity<RunePagesDb>().HasMany(runePage => runePage.CategoryRunePages).WithOne(categoryRunePage => categoryRunePage.runePage).HasForeignKey(categoryRunePage => categoryRunePage.runesPagesForeignKey);
            modelBuilder.Entity<CategoryDicDB>().HasOne(CategoryRunePageSet => CategoryRunePageSet.rune).WithMany(rune => rune.runesPages);



        }
    }
}