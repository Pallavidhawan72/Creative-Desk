using Creative_Desk.Models;

namespace Creative_Desk.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly WorkTrackingDbContext _context;

            public PaymentService(WorkTrackingDbContext context)
            {
                _context = context;
            }

            public async Task<List<Payment>> GetAllPaymentsAsync()
            {
                return await _context.Payments.Include(p => p.Project).ToListAsync();
            }

            public async Task<Payment> GetPaymentByIdAsync(int id)
            {
                return await _context.Payments.Include(p => p.Project)
                                              .FirstOrDefaultAsync(p => p.PaymentId == id);
            }

            public async Task CreatePaymentAsync(Payment payment)
            {
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
            }

            public async Task<Payment> UpdatePaymentAsync(int id, Payment payment)
            {
                var existingPayment = await _context.Payments.FindAsync(id);
                if (existingPayment == null) return null;

                existingPayment.Amount = payment.Amount;
                existingPayment.PaymentStatus = payment.PaymentStatus;
                existingPayment.PaymentDate = payment.PaymentDate;

                await _context.SaveChangesAsync();
                return existingPayment;
            }

            public async Task<bool> DeletePaymentAsync(int id)
            {
                var payment = await _context.Payments.FindAsync(id);
                if (payment == null) return false;

                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
                return true;
            }
        }

    }

