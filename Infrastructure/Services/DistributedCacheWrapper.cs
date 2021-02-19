using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using Template.Application.Services;
using Template.Domain.Entities;

namespace Template.Infrastructure.Services
{
	internal class DistributedCacheWrapper<TId, TEntity> : IDistributedCacheWrapper<TId, TEntity>
		where TEntity : BaseEntity<TId>
	{
		private readonly DistributedCacheWrapperSettings _distributedCacheWrapperSettings;
		private readonly ILogger<DistributedCacheWrapper<TId, TEntity>> _logger;
		private IDistributedCache _distributedCache;

		public DistributedCacheWrapper(
			DistributedCacheWrapperSettings distributedCacheWrapperSettings,
			ILogger<DistributedCacheWrapper<TId, TEntity>> logger)
		{
			_distributedCacheWrapperSettings = distributedCacheWrapperSettings;
			if (_distributedCacheWrapperSettings.UseRedis)
				_distributedCache = CreateRedis();
			else
				_distributedCache = CreateMemory();
			_logger = logger;
		}

		public async Task Add(TEntity entity)
		{
			var bytes = entity.ToBytes<TId, TEntity>();
			await _distributedCache.SetAsync(entity.Id.ToString(), bytes, CreateDistributedCacheEntryOptions());
		}

		public async Task<TEntity> Get(TId id)
		{
			var byteArray = await _distributedCache.GetAsync(id.ToString());
			if (byteArray == null)
				return default;

			return byteArray.ToEntity<TId, TEntity>();
		}

		private IDistributedCache CreateMemory()
		{
			var options = Options.Create(new MemoryDistributedCacheOptions());
			return new MemoryDistributedCache(options);
		}

		private IDistributedCache CreateRedis()
		{
			CreateConnectionMultiplexer(_distributedCacheWrapperSettings.ConnectionString);
			var redisCacheOptions = new RedisCacheOptions
			{
				Configuration = _distributedCacheWrapperSettings.ConnectionString
			};
			var options = Options.Create(redisCacheOptions);
			return new RedisCache(options);
		}

		private void CreateConnectionMultiplexer(string connectionString)
		{
			var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
			connectionMultiplexer.ConnectionFailed += (sender, args) =>
			{
				_logger.LogInformation("Redis connection failed");
				_distributedCache = CreateMemory();
			};
			connectionMultiplexer.ConnectionRestored += (sender, args) =>
			{
				_logger.LogInformation("Redis connection restored");
				_distributedCache = CreateRedis();
				foreach (var endpoint in connectionMultiplexer.GetEndPoints(true))
				{
					var server = connectionMultiplexer.GetServer(endpoint);
					server.FlushAllDatabases();
				}
			};
			connectionMultiplexer.ErrorMessage += ConnectionMultiplexerErrorMessage;
			connectionMultiplexer.InternalError += ConnectionMultiplexerInternalError;
		}

		private void ConnectionMultiplexerInternalError(object sender, InternalErrorEventArgs internalErrorEventArgs)
		{
			_logger.LogError(internalErrorEventArgs.Exception, JsonConvert.SerializeObject(internalErrorEventArgs));
		}

		private void ConnectionMultiplexerErrorMessage(object sender, RedisErrorEventArgs redisErrorEventArgs)
		{
			_logger.LogError(JsonConvert.SerializeObject(redisErrorEventArgs));
		}

		private DistributedCacheEntryOptions CreateDistributedCacheEntryOptions()
		{
			return new DistributedCacheEntryOptions
			{
				AbsoluteExpiration = DateTime.UtcNow.AddSeconds(_distributedCacheWrapperSettings.AbsoluteExpirationSeconds),
				SlidingExpiration = TimeSpan.FromSeconds(_distributedCacheWrapperSettings.SlidingExpirationSeconds)
			};
		}
	}
}