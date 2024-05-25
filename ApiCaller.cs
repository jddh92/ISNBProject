﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static ISNBProject.Enum;

namespace ParallelStaf
{
    public class ApiCaller
    {
        public static List<string> processedISBNCache = new List<string>();
        

        public async Task<List<string[]>> ProcessISBNBatch(List<string> isbnBatch)
        {
            List<string[]> processedISBNs = new List<string[]>();

            foreach (string isbn in isbnBatch)
            {
                string[] processedISBN = await ProcessISBN(isbn);
                processedISBNs.Add(processedISBN);
            }

            return processedISBNs;
        }



        public async Task<string[]> ProcessISBN(string isbn)
        {
            HttpClient client = new HttpClient();
            string apiUrl = $"https://openlibrary.org/api/books?bibkeys=ISBN:{isbn}&format=json&jscmd=data";
            //List<string> processedISBNCache = new List<string>();
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                dynamic bookInfo = JsonConvert.DeserializeObject(json);


                //Procesar la información del libro(obtener los datos requeridos)
                string title = bookInfo[$"ISBN:{isbn}"]["title"];
                string subtitle = bookInfo[$"ISBN:{isbn}"]["subtitle"];
                if (string.IsNullOrEmpty(subtitle))
                {
                    subtitle = "N/A";
                }


                //Get the author or authors
                JArray authorsArray = bookInfo[$"ISBN:{isbn}"]["authors"];

                string author = "";
                foreach (JToken authorToken in authorsArray)
                {
                    string authorName = authorToken["name"].ToString();
                    author += authorName + "; ";
                }

                // Eliminar la coma adicional al final, si es necesario
                if (!string.IsNullOrEmpty(author))
                {
                    author = author.Remove(author.Length - 2); // Remueve la última coma y el espacio
                }



                string pages = bookInfo[$"ISBN:{isbn}"]["number_of_pages"];
                if (string.IsNullOrEmpty(pages))
                {
                    pages = "N/A";
                }
                string publishDate = bookInfo[$"ISBN:{isbn}"]["publish_date"];




                // Aquí puedes procesar y almacenar la información obtenida en un CSV o realizar acciones adicionales
                Console.WriteLine($"ISBN: {isbn}, Title: {title},  Subtitle: {subtitle}, Author Name(s): {author}, Pages: {pages}, Publish Date: {publishDate}");

                
            }
            else
            {
                Console.WriteLine($"Failed to fetch data for ISBN: {isbn}. Status code: {response.StatusCode}");
                return new[] { "2", isbn, "", "", "", "", "" }; // En caso de error
            }

        }
    }
}