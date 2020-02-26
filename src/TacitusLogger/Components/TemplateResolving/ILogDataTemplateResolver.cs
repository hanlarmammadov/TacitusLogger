using System;
using System.Collections.Generic;
using System.Text;

namespace TacitusLogger.Components.TemplateResolving
{
    public interface ILogModelTemplateResolver
    {
        string Resolve(LogModel logModel, string template);
    }
}
