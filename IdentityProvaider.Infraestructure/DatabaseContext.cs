﻿using IdentityProvaider.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvaider.Infraestructure
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<LogUser> Log_Users { get; set; }

        public DbSet<Rol_User> Rol_User { get; set; }
        public DbSet<Password> SecurityPasswords { get; set; }

        public DbSet<Session> InSession { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<User>(o =>
            {
                o.HasKey(x => x.id_user).HasName("id_user");
            });

            modelBuilder.Entity<User>().OwnsOne(o => o.email, conf =>
            {
                conf.Property(x => x.value).HasColumnName("email");
            });

            modelBuilder.Entity<User>().OwnsOne(o => o.name, conf =>
            {
                conf.Property(x => x.value).HasColumnName("name");
            });
           
            modelBuilder.Entity<User>().OwnsOne(o => o.lastName, conf =>
            {
                conf.Property(x => x.value).HasColumnName("last_name");
            });

            modelBuilder.Entity<User>().OwnsOne(o => o.typeDocument, conf =>
            {
                conf.Property(x => x.value).HasColumnName("type_document");
            });

            modelBuilder.Entity<User>().OwnsOne(o => o.identification, conf =>
            {
                conf.Property(x => x.value).HasColumnName("document_number");
            });           

            modelBuilder.Entity<User>().OwnsOne(o => o.creationDate, conf =>
            {
                conf.Property(x => x.value).HasColumnName("creation_date");
            });

            modelBuilder.Entity<User>().OwnsOne(o => o.direction, conf =>
            {
                conf.Property(x => x.value).HasColumnName("address");
            });

            modelBuilder.Entity<User>().OwnsOne(o => o.state, conf =>
            {
                conf.Property(x => x.value).HasColumnName("state");
            });

            modelBuilder.Entity<LogUser>(o =>
            {
                o.HasKey(x => x.id_log).HasName("id_log");
            });

            modelBuilder.Entity<LogUser>().OwnsOne(o => o.id_edit_user, conf =>
            {
                conf.Property(x => x.value).HasColumnName("id_edit_user");
            });

            modelBuilder.Entity<LogUser>().OwnsOne(o => o.id_manager, conf =>
            {
                conf.Property(x => x.value).HasColumnName("id_manager");
            });

            modelBuilder.Entity<LogUser>().OwnsOne(o => o.iP, conf =>
            {
                conf.Property(x => x.value).HasColumnName("ip");
            });
   
            modelBuilder.Entity<LogUser>().OwnsOne(o => o.location, conf =>
            {
                conf.Property(x => x.value).HasColumnName("location");
            });
            modelBuilder.Entity<LogUser>().OwnsOne(o => o.coordinate, conf =>
            {
                conf.Property(x => x.value).HasColumnName("coordinate");
            });

            modelBuilder.Entity<LogUser>().OwnsOne(o => o.email, conf =>
            {
                conf.Property(x => x.value).HasColumnName("email");
            });

            modelBuilder.Entity<LogUser>().OwnsOne(o => o.name, conf =>
            {
                conf.Property(x => x.value).HasColumnName("name");
            });

            modelBuilder.Entity<LogUser>().OwnsOne(o => o.lastName, conf =>
            {
                conf.Property(x => x.value).HasColumnName("last_name");
            });

            modelBuilder.Entity<LogUser>().OwnsOne(o => o.typeDocument, conf =>
            {
                conf.Property(x => x.value).HasColumnName("type_document");
            });

            modelBuilder.Entity<LogUser>().OwnsOne(o => o.identification, conf =>
            {
                conf.Property(x => x.value).HasColumnName("document_number");
            });

            modelBuilder.Entity<LogUser>().OwnsOne(o => o.state, conf =>
            {
                conf.Property(x => x.value).HasColumnName("state");
            });

            modelBuilder.Entity<LogUser>().OwnsOne(o => o.direction, conf =>
            {
                conf.Property(x => x.value).HasColumnName("address");
            });

            modelBuilder.Entity<LogUser>().OwnsOne(o => o.logDate, conf =>
            {
                conf.Property(x => x.value).HasColumnName("log_date");
            });

            modelBuilder.Entity<LogUser>().OwnsOne(o => o.description, conf =>
            {
                conf.Property(x => x.value).HasColumnName("description");
            });

            modelBuilder.Entity<Session>(o =>
            {
                o.HasKey(x => x.id_session).HasName("id_session");
            });

            modelBuilder.Entity<Session>().OwnsOne(o => o.id_user, conf =>
            {
                conf.Property(x => x.value).HasColumnName("id_user");
            });

            modelBuilder.Entity<Session>().OwnsOne(o => o.loginDate, conf =>
            {
                conf.Property(x => x.value).HasColumnName("loginDate");
            });

            modelBuilder.Entity<Role>(o =>
            {
                o.HasKey(x => x.id_rol).HasName("id_rol");
            });

            modelBuilder.Entity<Role>().OwnsOne(o => o.name, conf =>
            {
                conf.Property(x => x.value).HasColumnName("rol_name");
            });

            modelBuilder.Entity<Role>().OwnsOne(o => o.description, conf =>
            {
                conf.Property(x => x.value).HasColumnName("description");
            });

            modelBuilder.Entity<Rol_User>().HasKey(sc => new { sc.id_user, sc.id_rol });

            modelBuilder.Entity<Rol_User>()
                .HasOne<User>(sc => sc.user)
                .WithMany(s => s.rol_Users)
                .HasForeignKey(sc => sc.id_user);

            modelBuilder.Entity<Rol_User>()
            .Property(o => o.id_user).HasColumnName("id_user");

            modelBuilder.Entity<Rol_User>()
                .HasOne<Role>(sc => sc.role)
                .WithMany(s => s.rol_Users)
                .HasForeignKey(sc => sc.id_rol);

            //modelBuilder.Entity<Rol_User>()
            //.Property(o => o.id_user).HasColumnName("id_rol");

            modelBuilder.Entity<Rol_User>().OwnsOne(o => o.creationDate, conf =>
            {
                conf.Property(x => x.value).HasColumnName("creation_rol");
            });

            modelBuilder.Entity<Rol_User>().OwnsOne(o => o.state, conf =>
            {
                conf.Property(x => x.value).HasColumnName("state_rol");
            });

            modelBuilder.Entity<Password>(o =>
            {
                o.HasKey(x => x.hash).HasName("hash");
            });
            modelBuilder.Entity<Password>().OwnsOne(o => o.password, conf =>
            {
                conf.Property(x => x.value).HasColumnName("password");
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}