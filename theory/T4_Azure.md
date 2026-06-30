# Azure Interview Prep — 100 Questions for .NET Full Stack Developer 

---

## Section 1: Cloud Basics (Q1–Q10)

**Q1. What is cloud computing, in simple words?**

**Cloud computing** is the delivery of computing services—such as **servers, storage, databases, networking, and software**—over the internet instead of using your own physical hardware.

You pay only for the resources you use, and you can **scale up or down** anytime based on demand. The cloud provider manages the infrastructure, maintenance, security updates, and availability.

- **No upfront hardware cost**
- **Pay-as-you-go pricing**
- **Scale resources on demand**
- **High availability** with data centers around the world
- **Provider handles maintenance and updates**

---

**Q2. What is IaaS, PaaS, and SaaS? Give a simple way to remember.**

| Service Model | **IaaS** | **PaaS** | **SaaS** |
|---|---|---|---|
| **What it is** | Virtual infrastructure | Platform to build and deploy apps | Ready-to-use software |
| **You manage** | OS, runtime, applications, and data | Applications and data | Nothing (just use the software) |
| **Cloud provider manages** | Hardware, networking, storage, virtualization | Infrastructure + OS + runtime + patching | Everything |
| **Azure examples** | Azure Virtual Machine (VM) | Azure App Service, Azure SQL Database | Microsoft 365 |

- **IaaS** → Renting an **empty flat** (you furnish and manage everything inside).
- **PaaS** → Renting a **furnished flat** (you only bring your belongings).
- **SaaS** → Staying in a **hotel** (everything is managed for you).

---

**Q3. What is the difference between Public, Private, and Hybrid cloud?**


| Feature | **Public Cloud** | **Private Cloud** | **Hybrid Cloud** |
|---|---|---|---|
| **Infrastructure** | Shared with multiple customers | Dedicated to one organization | Combination of public and private clouds |
| **Managed by** | Cloud provider | Organization or dedicated provider | Both organization and cloud provider |
| **Cost** | Lowest | Highest | Moderate |
| **Scalability** | Very high | Limited by owned infrastructure | High |
| **Security & Control** | Good | Highest | High |
| **Best For** | Startups, web apps, SaaS | Banks, government, regulated industries | Large enterprises migrating to the cloud |
| **Azure Example** | Azure Virtual Machines, App Service | Azure Stack HCI | On-premises + Azure connected via VPN or ExpressRoute |

---

**Q4. Why do Azure Regions always come in Pairs?**

**Region Pairs** are two Azure regions within the same geography that are paired for **disaster recovery** and **business continuity**.

If one region becomes unavailable due to a major outage, Microsoft prioritizes recovery of its paired region. Region pairs also help with planned updates and geo-redundant services.

- Disaster recovery (DR)
- High availability
- Planned maintenance with minimal downtime
- Geo-redundant data replication

**Example:** If your application is hosted in **Central India**, you can replicate critical data to its paired region so the application can recover during a regional outage.

---

**Q5. What are Region, Availability Zone, and Geography?**

- **Geography**: a big area like "India" or "United States" — used for compliance/legal boundaries.
- **Region**: a specific location with data centers, e.g., "Central India".
- **Availability Zone (AZ)**: separate physical buildings inside one region, each with its own power and cooling. Spreading your app across 2-3 AZs protects against one building going down.
- **Region pair**: two regions paired together for disaster recovery, e.g., Central India ↔ South India.

---

**Q6. Explain Azure's resource hierarchy.**

```text
Management Group
    └── Subscription
          └── Resource Group
                └── Resource (VM, Storage Account, SQL Database, etc.)
```

| Level | Purpose |
|---|---|
| **Management Group** | Groups multiple subscriptions to apply common policies and permissions. |
| **Subscription** | Billing, quota, and access boundary for Azure resources. |
| **Resource Group (RG)** | Logical container for related resources that share the same lifecycle (deploy, update, delete together). |
| **Resource** | The actual Azure service, such as a VM, Storage Account, App Service, or SQL Database. |

---

**Q7. Portal vs CLI vs PowerShell vs SDK — what's the difference?**

| Tool | Best for |
|---|---|
| Azure Portal | Learning, one-time manual changes, exploring |
| Azure CLI (`az`) | Scripts, Linux/Mac users, CI/CD pipelines |
| Azure PowerShell | Windows admins who like PowerShell objects |
| Azure SDK (.NET, etc.) | Calling Azure directly from your application code |

All four send requests to the same backend — **Azure Resource Manager (ARM)**.

---

**Q8. What is a Resource Group and why do we need it?**

A **Resource Group (RG)** is a **logical container** that holds related Azure resources, such as Virtual Machines, Storage Accounts, Databases, and App Services.

Resources that belong to the same application or environment are typically placed in the same Resource Group so they can be **managed, monitored, and deleted together**.

- Organizes related Azure resources.
- Apply **RBAC permissions**, **Azure Policies**, and **tags** at the Resource Group level.
- Simplifies deployment, monitoring, and cost management.
- Deleting the Resource Group **deletes all resources inside it**.

---

**Q9. What are Resource Tags?**

Tags are key-value labels you attach to a resource, like `Environment=Prod` or `Owner=TeamA`. Think of them as sticky notes on a resource that help you organize and automate things later. They help with:

- **Cost tracking** — see how much each team/client/project is spending; this matters a lot when one Azure subscription is shared across multiple client projects in an MNC delivery model.
- **Automation** — e.g., a script can auto shut down every VM tagged `AutoShutdown=true` at night to save dev/test cost.
- **Governance** — Azure Policy can be set up to block creation of any resource that's missing a required tag, so nothing ever goes untagged.

---

**Q10. What is the Azure Well-Architected Framework?**

The **Azure Well-Architected Framework (WAF)** is Microsoft's set of best practices for designing **secure, reliable, high-performing, cost-effective, and operationally efficient** cloud applications.

1. **Reliability** — does it survive failures?
2. **Security** — least privilege, encryption, zero trust.
3. **Cost Optimization** — are you paying only for what's needed?
4. **Operational Excellence** — automation, monitoring, CI/CD.
5. **Performance Efficiency** — right service, right size, caching.

`Azure Advisor` checks your live environment against this framework and gives suggestions.

---

## Section 2: Governance & Resource Manager (Q11–Q15)

**Q11. What is Azure Resource Manager (ARM)?**

**Azure Resource Manager (ARM)** is the **deployment and management service** for Azure. It acts as the control layer that creates, updates, and deletes Azure resources.

Whether you use the **Azure Portal**, **Azure CLI**, **PowerShell**, **SDKs**, or **REST APIs**, all requests are processed by ARM.

- Deploy and manage Azure resources consistently.
- Organize resources using **Resource Groups**.
- Apply **RBAC**, **Azure Policies**, and **tags**.
- Support **Infrastructure as Code (IaC)** using **ARM Templates** and **Bicep**.

---

**Q12. What is the difference between Azure Policy and Resource Locks?**

| **Azure Policy** | **Resource Locks** |
|---|---|
| Enforces governance and compliance rules. | Prevents accidental changes or deletion of resources. |
| Controls **what can or cannot be deployed**. | Protects **existing resources**. |
| Example: Allow resources only in **Central India** or require specific tags. | Example: Prevent a production Resource Group from being deleted. |

