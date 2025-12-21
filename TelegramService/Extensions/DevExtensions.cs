using System.Text.Json;

namespace TelegramService.Extensions
{
    public static class DevExtensions
    {
        public static async Task<string?> GetNgrokUrlAsync()
        {
            using var client = new HttpClient();
            var json = await client.GetStringAsync("http://ngrok:4040/api/tunnels");
            using var doc = JsonDocument.Parse(json);

            var tunnels = doc.RootElement.GetProperty("tunnels");

            foreach (var t in tunnels.EnumerateArray())
            {
                var url = t.GetProperty("public_url").GetString();
                if (url != null && url.StartsWith("https://"))
                    return url;
            }

            return null;
        }
    }
}
