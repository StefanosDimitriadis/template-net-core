using App.Metrics;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Template.Api.Decorators
{
	public class MapperMetricDecorator : IMapper
	{
		private readonly IMapper _mapper;
		private readonly IMetrics _metrics;

		public MapperMetricDecorator(
			IMapper mapper,
			IMetrics metrics)
		{
			_mapper = mapper;
			_metrics = metrics;
		}

		public IConfigurationProvider ConfigurationProvider => throw new NotImplementedException();

		public Func<Type, object> ServiceCtor => throw new NotImplementedException();

		public TDestination Map<TSource, TDestination>(TSource source)
		{
			var stopwatch = Stopwatch.StartNew();
			var destination = _mapper.Map<TSource, TDestination>(source);
			stopwatch.Stop();
			_metrics.Measure.Timer.Time(ApiMetricsRegistry.MapTimer, stopwatch.ElapsedMilliseconds);
			return destination;
		}

		public TDestination Map<TDestination>(object source)
		{
			throw new NotImplementedException();
		}

		public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
		{
			throw new NotImplementedException();
		}

		public object Map(object source, Type sourceType, Type destinationType)
		{
			throw new NotImplementedException();
		}

		public object Map(object source, object destination, Type sourceType, Type destinationType)
		{
			throw new NotImplementedException();
		}

		public TDestination Map<TDestination>(object source, Action<IMappingOperationOptions<object, TDestination>> opts)
		{
			throw new NotImplementedException();
		}

		public TDestination Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions<TSource, TDestination>> opts)
		{
			throw new NotImplementedException();
		}

		public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMappingOperationOptions<TSource, TDestination>> opts)
		{
			throw new NotImplementedException();
		}

		public object Map(object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions<object, object>> opts)
		{
			throw new NotImplementedException();
		}

		public object Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions<object, object>> opts)
		{
			throw new NotImplementedException();
		}

		public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, object parameters = null, params Expression<Func<TDestination, object>>[] membersToExpand)
		{
			throw new NotImplementedException();
		}

		public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, IDictionary<string, object> parameters, params string[] membersToExpand)
		{
			throw new NotImplementedException();
		}

		public IQueryable ProjectTo(IQueryable source, Type destinationType, IDictionary<string, object> parameters = null, params string[] membersToExpand)
		{
			throw new NotImplementedException();
		}
	}
}