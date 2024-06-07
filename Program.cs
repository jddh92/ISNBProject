using ParallelStaf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ISNBProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string filePath = "C:\\ProyectoNet\\ParallelStaf\\InputFile\\InputFile.txt";
            string outPath = "C:\\ProyectoNet\\ParallelStaf\\OutPutCSV\\output.csv";

            var httpClient = new HttpClient();
            var fileProcessor = new FileReaderClass();
            var apiCaller = new ApiCaller(httpClient);
            var csvWriter = new CSVWriter();

            var isbnList = await fileProcessor.GetISBNList(filePath);
            var isbnBatches = apiCaller.PrepareISBNBatches(isbnList, batchSize: 10);

            var tasks = isbnBatches.Select(apiCaller.ProcessISBNBatch).ToList();
            var processedISBNs = (await Task.WhenAll(tasks)).SelectMany(x => x).ToList();

            await csvWriter.WriteToCSV(outPath, processedISBNs);
        }
    }
}