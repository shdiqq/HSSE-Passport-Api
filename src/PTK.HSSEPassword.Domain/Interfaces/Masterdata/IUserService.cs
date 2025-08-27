
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Api.Domain.Interfaces.Masterdata
{
    public interface IUserService
    {
        public Task<BaseDTResponseModel<UserDTModel>> DataTablePagination(UserDTParamModel param, CancellationToken cancellationToken);

        public Task<List<User>> GetAll(CancellationToken cancellationToken);

        public Task<User> GetById(int id, CancellationToken cancellationToken);

        public Task<User> Create(UserModel param, CancellationToken cancellationToken);

        public Task<User> Update(int id, UserModel param, CancellationToken cancellationToken);

        public Task<User> Delete(int id, string updatedBy, CancellationToken cancellationToken);
        public Task<User> Login(UserModel param, CancellationToken cancellationToken);
        public Task<User> ForgotPassword(UserModel param, CancellationToken cancellationToken);
        public Task<User> ChangePassword(UserModel param, CancellationToken cancellationToken);

        public Task<User> ImageUser(int id, UserModel param, CancellationToken cancellationToken);
    }
}
