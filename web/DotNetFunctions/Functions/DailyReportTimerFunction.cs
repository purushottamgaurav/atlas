using DotNetFunctions.Models;
using DotNetFunctions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DotNetFunctions.Functions;

// ─── Timer Trigger ────────────────────────────────────────────────────────────
// Use case: Automated daily revenue report.
//
// Fires every day at midnight UTC. Aggregates yesterday's orders and writes a
// JSON summary blob to "daily-reports" storage. Finance teams can then pick up
// these blobs for downstream reporting (Power BI, email, Slack, etc.).
//
// CRON expression: "0 0 0 * * *"  →  second minute hour day month weekday
// ─────────────────────────────────────────────────────────────────────────────

public class DailyReportTimerFunction
{
    private readonly IOrderService _orders;
    private readonly ILogger<DailyReportTimerFunction> _logger;

    public DailyReportTimerFunction(IOrderService orders, ILogger<DailyReportTimerFunction> logger)
    {
        _orders = orders;
        _logger = logger;
    }

    [Function("DailyRevenueReport")]
    [BlobOutput("daily-reports/report-latest.json", Connection = "AzureWebJobsStorage")]
    public string Run([TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo)
    {
        if (timerInfo.IsPastDue)
            _logger.LogWarning("Daily report timer is running late — host was likely idle.");

        var yesterday = DateTime.UtcNow.Date.AddDays(-1);
        var todayStart = yesterday.AddDays(1);

        var orders = _orders.GetByDateRange(yesterday, todayStart).ToList();

        var delivered = orders.Where(o => o.Status == OrderStatus.Delivered).ToList();
        var cancelled = orders.Where(o => o.Status == OrderStatus.Cancelled).ToList();

        var report = new DailyReport
        {
            ReportDate = yesterday,
            GeneratedAt = DateTime.UtcNow,
            TotalOrders = orders.Count,
            DeliveredOrders = delivered.Count,
            CancelledOrders = cancelled.Count,
            TotalRevenue = delivered.Sum(o => o.TotalAmount),
            AverageOrderValue = delivered.Count > 0
                ? delivered.Average(o => o.TotalAmount)
                : 0,
            TopProducts = orders
                .SelectMany(o => o.Items)
                .GroupBy(i => i.ProductName)
                .OrderByDescending(g => g.Sum(i => i.Quantity))
                .Take(5)
                .Select(g => new TopProduct { Name = g.Key, UnitsSold = g.Sum(i => i.Quantity) })
                .ToList()
        };

        _logger.LogInformation(
            "Daily report for {Date}: {Orders} orders, £{Revenue:F2} revenue",
            yesterday.ToString("yyyy-MM-dd"), report.TotalOrders, report.TotalRevenue);

        // Return value is written to the BlobOutput binding
        return JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true });
    }
}

public class DailyReport
{
    public DateTime ReportDate { get; set; }
    public DateTime GeneratedAt { get; set; }
    public int TotalOrders { get; set; }
    public int DeliveredOrders { get; set; }
    public int CancelledOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageOrderValue { get; set; }
    public List<TopProduct> TopProducts { get; set; } = new();
}

public class TopProduct
{
    public string Name { get; set; } = string.Empty;
    public int UnitsSold { get; set; }
}
