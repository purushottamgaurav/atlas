# Azure Interview Q&A 

> Covers Azure Services, Security, DevOps, Containers, Messaging, and Architecture 

---

## Section 1: Azure Fundamentals & Identity

---

**Q1. What is Azure Resource Manager (ARM) and why does it matter?**

ARM is the management layer behind everything in Azure. Every time you create, update, or delete a resource — from the portal, CLI, SDK, or pipeline — it goes through ARM.

It matters because:
- ARM Templates and Bicep let you define infrastructure as code (repeatable, version-controlled).
- It enforces consistent access control via RBAC.
- It supports resource grouping, tagging, and deployment history.

Think of ARM as the single API that sits in front of all Azure services.

---

**Q2. What is the difference between Azure AD (Entra ID) and Azure AD B2C?**

- **Azure AD (Entra ID)** — identity for employees and internal users. Used for corporate logins, SSO, MFA. Integrates with Microsoft 365 and enterprise apps. Your org's IT team manages users here.
- **Azure AD B2C** — identity for external customers. Supports social logins (Google, Facebook, Apple), custom branded login pages, and local accounts. Built for public-facing consumer apps. It's a completely separate tenant from your corporate Azure AD.

Rule of thumb: Azure AD = your staff. B2C = your customers.

---

**Q3. What is a multi-tenant application in Azure AD?**

A multi-tenant app allows users from **any** Azure AD directory (not just yours) to sign in. Useful for SaaS products where each customer is a different organization.

To configure: in the App Registration, set "Supported account types" to "Accounts in any organizational directory." The app then accepts tokens from any Azure AD tenant. Each organization's users and data are kept isolated in your app's data layer — usually with a `TenantId` claim from the JWT token.

---

**Q4. What is Managed Identity and why is it better than storing credentials?**

Managed Identity gives an Azure resource (App Service, Function App, VM) an automatically managed identity in Azure AD. Other services like Key Vault or Azure SQL can trust this identity.

Why it's better:
- No passwords or connection strings to store, rotate, or leak.
- Azure handles the token lifecycle automatically.
- Works via `DefaultAzureCredential` in code — no changes needed when moving between local dev and production.

```csharp
// No credentials in code - uses Managed Identity in Azure, developer login locally
var client = new SecretClient(
    new Uri("https://myvault.vault.azure.net"),
    new DefaultAzureCredential());
var secret = await client.GetSecretAsync("MySecret");
```

---

**Q5. What is Azure Key Vault and how do you use it in a .NET app?**

Key Vault is a secure store for secrets (connection strings, API keys), certificates, and encryption keys. It keeps sensitive values out of your code and config files.

How to use it:
1. Create a Key Vault and add secrets.
2. Enable Managed Identity on your App Service.
3. Grant the identity the "Key Vault Secrets User" role.
4. Either reference secrets directly in App Settings using `@Microsoft.KeyVault(SecretUri=...)` syntax, or access them in code.

```csharp
// Add to configuration pipeline in Program.cs
builder.Configuration.AddAzureKeyVault(
    new Uri("https://myvault.vault.azure.net"),
    new DefaultAzureCredential());
// Secrets are now available like any appsettings value
var connStr = builder.Configuration["MyDbConnectionString"];
```

---

**Q6. What is RBAC in Azure and how does it work?**

RBAC (Role-Based Access Control) controls who can do what to which Azure resources. Instead of giving full admin access, you assign specific roles at specific scopes.

Three key concepts:
- **Security Principal** — who (user, group, service principal, managed identity).
- **Role** — what permissions (Owner, Contributor, Reader, or custom roles).
- **Scope** — where it applies (Management Group → Subscription → Resource Group → Resource).

For example: give a developer "Contributor" access only on the dev Resource Group, not production.

---

**Q7. What is the difference between Authentication and Authorization in Azure AD?**

- **Authentication** — proving who you are. Azure AD verifies identity via login and issues a token (JWT).
- **Authorization** — deciding what you're allowed to do. Your API checks the token's claims (roles, scopes) to decide if the request is permitted.

In .NET: `AddAuthentication` validates the token. `AddAuthorization` with policies checks the claims inside it.

---

**Q8. What is Azure Policy and how is it different from RBAC?**

