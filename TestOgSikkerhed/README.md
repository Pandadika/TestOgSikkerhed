# TestOgSikkerhed
Et projekt med two-factor authentication.
## Setup
* Opret blazor spa projekt med identity framework.
* Tilføj two-factor authentication QR ved confirm email.
* Registrering af nye brugere kræver at Two-factor authentication SKAL laves af brugeren da login kræver 2fa, samt kodeord skal være stærkt og mindst 8 karaktere langt.
* Tilføj felter på forsiden, så ledes man kan se om brugeren er logget ind og admin/alm. bruger.



Add SQLlite database
Add Microsoft.EntityFrameworkCore.Sqlite nuGet package.
In appsettings.json, add `"MockConnection": "Data Source=MockDb.db;"` under `"ConnectionStrings"` 
In program.cs change the connectionString builder block by chagning the GetConnectionString parameter to "MockConnection" and change the dbcontext options `options.UseSqlite`
in package manager run `add-migration init` followed by `update-database`
you are now using SQLite