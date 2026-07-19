using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Foundry.Core.Audit;

namespace Foundry.RealTime.Pipeline;

/// <summary>
/// A decorator audit sink that intercepts write logs and forwards them to the real-time broker.
/// </summary>
public class RealTimeAuditSink : IAuditSink
{
    private readonly IRealTimeNotificationBroker _broker;
    private readonly IAuditSink? _innerSink;

    public RealTimeAuditSink(IRealTimeNotificationBroker broker, IAuditSink? innerSink = null)
    {
        _broker = broker;
        _innerSink = innerSink;
    }

    public async Task WriteAsync(AuditLogEntry entry, CancellationToken ct = default)
    {
        if (_innerSink != null)
        {
            await _innerSink.WriteAsync(entry, ct);
        }
        await _broker.BroadcastMutationAsync(entry, ct);
    }

    public async Task WriteManyAsync(IReadOnlyList<AuditLogEntry> entries, CancellationToken ct = default)
    {
        if (_innerSink != null)
        {
            await _innerSink.WriteManyAsync(entries, ct);
        }
        foreach (var entry in entries)
        {
            await _broker.BroadcastMutationAsync(entry, ct);
        }
    }
}
