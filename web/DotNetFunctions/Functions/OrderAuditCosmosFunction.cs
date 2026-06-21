using DotNetFunctions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DotNetFunctions.Functions;

// ─── CosmosDB Trigger ─────────────────────────────────────────────────────────
// Use case: Immutable audit trail for all order changes.
//
// Cosmos DB Change Feed fires this function every time a document in the "orders"
// container is created or updated. The function writes a timestamped audit record
// capturing what changed and who changed it.
//
// This pattern satisfies compliance requirements (GDPR, PCI-DSS, SOX) by ensuring
// every mutation is recorded without any application code needing to explicitly
// call an audit API. The audit is infrastructure-level and cannot be bypassed.
// ─────────────────────────────────────────────────────────────────────────────

public class OrderAuditCosmosFunction
{
    private readonly ILogger<OrderAuditCosmosFunction> _logger;

    public OrderAuditCosmosFunction(ILogger<OrderAuditCosmosFunction> logger)
    {
        _logger = logger;
    }

    [Function("AuditOrderChanges")]
    public void Run(
        [CosmosDBTrigger(
            databaseName: "ecommerce",
            containerName: "orders",
            Connection = "CosmosDbConnection",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)]
        IReadOnlyList<Order> changedOrders,
        FunctionContext context)
    {
        if (changedOrders == null || changedOrders.Count == 0) return;

        _logger.LogInformation("CosmosDB change feed: {Count} order(s) changed", changedOrders.Count);

        foreach (var order in changedOrders)
        {
            var auditEntry = new OrderAuditEntry
            {
                AuditId = Guid.NewGuid().ToString(),
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                CustomerEmail = order.CustomerEmail,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                AuditedAt = DateTime.UtcNow,
                SnapshotJson = JsonSerializer.Serialize(order)
            };

            // In production: write to a separate "order-audit" Cosmos container
            // or Azure Table Storage for long-term cheap retention.
            _logger.LogInformation(
                "[AUDIT] Order {OrderId} | Customer: {Email} | Status: {Status} | Total: £{Total:F2} | At: {At:u}",
                auditEntry.OrderId,
                auditEntry.CustomerEmail,
                auditEntry.Status,
                auditEntry.TotalAmount,
                auditEntry.AuditedAt);
        }
    }
}

public class OrderAuditEntry
{
    public string AuditId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime AuditedAt { get; set; }
    public string SnapshotJson { get; set; } = string.Empty;
}