#### Types of Resource Locks
- **CanNotDelete** – Resources can be modified but **cannot be deleted**.
- **ReadOnly** – Resources **cannot be modified or deleted**.

---

**Q13. What is a Azure Landing Zone?**

An **Azure Landing Zone** is a **pre-configured, secure, and governed Azure environment** that serves as the foundation for deploying workloads.

It includes standard configurations for **networking, identity, security, RBAC, policies, and monitoring**, ensuring every new project follows the same best practices without starting from scratch.

**Benefits:** Faster deployments, consistent security, and easier governance across all Azure projects.

---

**Q14. What is Azure Policy and how is it different from RBAC?**


**Azure Policy** enforces **governance and compliance** by controlling **what resources and configurations are allowed** in Azure.

**RBAC** (Role-Based Access Control) controls **who can perform actions** on Azure resources.

| Feature | **RBAC** | **Azure Policy** |
|---|---|---|
| **Controls** | Who can perform actions | What can or cannot be deployed |
| **Purpose** | Access management | Governance and compliance |
| **Example** | Only Team A can create Virtual Machines | Storage accounts must use HTTPS and approved regions |

**Example:** A user with **Contributor** access (RBAC) can create a Storage Account, but if an **Azure Policy** requires HTTPS or restricts deployment to **Central India**, the deployment will fail if those rules aren't met.

---

**Q15. What is the difference between Azure RBAC and Microsoft Entra ID roles?**

| **Azure RBAC** | **Microsoft Entra ID Roles** |
|---|---|
| Controls access to **Azure resources**. | Controls administrative access to **Microsoft Entra ID**. |
| Examples: Virtual Machines, Storage Accounts, Resource Groups. | Examples: Users, Groups, Applications, Password Reset. |
| Common roles: Owner, Contributor, Reader. | Common roles: Global Administrator, User Administrator, Security Administrator. |

**Example:** A **Contributor** can create Azure resources but cannot create new users in Microsoft Entra ID. A **Global Administrator** can manage users and groups but doesn't automatically have access to Azure resources.

---

## Section 3: Identity & Access Management (Q16–Q25)

**Q16. What is Azure AD (now called Microsoft Entra ID)?**

**Microsoft Entra ID** is Microsoft's **cloud-based Identity and Access Management (IAM)** service. It authenticates users, manages identities, and controls access to Azure, Microsoft 365, and other enterprise applications.

It provides features such as **Single Sign-On (SSO)**, **Multi-Factor Authentication (MFA)**, **Conditional Access**, and identity-based security.

- Centralized user and identity management.
- One login (**SSO**) for multiple applications.
- Enhanced security with **MFA** and **Conditional Access**.
- Integrates with Azure, Microsoft 365, and thousands of third-party applications.

**Example:** An employee signs in once using their Microsoft Entra ID account and can securely access **Azure Portal**, **Microsoft Teams**, **Outlook**, and other enterprise applications without logging in again.


---

**Q17. Azure AD vs Azure AD B2C — what's the difference?**

| | Azure AD (Entra ID) | Azure AD B2C |
|---|---|---|
| Used for | Company employees | External customers |
| Login type | Corporate SSO, MFA | Social logins (Google, Facebook) + local accounts |
| Tenant | Your company tenant | Separate customer-facing tenant |

Simple way to remember: Azure AD = staff. B2C = customers.

---

**Q18. What is App Registration?**

It's how you tell Azure AD "this is my application." Registering gives you:

- **Application (client) ID** — unique ID for your app.
- **Redirect URI** — where Azure AD sends the user back after login.
- **Client secret/certificate** — used by confidential apps (web apps, daemons) to prove identity.
- **API permissions** — what other APIs your app is allowed to call.

Path: **Azure AD → App registrations → New registration**.

---

**Q19. Service Principal vs Managed Identity — what's the difference?**

| Feature | **Service Principal** | **Managed Identity** |
|---|---|---|
| **Created by** | Manually by the user/admin | Automatically by Azure |
| **Credentials** | Client secret or certificate | No credentials to manage |
| **Secret Rotation** | Managed by you | Automatically handled by Azure |
| **Best Used For** | External applications (GitHub Actions, Jenkins, on-premises apps) | Azure services (VMs, App Service, Azure Functions, Logic Apps) |
| **Security** | Higher risk if secrets are exposed | More secure—no secrets stored in code |

**Example:**  
- Use a **Managed Identity** for an Azure Function to securely access an Azure Key Vault or Storage Account.
- Use a **Service Principal** when GitHub Actions needs to deploy resources to Azure.

---

**Q20. What is RBAC (Role-Based Access Control)?**

It decides who can do what, where. Three parts:

- **Who** (Security Principal) — a user, group, or app identity.
- **What** (Role) — Owner, Contributor, Reader, or a Custom Role.
- **Where** (Scope) — Management Group, Subscription, Resource Group, or single Resource.

**Example:**
- Assign the **Contributor** role to a developer on a **Resource Group**.
- The developer can create, update, and delete resources **only within that Resource Group**, but cannot manage access permissions.

Set it in Portal: **Resource → Access control (IAM) → Add role assignment**.

---

**Q21. Authentication vs Authorization — explain with a .NET Web API example.**

- **Authentication (AuthN)** = proving who you are (login). Result: a signed JWT token.
- **Authorization (AuthZ)** = checking what you're allowed to do, using the claims inside that token (roles, scopes).

```csharp
// AuthN — validates the token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// AuthZ — checks a specific claim/role
builder.Services.AddAuthorization(options =>
    options.AddPolicy("CanApprove", p => p.RequireClaim("roles", "Orders.Approve")));

[Authorize(Policy = "CanApprove")]
[HttpPost("approve")]
public IActionResult Approve(int orderId) => Ok();
```


---

**Q22. What is Azure Key Vault, and how do you use it from a .NET app?**

A secure vault for secrets (connection strings, API keys), certificates, and encryption keys.

Steps:
1. Create the Key Vault.
2. Turn on Managed Identity for your App Service.
3. Give that identity the **Key Vault Secrets User** role.
4. Read secrets in code:

```csharp
builder.Configuration.AddAzureKeyVault(
    new Uri("https://myvault.vault.azure.net"),
    new DefaultAzureCredential());
var connStr = builder.Configuration["MyDbConnectionString"];
```


---

**Q23. What is a multi-tenant application, and what is one common mistake developers make?**

A **multi-tenant application** allows users from **multiple Microsoft Entra ID (Azure AD) tenants** (different organizations) to sign in and use the same application. It is commonly used for **SaaS products**.

**Common mistake:** Using only the **`oid` (Object ID)** to identify a user.

- ❌ `oid` alone is **not globally unique**.
- ✅ Always use **`tid` (Tenant ID) + `oid` (Object ID)** together to uniquely identify a user across all tenants.

---

**Q24. What is Conditional Accessin Microsoft Entra ID?**

**Conditional Access** is a security feature in **Microsoft Entra ID** that applies **access policies based on conditions** such as the user's location, device, application, sign-in risk, or user/group membership.

Instead of simply allowing or denying sign-in, it can enforce additional security requirements like **Multi-Factor Authentication (MFA)** or block access altogether.

