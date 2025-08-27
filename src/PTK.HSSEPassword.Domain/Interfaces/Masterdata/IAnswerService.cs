
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Api.Domain.Interfaces.Masterdata
{
    public interface IAnswerService
    {
        public Task<BaseDTResponseModel<AnswerDTModel>> DataTablePagination(AnswerDTParamModel param, CancellationToken cancellationToken);

        public Task<List<Answer>> GetAll(CancellationToken cancellationToken);

        public Task<Answer> GetById(int id, CancellationToken cancellationToken);

        public Task<Answer> Create(AnswerModel param, CancellationToken cancellationToken);

        public Task<Answer> Update(int id, AnswerModel param, CancellationToken cancellationToken);

        public Task<Answer> Delete(int id, string updatedBy, CancellationToken cancellationToken);
    }
}
