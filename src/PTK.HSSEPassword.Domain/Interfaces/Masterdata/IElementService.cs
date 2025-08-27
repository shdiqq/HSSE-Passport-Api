
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Api.Domain.Interfaces.Masterdata
{
    public interface IElementService
    {
        public Task<BaseDTResponseModel<ElementDTModel>> DataTablePagination(ElementDTParamModel param, CancellationToken cancellationToken);

        public Task<List<Element>> GetAll(CancellationToken cancellationToken);

        public Task<Element> GetById(int id, CancellationToken cancellationToken);

        public Task<Element> Create(ElementModel param, CancellationToken cancellationToken);

        public Task<Element> Update(int id, ElementModel param, CancellationToken cancellationToken);

        public Task<Element> Delete(int id, string updatedBy, CancellationToken cancellationToken);
    }
}