**Examples:**
- Block sign-ins from countries outside India.
- Require MFA when signing in from an unmanaged device.
- Allow access only from compliant devices.

---

**Q25. What is Privileged Identity Management (PIM)?**

**Privileged Identity Management (PIM)** is a Microsoft Entra ID feature that provides **just-in-time (JIT)** privileged access to Azure resources.

Instead of giving users **permanent admin roles**, PIM lets them **activate** roles (such as Owner or Contributor) **only when needed**, for a limited time. Access can require **approval**, **MFA**, and is **fully audited**.

**Example:**
A cloud administrator requests the **Owner** role for **4 hours** to perform maintenance. After the time expires, the elevated access is automatically removed.

---

## Section 4: Compute — App Service & VMs (Q26–Q35)

**Q26. App Service vs App Service Plan — what's the difference?**

| **App Service Plan** | **App Service** |
|----------------------|-----------------|
| Provides the **compute infrastructure** (VMs) | The **web app, API, or mobile backend** you deploy |
| Defines **pricing**, VM size, region, OS, and scaling | Hosts your application code |
| **This is what you pay for** | No additional compute cost if it shares an existing plan |
| One plan can host **multiple App Services** | Multiple apps can run on the same plan |

---

**Q27. What are App Service pricing tiers?**

| Tier | Use case |
|---|---|
| Free / Shared | Learning, hobby projects only |
| Basic | Small production, no auto-scale |
| Standard | Auto-scale + deployment slots + custom domain |
| Premium (v3) | VNet integration, more slots, faster CPU |
| Isolated (ASE) | Fully private environment, used for strict compliance |

Approach: list must-have features first (slots? VNet? auto-scale?), then pick the cheapest tier that satisfies all of them.

---

**Q28. What are Deployment Slots, and how do they give zero-downtime deployment?**

A slot is a separate live copy of your app (e.g., "staging") with its own URL, inside the same App Service.

```
1. Deploy new code → staging slot
2. Test on staging URL
3. Swap staging ↔ production (instant traffic switch)
4. If broken → swap back instantly
```

Note: connection strings marked "slot-specific" do NOT swap — they stay with the slot. Available from Standard tier upward.

---

**Q29. How do you make an App Service private (not reachable from the internet)?**

| Method | Purpose |
|--------|---------|
| **Access Restrictions** | Allow or block traffic based on IP addresses or virtual networks. |
| **Private Endpoint + VNet** | Gives the App Service a **private IP** so it is accessible only from your virtual network or connected networks. |
| **Application Gateway / Azure Front Door + WAF** | Expose only the gateway to the internet while filtering and securing incoming traffic before it reaches the App Service. |

**Best practice for internal enterprise applications:**
Use **Private Endpoint + Virtual Network (VNet)** so the App Service is **not publicly accessible** and can only be reached through your private network.

---

**Q30. VNet Integration vs Private Endpoint — this confuses a lot of people. Explain clearly.**

| Feature | Traffic Direction | Purpose | Example |
|---------|-------------------|---------|---------|
| **VNet Integration** | **Outbound** (App ➜ Resource) | Allows an App Service to access resources inside a Virtual Network. | App Service connects to a private Azure SQL Database or Storage Account. |
| **Private Endpoint** | **Inbound** (Client ➜ App) | Gives the App Service a **private IP** so it can only be accessed from a Virtual Network or connected networks. | Employees access the internal web app over VPN/ExpressRoute instead of the public internet. |

- **VNet Integration** = **App can reach private resources.**
- **Private Endpoint** = **Private users can reach the app.**


Use **both** together:
- **VNet Integration** → App securely accesses private services (Azure SQL, Storage, Key Vault, etc.).
- **Private Endpoint** → The App Service itself is **not publicly accessible** and is reachable only through the private network.

---

**Q31. Application Gateway vs Azure Front Door — when to use which?**

| Feature | Application Gateway | Azure Front Door |
|---------|----------------------|------------------|
| **Scope** | Regional | Global |
| **Traffic** | Routes traffic within a single Azure region | Routes users to the nearest healthy region worldwide |
| **Best for** | Internal or regional web applications | Public internet-facing applications with global users |
| **Routing** | Path-based and host-based routing | Global load balancing, latency-based routing, and failover |
| **Security** | Supports Web Application Firewall (WAF) | Supports Web Application Firewall (WAF) at the global edge |
| **Typical Use Case** | Route traffic to multiple apps/APIs within one region | Improve performance and high availability across multiple regions |

- **Application Gateway:** An internal enterprise application deployed only in **Central India**.
- **Azure Front Door:** An e-commerce website deployed in **India** and **US**. Users are automatically routed to the **nearest healthy region**, reducing latency and providing failover.

---

**Q32. Scale Up vs Scale Out — what's the difference?**

| Feature | Scale Up (Vertical Scaling) | Scale Out (Horizontal Scaling) |
|---------|------------------------------|--------------------------------|
| **What it does** | Increases the size of the existing instance (more CPU/RAM). | Adds more instances of the same App Service. |
| **Use when** | A single instance needs more power. | More users or requests need to be handled. |
| **Downtime** | May cause brief downtime during resizing. | Typically no downtime. |
| **Limit** | Limited by the largest available SKU. | Can scale to multiple instances (subject to plan limits). |
| **Requirement** | No code changes required. | Application should be **stateless**. |

- **Scale Up:** Change from **B1 → S1 → P1v3** for more CPU and memory.
- **Scale Out:** Increase instances from **2 → 5** to serve more users.

---

**Q33. What is an Azure Virtual Machine and when would you still use IaaS instead of PaaS?**

| **Use Azure VM (IaaS)** | **Use Azure App Service (PaaS)** |
|-------------------------|----------------------------------|
| Need full OS or administrator access | Only need to deploy web apps or APIs |
| Running legacy or third-party software | Modern web applications and REST APIs |
| Require custom OS configurations or drivers | Microsoft manages OS, patching, and infrastructure |
| Hosting applications that cannot run on PaaS | Faster deployment with minimal infrastructure management |

- **Azure VM:** Hosting a legacy Windows application that requires installing custom software and Windows Services.
- **Azure App Service:** Hosting an ASP.NET Core Web API without managing servers.

---

**Q34. What are Virtual Machine Scale Sets (VMSS)?**

 **Azure Virtual Machine Scale Set (VMSS)** is a group of **identical Virtual Machines** that can **automatically scale out or scale in** based on demand, while being managed as a single resource.

VMSS is ideal for applications that require **IaaS-level control** along with **high availability** and **automatic scaling**.

**Common use cases:**
- Self-hosted Azure DevOps agents
- Legacy applications that must run on VMs
- High-traffic web applications requiring custom VM configurations
- Batch processing and compute-intensive workloads

---

**Q35. What is Azure Bastion?**

**Azure Bastion** is a fully managed service that lets you securely connect to **Azure Virtual Machines** using **RDP (Windows)** or **SSH (Linux)** **directly from the Azure Portal**, without exposing the VMs to the public internet.

It eliminates the need to assign **public IP addresses** to VMs or maintain a **jump box (bastion host)**, significantly improving security.

- Secure RDP/SSH access over **TLS (HTTPS)**
- No public IP required on the VM
- No need to open RDP (3389) or SSH (22) ports to the internet
- Reduces the attack surface and simplifies secure VM administration

