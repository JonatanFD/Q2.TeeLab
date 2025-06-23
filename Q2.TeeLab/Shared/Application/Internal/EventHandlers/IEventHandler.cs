using Cortex.Mediator.Notifications;

namespace Q2.TeeLab.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
    
}