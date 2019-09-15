using System;
using AzureDevopsWork.Services.Models.Views;
using Newtonsoft.Json;
using System.IO;
using AzureDevopsWork.Services;

namespace AzConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Config config = RecuperarConfiguracao();
            if (!config.Validate())
            {
                Console.WriteLine("É necessário preencher ou utilizar a configuração padrão");
                RecuperarConfiguracao();
            }
            else
            {
                 //Recupera os itens no azure
                var azureService = new AzureService(config);
                var items = azureService.RecuperaItens();

                var repository = new AzureLocalRepository();
                repository.Save(items);
            }

        }

        private static Config RecuperarConfiguracao()
        {
            string console;
            Config config = new Config();

            do
            {
                Console.WriteLine("Deseja utilizar as configurações padrão?(S/N)");
                console = Console.ReadLine();
                console = console.ToUpper();
            } while ((!console.Equals("S")) && (!console.Equals("N")));

            if (console.Equals("N"))
            {
                Console.WriteLine("Digite a URI");
                config.uri = Console.ReadLine();

                Console.WriteLine("Digite o nome do Projeto desta URI");
                config.projeto = Console.ReadLine();

                Console.WriteLine("Digite o token de acesso");
                config.access_token = Console.ReadLine();

            }
            else
            {
                var BasePath = Directory.GetCurrentDirectory();
                var JSON = System.IO.File.ReadAllText(BasePath + "/config.json");
                config = JsonConvert.DeserializeObject<Config>(JSON);
            }

            return config;

        }
    }
}
