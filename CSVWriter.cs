using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ISNBProject
{
    public class CSVWriter
    {
        public async Task WriteToCSV(string outputPath, List<string[]> processedISBNs)
        {
            var csv = new StringBuilder();
            foreach (var isbnData in processedISBNs)
            {
                var line = string.Join(",", isbnData);
                csv.AppendLine(line);
            }
            await File.WriteAllTextAsync(outputPath, csv.ToString());
        }
    }
}