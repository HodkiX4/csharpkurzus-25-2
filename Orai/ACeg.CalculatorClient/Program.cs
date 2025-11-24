using System.Web;

using HttpClient client = new HttpClient();

//TODO: Fix later
const string ServerUrl = "http://localhost:8080/evaluate";
while (true)
{
    Console.Write("$>");
    string? expression = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(expression))
    {
        string encoded = HttpUtility.UrlEncode(expression);
        string url = $"{ServerUrl}?expression={encoded}";
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string output = await response.Content.ReadAsStringAsync();
            Console.WriteLine(output);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}