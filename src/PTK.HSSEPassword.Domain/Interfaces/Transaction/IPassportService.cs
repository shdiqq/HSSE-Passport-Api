
using Microsoft.AspNetCore.Http.HttpResults;
using PTK.HSSEPassport.Api.Data.Dao.Transaction;
using PTK.HSSEPassport.Api.Domain.Models.Transaction;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Api.Domain.Interfaces.Transaction
{
    public interface IPassportService
    {
        public Task<BaseDTResponseModel<PassportDTModel>> DataTablePagination(PassportDTParamModel param, CancellationToken cancellationToken);

        public Task<List<Passport>> GetAll(CancellationToken cancellationToken);

        public Task<Passport> GetById(int id, CancellationToken cancellationToken);

        public Task<Passport> Create(PassportModel param, CancellationToken cancellationToken);

        public Task<Passport> Update(int id, PassportModel param, CancellationToken cancellationToken);

        public Task<Passport> Delete(int id, string updatedBy, CancellationToken cancellationToken);

        public Task<Passport> Regulation(int id, PassportModel param, CancellationToken cancellationToken);
        public Task<Passport> UpdateRegulation(int id, CancellationToken cancellationToken);
        public Task<Passport> ChangeStatus(int id, PassportModel param, CancellationToken cancellationToken);
        public Task<ChartDataModel> DashboardStatus(int year, string createdBy, CancellationToken cancellationToken);
        public Task<ChartDataModel> DashboardExpired(int year, string createdBy, CancellationToken cancellationToken);
        public Task<List<SeriesDataModel>> DashboardRequestExpired(int year, string createdBy, CancellationToken cancellationToken);
    }
}
