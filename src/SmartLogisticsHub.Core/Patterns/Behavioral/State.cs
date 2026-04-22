using SmartLogisticsHub.Core.Models;

namespace SmartLogisticsHub.Core.Patterns.Behavioral;

// <<abstract>> State
public abstract class OrderState
{
    public abstract void Handle(StateContext context);
    public abstract string StatusName { get; }
}

// ConcreteStateA
public class NewOrderState : OrderState
{
    public override string StatusName => "New";
    public override void Handle(StateContext context) => context.SetState(new ProcessingOrderState());
}

// ConcreteStateB
public class ProcessingOrderState : OrderState
{
    public override string StatusName => "Processing";
    public override void Handle(StateContext context) => context.SetState(new DispatchedOrderState());
}

public class DispatchedOrderState : OrderState
{
    public override string StatusName => "Dispatched";
    public override void Handle(StateContext context) { }
}

// StateContext
public class StateContext
{
    public Order Order { get; }
    private OrderState _state;

    public StateContext(Order order)
    {
        Order = order;
        _state = order.Status switch
        {
            "Processing" => new ProcessingOrderState(),
            "Dispatched" => new DispatchedOrderState(),
            _ => new NewOrderState()
        };
    }

    public void SetState(OrderState state)
    {
        _state = state;
        Order.Status = _state.StatusName;
    }

    public void Request()
    {
        _state.Handle(this);
    }
}