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

			var files = GetAllMatchingFilesInAllSubdirectories(directoryToSearch, scriptFilePrefix);

			if (files != null && files.Count > 0)
            {
                files.ForEach(file =>
                    {
                        int startPos = file.LastIndexOf('_');
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

		private List<string> GetAllMatchingFilesInAllSubdirectories(string directoryToSearch, string scriptFilePrefix)
		{
			List<string> allFiles = new List<string>();
			
			// Get the files in the current directory first
			string searchCriteria = string.Format("{0}_*.sql", scriptFilePrefix);
			allFiles.AddRange(Directory.GetFiles(directoryToSearch, searchCriteria));

			// Get a list of sub directories that we need to search as well. We aggregate all the matching
			// files in all subdirectories to form a flat list of "{type}_####.sql"files
			var subDirs = Directory.EnumerateDirectories(directoryToSearch, "*.*", SearchOption.AllDirectories);
			foreach (var dir in subDirs)
			{
				allFiles.AddRange(Directory.GetFiles(dir, searchCriteria));
			}

			return allFiles;
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
