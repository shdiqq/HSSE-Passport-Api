
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Api.Domain.Interfaces.Masterdata
{
    public interface IPositionService
    {
        public Task<BaseDTResponseModel<PositionDTModel>> DataTablePagination(PositionDTParamModel param, CancellationToken cancellationToken);

        public Task<List<Position>> GetAll(CancellationToken cancellationToken);

        public Task<Position> GetById(int id, CancellationToken cancellationToken);

        public Task<Position> Create(PositionModel param, CancellationToken cancellationToken);

        public Task<Position> Update(int id, PositionModel param, CancellationToken cancellationToken);

        public Task<Position> Delete(int id, string updatedBy, CancellationToken cancellationToken);
    }
}
