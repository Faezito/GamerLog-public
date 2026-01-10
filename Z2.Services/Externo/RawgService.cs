using Azure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Z1.Model.APIs;

namespace Z2.Services.Externo
{
    public class RawgService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private readonly IAPIsServicos _apis;


        public RawgService(HttpClient http, IConfiguration config, IAPIsServicos apis)
        {
            _http = http;
            _apiKey = config["Rawg:ApiKey"];
            _apis = apis;

        }

        public async Task<List<RawgGameDto>> ObterJogos(int page = 1, int pageSize = 20, string? titulo = " ")
        {
            if(titulo == null) titulo = " ";
            var search = Uri.EscapeDataString(titulo);
            string chaveApi = await _apis.ObterChaveApi(11);

            var url =
                $"games?key={chaveApi}" +
                $"&search={search}" +
                $"&search_precise=true" +
                $"&page={page}" +
                $"&page_size={pageSize}";
            var response = await _http
                .GetFromJsonAsync<RawgListResponse<RawgGameDto>>(url);

            return response?.Results ?? new List<RawgGameDto>();
        }

        public async Task<RawgGameDto> ObterJogoPorID(int id)
        {
            string chaveApi = await _apis.ObterChaveApi(11);

            var url = $"games/{id}?key={chaveApi}";

            RawgGameDto response = await _http
                .GetFromJsonAsync<RawgGameDto>(url);

            return response;
        }



    }
}
