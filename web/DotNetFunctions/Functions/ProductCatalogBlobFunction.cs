using DotNetFunctions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DotNetFunctions.Functions;

// ─── Blob Trigger ─────────────────────────────────────────────────────────────
// Use case: Product catalog import.
//
// The merchandising team uploads a CSV to the "product-catalog" Azure Blob container.
// This function fires automatically, parses the file, and imports all products.
//
// CSV format (no header row):
//   ProductId,Name,Category,Price,Stock
//   P001,Wireless Headphones,Electronics,49.99,200
//
// In production you would upsert into Cosmos DB / SQL Database.
// ─────────────────────────────────────────────────────────────────────────────

public class ProductCatalogBlobFunction
{
    private readonly ILogger<ProductCatalogBlobFunction> _logger;

    public ProductCatalogBlobFunction(ILogger<ProductCatalogBlobFunction> logger)
    {
        _logger = logger;
    }

    [Function("ImportProductCatalog")]
    public async Task Run(
        [BlobTrigger("product-catalog/{name}", Connection = "AzureWebJobsStorage")] Stream blobStream,
        string name,
        FunctionContext context)
    {
        _logger.LogInformation("Processing product catalog file: {FileName}", name);

        if (!name.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("File '{FileName}' is not a CSV — skipping.", name);
            return;
        }

        var products = new List<Product>();
        var errors = new List<string>();

        using var reader = new StreamReader(blobStream);
        int lineNumber = 0;
        string? line;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            lineNumber++;

            if (string.IsNullOrWhiteSpace(line))
                continue;

            // Skip optional header row
            if (lineNumber == 1 && line.StartsWith("ProductId", StringComparison.OrdinalIgnoreCase)) continue;

            var parts = line.Split(',');
            if (parts.Length < 5)
            {
                errors.Add($"Line {lineNumber}: expected 5 columns, got {parts.Length}");
                continue;
            }

            if (!decimal.TryParse(parts[3].Trim(), out var price) ||
                !int.TryParse(parts[4].Trim(), out var stock))
            {
                errors.Add($"Line {lineNumber}: invalid price or stock value");
                continue;
            }

            products.Add(new Product
            {
                ProductId = parts[0].Trim(),
                Name = parts[1].Trim(),
                Category = parts[2].Trim(),
                Price = price,
                Stock = stock
            });
        }

        // In production: bulk upsert into database
        _logger.LogInformation("Imported {Count} products from '{FileName}'", products.Count, name);

        foreach (var p in products)
            _logger.LogInformation("  [{Id}] {Name} — £{Price:F2}, stock: {Stock}", p.ProductId, p.Name, p.Price, p.Stock);

        if (errors.Count > 0)
            _logger.LogWarning("{ErrorCount} rows skipped:\n{Errors}", errors.Count, string.Join("\n", errors));
    }
}
