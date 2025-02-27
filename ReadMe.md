# CVEGuard

CVEGuard is an open-source project that automatically retrieves today's vulnerabilities from the [National Institute of Standards and Technology (NIST)](https://nvd.nist.gov/) and generates summaries using AI-powered analysis. The project provides a simple REST API to access the latest Common Vulnerabilities and Exposures (CVEs) and their concise, AI-generated summaries.

## Features

- **Automated CVE Retrieval**: Fetches today's CVEs from the NIST database.
- **AI-Generated Summaries**: Uses an AI model (via Ollama) to summarize vulnerabilities.
- **REST API**: Provides endpoints to query the latest vulnerabilities and their summaries.
- **Docker Support**: Runs in a containerized environment with MySQL and Ollama AI.
- **Database Storage**: Saves retrieved CVEs in a MySQL database for persistence.

## Getting Started

### Prerequisites

- Docker and Docker Compose
- A GPU-supported environment (recommended for AI inference)

### Installation

1. Clone the repository:

   ```sh
   git clone https://github.com/your-repo/CVEGuard.git
   cd CVEGuard
   ```

2. Start the services using Docker Compose:

   ```sh
   docker-compose up -d
   ```

   This will start:
   - **CVEGuard API** (listening on ports `3333` and `3334`)
   - **MySQL Database** (stores CVE data)
   - **Ollama AI Model** (for summarization)

3. Verify that the containers are running:

   ```sh
   docker ps
   ```

### API Usage

- **Retrieve today's summarized CVEs**:

  ```sh
  curl http://localhost:3333/
  ```

- **Retrieve all stored CVEs**:

  ```sh
  curl http://localhost:3333/cves
  ```

### Configuration

- **Database Connection**: Configured via environment variables in `docker-compose.yml`:
  ```yaml
  environment:
    - ConnectionStrings__DefaultConnection=server=mysql;port=3306;database=cveguard;user=root;password=rootpassword
  ```

- **AI Model Configuration**:
  - The AI service runs Ollama and defaults to the `mistral` model.
  - You can modify the model in `Program.cs`:
    ```csharp
    var ollamaModal = builder.Configuration["Ollama:modal"] ?? "mistral";
    ```

### Development

To run the project locally without Docker:

1. Ensure you have `.NET 7+` installed.
2. Set up MySQL and configure the connection string.
3. Run the API:

   ```sh
   dotnet run --project CVEGuard
   ```

### Contributing

Contributions are welcome! Please submit a pull request or open an issue for any improvements.

### License

This project is licensed under the MIT License.
