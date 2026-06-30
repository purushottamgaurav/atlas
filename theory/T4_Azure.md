# Azure Interview Q&A

> Covers cloud computing basics, Azure portal, Azure AD/Entra ID, App Registration, Managed Identity, App Service, Azure Functions, Storage, Databases, Service Bus, Docker, AKS/Kubernetes, Azure DevOps (VSTS), CI/CD, IaC, Monitoring, Networking, Security, and Architecture.

---

## Section 1: Cloud Computing & Azure Fundamentals

---

**Q1. What is Cloud Computing?**

Cloud computing is the on-demand delivery of compute, storage, networking, and other IT resources over the internet, with pay-as-you-go pricing. Instead of buying and maintaining physical servers, you rent capacity from providers like Azure, AWS, or GCP and scale up or down as needed.

Key benefits:
- **Elasticity** — scale up for peak loads, scale down to save cost.
- **No upfront capex** — operational expense only.
- **Global reach** — deploy in dozens of regions worldwide.
- **Managed services** — provider handles OS patching, backups, HA.

---

**Q2. What is the difference between IaaS, PaaS, and SaaS?**

| Model | You Manage | Provider Manages | Azure Examples |
|---|---|---|---|
| **IaaS** | OS, runtime, app | Hardware, virtualization | Azure VM, VNet, Disks |
| **PaaS** | App and data | Everything below | App Service, Azure SQL, Functions |
| **SaaS** | Just use the app | Everything | Microsoft 365, Dynamics 365 |

Rule of thumb: more PaaS = less ops, faster delivery. IaaS gives you control; SaaS gives you a finished product.

---

**Q3. What are Public, Private, and Hybrid Cloud?**

- **Public Cloud** — resources shared across customers via the internet (Azure, AWS, GCP). Cheapest, most scalable.
- **Private Cloud** — resources dedicated to one organization, hosted on-premises or in a colocation. Most control, highest cost.
- **Hybrid Cloud** — combines both. Sensitive workloads on-prem, burst workloads to public cloud. Connected via VPN or ExpressRoute.

Azure supports hybrid via **Azure Arc** (manage on-prem servers from Azure) and **Azure Stack** (run Azure services on your own hardware).

---

**Q4. Why would a company choose Azure over AWS or GCP?**

- Deep integration with Microsoft ecosystem (Windows Server, Active Directory, SQL Server, .NET, Office 365).
- Hybrid cloud strength via Azure Arc / Stack / ExpressRoute.
- Enterprise Agreements with existing Microsoft licensing discounts.
- Strong compliance certifications for regulated industries.
- Azure DevOps for end-to-end ALM.

AWS is broader and more mature; GCP leads in data/AI. Azure is the default for .NET shops and large Microsoft customers.

---

**Q5. What are Azure Regions, Availability Zones, and Geographies?**

- **Geography** — a defined area of the world (e.g., United States, Europe) containing one or more regions. Used for data residency and compliance.
- **Region** — a set of data centers connected by a low-latency network (e.g., East US, West Europe).
- **Availability Zone (AZ)** — physically separate data centers within a region, each with its own power, cooling, and networking. Deploying across 2–3 AZs gives **high availability** within a region.
- **Region pair** — two regions in the same geography, paired for disaster recovery (e.g., East US ↔ West US).

---

**Q6. Explain the Azure resource management hierarchy.**

```
Management Group  →  Subscription  →  Resource Group  →  Resource
```

- **Management Group** — groups multiple subscriptions for policy/RBAC inheritance.
- **Subscription** — billing boundary and access control container.
- **Resource Group (RG)** — logical container for resources sharing lifecycle (deploy/delete together).
- **Resource** — individual service (VM, App Service, Storage Account).

Policies and RBAC applied at a higher level **inherit downward**.

---

**Q7. What's the difference between Azure Portal, Azure CLI, PowerShell, and SDKs?**

- **Azure Portal** — web UI at `portal.azure.com`. Best for exploration, one-off changes, learning.
- **Azure CLI** (`az`) — cross-platform command-line tool. Best for scripts on Linux/macOS or CI/CD.
- **Azure PowerShell** (`Az` module) — best for Windows admins comfortable with PowerShell objects.
- **Azure SDKs** — language libraries (`.NET`, Python, JS, Java) for programmatic access from your app code.

All four call the **same ARM REST API** under the hood — they're interchangeable.

---

**Q8. What is a Resource Group and why use it?**

A Resource Group (RG) is a logical container that holds related Azure resources. It is the **deployment, lifecycle, and access boundary**.

Best practices:
- Group resources that share the same lifecycle (delete the RG → all its resources go).
- One RG per environment (dev / staging / prod) or per workload.
- Apply RBAC at the RG level — easier than per resource.
- Cannot move resources across subscriptions in some cases — plan upfront.

In the Portal: **Home → Resource groups → + Create**.

---

**Q9. What are Resource Tags and how do you use them?**

Tags are key-value labels attached to resources for filtering, billing, and automation. Examples: `Environment=Prod`, `CostCenter=12345`, `Owner=team-payments`.

Use cases:
- **Cost analysis** — see spend per cost center, per environment.
- **Automation** — shut down VMs tagged `AutoShutdown=true` at night.
- **Governance** — enforce required tags via Azure Policy.

In the Portal: every resource has a **Tags** blade. Tags do not inherit from RGs by default.

---

**Q10. What are Resource Locks?**

Locks prevent accidental modification or deletion of critical resources.

- **CanNotDelete** — resource can be modified but not deleted.
- **ReadOnly** — resource cannot be modified or deleted; reads allowed.

Apply at subscription, resource group, or individual resource level. In Portal: **Resource → Locks → + Add**. Even Owners cannot delete a locked resource until the lock is removed.

---

## Section 2: Azure Resource Manager

---

**Q11. What is Azure Resource Manager (ARM) and why does it matter?**

ARM is the management layer behind everything in Azure. Every create/update/delete — from the Portal, CLI, SDK, or pipeline — goes through ARM.

It matters because:
- ARM Templates and Bicep let you define infrastructure as code (repeatable, version-controlled).
- It enforces consistent access control via RBAC.
- It supports resource grouping, tagging, and deployment history.

Think of ARM as the single REST API that sits in front of all Azure services.

---

## Section 3: Identity & Access Management (Azure AD / Entra ID)

