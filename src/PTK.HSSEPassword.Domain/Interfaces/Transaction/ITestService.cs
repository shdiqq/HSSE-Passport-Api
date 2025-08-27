
using PTK.HSSEPassport.Api.Data.Dao.Transaction;
using PTK.HSSEPassport.Api.Domain.Models.Transaction;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Api.Domain.Interfaces.Transaction
{
    public interface ITestService
    {
        public Task<BaseDTResponseModel<TestDTModel>> DataTablePagination(TestDTParamModel param, CancellationToken cancellationToken);

        public Task<List<Test>> GetAll(CancellationToken cancellationToken);

        public Task<Test> GetById(int id, CancellationToken cancellationToken);

        public Task<Test> Create(TestModel param, CancellationToken cancellationToken);

        public Task<Test> Update(int id, TestModel param, CancellationToken cancellationToken);

        public Task<Test> Delete(int id, string updatedBy, CancellationToken cancellationToken);
    }
}