---

**Q36. How do you configure a custom domain and HTTPS for an Azure App Service?**

1. Add your **custom domain** (e.g., `www.contoso.com`) in the App Service.
2. Create the required **DNS records** (CNAME or A record) with your domain provider.
3. Validate domain ownership.
4. Bind an **SSL/TLS certificate** (App Service Managed Certificate or your own certificate).
5. Enable **HTTPS Only** to force secure connections.

**Example:**
Users access `https://www.contoso.com` instead of the default `https://myapp.azurewebsites.net`.

---
**Q37. How do Azure Application Settings override `appsettings.json`?**

Azure **Application Settings** are **environment variables** that are loaded **after** `appsettings.json`. If the same configuration key exists in both, the **Application Setting overrides** the value in `appsettings.json`.

**App Service → Settings → Environment variables → Azure Application settings**

**Example:**

- `appsettings.json`
  ```
  ConnectionStrings:DefaultConnection = DevDb
  ```

- Azure Application Setting
  ```
  ConnectionStrings__DefaultConnection = ProdDb
  ```

**Result:** The application uses **`ProdDb`**.

---

## Section 5: Azure Functions & Serverless (Q36–Q42)

**Q38. What are the common trigger types in Azure Functions?**

| Trigger | Fires when |
|---|---|
| HTTP Trigger | An HTTP request comes in |
| Timer Trigger | On a schedule (CRON expression) |
| Queue Trigger | A message lands in Storage Queue / Service Bus |
| Blob Trigger | A file is uploaded to Blob Storage |
| Event Grid Trigger | An event happens elsewhere in Azure |

```csharp
[Function("ProcessOrder")]
public void Run([QueueTrigger("orders")] string message)
{
    // process the order
}
```

---

**Q39. Compare Consumption, Premium, and Dedicated hosting plans for Functions.**

| Plan | Cold start? | Scale | Best for |
|---|---|---|---|
| Consumption | Yes | 0 to many automatically | Unpredictable, low-traffic workloads |
| Premium | No (pre-warmed) | Min instances + burst | Latency-sensitive, needs VNet |
| Dedicated (App Service Plan) | No | Manual/auto | Predictable load, reusing existing plan |

---

**Q40. How do Azure Functions handle bindings (input/output)?**

An **Azure Function** runs when a **Trigger** fires, and **Bindings** let it read from or write to other Azure services **without writing connection or SDK code**.

| Component | Purpose | Example |
|-----------|---------|---------|
| **Trigger** | Starts the function | HTTP request, Timer, Queue message, Blob upload |
| **Input Binding** | Reads data from a service | Read a Queue message or Blob |
| **Output Binding** | Writes data to a service | Write to Blob Storage, Queue, or Cosmos DB |

**Example:**
A message arrives in **Azure Queue Storage** (**Queue Trigger**). The function processes it and saves the result to **Blob Storage** using an **Output Binding**.

---

**Q41. How do you secure an HTTP-triggered Azure Function?**

Azure Functions can be secured using multiple layers, depending on the scenario:

| Method | Use Case |
|---------|----------|
| **Function Key** | Simple authentication using a shared key (`?code=...`). Suitable for internal or low-risk scenarios. |
| **Microsoft Entra ID (Azure AD) Authentication** | Requires users or applications to present a valid JWT access token. Recommended for production APIs. |
| **Private Endpoint + VNet** | Makes the Function accessible only from a private network, blocking public internet access. |

**Example:**
A public-facing Azure Function API uses **Microsoft Entra ID authentication** so only authenticated users or applications with a valid access token can invoke it.

---

**Q42. What is a cold start in Azure Functions, and how can you reduce it?**

A **cold start** is the delay that occurs when an Azure Function is invoked after being idle. Azure needs to **start a new function host** before executing the request, increasing the response time.

**How to reduce cold starts:**
- Use a **Premium Plan** (keeps pre-warmed instances ready).
- Use a **Dedicated (App Service) Plan** with **Always On** enabled.
- Keep functions lightweight and minimize startup initialization.

**Example:**
A rarely used HTTP-triggered function on the **Consumption Plan** may take a few seconds to respond to the first request, while subsequent requests are much faster.

---

## Section 6: Storage (Q43–Q52)

**Q43. What are the main types of Azure Storage?**

| Type | Used for |
|---|---|
| Blob Storage | Unstructured files (images, videos, backups) |
| Table Storage | Simple NoSQL key-value data |
| Queue Storage | Simple messaging (max 64 KB/message) |
| File Storage | Managed SMB file shares (replaces file servers) |
| Disk Storage | Managed disks attached to VMs |

For .NET, use the `Azure.Storage.*` SDK packages with `DefaultAzureCredential` instead of account keys.

---

**Q44. Explain Blob Storage access tiers.**

| Tier | Storage cost | Access cost | Speed | Use case |
|---|---|---|---|---|
| Hot | High | Low | Instant | Frequently used files |
| Cool | Low | Higher | Instant | Backups accessed rarely (30+ days) |
| Cold | Lower | Higher | Instant | Very rarely accessed (90+ days) |
| Archive | Lowest | Highest | Hours (rehydrate needed) | Long-term compliance archive (180+ days) |

Use **Lifecycle Management** rules to auto-move old blobs to cheaper tiers — saves a lot on long-term audit logs.

---

**Q45. Explain Storage redundancy options: LRS, ZRS, GRS, RA-GRS.**

| Option | Copies | Survives |
|---|---|---|
| LRS | 3 copies, 1 datacenter | Disk/server failure |
| ZRS | 3 copies, 3 zones, same region | Datacenter failure |
| GRS | LRS + async copy to paired region | Region failure (manual failover) |
| RA-GRS | GRS + read access on secondary region | Region failure, with read access |


---

**Q46. Access Keys vs SAS Token vs Managed Identity — rank them by security.**

| Option | Security level | Notes |
|---|---|---|
| Account Access Key | Lowest | Full access to entire account, rotate manually |
| SAS Token | Medium | Time-limited, scoped to specific resource/permission |
| Managed Identity + RBAC | Highest | No secret at all, Azure handles everything |

```csharp
// Generate a time-limited SAS (read-only, 1 hour)
var sasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(1));
```

---

**Q47. Azure Files vs Blob Storage — when to use which?**

| Feature | **Azure Blob Storage** | **Azure Files** |
|---------|-------------------------|-----------------|
| **Storage Type** | Object storage | Managed file share |
| **Access** | REST API/SDK | SMB or NFS (network file share) |
| **Best For** | Cloud-native applications, images, videos, backups, logs | Lift-and-shift applications, shared folders, legacy apps |
| **Mount as Drive** | ❌ No | ✅ Yes (like a network drive) |

- **Blob Storage:** Store application uploads, backups, or media files.
- **Azure Files:** Share files between multiple VMs or migrate an application that expects a Windows UNC path (`\\Server\Share`).

---

**Q48. What is a Storage Account, and what's inside it?**

An **Azure Storage Account** is the **top-level Azure resource** that provides a namespace for storing data. It can contain multiple storage services under a single account.

