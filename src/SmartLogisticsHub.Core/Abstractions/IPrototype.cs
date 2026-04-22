namespace SmartLogisticsHub.Core.Abstractions;

public interface IPrototype<T>
{
    T Clone();
}
