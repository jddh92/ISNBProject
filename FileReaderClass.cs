using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ISNBProject
{
    public class FileReaderClass
    {
        public async Task<List<string>> GetISBNList(string filePath)
        {
            var isbnList = new List<string>();
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    isbnList.Add(line);
                }
            }
            return isbnList;
        }
    }
}