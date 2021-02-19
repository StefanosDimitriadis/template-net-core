using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using System.Text;
using Template.Domain.Entities;

namespace Template.Infrastructure
{
	internal static class SerializationExtensions
	{
		internal static byte[] ToBytes<TId, TEntity>(this TEntity entity)
			where TEntity : BaseEntity<TId>
		{
			var payload = JsonConvert.SerializeObject(entity);
			return Encoding.Unicode.GetBytes(payload);
		}

		internal static TEntity ToEntity<TId, TEntity>(this byte[] bytes)
			where TEntity : BaseEntity<TId>
		{
			var payload = Encoding.Unicode.GetString(bytes);
			return JsonConvert.DeserializeObject<TEntity>(
				value: payload,
				settings: new CustomJsonSerializerSettings());
		}
	}

	public class CustomJsonSerializerSettings : JsonSerializerSettings
	{
		public CustomJsonSerializerSettings()
		{
			Error = (object sender, ErrorEventArgs args) =>
			{
				if (args.CurrentObject == args.ErrorContext.OriginalObject)
				{
					var logger = LogManager.GetCurrentClassLogger();
					logger.Error(args.ErrorContext.Error, $"Error in serializer for {args.ErrorContext.Member} at {args.ErrorContext.Path}");
				}
			};
		}
	}
}