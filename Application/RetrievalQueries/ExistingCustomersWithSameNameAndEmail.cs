using Template.Application.RetrievalQueries;
using Template.Domain.Services;

namespace Template.Persistence.RetrievalQueries
{
	public class ExistingCustomersWithSameNameAndEmailRetrievalQueryRequest : BaseRequest<long>
	{
		public override long Id { get; protected set; }
		public string Name { get; }
		public string Email { get; }

		private ExistingCustomersWithSameNameAndEmailRetrievalQueryRequest(long id, string name, string email)
		{
			Id = id;
			Name = name;
			Email = email;
		}

		public static ExistingCustomersWithSameNameAndEmailRetrievalQueryRequest Create(string name, string email)
		{
			return new ExistingCustomersWithSameNameAndEmailRetrievalQueryRequest(IdGenerator.Generate(), name, email);
		}
	}

	public class ExistingCustomersWithSameNameAndEmailRetrievalQueryResult : BaseResult<long>
	{
		public override long Id { get; protected set; }
		public bool ExistingCustomersWithSameNameAndEmail { get; }

		private ExistingCustomersWithSameNameAndEmailRetrievalQueryResult(long id, bool existingCustomersWithSameNameAndEmail)
		{
			Id = id;
			ExistingCustomersWithSameNameAndEmail = existingCustomersWithSameNameAndEmail;
		}

		public static ExistingCustomersWithSameNameAndEmailRetrievalQueryResult Create(bool existingCustomersWithSameNameAndEmail)
		{
			return new ExistingCustomersWithSameNameAndEmailRetrievalQueryResult(IdGenerator.Generate(), existingCustomersWithSameNameAndEmail);
		}
	}
}