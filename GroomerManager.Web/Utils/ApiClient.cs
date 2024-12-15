using GroomerManager.Shared.DTOs.Response;
using Newtonsoft.Json;

namespace GroomerManager.Web.Utils;

public class ApiClient(
    HttpClient httpClient,
    ErrorMapper errorMapper
    )
{
    public async Task<T> GetAsync<T>(string path)
    {
        return await httpClient.GetFromJsonAsync<T>(path);
    }

    public async Task<T> PostAsync<T, TN>(string path, TN postModel)
    {
        var response = await httpClient.PostAsJsonAsync(path, postModel);

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());
            // throw new Exception(errorMapper.ErrorToMessage(error.Title));
            if (error.Errors != null && error.Errors.Count > 0)
            {
                // var fieldErrors = error.Errors
                //     .Select(e => $"{errorMapper.ErrorToMessage(e.Error)}")
                //     .ToList();
                
                var firstError = error.Errors.First();
                var errorMessage = errorMapper.ErrorToMessage(firstError.Error);

                // Rzucenie wyjątku z połączonymi wiadomościami błędów
                // throw new Exception(string.Join("; ", fieldErrors));
                throw new Exception(errorMessage);
            }

            // Jeśli jest tylko tytuł błędu (np. "InvalidEmailOrPassword")
            if (!string.IsNullOrEmpty(error.Title))
            {
                throw new Exception(errorMapper.ErrorToMessage(error.Title));
            }
        }

        return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
    }
}