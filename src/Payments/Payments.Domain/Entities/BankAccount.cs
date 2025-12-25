using Domain.Exceptions;

namespace Domain.Entities
{
    public class BankAccount(Guid userId)
    {
        public Guid BankAccountId { get; init; } = Guid.NewGuid();
        public Guid UserId { get; init; } = userId;
        public decimal Amount { get; private set; }

        // CAS токен
        public int Version { get; private set; }

        public void AddAmount(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InsufficientFundsException("The operation was declined.");
            }

            if (Amount + amount < 0)
            {
                throw new InsufficientFundsException("The operation was declined due to insufficient funds.");
            }

            Amount += amount;
            Version++;
        }

        public void SubtractAmount(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InsufficientFundsException("The operation was declined.");
            }

            if (Amount - amount < 0)
            {
                throw new InsufficientFundsException("The operation was declined due to insufficient funds.");
            }

            Amount -= amount;
            Version++;
        }
    }
}