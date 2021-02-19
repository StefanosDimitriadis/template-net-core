namespace Template.Domain.Services
{
	public static class IdGenerator
	{
		private readonly static IdGen.IdGenerator _idGenerator;

		static IdGenerator()
		{
			_idGenerator = new IdGen.IdGenerator(0);
		}

		public static long Generate()
		{
			return _idGenerator.CreateId();
		}
	}
}