using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Glav.SQLBuilder.Helpers
{
    /// <summary>
    /// This class looks for script in the form 'Prefix_#####.sql' where #### is the sequence number and Prefix typically
    /// represents 'Schema' or 'Data' but canbe anything. The script filenames are returned in order of sequence
    /// </summary>
    public class ScriptCollector : IScriptCollector
    {
        public ScriptCollectionResults GetListOfScripts(string directoryToSearch, string scriptFilePrefix)
        {
            ScriptCollectionResults results = new ScriptCollectionResults();

            string searchCriteria = string.Format("{0}_*.sql",scriptFilePrefix);
            var files = Directory.GetFiles(directoryToSearch, searchCriteria);
            if (files != null && files.Length > 0)
            {
                files.ToList().ForEach(file =>
                    {
                        int startPos = file.IndexOf('_');
                        int endPos = file.LastIndexOf(".sql", StringComparison.InvariantCultureIgnoreCase);
                        if (startPos >= 0 && endPos > startPos)
                        {
                            startPos++;
                            string sequenceText = file.Substring(startPos, endPos - startPos);
                            int seqNum;
                            if (int.TryParse(sequenceText, out seqNum))
                            {
                                var item = new ScriptItem() { Filename = file, Contents = GetFileContents(file) };
                                results.Scripts.Add(seqNum, item);
                                if (seqNum > results.LastSequenceNumberUsed)
                                    results.LastSequenceNumberUsed = seqNum;
                            }
                        }
                    });
            }

            return results;
        }

        public string GetFileContents(string filename)
        {
            if (File.Exists(filename))
                return File.ReadAllText(filename);

            return string.Empty;
        }

    }

    public class ScriptCollectionResults
    {
        public SortedDictionary<int, ScriptItem> Scripts = new SortedDictionary<int, ScriptItem>();
        public int LastSequenceNumberUsed = 0;
    }

    public class ScriptItem
    {
        public string Filename = null;
        public string Contents = null;
    }
}
