# GAP BI Challenge — Dashboard de Vendas

## Sobre o projeto

Aplicação full-stack desenvolvida para o desafio técnico da GAP BI.

O sistema realiza a importação de produtos, clientes e pedidos da API pública DummyJSON, armazena os dados em um banco relacional SQL Server e disponibiliza indicadores financeiros em um dashboard desenvolvido com React.

O projeto foi construído com foco em organização do código, separação de responsabilidades, persistência de dados e sincronização sem duplicação de registros.

## Tecnologias utilizadas

**Backend**
- C# e ASP.NET Core Web API (.NET 9)
- Entity Framework Core 9
- SQL Server Express
- Integração HTTP com DummyJSON

**Frontend**
- React
- Vite
- JavaScript
- Axios
- Bootstrap
- Recharts

**Ferramentas**
- Visual Studio Code
- SQL Server Management Studio (SSMS)
- Git e GitHub

## Funcionalidades

### Sincronização de dados

- Importação de produtos da DummyJSON.
- Importação de clientes.
- Importação de pedidos e seus respectivos itens.
- Atualização de registros existentes sem duplicação.
- Sincronização completa por meio de um endpoint.
- Registro da data e hora da última sincronização bem-sucedida.
- Tratamento de erros nas requisições.

### Indicadores financeiros

O dashboard apresenta:

- Número total de pedidos.
- Faturamento bruto.
- Faturamento líquido.
- Valor total de descontos concedidos.
- Percentual de desconto.
- Ticket médio.
- Faturamento líquido por categoria.
- Ranking dos cinco maiores clientes por faturamento líquido.

## Estrutura do projeto

```text
GapBiChallenge/
├── GapBi.Api/
│   ├── Controllers/
│   ├── Data/
│   ├── DTOs/
│   ├── Models/
│   ├── Services/
│   ├── Migrations/
│   └── Program.cs
├── gapbi-dashboard/
│   └── src/
│       ├── components/
│       ├── services/
│       ├── App.jsx
│       └── App.css
└── README.md
```

## Modelagem do banco de dados

O banco utiliza as seguintes entidades:

| Entidade | Responsabilidade |
|---|---|
| Cliente | Armazena os clientes importados |
| Produto | Armazena os produtos e suas categorias |
| Pedido | Armazena os pedidos e valores financeiros |
| PedidoItem | Relaciona os produtos aos pedidos |
| Sincronizacao | Registra as sincronizações bem-sucedidas |

Os identificadores externos são armazenados separadamente das chaves primárias internas.

As entidades Cliente, Produto e Pedido possuem índices únicos sobre seus identificadores externos, contribuindo para evitar duplicações durante novas sincronizações.

## Como executar o projeto

### Pré-requisitos

- .NET SDK 9
- Node.js e npm compatíveis com a versão do Vite utilizada
- SQL Server
- Acesso à internet para consultar a DummyJSON

### 1. Configurar o banco de dados

No arquivo `GapBi.Api/appsettings.json`, configure a conexão `ConexaoBancoDados` com a instância local do SQL Server.

Exemplo:

```json
{
  "ConnectionStrings": {
    "ConexaoBancoDados": "Server=localhost\\SQLEXPRESS;Database=GapBiDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

O endereço do servidor deverá ser ajustado conforme o ambiente.

### 2. Executar as migrations

Abra o terminal na pasta `GapBi.Api` e execute:

```powershell
dotnet restore
dotnet ef database update
```

### 3. Iniciar o backend

```powershell
dotnet run
```

### 4. Instalar e iniciar o frontend

Abra outro terminal na pasta `gapbi-dashboard`:

```powershell
npm install
npm run dev
```

Acesse o endereço informado pelo Vite, normalmente:

`http://localhost:5173`

**Importante:** o frontend utiliza o endereço da API configurado em `src/services/api.js`. Caso o backend esteja em outra porta, esse endereço deverá ser ajustado.

### 5. Sincronizar os dados

Com o backend e o frontend executando, clique no botão **Sincronizar dados**.

A aplicação importará os registros da DummyJSON e atualizará os indicadores do dashboard.

## Endpoints principais

| Método | Endpoint | Descrição |
|---|---|---|
| POST | `/api/Sincronizacao/produtos` | Sincroniza produtos |
| POST | `/api/Sincronizacao/clientes` | Sincroniza clientes |
| POST | `/api/Sincronizacao/pedidos` | Sincroniza pedidos |
| POST | `/api/Sincronizacao/completa` | Executa a sincronização completa |
| GET | `/api/Sincronizacao/ultima` | Consulta a última sincronização |
| GET | `/api/Indicadores` | Retorna os indicadores financeiros |

## Regras dos indicadores

- **Faturamento bruto:** soma dos valores totais dos pedidos.
- **Faturamento líquido:** soma dos valores dos pedidos após descontos.
- **Desconto concedido:** diferença entre faturamento bruto e líquido.
- **Percentual de desconto:** desconto concedido dividido pelo faturamento bruto, multiplicado por 100.
- **Ticket médio:** faturamento líquido dividido pelo número de pedidos.
- **Faturamento por categoria:** soma dos valores líquidos dos itens, agrupados pela categoria dos produtos.
- **Maiores clientes:** cinco clientes com maior faturamento líquido acumulado.

## Decisões técnicas

**Separação entre backend e frontend:** o backend concentra a importação, persistência e cálculo dos indicadores. O React fica responsável pela apresentação e interação com o usuário.

**Entity Framework Core:** utilizado para mapear entidades, configurar relacionamentos e gerenciar migrations.

**Sincronização idempotente:** os registros externos são identificados por IDs próprios, permitindo executar a sincronização novamente sem duplicar pedidos, clientes ou produtos.

**Transações:** a sincronização completa utiliza uma transação para evitar a confirmação parcial dos dados quando ocorre uma falha durante o processo.

**Datas em UTC:** os registros de sincronização utilizam UTC no backend, enquanto o frontend apresenta a data no horário de Brasília.

## Uso de inteligência artificial

O ChatGPT foi utilizado como ferramenta de apoio durante o desenvolvimento, auxiliando na organização das etapas, explicação de conceitos, elaboração e revisão de código e investigação de problemas.

As implementações foram executadas e testadas localmente, incluindo a validação dos indicadores no SQL Server e a verificação da comunicação entre backend e frontend.

## Possíveis melhorias futuras

- Criar testes automatizados para os serviços.
- Implementar paginação nas consultas.
- Adicionar filtros por categoria e cliente no dashboard.
- Configurar variáveis de ambiente para os endereços da API.
- Melhorar a visualização de categorias com faturamentos muito diferentes.
- Adicionar logs estruturados para acompanhar erros de sincronização.
- Preparar a aplicação para publicação em ambiente de produção.

## Tempo de desenvolvimento

**Tempo total:** aproximadamente 12 horas.

O período inclui planejamento, estruturação do backend, modelagem do banco de dados, integração com a API DummyJSON, implementação dos indicadores financeiros, desenvolvimento do dashboard React, testes e documentação.

## Autor

Carlos Eduardo de Oliveira Vieira

Desenvolvido como parte do processo seletivo da GAP BI.