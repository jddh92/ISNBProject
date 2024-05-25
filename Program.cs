using ParallelStaf;

namespace ISNBProject
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            string filePath = "C:\\ProyectoNet\\ParallelStaf\\InputFile\\InputFile.txt";
            string outPath = "C:\\ProyectoNet\\ParallelStaf\\OutPutCSV\\output.csv";

            FileReaderClass fileProcessor = new FileReaderClass();
            ApiCaller apiCaller = new ApiCaller();
            CSVWriter csvWriter = new CSVWriter();

            List<string[]> allISBNs = await ProcessISBNs(filePath, apiCaller, fileProcessor);
            await WriteProcessedISBNsToCSV(outPath, csvWriter, allISBNs);

        }

        private static async Task<List<string[]>> ProcessISBNs(string filePath, ApiCaller apiCaller, FileReaderClass fileProcessor)
        {
            List<string> isbnList = await fileProcessor.GetISBNList(filePath);
            List<List<string>> isbnBatches = apiCaller.PrepareISBNBatches(isbnList, batchSize: 10);

            List<string[]> allISBNs = new List<string[]>();
            foreach (var isbnBatch in isbnBatches)
            {
                List<string[]> processedBatch = await apiCaller.ProcessISBNBatch(isbnBatch);
                allISBNs.AddRange(processedBatch);
            }

            return allISBNs;
        }

        private static async Task WriteProcessedISBNsToCSV(string outputPath, CSVWriter csvWriter, List<string[]> processedISBNs)
        {
            await csvWriter.WriteToCSV(outputPath, processedISBNs);
        }
    }
}
