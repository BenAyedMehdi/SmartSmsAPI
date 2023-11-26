using Newtonsoft.Json;
using SmartSms.Model.Requests;
using System.Net.Http.Headers;
using System.Text;

namespace SmartSms.Service
{
    public class GptService: IGptService
    {
        private readonly HttpClient _httpClient;

        public GptService()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, certChain, policyErrors) => true
            });

            // Replace 'YOUR_ENDPOINT_URL' with your Azure Machine Learning endpoint URL
            _httpClient.BaseAddress = new Uri("https://junctionx-cgzsb.westeurope.inference.ml.azure.com/score");

            // Replace 'YOUR_API_KEY' with your Azure Machine Learning API key
            const string apiKey = "90O1qfksY0I0WdZD0iSZiHHO72hjF8g2";
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("A key should be provided to invoke the endpoint");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public async Task<string> GenerateTextAsync(string input)
        {
            var requestBody = $"{{\"inputs\" : \"{input}\"}}";
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            // This header will force the request to go to a specific deployment.
            // Replace 'openai-gpt-12' with your deployment name or remove this line if not needed.
            content.Headers.Add("azureml-model-deployment", "openai-gpt-12");

            HttpResponseMessage response = await _httpClient.PostAsync("", content);

            if (response.IsSuccessStatusCode)
            {
                var payload=  await response.Content.ReadAsStringAsync();
                var responseList = JsonConvert.DeserializeObject<List<GeneratedTextResponse>>(payload);
                var generatedText = responseList[0].generated_text;
                var answer = string.Empty;
                if (generatedText != null )
                {
                    var qa = generatedText.Split('\n');
                    if(qa != null)
                        answer = qa[1];
                }
                return answer;
            }
            else
            {
                // Handle the error as needed
                Console.WriteLine($"The request failed with status code: {response.StatusCode}");

                // Print the headers for debugging
                Console.WriteLine(response.Headers.ToString());

                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);

                // You might want to throw an exception or return a specific error message here
                throw new Exception($"The request failed with status code: {response.StatusCode}");
            }
        }

    }
}
