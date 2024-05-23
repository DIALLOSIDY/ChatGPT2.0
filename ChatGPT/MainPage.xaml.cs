using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;

namespace ChatGPT
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSendClicked(object sender, EventArgs e)
        {
            string jsonBody = "{\"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"user\", \"content\": \"" + question.Text + "\"}]}";

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://chatgpt-best-price.p.rapidapi.com/v1/chat/completions"),
                Headers =
                {
                    { "X-RapidAPI-Key", "0ecc3ebd7emsh522740b0d22b6d6p121e16jsn270b210d61a4" },
                    { "X-RapidAPI-Host", "chatgpt-best-price.p.rapidapi.com" },
                },
                Content = new StringContent(jsonBody)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                // Désérialiser la réponse JSON en utilisant JsonDocument
                using (JsonDocument doc = JsonDocument.Parse(body))
                {
                    // Obtenir l'élément racine du document JSON
                    JsonElement root = doc.RootElement;

                    // Accéder à la propriété "choices"
                    JsonElement choices = root.GetProperty("choices");

                    // Accéder au premier élément du tableau "choices"
                    JsonElement firstChoice = choices[0];

                    // Accéder à la propriété "message" de cet élément
                    JsonElement message = firstChoice.GetProperty("message");

                    // Vérifier si "content" existe et n'est pas null avant d'obtenir la chaîne de caractères
                    if (message.TryGetProperty("content", out JsonElement contentElement) && contentElement.ValueKind == JsonValueKind.String)
                    {
                        string assistantMessage = contentElement.GetString();

                        reponse.Text = assistantMessage;
                        question.Text = " ";
                        Console.WriteLine(assistantMessage);
                    }
                    else
                    {
                        reponse.Text = "Le message de l'assistant est introuvable ou n'est pas une chaîne de caractères.";
                        question.Text = " ";
                        
                    }

                    

                   
                }
            }
        }
    }
}
