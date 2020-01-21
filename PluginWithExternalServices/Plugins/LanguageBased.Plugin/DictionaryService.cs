using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LanguageBased.Plugin
{
    public interface IDictionaryService
    {
        Task<Dictionary<string, string>> GetDictionary();
    }

    public class DictionaryService : IDictionaryService
    {
        public async Task<Dictionary<string, string>> GetDictionary()
        {
            using (var stream = new StreamReader(Path.Combine(GetLocalExecutionPath(), "Plugins", "LanguageBased.Plugin", "Dictionary.json")))
            {
                var json = await stream.ReadToEndAsync();
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
    }

    private string GetLocalExecutionPath()
    {
        return AppDomain.CurrentDomain.BaseDirectory;
    }
}
}