When creating a Storage Account, you choose:
- **Performance:** Standard or Premium
- **Redundancy:** LRS, ZRS, GRS, or RA-GRS
- **Access Tier:** Hot, Cool, or Cold (for Blob Storage)

---

**Q49. How do you secure a Storage Account from public access?**

To secure an **Azure Storage Account**, restrict who can access it and avoid exposing it to the public internet.

- **Disable public network access** to prevent internet access.
- Use a **Private Endpoint** so the Storage Account is accessible only through a private IP in your Virtual Network.
- Configure **Firewall and Virtual Network rules** to allow access only from trusted IP addresses or VNets.
- Use **Microsoft Entra ID (Azure AD)** and **RBAC** instead of storage account keys whenever possible.
- If using **Shared Access Signatures (SAS)**, grant the **least privilege** and use **short expiration times**.

---

**Q50. What are Lifecycle Management Policies in Blob Storage?**

**Lifecycle Management Policies** are rules that **automatically move or delete blobs** based on conditions such as their age or last modified date. They help reduce storage costs by placing data in the most appropriate access tier.

**Common actions:**
- Move blobs to **Cool** tier after 30 days.
- Move blobs to **Archive** tier after 180 days.
- Delete blobs after a specified retention period (e.g., 7 years).

---

**Q51. What is Soft Delete and Versioning in Blob Storage?**

| Feature | Purpose |
|---------|---------|
| **Soft Delete** | Retains deleted blobs for a configurable retention period, allowing them to be restored before permanent deletion. |
| **Versioning** | Creates a new version every time a blob is modified or overwritten, allowing previous versions to be restored. |

**Example:**
- A user accidentally deletes a file → **Soft Delete** allows it to be recovered.
- A file is overwritten with incorrect data → **Versioning** lets you restore the previous version.

---

**Q52. How do you connect a .NET app to Blob Storage securely (code example)?**

```csharp
var blobServiceClient = new BlobServiceClient(
    new Uri("https://mystorageaccount.blob.core.windows.net"),
    new DefaultAzureCredential());

var containerClient = blobServiceClient.GetBlobContainerClient("uploads");
await containerClient.UploadBlobAsync("file.pdf", fileStream);
```

`DefaultAzureCredential` automatically uses Managed Identity in Azure, and your local developer login when testing locally — same code works in both places.

---

## Section 7: Databases (Q53–Q64)

**Q53. Azure SQL vs Cosmos DB vs Table Storage — how do you choose?**

| Feature | Azure SQL | Cosmos DB | Table Storage |
|---|---|---|---|
| Type | Relational | NoSQL (multi-model) | NoSQL key-value |
| Query | T-SQL | SQL/Mongo/etc. APIs | Limited (OData) |
| Scale | Vertical + replicas | Global, horizontal | Horizontal |
| Cost | Medium | Higher | Very cheap |

Decision rule: relational data with joins/transactions → Azure SQL. Global low-latency, flexible schema, huge write throughput → Cosmos DB. Simple cheap key-value lookups → Table Storage.

---

**Q54. Azure SQL Database vs Managed Instance vs SQL Server on a VM — what's the migration path?**

| Option | What you get | Best for |
|---|---|---|
| Azure SQL Database | Fully managed PaaS | New cloud-native apps |
| Managed Instance | Instance-level features (SQL Agent, cross-DB queries) | Lift-and-shift from on-prem with minimal code change |
| SQL Server on VM | Full IaaS control | Apps needing specific OS/SQL edition features |

Typical migration order: assess with **Data Migration Assistant** → if app uses SQL Agent jobs/linked servers, go to **Managed Instance** first → later modernize toward **Azure SQL Database**.

---

**Q55. DTU vs vCore pricing for Azure SQL — which should you pick?**

| Feature | **DTU Model** | **vCore Model** |
|---------|---------------|-----------------|
| **Resources** | CPU, memory, and I/O bundled into a single unit | CPU, memory, and storage are configured separately |
| **Flexibility** | Limited | More flexible and transparent |
| **Advanced Features** | Limited | Supports **Serverless**, **Hyperscale**, and **Azure Hybrid Benefit** |
| **Best For** | Simple or small workloads | Most production and enterprise workloads |

Recommendation: default to **vCore** for new projects — more transparent and cheaper long term if the client already owns SQL Server licenses.

---

**Q56. What are Cosmos DB consistency levels (in order from strongest to weakest)?**

1. **Strong** — always see latest write. Slowest.
2. **Bounded Staleness** — reads lag by limited time/operations.
3. **Session** — default; consistent within one client session.
4. **Consistent Prefix** — never see out-of-order writes.
5. **Eventual** — fastest, no ordering guarantee.

Rule: weaker consistency = faster and cheaper. Use the strongest level you can actually afford.

---

**Q57. How do you connect .NET to Azure SQL using Managed Identity (no password)?**

```sql
CREATE USER [my-app-name] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [my-app-name];
ALTER ROLE db_datawriter ADD MEMBER [my-app-name];
```

```csharp
var conn = new SqlConnection(
    "Server=myserver.database.windows.net;Database=mydb;Authentication=Active Directory Default;");
```

No secrets stored anywhere — Azure handles the token behind the scenes.

---

**Q58. What is Azure Redis Cache, and why use it?**

A managed in-memory cache used to speed up your app and reduce database load. Common uses:

