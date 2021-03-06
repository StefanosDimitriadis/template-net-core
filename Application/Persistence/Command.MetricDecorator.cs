﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Template.Application.PersistenceCommands;

namespace Template.Application.Persistence
{
	internal class CommandPersistenceMetricDecorator<TId, TPersistCommand> : ICommandPersistence<TId, TPersistCommand>
		where TPersistCommand : BasePersistenceCommand<TId>
	{
		private readonly ICommandPersistence<TId, TPersistCommand> _commandPersistence;
		private readonly IMetrics _metrics;

		public CommandPersistenceMetricDecorator(
			ICommandPersistence<TId, TPersistCommand> commandPersistence,
			IMetrics metrics)
		{
			_commandPersistence = commandPersistence;
			_metrics = metrics;
		}

		public async Task Persist(TPersistCommand persistCommand)
		{
			try
			{
				var stopwatch = Stopwatch.StartNew();
				await _commandPersistence.Persist(persistCommand);
				stopwatch.Stop();
				_metrics.MeasureTime(ApplicationMetricsRegistry.CommandPersistenceTimer, stopwatch.ElapsedMilliseconds);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}