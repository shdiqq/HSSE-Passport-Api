
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Api.Domain.Interfaces.Masterdata
{
    public interface IPertaminaService
    {
        public Task<BaseDTResponseModel<PertaminaDTModel>> DataTablePagination(PertaminaDTParamModel param, CancellationToken cancellationToken);

        public Task<List<Pertamina>> GetAll(CancellationToken cancellationToken);

        public Task<Pertamina> GetById(int id, CancellationToken cancellationToken);

        public Task<Pertamina> Create(PertaminaModel param, CancellationToken cancellationToken);

        public Task<Pertamina> Update(int id, PertaminaModel param, CancellationToken cancellationToken);

        public Task<Pertamina> Delete(int id, string updatedBy, CancellationToken cancellationToken);
    }
}