---

**Q12. What is the difference between Azure AD (Entra ID) and Azure AD B2C?**

- **Azure AD (Entra ID)** — identity for employees and internal users. Corporate logins, SSO, MFA. Integrates with Microsoft 365 and enterprise apps.
- **Azure AD B2C** — identity for external customers. Social logins (Google, Facebook, Apple), custom-branded login pages, local accounts. Separate tenant from corporate Azure AD.

Rule of thumb: Azure AD = your staff. B2C = your customers.

---

**Q13. What is an App Registration in Azure AD?**

App Registration is how you register an application with Azure AD so it can authenticate users and call APIs. The registration creates an **Application (client) ID** and lets you configure:

- **Redirect URIs** — where Azure AD sends tokens after login.
- **Client secrets / certificates** — for confidential clients (web apps, daemons).
- **API permissions** — what other APIs the app can call (Microsoft Graph, custom APIs).
- **Token configuration** — what claims to include.
- **Supported account types** — single-tenant, multi-tenant, or personal Microsoft accounts.

In Portal: **Azure AD → App registrations → + New registration**.

---

**Q14. What is the difference between a Service Principal and a Managed Identity?**

Both represent an application's identity in Azure AD.

- **Service Principal** — manual identity for an app. You create it, manage its secret/certificate, and rotate credentials yourself.
- **Managed Identity** — Azure-managed Service Principal automatically tied to an Azure resource (App Service, VM, Function). Azure rotates and manages credentials for you.

Use Managed Identity whenever the workload runs on Azure. Use a Service Principal for external/non-Azure systems (e.g., a GitHub Actions runner).

---

**Q15. What is the difference between System-Assigned and User-Assigned Managed Identity?**

- **System-Assigned** — created and tied 1:1 with a single Azure resource. Deleted when the resource is deleted. Simple, can't be shared.
- **User-Assigned** — created as a standalone Azure resource. Can be assigned to **multiple** resources. Lives independently of any single resource.

Use User-Assigned when several services (e.g., several App Services or a VM scale set) need the same identity. Use System-Assigned for one-off cases.

---

**Q16. What is Managed Identity and why is it better than storing credentials?**

Managed Identity gives an Azure resource an automatically managed identity in Azure AD. Other services like Key Vault or Azure SQL trust this identity.

Why it's better:
- No passwords or connection strings to store, rotate, or leak.
- Azure handles the token lifecycle automatically.
- `DefaultAzureCredential` works seamlessly between local dev and Azure.

```csharp
var client = new SecretClient(
    new Uri("https://myvault.vault.azure.net"),
    new DefaultAzureCredential());
var secret = await client.GetSecretAsync("MySecret");
```

---

**Q17. What is a multi-tenant application in Azure AD?**

A multi-tenant app allows users from **any** Azure AD directory (not just yours) to sign in. Useful for SaaS products where each customer is a different organization.

To configure: in the App Registration, set "Supported account types" to "Accounts in any organizational directory." The app accepts tokens from any Azure AD tenant. Each org's data is kept isolated in your app's data layer, typically by the `TenantId` claim in the JWT.

---

**Q18. What is Azure Key Vault and how do you use it in a .NET app?**

Key Vault is a secure store for secrets (connection strings, API keys), certificates, and encryption keys.

How to use:
1. Create a Key Vault and add secrets.
2. Enable Managed Identity on your App Service.
3. Grant the identity the **Key Vault Secrets User** role.
4. Reference secrets in App Settings using `@Microsoft.KeyVault(SecretUri=...)` or read them in code.

```csharp
builder.Configuration.AddAzureKeyVault(
    new Uri("https://myvault.vault.azure.net"),
    new DefaultAzureCredential());
var connStr = builder.Configuration["MyDbConnectionString"];
```

---

**Q19. What is RBAC in Azure?**

RBAC (Role-Based Access Control) decides who can do what to which Azure resources.

Three concepts:
- **Security Principal** — who (user, group, service principal, managed identity).
- **Role** — what permissions (Owner, Contributor, Reader, or custom roles).
- **Scope** — where it applies (Management Group → Subscription → Resource Group → Resource).

In Portal: **Resource → Access control (IAM) → + Add role assignment**.

---

**Q20. What is the difference between Authentication and Authorization in Azure AD?**

- **Authentication** — proving who you are. Azure AD verifies identity via login and issues a JWT token.
- **Authorization** — deciding what you're allowed to do. Your API checks the token's claims (roles, scopes) to decide if the request is allowed.

In .NET: `AddAuthentication` validates the token; `AddAuthorization` with policies checks the claims.

---

**Q21. What is Azure Policy and how is it different from RBAC?**

- **RBAC** — controls who can perform actions (create/delete/read).
- **Azure Policy** — controls what configurations are allowed (e.g., enforce tags, deny non-approved regions, require HTTPS-only on storage).

Example policy: "All storage accounts must have HTTPS-only enabled." RBAC can't enforce that — it only controls who can make changes, not the resulting config.

---

## Section 4: Azure Compute & App Service

---

**Q22. What is the difference between App Service and App Service Plan?**

- **App Service Plan** — the underlying compute (VM size, region, OS, scaling rules). It's what you pay for. Multiple apps can share one plan.
- **App Service** — the actual web app, API, or function running on the plan. No extra cost for adding apps to the same plan (up to its resource limits).

---

**Q23. What are App Service Pricing Tiers?**

| Tier | Use Case |
|---|---|
| Free / Shared | Hobby, dev/test |
| Basic | Small production, no auto-scale |
| Standard | Auto-scale, deployment slots, custom domains |
| Premium (v2/v3) | VNet integration, more slots, faster CPUs |
| Isolated (ASE) | Dedicated environment, highest scale, private |

Choose the lowest tier that supports your required features (slots, VNet, scale-out count).

---

**Q24. What are Deployment Slots and how do they enable zero-downtime deployments?**

Deployment slots are live environments within an App Service (e.g., staging, production). Each has its own URL.

Workflow:
1. Deploy new code to the **staging** slot.
2. Run smoke tests against the staging URL.
3. **Swap** staging ↔ production. Traffic switches instantly.
4. If broken, swap back — old version is still in the slot.

Slot settings (connection strings marked "slot-specific") stay with the slot and don't swap. Available on Standard tier and above.

---

