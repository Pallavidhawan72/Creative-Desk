using Creative_Desk.Models;

namespace Creative_Desk.Services
{
    public interface IPaymentService
    {
        Task<List<Payment>> GetAllPaymentsAsync();
        Task<Payment> GetPaymentByIdAsync(int id);
        Task CreatePaymentAsync(Payment payment);
        Task<Payment> UpdatePaymentAsync(int id, Payment payment);
        Task<bool> DeletePaymentAsync(int id);

    }
}
