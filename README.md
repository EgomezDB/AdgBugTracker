# AdgBugTracker

## Configuración de la Cadena de Conexión

Es recomendable no incluir la cadena de conexión directamente en el código fuente por motivos de seguridad y flexibilidad. En su lugar, colócala en los archivos de configuración como appsettings.json y appsettings.Development.json.

### Ejemplo de configuración en `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "cadena": "Host=localhost;port=5432;Username=postgres;Password=linux;Database=EDPSE"
  },
}
```
### Uso en `Program.cs`:

```csharp
builder.Services.AddDbContext<AdgBugTrackerDbContext>(options => 
    options.UseNpgsql(configuration.GetConnectionString("cadena")));
```
Aunque el ejemplo utiliza PostgreSQL, puedes aplicar el mismo patrón para otras bases de datos como SQL Server.

## Uso de Fluent API para Definir el Modelo de Base de Datos

Aunque las clases de modelo pueden crear la base de datos automáticamente, Fluent API proporciona un mayor control sobre la configuración de las columnas y relaciones.

```csharp
namespace AdgBugTracker.Infrastructure.Context
{
    public partial class AdgBugTrackerDbContext : DbContext
    {
        public DbSet<Bug> Bugs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bug>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasOne(e => e.Project).WithMany(p => p.Bugs).HasForeignKey(e => e.ProjectId);
                entity.HasOne(e => e.Status).WithMany(s => s.Bugs).HasForeignKey(e => e.StatusId);
            });
        }
    }
}
```
Ventajas de Fluent API:
- Mayor control sobre las propiedades y relaciones de las entidades.
- Evita el uso de virtual, lo que puede causar problemas de relaciones circulares en las respuestas JSON.

## Uso de Métodos Genéricos para CRUD

Los métodos genéricos permiten escribir código más reutilizable y eficiente. A continuación se muestra cómo implementar un repositorio genérico.

### Definición de la Interfaz Genérica:
```csharp
namespace AdgBugTracker.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<T?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
```
### Implementación del Repositorio Genérico:
```csharp
namespace AdgBugTracker.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AdgBugTrackerDbContext _context;

        public GenericRepository(AdgBugTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _context.Set<T>().Where(predicate);

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id) => await _context.Set<T>().FindAsync(id);

        public async Task<bool> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
```
### Inyección de Dependencias:
Para usar el repositorio genérico, es necesario registrarlo en Program.cs:
```csharp
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

```
### Uso de los Métodos Genéricos en una Clase:

```csharp
public class BugCommandHandler
{
    private readonly IGenericRepository<Bug> _repository;

    public BugCommandHandler(IGenericRepository<Bug> repository)
    {
        _repository = repository;
    }

    public async Task<bool> AddBug(Bug bug)
    {
        return await _repository.AddAsync(bug);
    }
}
```

Con esta estructura puedes implementar rápidamente operaciones CRUD para cualquier entidad utilizando el patrón de repositorio genérico.