**Q25. How do you restrict access to an App Service so it's not publicly accessible?**

Options (often combined):
- **Access Restrictions** — IP/CIDR allow-deny rules in Networking.
- **VNet Integration + Private Endpoint** — app lives inside a VNet, unreachable from the public internet.
- **Front Door / Application Gateway with WAF** — gateway filters requests before they reach the app.
- **Service Endpoints** — restrict to traffic from a specific subnet.

For internal enterprise apps: Private Endpoint + VNet is the gold standard.

---

**Q26. What is Application Gateway and how is it different from Azure Front Door?**

Both are reverse proxies with WAF, but at different scopes:

- **Application Gateway** — **regional** L7 load balancer. Good for internal/private apps and URL-based routing within one region.
- **Azure Front Door** — **global** L7 load balancer with built-in CDN. Routes traffic to the nearest healthy backend across regions.

One region, internal → Application Gateway. Global, public-facing → Front Door.

---

**Q27. What is VNet Integration in App Service?**

VNet Integration lets an App Service make **outbound** calls to resources inside a Virtual Network (a private SQL Server, Redis, internal API). The app still accepts inbound traffic from the internet unless combined with a Private Endpoint.

VNet Integration alone does **not** make the app private for inbound.

---

**Q28. What is the difference between Scale Up and Scale Out in App Service?**

- **Scale Up (vertical)** — change to a more powerful tier/SKU (more CPU, RAM). Brief downtime during the switch.
- **Scale Out (horizontal)** — add more instances of the same SKU. Auto-scale rules based on CPU, memory, or queue length. Zero downtime.

In Portal: **App Service → Scale up** vs **Scale out** blades. Always prefer scale-out for elasticity.

---

## Section 5: Azure Functions

---

**Q29. What is an Azure Function App?**

A serverless compute service that runs small pieces of code (functions) in response to triggers without managing infrastructure. Pay only for execution time. Ideal for event-driven workloads, background jobs, and integration glue.

In Portal: **+ Create resource → Function App**. Choose runtime stack (.NET, Node, Python), hosting plan, and storage account.

---

**Q30. What are the different trigger types in Azure Functions?**

- **HTTP Trigger** — invoked by an HTTP request. Build serverless APIs.
- **Timer Trigger** — runs on a CRON schedule.
- **Blob Trigger** — fires when a file is added/updated in Blob Storage.
- **Queue Trigger** — processes messages from Storage Queue.
- **Service Bus Trigger** — processes messages from Service Bus queues/topics.
- **Event Hub Trigger** — processes high-throughput streaming events.
- **Event Grid Trigger** — reacts to Event Grid events.
- **Cosmos DB Trigger** — fires on document changes (change feed).

---

**Q31. What are Bindings in Azure Functions?**

Bindings declaratively connect a function to other services as **input** or **output**, so you don't write boilerplate SDK code.

- **Input binding** — read data into the function (e.g., grab a blob, query Cosmos DB).
- **Output binding** — write data out (e.g., put a message on a queue, write to Cosmos DB).

```csharp
[Function("WriteOrder")]
[ServiceBusOutput("orders", Connection = "ServiceBusConn")]
public string Run([HttpTrigger] HttpRequestData req)
    => JsonSerializer.Serialize(new { Id = Guid.NewGuid() });
```

---

**Q32. How do you write an HTTP Trigger function?**

```csharp
public class GetProductFunction
{
    [Function("GetProduct")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id}")]
        HttpRequestData req, int id)
    {
        var product = await _productService.GetByIdAsync(id);
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(product);
        return response;
    }
}
```

`AuthorizationLevel.Function` requires a function key. Use `Anonymous` for public endpoints. The function URL is visible in the Portal under **Function → Get Function URL**.

---

**Q33. How do you write a Timer Trigger function? Explain the CRON format.**

```csharp
[Function("DailyReport")]
public void Run([TimerTrigger("0 0 8 * * *")] TimerInfo timer)
{
    // runs daily at 8:00 AM UTC
}
```

CRON format in Azure Functions has 6 fields:
`{second} {minute} {hour} {day} {month} {day-of-week}`

Examples:
- `0 */5 * * * *` — every 5 minutes.
- `0 0 0 * * *` — daily at midnight.
- `0 0 0 * * MON` — every Monday at midnight.

---

**Q34. What is the difference between the In-Process and Isolated Worker model?**

- **In-Process** — function runs inside the Functions host process. Supports .NET 6 only. Being retired.
- **Isolated Worker Process** — function runs in a separate .NET process. Supports .NET 6, 7, 8 and .NET Framework 4.8. Full ASP.NET Core middleware, DI, startup customization. **Recommended for all new development.**

---

**Q35. Compare Consumption, Premium, and Dedicated plans for Functions.**

| Plan | Cold Start | Scale | Cost | Best For |
|---|---|---|---|---|
| Consumption | Yes | 0 → ∞ | Pay per execution | Infrequent, unpredictable workloads |
| Premium | No (pre-warmed) | Always-on min instances | Per vCPU/memory | Low-latency, VNet integration |
| Dedicated | No | Manual/auto scale | App Service Plan rate | Predictable workloads, reuse plan |

Consumption gives 1 million free executions/month.

---

**Q36. What are Durable Functions and when would you use them?**

Durable Functions write **stateful, long-running workflows as code**. The framework handles state, checkpointing, retries automatically.

Patterns:
- **Function chaining** — A → B → C in sequence.
- **Fan-out/fan-in** — run tasks in parallel, wait for all.
- **Human interaction** — pause waiting for approval (hours/days).
- **Monitor** — poll until a condition is met.

```csharp
[Function("OrderWorkflow")]
public static async Task RunOrchestrator(
    [OrchestrationTrigger] TaskOrchestrationContext ctx)
{
    await ctx.CallActivityAsync("ValidateOrder", null);
    await ctx.CallActivityAsync("ChargePayment", null);
    await ctx.CallActivityAsync("SendConfirmation", null);
}
```

---

**Q37. How do you handle errors and retries in Azure Functions?**

- `try-catch` inside the function for expected errors.
- Queue/Service Bus triggers automatically retry failed messages based on `host.json`.
- Messages that fail all retries go to the **dead-letter queue**.
- Application Insights for monitoring and alerts.

