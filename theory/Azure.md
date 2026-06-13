# Azure Interview Q&A

---

1. **What is a multi-tenant app?**
A multi-tenant app serves multiple organizations (tenants) from a single deployment. Each tenant's data is isolated but the same codebase and infrastructure is shared. In Azure AD, a multi-tenant app registration allows users from any Azure AD directory to sign in — not just the home tenant. Configure by setting Supported account types to "Accounts in any organizational directory" in the app registration. Common in SaaS products.

2. **Azure AD vs Azure AD B2C?**
Azure AD (Entra ID) — identity platform for employees and internal users. Used for corporate login, SSO, MFA within an organization. Integrates with Office 365, enterprise apps.
Azure AD B2C — identity platform for external customers/consumers. Supports social logins (Google, Facebook, Apple), custom user flows, branded login pages, and local accounts. Built for customer-facing apps. Separate tenant from your corporate Azure AD.

3. **How to configure and access secrets from Key Vault?**
Create a Key Vault in Azure portal. Add secrets under Secrets blade. Grant your app identity access via Access Policies or RBAC (Key Vault Secrets User role). In App Service / Azure Function, enable System-Assigned Managed Identity. Reference the secret in app settings using the Key Vault reference syntax — @Microsoft.KeyVault(SecretUri=https://myvault.vault.azure.net/secrets/MySecret/). The app reads it like a normal environment variable. No credentials needed in code.
```csharp
// Or access directly in code using DefaultAzureCredential (no hardcoded credentials)
var client = new SecretClient(new Uri("https://myvault.vault.azure.net"), new DefaultAzureCredential());
var secret = await client.GetSecretAsync("MySecret");
```

4. **Azure Functions — types and supported .NET versions?**
Trigger types — HTTP Trigger, Timer Trigger, Queue Trigger (Storage/Service Bus), Blob Trigger, Event Hub Trigger, Event Grid Trigger, Cosmos DB Trigger, SignalR Trigger, Durable Functions (orchestration).
.NET version support:
In-process model — function runs inside the Functions host process. Supports .NET 6 (LTS). Being deprecated.
Isolated worker model — function runs in a separate process. Supports .NET 6, .NET 7, .NET 8 (LTS), .NET Framework 4.8. Recommended going forward — gives full control of the process, middleware support, and latest .NET versions.

5. **App Service vs App Service Plan?**
App Service Plan — defines the region, OS, number of VM instances, and pricing tier. It is the underlying compute resource. Multiple App Services can share one plan.
App Service — the actual web app/API/function running on a plan. Billed based on the plan, not per App Service.
Plan tiers — Free/Shared (dev/test, no SLA), Basic (manual scale), Standard (auto-scale, slots, custom domains, SSL), Premium (higher performance, more slots, VNet integration), Isolated (dedicated environment, VNet, highest scale — App Service Environment).

6. **What are VNets (Virtual Networks)?**
Azure Virtual Network (VNet) is a private network in Azure. It isolates resources from the public internet and other VNets. Enables communication between Azure resources (VMs, App Services, databases) securely. Key concepts: Subnets (subdivide the VNet), NSG (Network Security Groups — firewall rules), Peering (connect two VNets), VPN Gateway (connect on-premise to Azure), Private Endpoint (access Azure services privately without public IP), VNet Integration (connect App Service to a VNet).

7. **How to block an external URL to App Service?**
Use Access Restrictions in App Service networking settings — allow/deny rules based on IP address or CIDR range. Block all public traffic and allow only specific IPs or VNet subnets.
Use Azure Front Door or Application Gateway with WAF (Web Application Firewall) to filter and block requests upstream.
For specific URLs/paths — use URL Rewrite rules in web.config or Application Gateway routing rules.
Put the App Service behind a VNet with Private Endpoint — makes it completely inaccessible from the public internet.

8. **What is ARM (Azure Resource Manager) and its uses?**
ARM is the deployment and management layer for all Azure resources. Every resource creation/update/delete goes through ARM. ARM Templates are JSON files that define infrastructure as code — declarative, repeatable, idempotent deployments. Uses: automate infrastructure provisioning, enforce consistent environments, version control your infrastructure, deploy multiple resources in a single operation. Bicep is the newer DSL that compiles to ARM templates — simpler syntax.
```json
// ARM Template snippet
{ "type": "Microsoft.Web/sites", "apiVersion": "2022-03-01", "name": "myapp", ... }
```

9. **Slots significance in App Service?**
Deployment slots are live environments within an App Service (e.g., production, staging, dev). Each slot has its own URL (myapp-staging.azurewebsites.net). Use cases: deploy to staging, run smoke tests, then swap staging to production with zero downtime. Swap also exchanges app settings marked as slot-specific. Auto-swap can be enabled for fully automated deployments. Slots are available on Standard tier and above.

10. **Steps to configure a CI/CD pipeline in Azure?**
Create a repository in Azure DevOps Repos or connect GitHub. In Azure DevOps, create a pipeline using azure-pipelines.yml. Define stages: Build (restore, build, run tests, publish artifact), Deploy (download artifact, deploy to App Service/Function/Container). Use service connections to authenticate to Azure. Add environments with approval gates for production. Trigger on branch push or PR. Use release pipelines (classic) or YAML multi-stage pipelines (recommended).

11. **Different deployment environments. How to deploy from one to another?**
Common environments — Development, QA/Testing, Staging, Production. In Azure: use separate Resource Groups or separate App Service slots per environment. Use pipeline stages with environment approvals — pipeline promotes build artifact from dev → QA → staging → prod. Use slot swaps for prod deployments to achieve zero downtime. Store environment-specific config in App Settings per slot or use Key Vault references. Use feature flags to control feature rollout independently of deployments.

12. **Types of Azure Functions and which .NET versions they support?**
In-process model — function code runs in the same process as the Functions host. Supports .NET 6 only. Tightly coupled to host, limited middleware. Being retired.
Isolated worker process model — function runs in a separate .NET worker process. Supports .NET 6 (LTS), .NET 7, .NET 8 (LTS), .NET Framework 4.8. Supports ASP.NET Core middleware, Dependency Injection, full control of host. Recommended for all new development.

13. **Azure Functions plans — Consumption (Dynamic) vs Premium vs Dedicated?**
Consumption (Dynamic) plan — scale to zero, pay per execution. Cold starts on first request. Best for infrequent or unpredictable workloads. 1M free executions/month.
Premium plan — pre-warmed instances (no cold start), VNet integration, unlimited execution duration, more powerful instances. Pay per vCPU/memory. Good for latency-sensitive or long-running functions.
Dedicated (App Service) plan — functions run on an App Service Plan. Predictable cost, always on, manual or auto-scale. Good when you already have an App Service Plan and want to avoid extra cost.

14. **How to connect to Azure SQL without credentials or connection string?**
Use Managed Identity. Enable System-Assigned or User-Assigned Managed Identity on the App Service or Azure Function. In Azure SQL, create a contained database user for that managed identity and grant it the required role.
```sql
CREATE USER [my-app-service-name] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [my-app-service-name];
```
In code, use DefaultAzureCredential with Microsoft.Data.SqlClient — it automatically fetches a token for the managed identity. No password in code or config.
```csharp
var conn = new SqlConnection("Server=myserver.database.windows.net;Database=mydb;Authentication=Active Directory Default;");
```

15. **Deployment through YAML (Azure Pipelines)?**
Define the pipeline in azure-pipelines.yml at the root of the repository. Azure DevOps automatically detects and runs it on triggers.
```yaml
trigger:
  branches:
    include:
      - main

pool:
  vmImage: 'ubuntu-latest'

stages:
  - stage: Build
    jobs:
      - job: BuildJob
        steps:
          - task: UseDotNet@2
            inputs:
              version: '8.x'
          - script: dotnet restore
          - script: dotnet build --configuration Release
          - script: dotnet publish -c Release -o $(Build.ArtifactStagingDirectory)
          - task: PublishBuildArtifacts@1
            inputs:
              PathtoPublish: '$(Build.ArtifactStagingDirectory)'
              ArtifactName: 'drop'

  - stage: Deploy
    dependsOn: Build
    jobs:
      - deployment: DeployWeb
        environment: 'production'
        strategy:
          runOnce:
            deploy:
              steps:
                - task: AzureWebApp@1
                  inputs:
                    azureSubscription: 'MyServiceConnection'
                    appName: 'my-app-service'
                    package: '$(Pipeline.Workspace)/drop/**/*.zip'
```