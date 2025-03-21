$server = "(localdb)\MSSQLLocalDB"
$database = "TigrinhoGame"

# Criar e inicializar o banco de dados
sqlcmd -S $server -i "TigrinhoGame.Infrastructure/Data/Scripts/CreateDatabase.sql" -o "create_output.log"
Write-Host "Database created. Check create_output.log for details."

# Dados iniciais de propagação
sqlcmd -S $server -i "TigrinhoGame.Infrastructure/Data/Scripts/SeedData.sql" -o "seed_output.log"
Write-Host "Data seeded. Check seed_output.log for details." 