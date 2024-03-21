namespace Pantree.InventoryService.Domain.Entities;

/// <summary>
/// Domain model that stores events that are published by the service layers.
/// </summary>
public sealed class EventOutbox {
    
    /// <summary>
    /// The unique identifier that can be used to sort outgoing events in the
    /// correct sequence.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The name of the event that is being sent to our queue.
    /// </summary>
    public string EventName { get; set; } = string.Empty;

    /// <summary>
    /// The json data of that message that we want to publish.
    /// </summary>
    public string EventData { get; set; } = string.Empty;
    
    /// <summary>
    /// The date that the event was created and queued into the outbox.
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// The date that the event was successfully pushed onto the message queue.
    /// </summary>
    public DateTime? SentDate { get; set; }
    
    /// <summary>
    /// The total number of attempts that was made in trying to push this
    /// message onto the message queue.
    /// </summary>
    public int TotalAttempts { get; set; }
}