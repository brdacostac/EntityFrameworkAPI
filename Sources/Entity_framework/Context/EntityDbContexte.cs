using Entity_framework.DataBase;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Entity_framework
{
    public class EntityDbContexte : DbContext
    {
        public DbSet<ChampionDB> ChampionsSet { get; set; }
        public DbSet<RuneDB> RunesSet { get; set; }
        public DbSet<SkillDB> SkillSet { get; set; }
        public DbSet<SkinDB> SkinsSet { get; set; }
        public DbSet<CategoryDicDB> CategoryRunePageSet { get; set; }
        public DbSet<RunePagesDb> RunePagesSet { get; set; }
        public EntityDbContexte()
        { }

        public EntityDbContexte(DbContextOptions<EntityDbContexte> options)
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
            
            modelBuilder.Entity<ChampionDB>().HasMany(champ => champ.Skins).WithOne(skin => skin.Champion).HasForeignKey(skin => skin.ChampionForeignKey);
            modelBuilder.Entity<SkillDB>().HasOne(skill => skill.Champion).WithMany(champ => champ.Skills);
            modelBuilder.Entity<ChampionDB>().HasIndex(champ => champ.Name).IsUnique();

            modelBuilder.Entity<RunePagesDb>().HasMany(runePage => runePage.champions).WithMany(champ => champ.RunePages);
            //modelBuilder.Entity<ChampionDB>().HasMany(champ => champ.Skins).WithMany(skin => skin.Champion);
            modelBuilder.Entity<RunePagesDb>().HasMany(runePage => runePage.CategoryRunePages).WithOne(categoryRunePage => categoryRunePage.runePage).HasForeignKey(categoryRunePage => categoryRunePage.runesPagesForeignKey);
            modelBuilder.Entity<CategoryDicDB>().HasOne(categoryRunePage => categoryRunePage.rune).WithMany(rune => rune.runesPages);


           

        }
    }
}