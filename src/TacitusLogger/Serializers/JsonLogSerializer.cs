using Newtonsoft.Json;
using System;
using System.Text;
using TacitusLogger.Components.Json;
using TacitusLogger.Components.Strings;
using TacitusLogger.Exceptions;

namespace TacitusLogger.Serializers
{ 
    /// <summary>
    /// Implements log model serialization using specified factory method that takes log model and returns an arbitrary object to be JSON serialized afterwards.
    /// </summary> 
    public class JsonLogSerializer : ILogSerializer
    {
        private static readonly JsonSerializerSettings _defaultJsonSerializerSettings = Defaults.JsonSerializerSettings;
        private static readonly LogModelFunc<Object> _defaultConverter = d => new SerializableLogModel(d) ?? null;
        private readonly LogModelFunc<Object> _converter;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private IJsonSerializerFacade _jsonSerializerFacade;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Serializers.JsonLogSerializer</c> using log model to custom type converter 
        /// and JSON serializer settings.
        /// </summary>
        /// <param name="converter">Delegate that is used to create custom type object from log model.</param>
        /// <param name="jsonSerializerSettings">JSON serializer settings that will be used to serialize the created object.</param>
        public JsonLogSerializer(LogModelFunc<Object> converter, JsonSerializerSettings jsonSerializerSettings)
        {
            _converter = converter ?? throw new ArgumentNullException("converter");
            _jsonSerializerSettings = jsonSerializerSettings ?? throw new ArgumentNullException("jsonSerializerSettings");
            _jsonSerializerFacade = new NewtonsoftJsonSerializerFacade();
        }
        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Serializers.JsonLogSerializer</c> using log model to custom type converter.
        /// </summary>
        /// <param name="converter">Delegate that is used to create custom type object from log model.</param>
        public JsonLogSerializer(LogModelFunc<Object> converter)
            : this(converter, DefaultJsonSerializerSettings)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonSerializerSettings"></param>
        public JsonLogSerializer(JsonSerializerSettings jsonSerializerSettings)
          : this(_defaultConverter, jsonSerializerSettings)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public JsonLogSerializer()
           : this(_defaultConverter, DefaultJsonSerializerSettings)
        {

        }


        /// <summary>
        /// Gets default JSON serializer settings.
        /// </summary>
        public static JsonSerializerSettings DefaultJsonSerializerSettings => _defaultJsonSerializerSettings;
        /// <summary>
        /// Gets default converter. For testing purposes.
        /// </summary>
        public static LogModelFunc<Object> DefaultConverter => _defaultConverter;
        /// <summary>
        /// Gets the converter delegate that was specified during the initialization.
        /// </summary>
        public LogModelFunc<Object> Converter => _converter;
        /// <summary>
        /// Gets the JSON serializer settings that was specified during the initialization.
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings => _jsonSerializerSettings;
        /// <summary>
        /// Gets the JSON serializer facade that was specified during the initialization.
        /// </summary>
        public IJsonSerializerFacade JsonSerializerFacade => _jsonSerializerFacade;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="logModel"></param>
        /// <returns></returns>
        public string Serialize(LogModel logModel)
        {
            try
            {
                return _jsonSerializerFacade.Serialize(_converter(logModel), _jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                throw new LogSerializerException("Error when serializing log model to JSON. See the inner exception.", ex);
            }
        }
        public void Dispose()
        {

        }
        /// <summary>
        /// Replaces the default json serializer facade with the provided one.
        /// </summary>
        /// <param name="jsonSerializerFacade">New json serializer facade.</param>
        public void ResetJsonSerializerFacade(IJsonSerializerFacade jsonSerializerFacade)
        {
            _jsonSerializerFacade = jsonSerializerFacade ?? throw new ArgumentNullException("jsonSerializerFacade");
        }
        public override string ToString()
        {
            return new StringBuilder()
               .AppendLine(this.GetType().FullName) 
               .Append($"Json serializer settings: {_jsonSerializerSettings.ToString().AddIndentationToLines()}") 
               .ToString();
        } 
    }
}
