
using PTK.HSSEPassport.Api.Data.Dao.MasterData;
using PTK.HSSEPassport.Api.Domain.Models.MasterData;
using PTK.HSSEPassport.Utilities.Base;

namespace PTK.HSSEPassport.Api.Domain.Interfaces.Masterdata
{
	public interface IQuestionService
	{
		public Task<BaseDTResponseModel<QuestionDTModel>> DataTablePagination(QuestionDTParamModel param, CancellationToken cancellationToken);

		public Task<List<Question>> GetAll(CancellationToken cancellationToken);

		public Task<Question> GetById(int id, CancellationToken cancellationToken);

		public Task<Question> Create(QuestionModel param, CancellationToken cancellationToken);

		public Task<Question> Update(int id, QuestionModel param, CancellationToken cancellationToken);

		public Task<Question> Delete(int id, string updatedBy, CancellationToken cancellationToken);

		// submit multi data
		public Task<Question> PostAllWithAnswer(List<QuestionModel> param, CancellationToken cancellationToken);
		public Task<QuestionModel> GetQuestionDetailById(int id, CancellationToken cancellationToken);
	}
}
