using DotNetFunctions.Models;
using System.Collections.Concurrent;

namespace DotNetFunctions.Services;

public interface IOrderService
{
    Order Create(Order order);
    Order? GetById(string orderId);
    IEnumerable<Order> GetAll();
    bool UpdateStatus(string orderId, OrderStatus status);
    IEnumerable<Order> GetByDateRange(DateTime from, DateTime to);
}

// In-memory store — in production, replace with Cosmos DB / SQL / Redis.
public class OrderService : IOrderService
{
    private readonly ConcurrentDictionary<string, Order> _store = new();

    public Order Create(Order order)
    {
        _store[order.OrderId] = order;
        return order;
    }

    public Order? GetById(string orderId) =>
        _store.TryGetValue(orderId, out var order) ? order : null;

    public IEnumerable<Order> GetAll() => _store.Values;

    public bool UpdateStatus(string orderId, OrderStatus status)
    {
        if (!_store.TryGetValue(orderId, out var order)) return false;
        order.Status = status;
        order.UpdatedAt = DateTime.UtcNow;
        return true;
    }

    public IEnumerable<Order> GetByDateRange(DateTime from, DateTime to) =>
        _store.Values.Where(o => o.PlacedAt >= from && o.PlacedAt <= to);
}
