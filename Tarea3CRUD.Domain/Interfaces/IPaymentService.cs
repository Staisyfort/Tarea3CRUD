namespace Tarea3CRUD.Domain.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ProcessPaymentAsync(decimal amount, string cardNumber);
    }
}
