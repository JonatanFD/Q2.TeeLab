using Cortex.Mediator.Notifications;
using Q2.TeeLab.Shared.Domain.Model.Events;

namespace Q2.TeeLab.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
    
}