- Caching expensive database query results.
- Storing session state when running multiple app instances (in-memory session doesn't work across instances).
- Rate limiting counters.

```csharp
builder.Services.AddStackExchangeRedisCache(o =>
    o.Configuration = builder.Configuration["RedisConnectionString"]);
```

---

**Q59. What is an Elastic Pool in Azure SQL?**

An **Elastic Pool** is a collection of **Azure SQL Databases** that **share a common pool of compute resources (CPU and memory)** instead of each database having dedicated resources.

It is ideal when multiple databases have **unpredictable or varying workloads**, as idle databases can share resources with busy ones, reducing costs.

**Example:**
A SaaS application has **one database per customer**. Since not all customers are active at the same time, placing the databases in an **Elastic Pool** allows them to share resources instead of paying for each database individually.

---

**Q60. What is database sharding/partitioning, and why does it matter for scale?**

**Database sharding** is the practice of **splitting a large dataset across multiple databases or partitions** so that no single database becomes a performance or storage bottleneck.

This improves **scalability**, **performance**, and **availability** by distributing data and workload.

**Example:**
A SaaS application stores **one database per customer** or partitions data by **Customer ID** or **Region**, so requests are spread across multiple databases instead of one large database.

---

**Q61. How do you implement Backup and DR for Azure SQL?**

- **Automated backups** — built-in, geo-redundant by default.
- **Active geo-replication** — readable secondary copy in another region, manual failover.
- **Auto-failover groups** — automatic failover to secondary region, app connection string doesn't need to change.

Always define **RTO** (how fast you need to recover) and **RPO** (how much data loss is acceptable) before choosing the DR setup — tighter numbers cost more.

---

**Q62. How do you monitor the performance of an Azure SQL Database**

Azure provides several tools to monitor database health and performance:

- **Azure Monitor** – Tracks CPU, DTU/vCore usage, storage, and other metrics.
- **Query Performance Insight** – Identifies slow and resource-intensive queries.
- **Query Store** – Stores query history and execution plans for troubleshooting.
- **Intelligent Insights** – Automatically detects and recommends fixes for performance issues.

---

**Q63. What is a Read Replica, and when would you add one?**

A **Read Replica** is a **read-only copy** of a primary database that stays synchronized with it. It allows **read-only queries** to be offloaded from the primary database, improving performance and scalability.

**When to use it:**
- Heavy reporting or analytics
- Dashboards with frequent read requests
- Applications with significantly more reads than writes

**Example:**
An e-commerce application sends **customer orders and updates** to the primary database, while **product searches, reports, and dashboards** query the read replica. This reduces the load on the primary database and improves overall performance.

---

**Q64. How would you decide between Azure SQL and Cosmos DB for a brand-new microservice?**

| **Azure SQL** | **Azure Cosmos DB** |
|---------------|---------------------|
| Relational data with complex relationships | NoSQL data with flexible schema |
| Supports joins, transactions, and SQL queries | Optimized for massive scale and low-latency reads/writes |
| Best for transactional (OLTP) applications | Best for globally distributed and high-throughput applications |
| Uses a fixed schema | Supports schema evolution |

**Examples:**
- **Azure SQL:** Banking, order management, ERP, inventory systems.
- **Azure Cosmos DB:** IoT, user profiles, product catalogs, gaming, real-time applications.

---

## Section 8: Messaging & Integration (Q65–Q70)

**Q65. Storage Queue vs Service Bus — what's the difference?**

| Feature | Storage Queue | Service Bus |
|---|---|---|
| Max message size | 64 KB | 256 KB (Std) / 100 MB (Premium) |
| Dead-letter queue | No | Yes |
| Ordering | Best-effort | Guaranteed (sessions) |
| Pub/sub (Topics) | No | Yes |

Use Storage Queue for simple, cheap, high-volume jobs. Use Service Bus for enterprise workflows needing ordering, dead-lettering, or pub/sub.

---

**Q66. Service Bus Queue vs Topic — what's the difference?**

| **Queue** | **Topic** |
|-----------|-----------|
| **Point-to-point** messaging | **Publish/Subscribe (Pub/Sub)** messaging |
| Each message is processed by **one receiver** | Each **subscription** receives its own copy of the message |
| Best for background processing and work distribution | Best for notifying multiple independent services |

**Example:**
An **Order Service** publishes an `OrderPlaced` message:
- **Queue:** One **Order Processor** consumes the message.
- **Topic:** Multiple subscribers—**Inventory**, **Email**, **Billing**, and **Analytics**—each receive and process their own copy independently.

---

**Q67. Event Grid vs Event Hub vs Service Bus — quick comparison.**

These Azure messaging services serve different purposes:


| Service | Best For | Messaging Pattern | Example |
|---------|----------|-------------------|---------|
| **Event Grid** | Event notifications | Event-driven (Pub/Sub) | Trigger a Function when a Blob is uploaded |
| **Service Bus** | Reliable message processing | Queue / Publish-Subscribe | Process orders, payments, and workflows |
| **Event Hubs** | High-volume event streaming | Streaming ingestion | Collect IoT telemetry or application logs |

**Examples:**
- **Event Grid:** A file is uploaded to Blob Storage → automatically trigger an Azure Function.
- **Service Bus:** An `OrderPlaced` message is processed reliably by downstream services.
- **Event Hubs:** Millions of IoT devices continuously send telemetry data for real-time analytics.

---

**Q68. What is a Dead-Letter Queue (DLQ)?**

A **Dead-Letter Queue (DLQ)** is a special queue that stores **messages that cannot be processed successfully** after exceeding the maximum retry count or due to errors such as invalid format or expiration.

Instead of losing these messages, they are moved to the DLQ so they can be **inspected, fixed, and reprocessed** later.

**Example:**
An order message contains invalid data. After several failed processing attempts, it is moved to the **Dead-Letter Queue** instead of blocking the main queue.

---

**Q69. What is idempotency, and why does it matter in messaging systems?**

**Idempotency** means that **processing the same message multiple times produces the same result**. It prevents duplicate side effects when a message is delivered more than once.

This is important because messaging systems may **redeliver messages** if processing fails or acknowledgments are lost.

**Example:**
An `OrderPlaced` message is delivered twice. The application checks the **Message ID** or **Order ID** and processes it only once, preventing duplicate orders or charges.

---

**Q70. How would you design an order-processing system using Azure messaging services?**

Use an **Azure Service Bus Topic** to publish the `OrderPlaced` event, allowing multiple services to process the order independently through **Subscriptions**.

```text
OrderPlaced (Service Bus Topic)
   ├── Inventory Subscription  → Reduce stock
   ├── Payment Subscription    → Process payment
   ├── Email Subscription      → Send confirmation
   └── Analytics Subscription  → Update reports
```

#### Benefits
- Services are **loosely coupled** and process messages independently.
- New subscribers can be added without changing the publisher.
- If one subscriber fails, only its message is moved to its **Dead-Letter Queue (DLQ)**—other subscribers continue processing normally.

---

## Section 9: Docker & Containers (Q71–Q78)

**Q71. What is Docker, and why is it useful for .NET apps?**

**Docker** is a containerization platform that packages an application along with its **runtime, dependencies, and configuration** into a portable **container image**.

This ensures the application runs **consistently across development, testing, and production**, eliminating environment-related issues.

**Example:**
A .NET Web API is packaged into a Docker image and runs the same on a developer's laptop, a test server, and Azure without any code changes.

---

**Q72. Explain a typical Dockerfile for a .NET app.**

 A **Dockerfile** contains the instructions to build a Docker image. For .NET applications, a **multi-stage build** is commonly used to create smaller, production-ready images.

```dockerfile
# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "MyApp.dll"]
```

### Common Dockerfile Commands

| Command | Purpose |
|---------|---------|
| `FROM` | Specifies the base image |
| `WORKDIR` | Sets the working directory inside the container |
| `COPY` | Copies files into the image |
| `RUN` | Executes commands during image build (e.g., `dotnet restore`) |
| `EXPOSE` | Documents the port the application listens on |
| `ENTRYPOINT` | Defines the command that starts the application |

---

**Q73. What is a multi-stage Docker build, and why does it matter?**

A **multi-stage Docker build** uses **multiple `FROM` statements** in a Dockerfile. One stage is used to **build** the application, and the final stage contains **only the compiled application and runtime**, creating a smaller production image.

#### Benefits

- **Smaller image size**
- **Faster image downloads and deployments**
- **Excludes source code and build tools** from the final image
- **Improves security** by reducing the attack surface

**Example:**
A .NET application is built using the **.NET SDK image**, then only the published files are copied to the smaller **ASP.NET Runtime image** for deployment.

---

**Q74. What is Azure Container Registry (ACR)?**

**Azure Container Registry (ACR)** is a **private Docker image registry** that stores and manages container images in Azure.

You **push** container images to ACR from your CI/CD pipeline and **pull** them when deploying to services such as **Azure App Service, AKS, or Azure Container Apps**.

**Example:**

```bash
az acr build --registry myacr --image myapp:v1 .
```

This command builds the Docker image in Azure and stores it in the **Azure Container Registry**.


- Private and secure image storage
- Integrates with **AKS**, **App Service**, and **Container Apps**
- Supports **Microsoft Entra ID** and **RBAC** (e.g., **`AcrPull`** role)
- Supports image vulnerability scanning through integrations

---

**Q75. ACI vs Container Apps vs AKS — when to use which?**

| Service | Best For | Messaging Pattern | Example |
|---------|----------|-------------------|---------|
| **Event Grid** | Event notifications | Event-driven (Pub/Sub) | Trigger a Function when a Blob is uploaded |
| **Service Bus** | Reliable message processing | Queue / Publish-Subscribe | Process orders, payments, and workflows |
| **Event Hubs** | High-volume event streaming | Streaming ingestion | Collect IoT telemetry or application logs |

**Examples:**
- **Event Grid:** A file is uploaded to Blob Storage → automatically trigger an Azure Function.
- **Service Bus:** An `OrderPlaced` message is processed reliably by downstream services.
- **Event Hubs:** Millions of IoT devices continuously send telemetry data for real-time analytics.

---

**Q76. What is `docker-compose`, and when would you use it?**

**Docker Compose** is a tool that lets you **define and run multiple Docker containers** using a single **`docker-compose.yml`** file.

It is commonly used for **local development and testing**, where an application depends on multiple services.

**Example:**
A .NET application runs with:
- A **Web API** container
- A **SQL Server** container
- A **Redis** container

All services can be started with a single command:

```bash
docker compose up
```

**Sample `docker-compose.yml`:**

```yaml
services:
  api:
    build: .
    ports:
      - "8080:8080"

  db:
    image: mcr.microsoft.com/mssql/server

  redis:
    image: redis
```
---

**Q77. What's the difference between a Docker Image and a Container?**

| **Docker Image** | **Docker Container** |
|------------------|----------------------|
| A **read-only template** containing the application, runtime, dependencies, and configuration | A **running instance** of a Docker image |
| Built once and stored in a registry (e.g., Docker Hub or Azure Container Registry) | Created and started from an image |
| Does not execute by itself | Executes the application inside an isolated environment |
| One image can create multiple containers | Each container runs independently |

**Example:**
A single `myapp:v1` image can be used to start multiple containers, each running the same application.

---

**Q78. How do you keep Docker images small and secure for production?**

- Use **multi-stage builds** to include only the compiled application in the final image.
- Use **official, lightweight base images** (such as `mcr.microsoft.com/dotnet/aspnet:8.0`).
- **Run containers as a non-root user** whenever possible.
- Keep base images **up to date** with the latest security patches.
- **Scan images for vulnerabilities** before deployment (e.g., using Azure Container Registry integrations or other security tools).
- Avoid storing **secrets** inside the image; use environment variables or **Azure Key Vault**.

---

## Section 10: Kubernetes & AKS (Q79–Q88)

**Q79. What is AKS, and when would you pick it over App Service?**

| **AKS** | **App Service** |
|---------|------------------|
| Runs containerized applications using Kubernetes | Hosts web apps and APIs with minimal management |
| Ideal for large microservice architectures | Ideal for web apps, APIs, and simpler workloads |
| Supports advanced Kubernetes features, networking, and scaling | Easier to deploy and manage |
| Requires Kubernetes knowledge | No Kubernetes expertise required |

**Example:**
- **AKS:** An e-commerce platform with dozens of independently scalable microservices.
- **App Service:** A single ASP.NET Core Web API or MVC application.

---

**Q80. Pod vs Deployment vs Service — explain simply.**

- **Pod** — smallest unit; one or more containers running together.
- **Deployment** — manages a set of identical Pods, handles rolling updates and rollbacks.
- **Service** — a stable network address that routes traffic to Pods even as Pods come and go.

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

**Q81. ConfigMap vs Secret in Kubernetes?**

| **ConfigMap** | **Secret** |
|---------------|------------|
| Stores **non-sensitive** configuration | Stores **sensitive** data such as passwords, API keys, and tokens |
| Plain key-value pairs | Base64-encoded values (not encrypted by default) |
| Used for application settings | Used for credentials and secrets |

**Example:**
- **ConfigMap:** Connection timeout, log level, feature flags.
- **Secret:** Database password, API key, JWT signing key.

---

**Q82. What is the Horizontal Pod Autoscaler (HPA)?**

The **Horizontal Pod Autoscaler (HPA)** automatically **increases or decreases the number of Pods** in a deployment based on metrics such as **CPU**, **memory**, or **custom metrics**.

**Example:**
If CPU utilization exceeds **70%**, HPA can scale an application from **2 Pods** to **10 Pods** to handle increased traffic. When demand drops, it scales the Pods back down.

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

---

**Q83. What are Kubernetes Namespaces?**

A **Kubernetes Namespace** is a logical partition within a cluster that **isolates and organizes resources**. It allows multiple teams, environments, or applications to share the same cluster without resource name conflicts.

**Common uses:**
- Separate **dev**, **staging**, and **production** environments
- Isolate applications for different teams
- Apply **RBAC**, resource quotas, and network policies per namespace

**Example:**

```bash
kubectl create namespace staging
kubectl apply -f deployment.yaml -n staging
```
---

**Q84. What is an Ingress in AKS?**

An **Ingress** is a Kubernetes resource that **routes external HTTP/HTTPS traffic** to the appropriate **Service** inside an AKS cluster based on the **hostname** or **URL path**. It can also handle **TLS/SSL termination**.

**Example:**
- `api.contoso.com` → API Service
- `app.contoso.com` → Web App Service
- `contoso.com/orders` → Order Service

**Common Ingress Controllers:**
- **NGINX Ingress Controller**
- **Application Gateway Ingress Controller (AGIC)**

---

**Q85. List the most-used `kubectl` commands.**

```bash
kubectl get pods                       # list pods
kubectl get nodes                      # list nodes
kubectl describe pod myapp-abc         # detailed info
kubectl logs myapp-abc                 # view logs
kubectl exec -it myapp-abc -- bash     # shell into a pod
kubectl apply -f deployment.yaml       # create/update resources
kubectl rollout undo deployment/myapp  # rollback a bad deployment
kubectl scale deploy myapp --replicas=5
```

---

**Q86. What are AKS Node Pools, and how do you upgrade AKS with zero downtime?**

An **AKS Node Pool** is a group of **worker nodes (VMs)** with the same configuration. Different node pools can be used for different workloads.

**Common node pool types:**
- **System Pool** – Runs Kubernetes system components.
- **User Pool** – Hosts your application workloads.
- **Spot Pool** – Uses low-cost Spot VMs for fault-tolerant or batch workloads.

**Zero-downtime upgrade:**
- Upgrade the **control plane** first.
- Upgrade **node pools** one at a time.
- Enable **surge upgrades** to temporarily add extra nodes during the upgrade.
- Use **Pod Disruption Budgets (PDBs)** to ensure enough application Pods remain available.
- 
---

**Q87. How do you secure networking inside an AKS cluster?**

Secure an AKS cluster using multiple layers of network and identity controls:

- **Network Policies** – Control which Pods can communicate with each other (micro-segmentation).
- **Private Cluster** – Makes the Kubernetes API server accessible only from a private network.
- **Network Security Groups (NSGs)** – Restrict inbound and outbound traffic at the subnet level.
- **Workload Identity** – Allows Pods to securely access Azure resources using **Managed Identity**, eliminating the need to store credentials.

---

**Q88. What is a readiness probe vs a liveness probe in Kubernetes?**

| **Liveness Probe** | **Readiness Probe** |
|--------------------|---------------------|
| Checks whether the application is **still running** | Checks whether the application is **ready to receive traffic** |
| If it fails, Kubernetes **restarts the Pod** | If it fails, Kubernetes **removes the Pod from service traffic** until it becomes ready again |
| Used to recover from hung or crashed applications | Used to prevent traffic from reaching an application that is still starting or temporarily unavailable |

**Example:**

```yaml
livenessProbe:
  httpGet:
    path: /health/live
    port: 8080

readinessProbe:
  httpGet:
    path: /health/ready
    port: 8080
```


## Section 11: Azure DevOps & CI/CD (Q89–Q97)

**Q89. What is a CI/CD pipeline?**

An automated process that builds, tests, and deploys your code every time you push a change.

| CI (Continuous Integration) | CD (Continuous Delivery/Deployment) |
|---|---|
| Builds the app automatically | Deploys the app automatically |
| Runs unit/integration tests | Pushes to Dev → QA → Prod |
| Catches problems early | Delivers updates faster |

```
Code Commit → Build → Test → Deploy
```

---

**Q90. What is the structure of an Azure DevOps YAML pipeline?**

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

  - stage: Deploy
    dependsOn: Build
    jobs:
      - deployment: DeployToProd
        environment: 'production'
        strategy:
          runOnce:
            deploy:
              steps:
                - task: AzureWebApp@1
                  inputs:
                    azureSubscription: 'MyServiceConnection'
                    appName: 'my-app-service'
```

Hierarchy to remember: **Pipeline → Stages → Jobs → Steps**.

---

**Q91. What are Environments and Approval Gates?**

An **Environment** represents a deployment target, such as **Development, QA, Staging, or Production**.

An **Approval Gate** (Approval Check) requires **manual or automated approval** before a deployment can proceed to an environment.

**Common setup:**
- **Dev / QA:** Automatic deployment
- **Production:** Manual approval required before deployment

**Azure DevOps path:**
**Pipelines → Environments → Production → Approvals and checks**

---

**Q92. What is a Service Connection in Azure DevOps?**

A **Service Connection** is a secure authentication mechanism that allows an **Azure DevOps pipeline** to connect to external services such as an **Azure subscription**, **Azure Container Registry (ACR)**, **GitHub**, or **Docker Hub**.

It enables pipelines to deploy applications and access resources without embedding credentials in the pipeline.

**Best Practice:**
Use **Workload Identity Federation** instead of stored client secrets or passwords. This provides secure, secretless authentication between Azure DevOps and Azure.

---

**Q93. Pipeline Variables vs Variable Groups — what's the difference?**


| **Pipeline Variables** | **Variable Groups** |
|-------------------------|---------------------|
| Defined within a single pipeline | Shared across multiple pipelines |
| Used for pipeline-specific values | Used for common configuration and secrets |
| Scoped to one pipeline | Managed centrally in **Library** |
| Cannot be reused automatically | Can be linked to **Azure Key Vault** |

```yaml
variables:
  - group: 'shared-prod-secrets'
  - name: buildConfig
    value: Release
```

Always use Variable Groups for anything secret or shared.

---

**Q94. Microsoft-Hosted vs Self-Hosted Agents — when to use each?**

| | Microsoft-Hosted | Self-Hosted |
|---|---|---|
| Setup | None — ready to use | You install and register the agent |
| Cost | Free minutes/month included | You pay for the VM |
| Use case | Most standard builds | Need private network access, custom tools, or more compute |

Use self-hosted agents when you need to deploy into a client's private network without exposing it publicly.

---

**Q95. ARM Templates vs Bicep vs Terraform — compare quickly.**

| Tool | Language | Scope | Best for |
|---|---|---|---|
| ARM Templates | JSON | Azure only | Legacy, very verbose |
| Bicep | Simplified DSL | Azure only | Modern Azure-only IaC |
| Terraform | HCL | Multi-cloud | Teams managing AWS + Azure together |

```bicep
resource appService 'Microsoft.Web/sites@2022-03-01' = {
  name: 'my-app'
  location: resourceGroup().location
  properties: { serverFarmId: appServicePlan.id }
}
```

---

**Q96. What is Blue-Green deployment, and how do you implement it in Azure?**

Run two identical environments — **Blue** (currently live) and **Green** (new version). Deploy and test on Green, then switch traffic to it. If something breaks, switch back instantly.

- In **App Service**: deployment slots (swap staging ↔ production).
- In **AKS**: two separate Deployments (blue/green), update the Service's selector to point to whichever is active.

---

**Q97. What is a Feature Flag, and how do you manage them in Azure?**

A toggle that turns a feature on/off at runtime without redeploying code. Use **Azure App Configuration** with the Feature Management library.

```csharp
builder.Configuration.AddAzureAppConfiguration(o =>
    o.Connect(connStr).UseFeatureFlags());
builder.Services.AddFeatureManagement();

if (await _featureManager.IsEnabledAsync("NewCheckout"))
    return View("NewCheckout");
```

Useful for gradual rollouts, A/B testing, or quickly disabling a broken feature without a deployment.

---

## Section 12: Monitoring, Networking & Real-World Scenarios (Q98–Q100)

**Q98. Azure Monitor vs Application Insights — what's the difference, and how do you query logs?**

- **Azure Monitor** — the overall platform collecting metrics/logs from all Azure resources.
- **Application Insights** — the part specifically for your app code (response times, exceptions, dependency calls).

```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

Logs are stored in **Log Analytics**, queried using **KQL**:

```kql
requests
| where timestamp > ago(1h)
| where success == false
| summarize count() by name, resultCode
| order by count_ desc
```

---

**Q99. NSG vs Azure Firewall vs Private Endpoint — quick recap of network security layers.**

| Layer | What it does |
|---|---|
| NSG | Basic allow/deny rules at subnet/NIC level (free) |
| Azure Firewall | Centralized, advanced filtering — FQDN rules, threat intel (paid) |
| Private Endpoint | Gives a PaaS resource a private IP, removing it from the public internet entirely |

Typical setup: NSG for basic subnet rules, Azure Firewall for centralized egress control in a hub-spoke network, Private Endpoint on sensitive data services like SQL/Storage.

---

**Q100. Scenario: A client-facing .NET API on App Service is returning intermittent 502/504 errors. Walk through your troubleshooting steps.**

1. Check **Application Insights** for spikes in response time, exceptions, or dependency failures around the incident time.
2. Check **App Service metrics** — CPU/memory saturation, or thread pool starvation (often caused by blocking `.Result`/`.Wait()` calls instead of `await`).
3. Check **downstream dependencies** — is the database maxed out (DTU/vCore)? Is a dependent API timing out (visible in Application Map)?
4. Check **recent deployments** — does the incident start time match a recent deploy/config change? If yes, consider rolling back via slot swap.
5. **Mitigate first**: scale out, restart the app, or swap back to a known-good slot — do root-cause analysis after the immediate fire is out.
6. **Postmortem**: document the root cause and add a monitoring alert so it's caught earlier next time.

---
    