```json
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

**Q38. How do you inject dependencies into an Azure Functions app (Isolated model)?**

Configure DI in `Program.cs` just like regular ASP.NET Core.

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

**Q39. How do you develop and test Azure Functions locally?**

- Install **Azure Functions Core Tools** (`func`).
- Use **Azurite** as a local emulator for Blob/Queue/Table storage.
- Use a **local Service Bus namespace** or connect to a dev instance.
- Run `func start` from the project folder — debugger attaches in VS / VS Code.
- Settings live in `local.settings.json` (not deployed; mirror in App Settings in Azure).

```bash
npm install -g azure-functions-core-tools@4
func start
```

---

## Section 6: Azure Storage

---

**Q40. What are the types of Azure Storage and when do you use each?**

- **Blob Storage** — unstructured files (images, videos, backups). Tiers: Hot, Cool, Archive.
- **Table Storage** — NoSQL key-value store. Cheap, good for semi-structured data without complex queries.
- **Queue Storage** — simple message queue (max 64 KB per message).
- **File Storage** — managed SMB file shares. Replace traditional file servers.
- **Disk Storage** — managed disks for VMs.

For .NET: use `Azure.Storage.*` packages with `DefaultAzureCredential` — no account keys.

---

**Q41. What are Blob Storage access tiers (Hot, Cool, Archive)?**

| Tier | Access Cost | Storage Cost | Access Speed | Best For |
|---|---|---|---|---|
| Hot | Low | High | Instant | Frequently accessed data |
| Cool | Higher | Lower | Instant | Infrequent (≥30 days) backups, older logs |
| Archive | Highest | Lowest | Hours (rehydrate) | Compliance, deep archive (≥180 days) |

Use **Lifecycle Management Policies** to automatically move blobs between tiers based on age.

---

**Q42. Explain Storage Redundancy options (LRS / ZRS / GRS / RA-GRS).**

- **LRS (Locally Redundant Storage)** — 3 copies in one data center. Cheapest. Survives disk/server failure.
- **ZRS (Zone-Redundant)** — 3 copies across 3 Availability Zones in one region. Survives AZ failure.
- **GRS (Geo-Redundant)** — LRS + async replication to paired region. Survives region failure (manual failover).
- **RA-GRS** — GRS + read access to the secondary region.
- **GZRS / RA-GZRS** — ZRS + geo-replication.

Choose based on RTO/RPO and budget.

---

**Q43. What are Shared Access Signatures (SAS) and how do they compare to Access Keys?**

- **Account Access Keys** — root credentials giving full access to the entire storage account. Two keys; rotate one at a time.
- **SAS Tokens** — time-limited, scoped tokens granting specific permissions to specific resources (e.g., "read this blob for 1 hour"). Safer to share with clients.

Better than both: **Managed Identity + RBAC** on the storage account — no secrets at all.

```csharp
// Generate a user-delegation SAS using Managed Identity
var sasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(1));
```

---

**Q44. What is the difference between Azure Files and Blob Storage?**

- **Blob Storage** — object storage with REST API. Best for app data, backups, static assets.
- **Azure Files** — managed SMB/NFS file share, mounts as a drive on Windows/Linux. Best for lift-and-shift of apps that expect a network file share.

Blob for cloud-native apps; Files for legacy app compatibility.

---

**Q45. What is a Lifecycle Management Policy on Storage?**

A rule engine that automatically moves or deletes blobs based on age and access patterns. Example: move to Cool after 30 days, Archive after 180 days, delete after 7 years.

Configured in JSON in the Portal under **Storage Account → Lifecycle management → + Add rule**. Saves significant cost on long-tail data.

---

## Section 7: Azure Databases

---

**Q46. Compare Azure SQL, Cosmos DB, and Azure Table Storage.**

| Feature | Azure SQL | Cosmos DB | Table Storage |
|---|---|---|---|
| Type | Relational | NoSQL (multi-model) | NoSQL key-value |
| Query | T-SQL | SQL API, Mongo API, etc. | OData (limited) |
| Scale | Vertical + read replicas | Global horizontal | Horizontal |
| Consistency | Strong | 5 tunable levels | Eventual |
| Cost | Moderate | Higher | Very cheap |

SQL → structured relational. Cosmos → global, low-latency, flexible schema. Table → cheap simple lookups.

---

**Q47. What is the difference between Azure SQL Database, Managed Instance, and SQL on a VM?**

- **Azure SQL Database** — fully managed PaaS, single DB or elastic pool. Easiest, most cloud-native.
- **Azure SQL Managed Instance** — fully managed instance-level compatibility (cross-DB queries, SQL Agent, CLR). Best for lift-and-shift from on-prem SQL Server.
- **SQL Server on Azure VM** — IaaS, you manage the OS and SQL. Full control, more ops work.

Choose SQL DB for new cloud apps, Managed Instance for migrations, VM only when you need OS-level access.

---

**Q48. What is the difference between DTU and vCore pricing for Azure SQL?**

- **DTU (Database Transaction Unit)** — bundled CPU + memory + I/O capacity. Simple, fixed sizes (Basic, Standard, Premium). Good for small/predictable workloads.
- **vCore** — buy CPU cores, memory, and storage separately. More flexible; supports Hyperscale and serverless. Required for Managed Instance and most enterprise scenarios.

For new workloads choose **vCore** — better transparency and Azure Hybrid Benefit eligibility.

---

**Q49. What are Cosmos DB consistency levels?**

From strongest to weakest:

1. **Strong** — linearizable; reads see the latest committed write. Highest latency.
2. **Bounded Staleness** — reads lag by ≤ N operations or T time.
3. **Session** — within one client session, monotonic reads + read your writes. **Default; best for most apps.**
4. **Consistent Prefix** — reads never see writes out of order.
5. **Eventual** — lowest latency, no ordering guarantees.

Weaker = lower latency and cost. Pick the strongest you can afford.

---

**Q50. How do you connect to Azure SQL from .NET without a password, using Managed Identity?**

1. Enable Managed Identity on the App Service / Function.
2. In Azure SQL, create a contained user for the identity:

```sql
CREATE USER [my-app-name] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [my-app-name];
ALTER ROLE db_datawriter ADD MEMBER [my-app-name];
```

3. Use Active Directory authentication in the connection string:

```csharp
var conn = new SqlConnection(
    "Server=myserver.database.windows.net;Database=mydb;Authentication=Active Directory Default;");
