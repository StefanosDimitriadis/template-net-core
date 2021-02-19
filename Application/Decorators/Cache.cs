using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Services;
using Template.Domain.Entities;

namespace Template.Application.Decorators
{
	internal class EntityRetrievalPersistenceCacheDecorator<TId, TEntity> : IEntityRetrievalPersistence<TId, TEntity>
		where TEntity : BaseEntity<TId>
	{
		private readonly IDistributedCacheWrapper<TId, TEntity> _distributedCacheWrapper;
		private readonly IEntityRetrievalPersistence<TId, TEntity> _queryRetrievalPersistence;

		public EntityRetrievalPersistenceCacheDecorator(
			IDistributedCacheWrapper<TId, TEntity> distributedCacheWrapper,
			IEntityRetrievalPersistence<TId, TEntity> queryRetrievalPersistence)
		{
			_distributedCacheWrapper = distributedCacheWrapper;
			_queryRetrievalPersistence = queryRetrievalPersistence;
		}

		public async Task<TEntity> Retrieve(TId id)
		{
			var cachedEntity = await _distributedCacheWrapper.Get(id);
			if (cachedEntity != null)
				return cachedEntity;

			var entity = await _queryRetrievalPersistence.Retrieve(id);
			if (entity != null)
				await _distributedCacheWrapper.Add(entity);

			return entity;
		}
	}

	internal class EntityModificationCacheDecorator<TEventId, TId, TEntity, TModification> : IEntityModificationPersistence<TEventId, TId, TEntity, TModification>
		where TEntity : BaseEntity<TId>
		where TModification : IModification<TEventId, TId, TEntity>
	{
		private readonly IDistributedCacheWrapper<TId, TEntity> _distributedCacheWrapper;
		private readonly IEntityModificationPersistence<TEventId, TId, TEntity, TModification> _entityModificationPersistence;

		public EntityModificationCacheDecorator(
			IDistributedCacheWrapper<TId, TEntity> distributedCacheWrapper,
			 IEntityModificationPersistence<TEventId, TId, TEntity, TModification> entityModificationPersistence)
		{
			_distributedCacheWrapper = distributedCacheWrapper;
			_entityModificationPersistence = entityModificationPersistence;
		}

		public async Task Persist(TModification modification)
		{
			await _distributedCacheWrapper.Add(modification.Entity);
			await _entityModificationPersistence.Persist(modification);
		}
	}
}