# BookStore App ASP.NET 8 Blazor e API App - Progetto Educativo

Questo progetto combina le potenzialità di ASP.NET 8 con Blazor e le API RESTful per creare un'applicazione moderna e versatile. Ideato per scopi educativi, il progetto permette di esplorare tecniche avanzate di sviluppo web, come la gestione del frontend interattivo con Blazor e l'implementazione di API efficienti per il backend. È perfetto per imparare a costruire applicazioni full-stack utilizzando le tecnologie più recenti di .NET, con un'attenzione particolare a best practice, sicurezza e prestazioni.


## Database Schema

### la query SQL per creare la tabella `Authors` nel database:
```sql
CREATE TABLE [dbo].[Authors]
(
  [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] NVARCHAR(50) NULL, 
    [LastName] NVARCHAR(50) NULL, 
    [Bio] NVARCHAR(250) NULL
)
```
