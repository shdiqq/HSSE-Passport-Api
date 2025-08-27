
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Data.Dao.Transaction;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Api.Domain.Models.Transaction;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Api.Domain.Interfaces.Transaction
{
    public interface ITestDetailService
    {
        public Task<BaseDTResponseModel<TestDetailDTModel>> DataTablePagination(TestDetailDTParamModel param, CancellationToken cancellationToken);

        public Task<List<TestDetail>> GetAll(CancellationToken cancellationToken);

        public Task<TestDetail> GetById(int id, CancellationToken cancellationToken);

        public Task<TestDetail> Create(TestDetailModel param, CancellationToken cancellationToken);

        public Task<TestDetail> Update(int id, TestDetailModel param, CancellationToken cancellationToken);

        public Task<TestDetail> Delete(int id, string updatedBy, CancellationToken cancellationToken);
        public Task<List<TestDetail>> GetAllByTestId(int id, CancellationToken cancellationToken);
        public Task<List<ElementModel>> GetQuestionRandom(CancellationToken cancellationToken);
        public Task<List<ElementModel>> GetQuestionAnswer(int id, int passportId, CancellationToken cancellationToken);
    }
}
