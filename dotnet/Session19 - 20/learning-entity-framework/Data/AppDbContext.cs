using learning_entity_framework.Models;
using Microsoft.EntityFrameworkCore;

namespace learning_entity_framework.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Person>().ToTable("Users");

        // modelBuilder.Entity<Person>()
        //             .Property(e => e.Email)
        //             .HasColumnName("User_Email");

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Email)
                .HasColumnName("User_email");
        });

        modelBuilder.Entity<PersonAddress>().ToTable("PersonAddress");

        modelBuilder.Entity<Enrollment>()
                    .HasKey(e => new {e.CouseId, e.StudentId});

        modelBuilder.Entity<Student>()
                    .HasMany(e => e.Enrollments)
                    .WithOne(e => e.Student)
                    .HasForeignKey(e => e.StudentId);

        modelBuilder.Entity<Course>()
                    .HasMany(e => e.Enrollments)
                    .WithOne(e => e.Course)
                    .HasForeignKey(e => e.CouseId);
    }

    
    public DbSet<Person> Persons => Set<Person>(); 
    public DbSet<Todo> Todos => Set<Todo>();
    public DbSet<PersonAddress> PersonAddresses => Set<PersonAddress>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
}