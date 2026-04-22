namespace SmartLogisticsHub.Core.Patterns.Behavioral;

// Strategy
public interface IShippingStrategy
{
    decimal AlgorithmInterface(double weight);
}

// ConcreteStrategyA
public class StandardShippingStrategy : IShippingStrategy
{
    public decimal AlgorithmInterface(double weight) => (decimal)(weight * 5);
}

// ConcreteStrategyB
public class ExpressShippingStrategy : IShippingStrategy
{
    public decimal AlgorithmInterface(double weight) => (decimal)(weight * 15);
}

// Context
public class ShippingContext
{
    private IShippingStrategy? _strategy;

    public void SetStrategy(IShippingStrategy strategy) => _strategy = strategy;

    public decimal ContextInterface(double weight)
    {
        return _strategy?.AlgorithmInterface(weight) ?? 0;
    }
}