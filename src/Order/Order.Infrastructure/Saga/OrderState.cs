using MassTransit;

namespace Ordering.Infrastructure.Saga
{
    public class OrderState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public DateTime OrderDate { get; set; }
        public int RetryCount { get; set; }
        public string SerializedItems { get; set; }
    }
}
