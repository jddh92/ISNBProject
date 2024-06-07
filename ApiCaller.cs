using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static ISNBProject.Enum;

namespace ParallelStaf
{
    public class ApiCaller
    {
        private static readonly List<string> processedISBNCache = new List<string>();
        private readonly HttpClient client;

        public ApiCaller(HttpClient httpClient)
        {
            client = httpClient;
        }

        public List<List<string>> PrepareISBNBatches(List<string> isbnList, int batchSize)
        {
            return Enumerable.Range(0, (isbnList.Count + batchSize - 1) / batchSize)
                .Select(i => isbnList.Skip(i * batchSize).Take(batchSize).ToList())
                .ToList();
        }

        public async Task<List<string[]>> ProcessISBNBatch(List<string> isbnBatch)
        {
            var tasks = isbnBatch.Select(ProcessISBN).ToList();
            return (await Task.WhenAll(tasks)).ToList();
        }

        public async Task<string[]> ProcessISBN(string isbn)
        {
            string apiUrl = $"https://openlibrary.org/api/books?bibkeys=ISBN:{isbn}&format=json&jscmd=data";
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to fetch data for ISBN: {isbn}. Status code: {response.StatusCode}");
                return new[] { "2", isbn, "", "", "", "", "" };
            }

            string json = await response.Content.ReadAsStringAsync();
            dynamic bookInfo = JsonConvert.DeserializeObject(json);

            string title = bookInfo[$"ISBN:{isbn}"]["title"];
            string subtitle = bookInfo[$"ISBN:{isbn}"]["subtitle"] ?? "N/A";
            string author = string.Join("; ", bookInfo[$"ISBN:{isbn}"]["authors"]);
            string pages = bookInfo[$"ISBN:{isbn}"]["number_of_pages"] ?? "N/A";
            string publishDate = bookInfo[$"ISBN:{isbn}"]["publish_date"];

            DataRetrievalType dataRetrievalType = processedISBNCache.Contains(isbn) ? DataRetrievalType.Cache : DataRetrievalType.Server;
            if (dataRetrievalType == DataRetrievalType.Server)
            {
                processedISBNCache.Add(isbn);
            }

            return new[] { dataRetrievalType.ToString(), isbn, title, subtitle, author, pages, publishDate };
        }
    }
}