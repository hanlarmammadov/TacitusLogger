using Newtonsoft.Json;
using System;

namespace TacitusLogger.Components.Json
{
   public interface IJsonSerializerFacade
   {
       string Serialize(Object obj, JsonSerializerSettings jsonSerializerSettings);
   }
}
