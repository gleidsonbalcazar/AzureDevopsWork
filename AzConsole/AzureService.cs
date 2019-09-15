using AzureDevopsWork.Services.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using AzureDevopsWork.Services;
using AzureDevopsWork.Services.Models.DTO;
using Newtonsoft.Json.Linq;

namespace AzConsole
{
    public class AzureService
    {
        private readonly Config _config;

        public AzureService(Config config)
        {
            _config = config;
        }

        public List<ItemsDTO> RecuperaItens()
        {
            Console.WriteLine("Recuperando Item(ns) na URI.");

            var ids = RecuperaIds();
            var apiResponse = new JObject();

            using (var client = new HttpService().IniciateHttp(_config))
            {
                var fields = ReturnFields();

                var requestUri = "_apis/wit/workitems?ids=" + string.Join(",", ids) +
                                    "&fields=" + string.Join(",", fields) +
                                    "&api-version=5.1";
                apiResponse = GetResponseApi(apiResponse, client, requestUri);
            }

            var items = new List<ItemsDTO>();
            FormatarResultado(apiResponse, items: items);

            Console.WriteLine($"Total de {items.Count} encontrados");

            return items;
        }


        private static void FormatarResultado(JObject apiResponse, List<ItemsDTO> items)
        {
            items.AddRange(from dynamic workItem in (JArray) apiResponse["value"]
                select (JObject) workItem.fields
                into field
                select new ItemsDTO
                {
                    IdWorkItem = (int) field.GetValue("System.Id"),
                    CreatedDate = (DateTime) field.GetValue("System.CreatedDate"),
                    Title = (string) field.GetValue("System.Title"),
                    WorkItemType = (string) field.GetValue("System.WorkItemType"),
                });
        }

        private static string[] ReturnFields()
        {
            return new[] {
                    "System.Id",
                    "System.Title",
                    "System.WorkItemType",
                    "System.CreatedDate"
                };
        }

        private IEnumerable<int> RecuperaIds()
        {
            var ids = new HashSet<int>();
            var apiResponse = new JObject();

            using (var client = new HttpService().IniciateHttp(_config))
            {
                var query = "Select [System.Id] From WorkItems";
                var requestBody = new StringContent("{ \"query\": \"" + query + "\" }",
                    Encoding.UTF8, "application/json");

                apiResponse = PostResponseApi(apiResponse, client, requestBody);

                var workItems = (JArray)apiResponse["workItems"];

                foreach (dynamic workItem in workItems)
                {
                    ids.Add((int)workItem.id);
                }

                return ids.ToArray();
            }
        }

        private static JObject PostResponseApi(JObject apiResponse, HttpClient client, StringContent requestBody)
        {
            try
            {
                using (var response = client.PostAsync("_apis/wit/wiql?api-version=5.1", requestBody).Result)
                {
                    response.EnsureSuccessStatusCode();
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    apiResponse = JObject.Parse(responseBody);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return apiResponse;
        }

        private static JObject GetResponseApi(JObject apiResponse, HttpClient client, string requestURI)
        {
            try
            {
                using (var response = client.GetAsync(requestURI).Result)
                {
                    response.EnsureSuccessStatusCode();
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    apiResponse = JObject.Parse(responseBody);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return apiResponse;
        }

    }
}
