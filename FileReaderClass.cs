using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISNBProject
{
    public class FileReaderClass
    {
        public async Task<List<string>> GetISBNList(string filePath)
        {
            List<string> isbnList = new List<string>();

            try
            {
                StreamReader reader = new StreamReader(filePath);
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        isbnList.AddRange(line.Split(',').Select(isbn => isbn.Trim()));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer el archivo: {ex.Message}");
            }

            return isbnList;
        }
    }
}