```

No secrets anywhere.

---

**Q51. What is Azure Redis Cache and when would you use it?**

A managed in-memory data store to speed up apps via caching.

Use it for:
- Caching DB query results to reduce DB load.
- Storing session state in multi-instance deployments (in-memory session won't work).
- Rate limiting counters.
- Pub/sub between services.

```csharp
builder.Services.AddStackExchangeRedisCache(o =>
    o.Configuration = builder.Configuration["RedisConnectionString"]);
```

---

## Section 8: Messaging & Integration

---

**Q52. What is Azure Service Bus and when would you use it over Storage Queue?**

| Feature | Storage Queue | Service Bus |
|---|---|---|
| Max message size | 64 KB | 256 KB (Std), 100 MB (Premium) |
| Dead-letter queue | No | Yes |
| Message ordering | Best-effort | Guaranteed (sessions) |
| Topics / pub-sub | No | Yes |
| Transactions | No | Yes |

Storage Queue → simple, high-volume, cheap. Service Bus → enterprise workflows, ordering, pub/sub, dead-letter.

---

**Q53. What's the difference between Service Bus Queue and Topic?**

- **Queue** — one sender, one receiver. Each message processed by exactly one consumer. Point-to-point.
- **Topic + Subscriptions** — one sender, multiple receivers. Each subscription gets its own copy. Pub/sub.

Example: an `OrderPlaced` topic delivers to inventory, email, and analytics subscriptions independently.

---

**Q54. What is Azure Event Grid and how does it differ from Service Bus?**

- **Event Grid** — lightweight event routing. Push-based, no queue. Discrete events ("file uploaded"). Millions of events for pennies.
- **Service Bus** — message broker with durability, ordering, and complex routing. For commands and workflows.

Rule: Event Grid for "something happened" notifications. Service Bus for "do this work" commands.

---

**Q55. What is Azure Event Hub and when would you use it?**

A high-throughput **event streaming** platform — Azure's Kafka. Millions of events/sec.

Use for:
- Telemetry / IoT pipelines.
- Log aggregation.
- Real-time analytics.
- Clickstream data.

Event Hub is for high-volume streaming + replay, not individual message processing.

---

**Q56. What is a Dead-Letter Queue (DLQ)?**

A sub-queue that holds messages which couldn't be processed after the configured number of retries (poison messages, schema mismatches, expired messages).

You monitor the DLQ, inspect failed messages, fix the underlying issue, and optionally resubmit. Service Bus has DLQs built in; Storage Queue does not.

```csharp
[Function("DeadLetterProcessor")]
public async Task Run(
    [ServiceBusTrigger("orders-queue/$DeadLetterQueue", Connection = "Sb")]
    ServiceBusReceivedMessage message) { /* alert, archive, fix */ }
```

---

## Section 9: Containers & Docker

---

**Q57. What is Docker and why is it useful for .NET applications?**

Docker packages your app + dependencies (runtime, libs, config) into a portable image. The image runs identically anywhere — no "works on my machine."

For .NET: build a `Dockerfile`, push the image to Azure Container Registry, deploy to App Service, Container Apps, or AKS.

---

**Q58. Explain the structure of a Dockerfile for a .NET app.**

```dockerfile
# Stage 1 — build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Stage 2 — runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "MyApp.dll"]
```

Key directives: `FROM`, `WORKDIR`, `COPY`, `RUN`, `EXPOSE`, `ENTRYPOINT`.

---

**Q59. What is a multi-stage Docker build?**

A Dockerfile with multiple `FROM` statements. The earlier stage(s) compile code; the final stage copies only the build output into a slim runtime image.

Benefits:
- Final image is small (no SDK, no source code, no build tools).
- Better security — less attack surface.
- Faster pulls and deploys.

The example above is a 2-stage build. The final image is `aspnet`, not `sdk`.

---

**Q60. What is Azure Container Registry (ACR)?**

A private Docker registry inside your Azure subscription. Push container images to it; pull from AKS, App Service, ACI during deployments.

Benefits:
- Private (no public exposure).
- Geo-replication for global deploys.
- Vulnerability scanning via Microsoft Defender.
- Integrates with AKS and App Service via RBAC (`AcrPull` role).

```bash
az acr build --registry myacr --image myapp:v1 .
```

---

**Q61. Compare Azure Container Instances (ACI), Azure Container Apps, and AKS.**

| Service | Use Case |
|---|---|
| **ACI** | Single-container, fire-and-forget jobs, sidecar to AKS. No orchestration. |
| **Container Apps** | Serverless containers with built-in scaling (KEDA), HTTPS, revisions. Microservices without Kubernetes ops. |
| **AKS** | Full Kubernetes — pods, deployments, services. Maximum control and ecosystem. |

Quick batch job → ACI. Microservices, want simplicity → Container Apps. Large/complex platform → AKS.

---

## Section 10: Kubernetes & AKS

---

**Q62. What is Azure Kubernetes Service (AKS) and when would you choose it over App Service?**

AKS is managed Kubernetes — Azure operates the control plane for free; you manage the worker nodes.

Choose AKS when:
- Many microservices needing independent scaling.
- Need fine-grained control over networking, scheduling, resource limits.
- Stateful workloads or batch jobs.
- Team already knows Kubernetes.

Choose App Service for simpler web apps/APIs with less ops overhead.

---

**Q63. What is the difference between a Pod, Deployment, and Service in Kubernetes?**

- **Pod** — smallest unit; one or more containers running together on the same node.
- **Deployment** — manages a set of identical Pods. Handles rolling updates, rollbacks, desired replica count.
- **Service** — stable network endpoint that routes traffic to Pods. Pods come and go; the Service IP stays constant.

```yaml
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
apiVersion: v1
kind: Service
spec:
  selector: { app: myapp }
  ports: [{ port: 80 }]
```

---

**Q64. What are ConfigMaps and Secrets in Kubernetes?**

- **ConfigMap** — stores non-sensitive config as key-value pairs. Mounted as env vars or files.
- **Secret** — stores sensitive data (passwords, tokens) base64-encoded.

In production AKS, back Secrets with **Azure Key Vault Provider for Secrets Store CSI Driver** so secrets come from Key Vault, not the cluster.

---

**Q65. What is a Horizontal Pod Autoscaler (HPA) in AKS?**

HPA scales the number of Pods up or down based on CPU, memory, or custom metrics. Pair with **Cluster Autoscaler** to add/remove nodes when Pods can't be scheduled.

```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
spec:
  scaleTargetRef: { name: myapp-deployment }
  minReplicas: 2
  maxReplicas: 10
  metrics:
    - type: Resource
      resource: { name: cpu, target: { averageUtilization: 70 } }
