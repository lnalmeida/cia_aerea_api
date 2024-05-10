# API de Companhia Aérea

Esta é uma API simples para uma companhia aérea, permitindo o gerenciamento de pilotos, aeronaves, voos e manutenções. Também inclui recursos para cancelar voos, filtrar voos e manutenções por datas e tipos, e gerar relatórios de cancelamentos.

## Tecnologias Utilizadas

- .NET Core 8
- Entity Framework
- Fluent Validation
- DinkToPDF
- SQL Server

## Funcionalidades

### Pilotos

- [ ] Cadastro de pilotos
- [ ] Atualização de dados de pilotos
- [ ] Exclusão de pilotos
- [ ] Listagem de pilotos

### Aeronaves

- [ ] Cadastro de aeronaves
- [ ] Atualização de dados de aeronaves
- [ ] Exclusão de aeronaves
- [ ] Listagem de aeronaves

### Voos

- [ ] Cadastro de voos
- [ ] Atualização de dados de voos
- [ ] Exclusão de voos
- [ ] Listagem de voos
- [ ] Cancelamento de voos
- [ ] Filtragem de voos por data de partida e chegada

### Manutenções

- [ ] Cadastro de manutenções
- [ ] Atualização de dados de manutenções
- [ ] Exclusão de manutenções
- [ ] Listagem de manutenções
- [ ] Filtragem de manutenções por tipo, data e período

### Relatórios

- [ ] Geração de relatório de cancelamentos de voos em PDF

## Endpoints

- `/api/pilots`: Endpoints relacionados a pilotos
- `/api/airplanes`: Endpoints relacionados a aeronaves
- `/api/flights`: Endpoints relacionados a voos
- `/api/flights/{id}/cancellation`: Endpoints relacionados a cancelamento voos
- `/api/maintenances`: Endpoints relacionados a manutenções

## Instruções de Uso

1. Clone este repositório.
2. Instale as dependências usando o comando `dotnet restore`.
3. Configure o banco de dados SQL Server e atualize a string de conexão no arquivo `appsettings.json`.
4. Execute as migrações para criar o banco de dados usando o comando `dotnet ef database update`.
5. Execute o aplicativo usando o comando `dotnet run`.
6. Acesse a API em `http://localhost:5211/swagger`.

## Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir uma issue ou enviar um pull request com melhorias, correções de bugs ou novas funcionalidades.

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
