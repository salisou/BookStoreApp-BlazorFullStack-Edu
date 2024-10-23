# BookStore App ASP.NET 8 Blazor e API App - Progetto Educativo

Questo progetto combina le potenzialità di ASP.NET 8 con Blazor e le API RESTful per creare un'applicazione moderna e versatile. Ideato per scopi educativi, il progetto permette di esplorare tecniche avanzate di sviluppo web, come la gestione del frontend interattivo con Blazor e l'implementazione di API efficienti per il backend. È perfetto per imparare a costruire applicazioni full-stack utilizzando le tecnologie più recenti di .NET, con un'attenzione particolare a best practice, sicurezza e prestazioni.

## Configurazione del database Program.cs
```sql
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```
## Connection al database appsettings.json

```sql
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;"
  }
}
```

## Database Schema

### Tabella `Authors`
```sql
CREATE TABLE [dbo].[Authors]
(
  [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] NVARCHAR(50) NULL, 
    [LastName] NVARCHAR(50) NULL, 
    [Bio] NVARCHAR(250) NULL
)
```
### Tabella `Authors`

```sql
CREATE TABLE [dbo].[Authors]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] NVARCHAR(50) NULL, 
    [LastName] NVARCHAR(50) NULL, 
    [Bio] NVARCHAR(250) NULL
);
```

## Definisci le tue tabelle nel DbContext

```sql
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
}

```
Classi che rappresentano le tabelle 

```sql
public class Author
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Bio { get; set; }
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string ISBN { get; set; }
    public string Summary { get; set; }
    public string Image { get; set; }
    public decimal Price { get; set; }
    
    public int AuthorId { get; set; }
    public Author Author { get; set; }
}

```
## Esegui il comando Add-Migration

```sql
Add-Migration AddCodeFirstTable
// Oppure
dotnet ef migrations add AddCodeFirstTable
```

Aggiorna il database
```sql
Update-Database
```
Da .NET CLI:

```sql
dotnet ef database update
```
### Problema:
A causa di un cambiamento in **Entity Framework Core 7**, potresti incontrare il seguente errore quando tenti di impalcatura (scaffold):

### Soluzione:
Per risolvere questo problema, aggiungi `Encrypt=False` alla tua stringa di connessione nel file di configurazione. 

Esempio di stringa di connessione:

```plaintext
'Server=localhost\\sqlexpress; Database=BookStoreDb; Trusted_Connection=true; MultipleActiveResultSets=true; Encrypt=False'
```

### Spiegazione:

- **Titolo e Sezione "Avviso"**: Il titolo e la sezione chiariscono che si tratta di un avviso relativo a un problema comune con Entity Framework Core 7.
- **Descrizione del problema**: Ho inserito il messaggio di errore nel blocco di codice per una facile identificazione.
- **Soluzione**: Ho spiegato chiaramente come risolvere il problema, includendo l'esempio di una stringa di connessione con `Encrypt=False`.
- **Pacchetto richiesto**: Ho ricordato che è necessario il pacchetto `Microsoft.EntityFrameworkCore.SqlServer`.

```sql
```

```sql
```