```

---

**Q66. What are Kubernetes Namespaces?**

A logical partition inside a cluster. Used to isolate resources (dev, staging, team-a, team-b) within the same cluster.

```bash
kubectl create namespace staging
kubectl apply -f deployment.yaml -n staging
```

Built-in namespaces: `default`, `kube-system`, `kube-public`. RBAC and resource quotas can be applied per namespace.

---

**Q67. What is an Ingress in AKS?**

An Ingress exposes HTTP/HTTPS routes from outside the cluster to Services inside, with host/path-based routing and TLS termination.

Popular Ingress controllers in AKS: **NGINX Ingress**, **AGIC** (Application Gateway Ingress Controller), **Traefik**.

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
spec:
  rules:
    - host: api.example.com
      http:
        paths:
          - path: /orders
            backend: { service: { name: orders-svc, port: { number: 80 } } }
```

---

**Q68. What are the most common kubectl commands?**

```bash
kubectl get pods                       # list pods
kubectl get nodes                      # list nodes
kubectl describe pod myapp-abc         # detailed info
kubectl logs myapp-abc                 # view logs
kubectl logs -f myapp-abc              # follow logs
kubectl exec -it myapp-abc -- bash     # shell into a pod
kubectl apply -f deployment.yaml       # create/update resources
kubectl delete -f deployment.yaml      # delete resources
kubectl rollout undo deployment/myapp  # rollback
kubectl scale deploy myapp --replicas=5
```

`kubectl config use-context my-aks` switches between clusters.

---

**Q69. What are AKS Node Pools?**

A node pool is a set of identical VMs (worker nodes) backing your AKS cluster. You can have multiple pools with different VM sizes or OS types.

Common patterns:
- **System pool** — runs Kubernetes system pods.
- **User pool** — runs your workloads.
- **Spot pool** — cheap, interruptible VMs for batch/CI workloads.
- **GPU pool** — for ML inference.

`az aks nodepool add` to add a pool. Each pool scales independently.

---

## Section 11: Azure DevOps & CI/CD

---

**Q70. What is the difference between Azure DevOps and GitHub Actions?**

- **Azure DevOps** — Microsoft's full DevOps suite: **Repos**, **Pipelines**, **Boards** (work tracking), **Test Plans**, **Artifacts**. Enterprise-strong.
- **GitHub Actions** — CI/CD built into GitHub. Simpler YAML, huge marketplace of community actions.

Azure Pipelines can deploy anywhere; both integrate with Azure. Same company now — increasingly converging.

---

**Q71. What are Azure Boards and Work Items in Azure DevOps?**

Azure Boards is the work tracking tool — Agile/Scrum/Kanban boards for planning. Work Items include:

- **Epic** — large initiative.
- **Feature** — capability under an epic.
- **User Story / Product Backlog Item** — user-facing requirement.
- **Task** — small unit of work assigned to one person.
- **Bug** — defect.

Work items can be linked to commits, PRs, and releases for end-to-end traceability.

---

**Q72. What is Azure Repos and how do Pull Requests work in VSTS?**

Azure Repos = Git hosting inside Azure DevOps. Standard Git flow:

1. Developer creates a branch from `main`.
2. Pushes commits, opens a **Pull Request** in the Portal.
3. Required reviewers, build validation, and policies must pass.
4. PR is **completed** (merge, squash, or rebase) — typically with the linked Work Item closed automatically.

**Branch policies** (Portal → Repos → Branches → … → Branch policies) can require minimum reviewers, build success, linked work items, and comments resolved before merge.

---

**Q73. What is CI/CD pipeline?**

A **CI/CD pipeline** is an automated workflow that **builds, tests, and deploys** an application whenever code is committed.

| CI (Continuous Integration) | CD (Continuous Delivery/Deployment) |
|------------------------------|--------------------------------------|
| Automatically builds the application | Automatically deploys the application |
| Runs unit/integration tests | Deploys to Dev, QA, and Production |
| Detects issues early | Delivers updates quickly and reliably |

**Pipeline Flow:**

```text
Code Commit → Build → Test → Deploy
```

**Example:**  
A developer pushes code to GitHub. An Azure DevOps pipeline automatically builds the application, runs tests, and deploys it to Azure.

---

**Q74. What is the structure of a YAML pipeline in Azure DevOps?**

```yaml
trigger:
  branches: { include: [main] }

pool: { vmImage: 'ubuntu-latest' }

stages:
  - stage: Build
    jobs:
      - job: BuildJob
        steps:
          - task: UseDotNet@2
            inputs: { version: '8.x' }
          - script: dotnet restore
          - script: dotnet build --configuration Release
          - script: dotnet test
          - script: dotnet publish -c Release -o $(Build.ArtifactStagingDirectory)
          - task: PublishBuildArtifacts@1
            inputs: { ArtifactName: 'drop' }

  - stage: Deploy
    dependsOn: Build
    jobs:
      - deployment: DeployToProduction
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

Hierarchy: **Pipeline → Stages → Jobs → Steps**.

---

**Q75. What are Environments and Approval Gates in Azure DevOps?**

An **Environment** represents a deployment target (dev, staging, production). Defined once and referenced in pipeline stages.

**Approval gates** require a named person/group to manually approve before deployment proceeds. Other gates: business hours, query results from work items, Azure Monitor health check.

In Portal: **Pipelines → Environments → Production → Approvals and checks**.

---

**Q76. What is a Service Connection in Azure DevOps?**

A stored credential or identity that allows the pipeline to authenticate to an external service (Azure subscription, Docker registry, GitHub, NuGet).

For Azure deployments, prefer **Workload Identity Federation** (no stored secret; uses federated OIDC token) over a Service Principal secret.

In Portal: **Project Settings → Service connections → + New service connection**.

---

**Q77. What is the difference between Pipeline Variables and Variable Groups?**

- **Pipeline Variables** — defined per pipeline (in YAML or Portal). Scope: one pipeline.
- **Variable Groups** — central group of variables shared across pipelines. Live in the **Library** under **Pipelines**. Can be linked to **Azure Key Vault** so the values come from Key Vault.

```yaml
variables:
  - group: 'shared-prod-secrets'   # variable group from Library
  - name: buildConfig
    value: Release
