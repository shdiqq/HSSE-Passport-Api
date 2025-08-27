
using PTK.HSSEPassport.Api.Data.Dao.Transaction;
using PTK.HSSEPassport.Api.Domain.Models.Transaction;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Api.Domain.Interfaces.Transaction
{
    public interface IFraudService
    {
        public Task<BaseDTResponseModel<FraudDTModel>> DataTablePagination(FraudDTParamModel param, CancellationToken cancellationToken);

        public Task<List<Fraud>> GetAll(CancellationToken cancellationToken);

        public Task<Fraud> GetById(int id, CancellationToken cancellationToken);

        public Task<Fraud> Create(FraudModel param, CancellationToken cancellationToken);

        public Task<Fraud> Update(int id, FraudModel param, CancellationToken cancellationToken);

        public Task<Fraud> Delete(int id, string updatedBy, CancellationToken cancellationToken);
    }
}
