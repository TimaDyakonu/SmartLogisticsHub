using SmartLogisticsHub.Core.Models;
using System.Collections.Generic;

namespace SmartLogisticsHub.Core.Patterns.Behavioral;

// Observer
public interface IOrderObserver
{
    string Update(Order order);
}

// Subject
public abstract class OrderSubject
{
    protected List<IOrderObserver> observerCollection = new();

    public void RegisterObserver(IOrderObserver observer) => observerCollection.Add(observer);
    
    public void UnregisterObserver(IOrderObserver observer) => observerCollection.Remove(observer);

    public virtual IEnumerable<string> NotifyObservers(Order order)
    {
        var logs = new List<string>();
        foreach (var observer in observerCollection)
        {
            logs.Add(observer.Update(order));
        }
        return logs;
    }
}

// Concrete Subject
public class OrderDispatcher : OrderSubject { }

// ConcreteObserverA
public class EmailObserver : IOrderObserver
{
    public string Update(Order order) => $"Email sent: Order {order.Id} is {order.Status}";
}

// ConcreteObserverB
public class SmsObserver : IOrderObserver
{
    public string Update(Order order) => $"SMS sent: Order {order.Id} is {order.Status}";
}