```

Use Variable Groups for any secret or shared value (DB connections, API keys).

---

**Q78. What is the difference between Microsoft-Hosted and Self-Hosted Agents in Azure Pipelines?**

- **Microsoft-Hosted Agents** — VMs Azure provides on demand (`ubuntu-latest`, `windows-latest`, `macOS-latest`). No setup; recycled each run. Free minutes per month.
- **Self-Hosted Agents** — VMs/machines you own and register with Azure DevOps. Needed when you require custom tools, private network access, or more compute.

Self-host agents in AKS or VMSS to scale automatically with demand.

---

**Q79. What is the difference between ARM Templates, Bicep, and Terraform?**

| Tool | Language | Scope | Best For |
|---|---|---|---|
| ARM Templates | JSON | Azure only | Legacy, verbose |
| Bicep | DSL → ARM | Azure only | Modern Azure IaC |
| Terraform | HCL | Multi-cloud | Teams managing AWS + Azure + GCP |
| Pulumi | C#/Python/TS | Multi-cloud | Devs preferring real code |

Azure-only → Bicep. Multi-cloud → Terraform.

```bicep
resource appService 'Microsoft.Web/sites@2022-03-01' = {
  name: 'my-app'
  location: resourceGroup().location
  properties: { serverFarmId: appServicePlan.id }
}
```

---

**Q80. What is Blue-Green Deployment and how do you implement it in Azure?**

Run two identical production environments. **Blue** is live; **Green** is the new version. Deploy and test Green, then switch traffic. If broken, switch back instantly.

In **App Service** → deployment slots do this (staging slot swap).
In **AKS** → two Deployments (blue/green) and update Service selector to point to green.

---

**Q81. What is a Feature Flag and how do you manage them in Azure?**

A toggle that enables/disables a feature at runtime without deploying code. Use **Azure App Configuration** with the Feature Management library.

```csharp
builder.Configuration.AddAzureAppConfiguration(o =>
    o.Connect(connStr).UseFeatureFlags());
builder.Services.AddFeatureManagement();

if (await _featureManager.IsEnabledAsync("NewCheckout"))
    return View("NewCheckout");
return View("OldCheckout");
```

Toggle per environment, user segment, or percentage rollout — no redeployment.

---

**Q82. What is Azure App Configuration and how does it differ from appsettings.json?**

`appsettings.json` is a local file baked into the deployment — changes require redeploy.

**Azure App Configuration** is a centralized config store:
- Change config without redeploy.
- Push updates with the **sentinel refresh** pattern.
- Share across multiple apps/environments.
- Key Vault references for secrets.
- Built-in feature flags.

```csharp
builder.Configuration.AddAzureAppConfiguration(o =>
    o.Connect(connStr)
     .ConfigureRefresh(r => r.Register("Sentinel", refreshAll: true)));
```

---

## Section 12: Monitoring & Reliability

---

**Q83. What is Azure Monitor and how does it relate to Application Insights?**

- **Azure Monitor** — overarching platform for collecting/analyzing telemetry from all Azure resources (metrics, logs, alerts, dashboards).
- **Application Insights** — APM component for your app code: request times, dependency calls, exceptions, custom events, live metrics.

In .NET: `builder.Services.AddApplicationInsightsTelemetry()` captures everything automatically.

---

**Q84. How do you set up Alerts in Azure Monitor?**

1. Define a **metric alert** (CPU > 80% for 5 min) or **log alert** (KQL query returns > N rows).
2. Create an **Action Group** — who to notify (email, SMS, Teams webhook, Function, Logic App).
3. Attach the action group to the alert rule.

For App Insights: use **Smart Detection** for automatic anomaly alerts, and **Availability Tests** to ping endpoints from multiple regions.

---

**Q85. What is Distributed Tracing and how does App Insights support it?**

In a microservices app, one user request flows through many services. Distributed tracing links all those operations under a single **Trace ID**.

App Insights uses the W3C Trace Context standard. When enabled on all services, it propagates the `traceparent` header across HTTP, Service Bus, etc. View the end-to-end call in **Transaction Search** and **Application Map**.

---

**Q86. What is Log Analytics and KQL?**

**Log Analytics** is the workspace where Azure Monitor stores log data (App Insights, AKS logs, Activity Logs).

**KQL (Kusto Query Language)** is the query language to analyze logs.

```kql
requests
| where timestamp > ago(1h)
| where success == false
| summarize count() by name, resultCode
| order by count_ desc
```

Used in Portal → **Logs** blade, alerts, and dashboards.

---

**Q87. What is the difference between Horizontal and Vertical Scaling in Azure?**

- **Vertical (scale up)** — move to a bigger SKU. Has a ceiling. Possible brief downtime.
- **Horizontal (scale out)** — add more instances of the same size. Near-infinite. Zero downtime. Requires stateless apps.

In App Service: configure auto-scale rules on CPU/memory/queue length.

---

**Q88. How do you implement Health Checks in an Azure-hosted .NET app?**

Use ASP.NET Core health checks. App Service and AKS poll these to decide if an instance is healthy.

```csharp
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "database")
    .AddRedis(redisConnection, name: "redis");

