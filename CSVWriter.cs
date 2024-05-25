using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISNBProject
{
    public class CSVWriter
    {
        public async Task WriteToCSV(string filePath, List<string[]> processedISBNs)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Row Number;Data Retrieval Type;ISBN;Title;Subtitle;Author Name(s);Number of Pages;Publish Date");

                int rowNumber = 1;
                foreach (string[] line in processedISBNs)
                {
                    csvContent.AppendLine($"{rowNumber};" + $"{string.Join(";", line.Select(item => item.Contains(";") ? $"\"{item}\"" : item))}");
                    rowNumber++;
                }

                await writer.WriteAsync(csvContent.ToString());
            }
        }
    }
}
