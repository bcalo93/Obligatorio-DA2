Obligatorio IndicatorsManager

# Autores
Brahian Calo - 170540
Ramiro Gonzalez - 167011

# Ejecutar Pruebas
<ul>
<li>dotnet test backend\IndicatorsManager.DataAccess.Test</li>
<li>dotnet test backend\IndicatorsManager.BusinessLogic.Test</li>
<li>dotnet test backend\IndicatorsManager.WebApi.Test</li>
</ul>

# Migraciones
<ul>
<li>dotnet ef migrations add MyMigration --startup-project="..\IndicatorsManager.WebApi"</li>  
<li>dotnet ef database update --startup-project="..\IndicatorsManager.WebApi"</li>
<ul>