app.MapHealthChecks("/health/live");   // liveness — is the app running?
app.MapHealthChecks("/health/ready",
    new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") });
```

In AKS: configure `livenessProbe` and `readinessProbe` in the Pod spec.

---

## Section 13: Security & API Management

---

**Q89. How do you secure an API in Azure using API Management (APIM)?**

APIM sits in front of your APIs and adds:
- **JWT validation** before requests reach the service.
- **Rate limiting** per subscription key or IP.
- **IP filtering**.
- **OAuth2** integration with Azure AD.
- **Request/response transformation**.
- **Logging** to Application Insights.

The backend API is reachable only through APIM, not directly from the internet.

---

**Q90. What are APIM Policies?**

Policies are XML rules applied at API or operation level to transform requests/responses or enforce behavior. Common policies:

```xml
<policies>
  <inbound>
    <validate-jwt header-name="Authorization" require-scheme="Bearer">
      <openid-config url="https://login.microsoftonline.com/{tenant}/.well-known/openid-configuration" />
      <required-claims><claim name="aud"><value>api://my-api</value></claim></required-claims>
    </validate-jwt>
    <rate-limit calls="100" renewal-period="60" />
    <set-header name="X-Forwarded-For" exists-action="override">
      <value>@(context.Request.IpAddress)</value>
    </set-header>
  </inbound>
</policies>
```

---

**Q91. What is Microsoft Defender for Cloud (formerly Security Center)?**

A unified security posture and threat protection platform across Azure. Provides:

- **Secure Score** — quantified rating of how secure your setup is.
- Actionable recommendations (enable MFA, restrict storage public access, patch VMs).
- Container image vulnerability scanning in ACR.
- SQL threat detection (SQL injection, anomalous logins).
- Just-in-time VM access.

---

**Q92. How do you handle secrets rotation for an Azure SQL connection string in production?**

With **Managed Identity** → nothing to rotate; tokens are issued automatically and expire in ~1 hour.

For apps that must use passwords:
1. Store the connection string in **Key Vault**.
2. Use Key Vault's built-in **secret rotation** with Azure SQL — rotates the password on a schedule.
3. App reads via Key Vault reference — picks up new value on next read; no redeploy.

---

**Q93. How do you implement network security for an AKS cluster?**

- **Network Policies** — define which Pods can talk to which (internal firewall). Default: all-to-all.
- **Private Cluster** — control plane not reachable from the public internet.
- **NSG** — control inbound/outbound at the subnet level.
- **Azure Firewall** — inspect and filter cluster egress.
- **Pod Identity / Workload Identity** — Pods use Managed Identity instead of service credentials.

---

## Section 14: Networking

---

**Q94. What is a Virtual Network (VNet) in Azure?**

A VNet is your private network in Azure. You control the IP range, subnets, routing, and connectivity.

Key concepts:
- **Address space** — overall CIDR (e.g., 10.0.0.0/16).
- **Subnets** — sub-ranges within the VNet (10.0.1.0/24 for App tier, 10.0.2.0/24 for DB).
- **Peering** — connect two VNets together.
- **VPN Gateway / ExpressRoute** — connect on-prem networks.

Resources like VMs, AKS, Private Endpoints live inside a VNet.

---

**Q95. What is a Network Security Group (NSG) vs Azure Firewall?**

- **NSG** — stateful packet filter (5-tuple: source IP/port, dest IP/port, protocol). Attached to subnet or NIC. Free.
- **Azure Firewall** — managed L3-L7 firewall with FQDN filtering, threat intelligence, TLS inspection. Paid service.

NSGs handle basic allow/deny at the subnet level. Azure Firewall handles centralized egress filtering and advanced rules for hub-spoke topologies.

---

**Q96. What is the difference between a Private Endpoint and a Service Endpoint?**

- **Service Endpoint** — extends VNet identity to the service, but the service still has a public IP. Traffic stays on the Azure backbone. Cheaper, simpler.
- **Private Endpoint** — gives the service a **private IP** inside your VNet. Service is unreachable from the public internet. More secure.

For sensitive workloads, use Private Endpoints + disable public network access on the resource.

---

**Q97. What is Azure Bastion?**

A managed service that provides secure RDP/SSH access to VMs **directly through the Azure Portal**, over TLS, without exposing the VM's public IP.

Benefits:
- No public IP needed on the VM.
- No VPN needed.
- Multi-factor auth via Azure AD.

Replaces the old anti-pattern of jump boxes with public IPs.

---

## Section 15: Reliability & Architecture

---

**Q98. How do you implement Backup and Disaster Recovery in Azure?**

- **Azure Backup** — managed backup for VMs, Azure Files, SQL in VM, on-prem servers. Define a Recovery Services Vault and a backup policy.
- **Azure Site Recovery (ASR)** — orchestrates failover of VMs to a secondary region for DR.
- **Azure SQL** — automated backups, geo-redundant, **active geo-replication** for read replicas + manual failover, **auto-failover groups** for automatic failover.
- **Storage** — GRS/RA-GRS for cross-region replication.

Define **RTO** (recovery time) and **RPO** (recovery point) per workload and pick services accordingly.

---

**Q99. How do you control and optimize cost in Azure?**

- **Azure Cost Management + Billing** — view spend by RG, subscription, tag. Set **Budgets** with alerts.
- **Azure Advisor** — recommends right-sizing, idle resource cleanup, reserved instance purchases.
- **Reserved Instances / Savings Plans** — 1- or 3-year commitments for 40–70% discount on predictable workloads.
- **Auto-shutdown** dev VMs at night via Tags + Logic App / Policy.
- **Right-size** SKUs based on actual usage.
- **Spot VMs** for batch / interruptible workloads (up to 90% off).
- **Lifecycle policies** on storage to move blobs to Cool/Archive.

In Portal: **Cost Management + Billing → Cost analysis**.

---

**Q100. You need to build a .NET microservices app on Azure. Walk through the key architecture decisions.**

A reference architecture:

- **Hosting** — AKS for services that need fine-grained scaling; App Service for simpler ones; Container Apps as a middle ground.
- **API Gateway** — Azure API Management in front of all services — handles auth, rate limiting, routing.
- **Identity** — Azure AD for internal users, B2C for customers. Services validate JWTs independently.
- **Messaging** — Service Bus for commands and workflows; Event Grid for lightweight notifications.
- **Data** — each microservice owns its own Azure SQL DB or Cosmos DB. No shared databases.
- **Secrets** — all credentials in Key Vault; apps use Managed Identity / Workload Identity.
- **Caching** — Azure Redis Cache in front of hot data.
- **Observability** — App Insights on every service, distributed tracing, centralized Log Analytics workspace, Azure Monitor alerts.
- **CI/CD** — Azure DevOps multi-stage YAML pipelines, build once + deploy through dev → staging → production, deployment slots for zero-downtime.
- **IaC** — Bicep templates in the same repo; pipeline provisions infrastructure before deploying the app.
- **Resilience** — Polly for HTTP retries and circuit breakers, health checks on every service, HPA + Cluster Autoscaler in AKS.
- **Security** — Private Endpoints for data services, NSGs/Firewall on subnets, Defender for Cloud enabled, Secure Score tracked.

---

