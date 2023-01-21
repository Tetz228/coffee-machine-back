namespace WebApi.Services
{
    using Db.Models;

    using Enums;

    using Interfaces;

    using UoW.Interfaces;

    /// <inheritdoc cref="IChangeService" />
    public class ChangeService : IChangeService
    {
        /// <inheritdoc cref="IUnitOfWork" />
        private readonly IUnitOfWork _unitOfWork;

        /// <inheritdoc cref="IChangeService" />
        /// <param name="unitOfWork">Единица работы.</param>
        public ChangeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc cref="IChangeService.CalculateChange" />
        public async Task<Dictionary<Bills, int>> CalculateChange(User user)
        {
            var balance = user.Balance;
            var bills = Enum.GetValues(typeof(Bills)).Cast<Bills>().Reverse()
                .ToDictionary(bill => bill, _ => 0);

            foreach (var bill in bills.Keys.ToArray())
            {
                var valueBill = (decimal)bill;
                var countBills = (int)(balance / valueBill);

                balance -= valueBill * countBills;
                bills[bill] = countBills;
            }

            user.Balance = balance;

            _unitOfWork.UserRepository.UpdateUser(user);

            await _unitOfWork.SaveChangesAsync();

            return bills;
        }
    }
}