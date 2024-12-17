using System.Globalization;
using System.Net.Http.Headers;
using GroomerManager.Shared.DTOs.Response;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Localization;
using Newtonsoft.Json;

namespace GroomerManager.Web.Utils;

public class ApiClient(
    HttpClient httpClient,
    ErrorMapper errorMapper,
    ProtectedLocalStorage localStorage,
    NavigationManager navigationManager,
    AuthenticationStateProvider authStateProvider
    )
{
    public async Task SetAuthorizeHeader()
    {
        var sessionState = (await localStorage.GetAsync<LoginResponseDto>("auth_token")).Value;
        if (sessionState != null && !string.IsNullOrEmpty(sessionState.Token))
        {
            if (sessionState.TokenExpired < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                await ((CustomAuthStateProvider)authStateProvider).MarkUserAsLoggedOut();
                navigationManager.NavigateTo("/");
            }
            else if (sessionState.TokenExpired < DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds())
            {
                var res = await httpClient.GetFromJsonAsync<LoginResponseDto>($"/api/Auth/LoginRefreshToken?RefreshToken{sessionState.RefreshToken}");
                if (res != null)
                {
                    await ((CustomAuthStateProvider)authStateProvider).MarkUserAsAuthenticated(res);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", res.Token);
                }
                else
                {
                    await ((CustomAuthStateProvider)authStateProvider).MarkUserAsLoggedOut();
                    navigationManager.NavigateTo("/");
                }
            }
            else
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionState.Token);
            }

            var requestCulture = new RequestCulture(
                CultureInfo.CurrentCulture,
                CultureInfo.CurrentUICulture
            );
            var cultureCookieValue = CookieRequestCultureProvider.MakeCookieValue(requestCulture);

            httpClient.DefaultRequestHeaders.Add("Cookie", $"{CookieRequestCultureProvider.DefaultCookieName}={cultureCookieValue}");
        }
    }
    public async Task<T> GetAsync<T>(string path)
    {
        await SetAuthorizeHeader();
        return await httpClient.GetFromJsonAsync<T>(path);
    }

    public async Task<T> PostAsync<T, TN>(string path, TN postModel)
    {
        await SetAuthorizeHeader();
        var response = await httpClient.PostAsJsonAsync(path, postModel);

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            if (error?.Errors != null && error.Errors.Count > 0)
            {

                var firstError = error.Errors.First();
                var errorMessage = errorMapper.ErrorToMessage(firstError.Error);

                throw new Exception(errorMessage);
            }

            if (!string.IsNullOrEmpty(error?.Title))
            {
                throw new Exception(errorMapper.ErrorToMessage(error.Title));
            }
        }

        return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())!;
    }
}