- **RBAC** — controls who can perform actions (create/delete/read resources).
- **Azure Policy** — controls what configurations are allowed (enforce resource naming, require specific tags, prevent certain SKUs, require resources to be in approved regions).

Example policy: "All storage accounts must have HTTPS-only enabled." RBAC can't enforce this — it only controls who can make changes, not what the result must look like.

---

## Section 2: App Service & Hosting

---

**Q9. What is the difference between App Service and App Service Plan?**

- **App Service Plan** — the underlying compute (VM size, region, OS, scaling rules). It's what you pay for. Multiple apps can share one plan.
- **App Service** — the actual web app, API, or function running on the plan. No extra cost for adding apps to the same plan (up to the plan's resource limits).

Plan tiers: Free/Shared (dev), Basic (no auto-scale), Standard (auto-scale, deployment slots, custom domains), Premium (VNet integration, more slots), Isolated (dedicated environment, highest scale).

---

**Q10. What are deployment slots and how do you use them for zero-downtime deployments?**

Deployment slots are live environments within an App Service (e.g., staging, production). Each slot has its own URL.

Zero-downtime workflow:
1. Deploy new code to the **staging** slot.
2. Run smoke tests against the staging URL.
3. **Swap** staging ↔ production. Traffic switches instantly.
4. If something goes wrong, swap back — the old version is still in the slot.

Slot settings (like connection strings marked "slot-specific") stay with the slot and don't swap, which prevents staging DB config from reaching production.

Available on Standard tier and above.

---

**Q11. How do you restrict access to an App Service so it's not publicly accessible?**

Several options, often combined:

- **Access Restrictions** — allow/deny rules based on IP address or CIDR range in the Networking settings.
- **VNet Integration + Private Endpoint** — app lives inside a VNet, not reachable from the public internet.
- **Azure Front Door or Application Gateway with WAF** — put a gateway in front that filters requests before they reach the app.
- **Service Endpoints** — restrict the App Service to only accept traffic from a specific subnet.

For internal enterprise apps: Private Endpoint + VNet is the gold standard.

---

**Q12. What is Azure Application Gateway and how is it different from Azure Front Door?**

Both are reverse proxies/load balancers but operate at different scopes:

- **Application Gateway** — regional load balancer with WAF. Routes traffic to backends within a single region. Good for internal/private apps and complex URL-based routing.
- **Azure Front Door** — global load balancer with CDN and WAF. Routes traffic to the nearest healthy backend across regions. Good for globally distributed, high-availability apps.

If your app is in one region and not public-facing → Application Gateway. If it's global → Front Door.

---

**Q13. What is VNet Integration in App Service?**

VNet Integration allows an App Service to make **outbound** calls to resources inside a Virtual Network (e.g., a SQL Server, Redis, or internal API that's not publicly accessible). The app can still receive traffic from the internet unless combined with a Private Endpoint.

It does **not** allow inbound private access to the App Service by itself — for that, use a Private Endpoint.

---

## Section 3: Azure Functions

---

**Q14. What is the difference between the In-Process and Isolated Worker model in Azure Functions?**

- **In-Process** — function runs inside the same process as the Functions host. Supports .NET 6 only. Limited middleware support. Being retired.
- **Isolated Worker Process** — function runs in a separate .NET process. Supports .NET 6, 7, 8, and .NET Framework 4.8. Supports full ASP.NET Core middleware, DI, and startup customization. **Recommended for all new development.**

The isolated model is like a regular .NET app that happens to be triggered by Azure Functions.

---

**Q15. What is the difference between Consumption, Premium, and Dedicated plans for Azure Functions?**

| Plan | Cold Start | Scale | Cost | Best For |
|---|---|---|---|---|
| Consumption | Yes | 0 to ∞ | Pay per execution | Infrequent, unpredictable workloads |
| Premium | No (pre-warmed) | Always-on instances | Per vCPU/memory | Low-latency, VNet integration |
| Dedicated | No | Manual/auto scale | App Service Plan rate | Predictable workloads, existing plan |

Consumption gives you 1 million free executions/month. Premium eliminates cold starts but costs more.

---

**Q16. What are Durable Functions and when would you use them?**

Durable Functions let you write stateful, long-running workflows as code. The framework handles state, checkpointing, and retries automatically.

Common patterns:
- **Function chaining** — step A → step B → step C in sequence.
- **Fan-out/fan-in** — run multiple tasks in parallel, wait for all to finish.
- **Human interaction** — pause the workflow and wait for approval (hours or days).
- **Monitor** — poll until a condition is met.

```csharp
[FunctionName("OrderWorkflow")]
public static async Task RunOrchestrator(
    [OrchestrationTrigger] IDurableOrchestrationContext context)
{
    await context.CallActivityAsync("ValidateOrder", null);
    await context.CallActivityAsync("ChargePayment", null);
    await context.CallActivityAsync("SendConfirmation", null);
}
```

Use when a workflow has multiple steps, long waits, or needs retry/compensation logic.

---

**Q17. How do you handle errors and retries in Azure Functions?**

- Use `try-catch` inside the function for expected errors.
- For queue/Service Bus triggers, Azure Functions automatically retries failed messages based on the `host.json` retry policy.
- Messages that fail all retries go to the **dead-letter queue**.
- Use Application Insights for monitoring and alerts on failures.

```json
// host.json - exponential backoff retry
{
  "retry": {
    "strategy": "exponentialBackoff",
    "maxRetryCount": 5,
    "minimumInterval": "00:00:10",
    "maximumInterval": "00:05:00"
  }
}
```

---

**Q18. How do you inject dependencies into an Azure Functions app (Isolated model)?**

In the isolated model, configure DI in `Program.cs` just like a regular ASP.NET Core app.

```csharp
var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IProductService, ProductService>();
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnection")));
    })
    .Build();

await host.RunAsync();
```

Then inject via constructor as normal.

---

## Section 4: Azure Storage & Databases

---

**Q19. What are the types of Azure Storage and when do you use each?**

- **Blob Storage** — unstructured files (images, videos, documents, backups). Has Hot, Cool, and Archive tiers by access frequency.
- **Table Storage** — NoSQL key-value store. Cheap, good for semi-structured data without complex queries.
- **Queue Storage** — simple message queue for decoupling services. Max 64KB per message.
- **File Storage** — managed file shares accessible via SMB protocol. Replace traditional file servers.

For .NET: use the `Azure.Storage.*` NuGet packages with `DefaultAzureCredential` — no account keys in code.

---

**Q20. What is the difference between Azure SQL, Cosmos DB, and Azure Table Storage?**

| Feature | Azure SQL | Cosmos DB | Table Storage |
|---|---|---|---|
| Type | Relational | NoSQL (multi-model) | NoSQL key-value |
| Query | SQL | SQL API, Mongo API, etc. | OData/LINQ (limited) |
| Scale | Vertical + read replicas | Global horizontal | Horizontal |
| Consistency | Strong | Tunable (5 levels) | Eventual |
| Cost | Moderate | Higher | Very cheap |

Use Azure SQL for structured relational data. Use Cosmos DB for globally distributed, high-scale, flexible-schema data. Use Table Storage for cheap, simple lookups.

---

**Q21. How do you connect to Azure SQL from a .NET app without a password using Managed Identity?**

1. Enable Managed Identity on the App Service or Function App.
2. In Azure SQL, create a contained user for the identity:

```sql
CREATE USER [my-app-name] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [my-app-name];
ALTER ROLE db_datawriter ADD MEMBER [my-app-name];
```

3. In the connection string, use Active Directory Default authentication:

```csharp
// No password - token fetched automatically by Managed Identity
var conn = new SqlConnection(
    "Server=myserver.database.windows.net;Database=mydb;Authentication=Active Directory Default;");
```

No credentials anywhere in code or config.

---

**Q22. What is Azure Redis Cache and when would you use it in a .NET app?**

Azure Redis Cache is a managed in-memory data store used to dramatically speed up apps by caching frequently accessed data.

Use it for:
- Caching database query results to reduce DB load.
- Storing session state in multi-instance deployments (can't use in-memory session with multiple servers).
- Rate limiting counters.
- Pub/sub messaging between services.

```csharp
builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration["RedisConnectionString"]);

// In service
await _cache.SetStringAsync("product_1", JsonSerializer.Serialize(product), 
    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });
```

---

## Section 5: Messaging & Integration

---

**Q23. What is Azure Service Bus and when would you use it over Azure Storage Queue?**

Azure Service Bus is an enterprise message broker supporting queues and topics/subscriptions.

| Feature | Storage Queue | Service Bus |
|---|---|---|
| Max message size | 64 KB | 256 KB (Standard), 100 MB (Premium) |
| Dead-letter queue | No | Yes |
| Message ordering | Best effort | Guaranteed (sessions) |
| Topics/pub-sub | No | Yes |
| Transactions | No | Yes |

Use Storage Queue for simple, high-volume, cheap messaging. Use Service Bus for enterprise workflows, guaranteed ordering, pub/sub fan-out, or when you need dead-letter handling.

---

**Q24. What is the difference between Service Bus Queue and Service Bus Topic?**

- **Queue** — one sender, one receiver. Each message is processed by exactly one consumer. Point-to-point.
- **Topic + Subscriptions** — one sender, multiple receivers. Each subscription gets its own copy of the message. Pub/sub pattern.

Example: An order placed event goes to a topic. The inventory subscription, the email subscription, and the analytics subscription each receive and process it independently.

---

**Q25. What is Azure Event Grid and how is it different from Service Bus?**

- **Event Grid** — lightweight event routing for reactive, event-driven architectures. Designed for discrete events ("file uploaded", "resource deleted"). Push-based, no queue. Low cost, millions of events for pennies.
- **Service Bus** — message broker with durability, ordering, and complex routing. Designed for commands and workflows that need guaranteed delivery.

Rule: Event Grid for "something happened" notifications. Service Bus for "do this work" commands.

---

**Q26. What is Azure Event Hub and when would you use it?**

Event Hubs is a high-throughput event streaming platform — like Azure's version of Apache Kafka. It can ingest millions of events per second.

Use it for:
- Telemetry and IoT data pipelines.
- Application log aggregation.
- Real-time analytics.
- Clickstream data.

Unlike Service Bus, Event Hub is designed for high-volume streaming and replay, not individual message processing with retries.

---

## Section 6: Containers & Kubernetes

---

**Q27. What is Docker and why is it useful for .NET applications?**

Docker packages your app and all its dependencies (runtime, libraries, config) into a container image. The image runs identically on any machine — no "works on my machine" problems.

For .NET: containerize the app with a `Dockerfile`, push the image to Azure Container Registry, and deploy it anywhere (App Service, AKS, Container Instances).

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyApp.dll"]
```

---

**Q28. What is Azure Container Registry (ACR)?**

ACR is Azure's private Docker registry — like Docker Hub but inside your Azure subscription. You push container images to it and pull from it during deployments.

Benefits:
- Private and secure — no public exposure.
- Geo-replication for global deployments.
- Integrates with AKS and App Service directly.
- Supports vulnerability scanning with Microsoft Defender.

Grant AKS or App Service the "AcrPull" role on ACR so they can pull images without credentials.

---

**Q29. What is Azure Kubernetes Service (AKS) and when would you choose it over App Service?**

AKS is a managed Kubernetes service. Kubernetes orchestrates containers — it handles deployment, scaling, self-healing, and rolling updates.

Choose AKS when:
- You have many microservices that need independent scaling.
- You need fine-grained control over networking, resource limits, and scheduling.
- You're running stateful apps or batch workloads.
- Your team already knows Kubernetes.

Choose App Service when you have a simpler web app or API and want less operational overhead.

---

**Q30. What is the difference between a Pod, Deployment, and Service in Kubernetes?**

- **Pod** — the smallest unit in Kubernetes. One or more containers running together on the same node.
- **Deployment** — manages a set of identical Pods. Handles rolling updates, rollbacks, and desired replica count.
- **Service** — a stable network endpoint that routes traffic to Pods. Pods come and go; the Service IP stays constant.

```yaml
# Deployment - run 3 replicas of the app
apiVersion: apps/v1
kind: Deployment
spec:
  replicas: 3
  template:
    spec:
      containers:
        - name: myapp
          image: myacr.azurecr.io/myapp:latest
---
# Service - expose the deployment internally
apiVersion: v1
kind: Service
spec:
  selector:
    app: myapp
  ports:
    - port: 80
```

---

**Q31. What are ConfigMaps and Secrets in Kubernetes?**

- **ConfigMap** — stores non-sensitive configuration as key-value pairs. Mounted as environment variables or files in the Pod.
- **Secret** — stores sensitive data (passwords, tokens) base64-encoded. Should be backed by Azure Key Vault in production using the Secrets Store CSI Driver.

```yaml
# ConfigMap
apiVersion: v1
kind: ConfigMap
data:
  APP_ENV: "production"
  LOG_LEVEL: "info"
---
# Secret (base64 encoded value)
apiVersion: v1
kind: Secret
type: Opaque
data:
  db-password: cGFzc3dvcmQ=
```

In production AKS: use **Azure Key Vault Provider for Secrets Store CSI Driver** so secrets come from Key Vault, not the cluster.

---

**Q32. What is a Horizontal Pod Autoscaler (HPA) in AKS?**

HPA automatically scales the number of Pods up or down based on CPU, memory, or custom metrics. When traffic increases, it adds Pods. When traffic drops, it removes them.

```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
spec:
  scaleTargetRef:
    name: myapp-deployment
  minReplicas: 2
  maxReplicas: 10
  metrics:
    - type: Resource
      resource:
        name: cpu
        target:
          averageUtilization: 70
```

Pair with **Cluster Autoscaler** to also add/remove nodes when Pods can't be scheduled.

---

## Section 7: DevOps & CI/CD

---

**Q33. What is the difference between Azure DevOps and GitHub Actions?**

Both are CI/CD platforms but with different homes:

- **Azure DevOps** — Microsoft's full DevOps suite: Repos, Pipelines, Boards (work tracking), Test Plans, Artifacts. Preferred for enterprise teams already in the Microsoft ecosystem.
- **GitHub Actions** — CI/CD built into GitHub. Simpler YAML syntax, huge marketplace of community actions. Preferred for open-source or GitHub-native teams.

Azure Pipelines can deploy to any cloud. Both integrate with Azure for deployments.

---

**Q34. What is a YAML pipeline in Azure DevOps and what is its structure?**

A YAML pipeline defines build and deploy steps as code, stored in your repo. It runs automatically on triggers (push, PR, schedule).

```yaml
trigger:
  branches:
    include: [main]

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
          - script: dotnet test
          - script: dotnet publish -c Release -o $(Build.ArtifactStagingDirectory)
          - task: PublishBuildArtifacts@1
            inputs:
              ArtifactName: 'drop'

  - stage: Deploy
    dependsOn: Build
    jobs:
      - deployment: DeployToProduction
        environment: 'production'   # can have approval gates
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

---

**Q35. What are Environments and Approval Gates in Azure DevOps?**

An **Environment** in Azure DevOps represents a deployment target (dev, staging, production). You define it once and reference it in pipeline stages.

**Approval gates** require a named person or group to manually approve before the pipeline continues to that stage. This prevents accidental production deployments.

Other gates: business hours check, quality gates (work item query must be empty), Azure Monitor health check.

---

**Q36. What is a Service Connection in Azure DevOps?**

A Service Connection is a stored credential or identity that allows the pipeline to authenticate to an external service (Azure subscription, Docker Registry, GitHub, etc.).

For Azure deployments: use a **Workload Identity Federation** service connection (recommended — no secrets stored, uses federated token) or a Service Principal with a secret. The pipeline uses this to deploy without embedding credentials in the YAML.

---

**Q37. What is the difference between ARM Templates, Bicep, and Terraform?**

All three are Infrastructure as Code (IaC) tools to provision Azure resources:

| Tool | Language | Scope | Best For |
|---|---|---|---|
| ARM Templates | JSON | Azure only | Legacy, verbose, still widely used |
| Bicep | DSL (compiles to ARM) | Azure only | Modern Azure-native IaC, simpler syntax |
| Terraform | HCL | Multi-cloud | Teams managing AWS + Azure + GCP |
| Pulumi | C#/Python/TypeScript | Multi-cloud | Devs who prefer real code over DSL |

For Azure-only shops: use Bicep. For multi-cloud or existing Terraform investment: use Terraform.

```bicep
// Bicep - much cleaner than ARM JSON
resource appService 'Microsoft.Web/sites@2022-03-01' = {
  name: 'my-app'
  location: resourceGroup().location
  properties: {
    serverFarmId: appServicePlan.id
  }
}
```

---

**Q38. What is a Blue-Green deployment and how do you implement it in Azure?**

Blue-Green runs two identical production environments. "Blue" is live. You deploy to "Green," test it, then switch traffic. If something breaks, you switch back instantly.

In Azure App Service: this is exactly what **deployment slots** do. Staging slot = Green, Production slot = Blue. The slot swap is the traffic switch.

In AKS: use two Deployments (blue and green) and update the Service selector to point to the green Deployment when ready.

---

**Q39. What is a feature flag and how do you manage them in Azure?**

A feature flag is a toggle that enables or disables a feature at runtime without deploying code. This lets you deploy code that's "off" and gradually roll it out.

In Azure, use **Azure App Configuration** with the Feature Management library:

```csharp
builder.Configuration.AddAzureAppConfiguration(options =>
    options.Connect(connectionString).UseFeatureFlags());
builder.Services.AddFeatureManagement();

// In controller
if (await _featureManager.IsEnabledAsync("NewCheckout"))
    return View("NewCheckout");
return View("OldCheckout");
```

Toggle features per environment, user segment, or percentage rollout from the App Configuration portal — no redeployment needed.

---

**Q40. What is Azure App Configuration and how does it differ from appsettings.json?**

`appsettings.json` is a local file baked into the deployment. Changing a value requires redeployment.

**Azure App Configuration** is a centralized, managed configuration store:
- Change config values without redeployment.
- Push changes to running apps with the sentinel refresh pattern.
- Share config across multiple apps and environments.
- Integrates with Key Vault references for secrets.
- Supports feature flags.

```csharp
builder.Configuration.AddAzureAppConfiguration(options =>
    options.Connect(connectionString)
           .ConfigureRefresh(refresh => refresh
               .Register("Sentinel", refreshAll: true)));
```

---

## Section 8: Monitoring & Reliability

---

**Q41. What is Azure Monitor and how does it relate to Application Insights?**

**Azure Monitor** is the overarching platform for collecting, analyzing, and acting on telemetry from all Azure resources — metrics, logs, alerts, dashboards.

**Application Insights** is the APM (Application Performance Monitoring) component of Azure Monitor specifically for your application code. It collects:
- Request/response times and failure rates.
- Dependency calls (SQL, HTTP, Redis).
- Exceptions with stack traces.
- Custom events and metrics.
- Live Metrics stream.

In .NET: add `builder.Services.AddApplicationInsightsTelemetry()` and everything is captured automatically.

---

**Q42. How do you set up alerts in Azure Monitor?**

1. Define a **metric alert** (CPU > 80% for 5 minutes) or **log alert** (query returns error count > 10).
2. Set an **Action Group** — who to notify (email, SMS, Teams webhook, Azure Function, Logic App).
3. Attach the action group to the alert rule.

For App Insights: use **Smart Detection** for automatic anomaly alerts (sudden spike in failure rate, unusual latency). Also use **Availability Tests** to ping your endpoint from multiple regions every 5 minutes.

---

**Q43. What is distributed tracing and how does Application Insights support it?**

In a microservices app, a single user request may flow through 5 different services. Distributed tracing links all those operations under a single **Trace ID** so you can see the full journey in one view.

Application Insights implements this via the W3C Trace Context standard. When App Insights is enabled on all services, it automatically propagates the `traceparent` header across HTTP calls, Service Bus messages, etc. You can see the entire end-to-end call in the **Transaction Search** and **Application Map** in the portal.

---

**Q44. What is the difference between horizontal and vertical scaling in Azure?**

- **Vertical scaling (scale up)** — move to a bigger VM or App Service tier (more CPU/RAM). Has a ceiling. Can cause brief downtime during scale.
- **Horizontal scaling (scale out)** — add more instances of the same size. No ceiling (in theory). Zero downtime. Requires the app to be stateless.

In App Service: configure auto-scale rules based on CPU, memory, or request queue length. Azure adds or removes instances automatically within min/max bounds you define.

---

**Q45. How do you implement health checks in an Azure-hosted .NET app?**

Use ASP.NET Core's built-in health check middleware. App Service and AKS poll the health endpoint to decide if an instance is healthy.

```csharp
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "database")
    .AddRedis(redisConnection, name: "redis");

app.MapHealthChecks("/health/live");   // liveness - is the app running?
app.MapHealthChecks("/health/ready",  // readiness - is the app ready for traffic?
    new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") });
```

In AKS: configure `livenessProbe` and `readinessProbe` in the Pod spec pointing to these endpoints. If liveness fails, Kubernetes restarts the Pod. If readiness fails, it stops sending traffic.

---

## Section 9: Security & Compliance

---

**Q46. How do you secure an API in Azure using API Management (APIM)?**

Azure API Management sits in front of your APIs and adds cross-cutting concerns without touching your code:

- **JWT validation** — validate Bearer tokens before requests reach your service.
- **Rate limiting** — throttle per subscription key or IP.
- **IP filtering** — block specific origins.
- **OAuth2** — integrate with Azure AD for authentication.
- **Request/response transformation** — strip sensitive headers, add CORS.
- **Logging** — log all requests to Application Insights.

The backend API is only reachable through APIM — not directly from the internet.

---

**Q47. What is Azure Defender for Cloud (Security Center) and how does it help?**

Defender for Cloud monitors your Azure resources and gives a **Secure Score** — a rating of how secure your setup is. It gives actionable recommendations like "enable MFA," "restrict public access to storage," or "update vulnerable container images."

For .NET teams it matters because: it scans container images in ACR for vulnerabilities, monitors SQL databases for threats, and alerts on unusual activity (SQL injection patterns, impossible travel logins).

---

**Q48. How do you handle secrets rotation for an Azure SQL connection string in production?**

With Managed Identity, **there's nothing to rotate** — tokens are issued automatically by Azure AD and expire after 1 hour. This is the ideal approach.

For apps that must use passwords:
1. Store the connection string in Key Vault.
2. Use Key Vault's built-in **secret rotation** feature with Azure SQL — it automatically rotates the password on a schedule.
3. The app reads the secret via Key Vault reference — when the secret rotates, the app picks up the new value on next read (no redeployment).

---

**Q49. How do you implement network security for an AKS cluster?**

- **Network Policies** — define which Pods can communicate with which other Pods (like a firewall inside the cluster). Default: all Pods can talk to all other Pods.
- **Private Cluster** — the API server (control plane) is not reachable from the public internet. Only accessible from within the VNet.
- **NSG (Network Security Groups)** — control inbound/outbound traffic at the subnet level.
- **Azure Firewall** — inspect and filter all egress traffic from the cluster.
- **Pod Identity / Workload Identity** — Pods use Managed Identity instead of storing service credentials.

---

## Section 10: Architecture & Scenario

---

**Q50. You need to build a .NET microservices app on Azure. Walk through the key architecture decisions.**

Here's a reference architecture for a production .NET microservices app:

**Hosting:** AKS for services that need fine-grained scaling. App Service for simpler services.

**API Gateway:** Azure API Management in front of all services — handles auth, rate limiting, routing.

**Identity:** Azure AD for internal users. B2C for external customers. Services validate JWTs independently — no shared session.

**Messaging:** Azure Service Bus for commands and workflows. Event Grid for lightweight event notifications between services.

**Data:** Each microservice owns its own Azure SQL database or Cosmos DB — no shared databases.

**Secrets:** All credentials in Key Vault. Apps and AKS Pods use Managed Identity / Workload Identity to access them.

**Caching:** Azure Redis Cache in front of hot data.

**Observability:** Application Insights on every service with distributed tracing. Azure Monitor alerts. Centralized Log Analytics workspace.

**CI/CD:** Azure DevOps multi-stage YAML pipelines. Build once, deploy the same artifact through dev → staging → production. Deployment slots for zero-downtime.

**IaC:** Bicep templates in the same repo as the app code. Pipeline provisions infrastructure before deploying the app.

**Resilience:** Polly for HTTP retries and circuit breakers. Health checks on every service. HPA + Cluster Autoscaler in AKS.

---

