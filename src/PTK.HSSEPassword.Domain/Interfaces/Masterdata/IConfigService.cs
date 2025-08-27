
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Api.Domain.Interfaces.Masterdata
{
    public interface IConfigService
    {
        public Task<BaseDTResponseModel<ConfigDTModel>> DataTablePagination(ConfigDTParamModel param, CancellationToken cancellationToken);

        public Task<List<Config>> GetAll(CancellationToken cancellationToken);

        public Task<Config> GetById(int id, CancellationToken cancellationToken);

        public Task<Config> Create(ConfigModel param, CancellationToken cancellationToken);

        public Task<Config> Update(int id, ConfigModel param, CancellationToken cancellationToken);

        public Task<Config> Delete(int id, string updatedBy, CancellationToken cancellationToken);
    }
}
