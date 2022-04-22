using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LiquidationDashboard.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _client;

        public ApiService()
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri("https://api.bellman.top/v1/"),
            };
        }

        public async Task<IEnumerable<GetStoragesDto>> GetStorages(string symbol)
        {
            try
            {
                var response = await _client.GetAsync($"getstorages?symbol={symbol}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var deserialize = JsonSerializer.Deserialize<IEnumerable<GetStoragesDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return deserialize;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<IEnumerable<string>> GetActiveSymbols()
        {
            try
            {
                var response = await _client.GetAsync($"getactiveorderedsymbols");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var deserialize = JsonSerializer.Deserialize<IEnumerable<string>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return deserialize;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
    }
    public class GetStoragesDto
    {
        public string Address { get; set; }
        public Storagestate StorageState { get; set; }

        public class Storagestate
        {
            public int User_global_max_borrow_in_dollars { get; set; }
            public int User_global_borrowed_in_dollars { get; set; }
        }
    }
}
