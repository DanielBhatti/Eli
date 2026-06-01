```
cd Eli.Database
export EliConnectionString="Host=localhost;Port=5433;Database=eli;Username=eli_owner;Password=<PASSWORD>"
dotnet ef dbcontext scaffold $EliConnectionString Npgsql.EntityFrameworkCore.PostgreSQL --output-dir Models --context EliDbContext --context-dir . --no-onconfiguring --force --data-annotations
```