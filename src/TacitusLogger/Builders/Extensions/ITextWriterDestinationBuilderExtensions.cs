using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using TacitusLogger.Destinations.TextWriter;
using TacitusLogger.Serializers;

namespace TacitusLogger.Builders
{
    /// <summary>
    /// Adds extension methods to <c>TacitusLogger.Builders.ITextWriterDestinationBuilder</c> interface and its implementations.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ITextWriterDestinationBuilderExtensions
    {
        #region Extension methods calling WithWriter method

        /// <summary>
        /// Adds a custom TextWriter provider of type <c>TacitusLogger.Serializers.TextWriterProvider</c> with the provided
        /// TextWriter instance.
        /// </summary>
        /// <param name="self">TextWriter destination builder.</param> 
        /// <param name="textWriter">Instance of <c>System.IO.TextWriter</c>.</param>
        /// <returns>Self.</returns>
        public static ITextWriterDestinationBuilder WithWriter(this ITextWriterDestinationBuilder self, TextWriter textWriter)
        {
            return self.WithWriter(new TextWriterProvider(textWriter));
        }
        /// <summary>
        /// Adds a custom TextWriter provider of type <c>TacitusLogger.Serializers.FactoryMethodTextWriterProvider</c> with the provided
        /// factory method of type <c>TacitusLogger.LogModelFunc<TextWriter></c> that will be used to provide TextWriters.
        /// </summary>
        /// <param name="self">TextWriter destination builder.</param>  
        /// <param name="factoryMethod">TextWriter factory method.</param>      
        /// <returns>Self.</returns> 
        public static ITextWriterDestinationBuilder WithWriter(this ITextWriterDestinationBuilder self, LogModelFunc<TextWriter> factoryMethod)
        {
            return self.WithWriter(new FactoryMethodTextWriterProvider(factoryMethod));
        }

        #endregion
    }
}
