# app-nary
Para executar a aplicação é necessário configurar no "appsettings.json" do projeto AppNary.Host as propriedades:
- JwtToken:Scret (Chave privada da criptografia do JWT)
- ConnectionStrings:SqlServer (Conexão com o banco de dados)
- ImageStorage:ConnectionString (Conexão do container do storage account na azure)
- ImageStorage:ContainerName (Nome do container do storage account na azure)

Para iniciar criar o banco de dados é necessário rodar as migrations do EF Core manualmente, para fazer isso você deverá: 
- Ter instalado o "dotnet cli" com o pacote do EF Core
- Ir no prompt na pasta "src"
- digitar o comando "dotnet ef database update -s AppNary.Host -p AppNary.Data"
