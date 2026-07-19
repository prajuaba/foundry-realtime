using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Foundry.RealTime.SignalR;

/// <summary>
/// SignalR Hub for real-time client connections.
/// </summary>
public class NotificationHub : Hub
{
    private readonly ILogger<NotificationHub> _logger;

    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Allows a client to subscribe to real-time events for a specific entity type (e.g., "Customer", "Invoice").
    /// </summary>
    public async Task SubscribeToEntity(string entityName)
    {
        if (string.IsNullOrWhiteSpace(entityName)) return;
        
        string groupName = $"entity:{entityName.Trim()}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Client {ConnectionId} subscribed to entity group: {GroupName}", Context.ConnectionId, groupName);
    }

    /// <summary>
    /// Allows a client to unsubscribe from real-time events for a specific entity type.
    /// </summary>
    public async Task UnsubscribeFromEntity(string entityName)
    {
        if (string.IsNullOrWhiteSpace(entityName)) return;
        
        string groupName = $"entity:{entityName.Trim()}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Client {ConnectionId} unsubscribed from entity group: {GroupName}", Context.ConnectionId, groupName);
    }

    /// <summary>
    /// Allows a client to subscribe to real-time updates for a single specific record ID.
    /// </summary>
    public async Task SubscribeToRecord(string recordId)
    {
        if (string.IsNullOrWhiteSpace(recordId)) return;
        
        string groupName = $"record:{recordId.Trim()}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Client {ConnectionId} subscribed to record group: {GroupName}", Context.ConnectionId, groupName);
    }

    /// <summary>
    /// Allows a client to unsubscribe from real-time updates for a single specific record ID.
    /// </summary>
    public async Task UnsubscribeFromRecord(string recordId)
    {
        if (string.IsNullOrWhiteSpace(recordId)) return;
        
        string groupName = $"record:{recordId.Trim()}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Client {ConnectionId} unsubscribed from record group: {GroupName}", Context.ConnectionId, groupName);
    }

    public override Task OnConnectedAsync()
    {
        _logger.LogDebug("Client connected to RealTimeHub: {ConnectionId}", Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogDebug(exception, "Client disconnected from RealTimeHub: {ConnectionId}", Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}
