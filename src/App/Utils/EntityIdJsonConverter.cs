using System.Diagnostics;

using System.Text.Json;
using System.Text.Json.Serialization;

using Domain.Common;

namespace App.Utils;

internal sealed class EntityIdJsonConverter : JsonConverterFactory
{
	private sealed class EntityIdJsonConverterInner<TKey>(JsonSerializerOptions options) : JsonConverter<TKey>
		where TKey : IEntityId
	{
		private readonly JsonConverter<Guid> jsonConverter = (JsonConverter<Guid>)options.GetConverter(typeof(Guid));

		public override TKey? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
			throw new NotImplementedException();

		public override void Write(Utf8JsonWriter writer, TKey value, JsonSerializerOptions options) =>
			jsonConverter.Write(writer, value.Value, options);
	}

	public override bool CanConvert(Type typeToConvert) =>
		typeToConvert.IsAssignableTo(typeof(IEntityId));

	public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		var jsonConverterType = typeof(EntityIdJsonConverterInner<>).MakeGenericType(typeToConvert);
		var jsonConverter = (JsonConverter?)Activator.CreateInstance(jsonConverterType, new[] { options });
		Debug.Assert(jsonConverter is not null);

		return jsonConverter;
	}
}
