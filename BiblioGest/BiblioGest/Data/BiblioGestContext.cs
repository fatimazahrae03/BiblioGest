using Microsoft.EntityFrameworkCore;
using System;
using BiblioGest.Models;

namespace BiblioGest.Data
{
    public class BiblioGestContext : DbContext
    {
        public DbSet<Bibliothecaire> Bibliothecaire { get; set; }
        public DbSet<Livre> Livre { get; set; }
        public DbSet<Categorie> Categorie { get; set; }
        public DbSet<Exemplaire> Exemplaire { get; set; }
        public DbSet<Emprunt> Emprunt{ get; set; }
        public DbSet<Adherent> Adherent { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Remplacez ces valeurs par vos propres paramètres de connexion MySQL
            optionsBuilder.UseMySql(
                "server=localhost;port=3306;database=bibliogest;user=root;password=",
                ServerVersion.AutoDetect("server=localhost;port=3306;database=bibliogest;user=root;password=")
            );
        }
        
       
    }
  
}