using System;
namespace Glav.SQLBuilder.Helpers
{
    public interface IScriptCollector
    {
        ScriptCollectionResults GetListOfScripts(string directoryToSearch, string scriptFilePrefix);
    }
}
