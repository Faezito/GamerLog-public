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

        public RawgService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["Rawg:ApiKey"];
        }

        public async Task<List<RawgGameDto>> ObterJogos(int page = 1, int pageSize = 20, string? titulo = " ")
        {
            if(titulo == null) titulo = " ";
            var search = Uri.EscapeDataString(titulo);

            var url =
                $"games?key={_apiKey}" +
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
            var url = $"games/{id}?key={_apiKey}";

            RawgGameDto response = await _http
                .GetFromJsonAsync<RawgGameDto>(url);

            return response;
        }
    }
}
