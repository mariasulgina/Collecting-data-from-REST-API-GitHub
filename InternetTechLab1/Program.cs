// using System.Net.Http;
// using System.Net; 
// using System.Text.Json;
// using System.Threading.Tasks;
// using System.Linq;
// using System.Collections.Generic;
// using System.Diagnostics; 
// using System.Text;
// using InternetTechLab1.Models;

// namespace InternetTechLab1;
// class Program 
// {
//     static readonly HttpClient _httpClient = new HttpClient();
//     static readonly string baseAddress = "https://api.github.com/users"; //"https://api.github.com/users/fawazahmed0/repos" , "https://api.github.com/users/octocat/repos"
//     //https://api.github.com/users/{username}
//     //GET https://api.github.com/users/{username}/repos
//     //GET https://api.github.com/users/{username}/starred
//     //GET https://api.github.com/users/{username}/following
//     //GET https://api.github.com/users/{username}/followers
//     //GET https://api.github.com/users/{username}/orgs
//     //GET https://api.github.com/users/{username}/gists

//     private static readonly HttpClient sharedClient = new() 
//     {
//         BaseAddress = new Uri("https://api.github.com"),
//     };

//     static async Task Main(string[] args) 
//     {
//         _httpClient.DefaultRequestHeaders.Add("User-Agent", "InternetTechLab1/1.0");
//         _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
//         await GetUsers(baseAddress, "octocat");
//         //JsonDeserializerTime();
//         //await asyncJsonDeserializeTime(); 
//         asyncJsonDeserialize();
//     }

//     static async Task GetUsers(string baseAddress, string nameRepositiry) 
//     {
//         HttpResponseMessage response = await _httpClient.GetAsync(baseAddress + "/" + nameRepositiry + "/repos"); // GetAsync возвращщает http response, у него есть свойства 
//         HttpStatusCode statusCode = response.StatusCode;

//         if (response.IsSuccessStatusCode) 
//         {
//             var content = await response.Content.ReadAsStringAsync(); //уже JSON 
//             //Console.WriteLine(content); //вывод всей информации в текстовом формате 
//             //Console.WriteLine(response.StatusCode); //код успеха не успеха

//             //string jsonString = JsonSerializer.Serialize(content); //создание JSON в виде строки
//             var repos = JsonSerializer.Deserialize<List<GitHubRepo>>(content);
//             //Console.WriteLine(content);
//             foreach (var rep in repos) 
//             {
//                 //Console.WriteLine(rep);
//                 //Console.WriteLine($"{rep.Owner}/ {rep.NameRepository,-30} ⭐{rep.Id,4}");
//                 Console.WriteLine($"⭐{rep.id}");
//             }
//         }
//     }

//     static async Task PostUsers(string baseAddress) 
//     {
//         //var response = await _httpClient.PostAsync(url, content);
//     }

//     static async Task GetAsync(HttpClient httpClient) 
//     {
//         using HttpResponseMessage response = await httpClient.GetAsync("/repos");
//     }

//     // static async Task GetFromJsonAsync(HttpClient httpClient) 
//     // {
//     //     var todos = await httpClient.GetFromJsonAsync<List<Todo>>("todos?userId=1&completed=false");

//     //     Console.WriteLine("GET https://jsonplaceholder.typicode.com/todos?userId=1&completed=false HTTP/1.1");
//     //     todos?.ForEach(Console.WriteLine);
//     //     Console.WriteLine();
//     // }

//     static void JsonDeserializer() 
//     {
//         string jsonString = """
//         {
//         "id": 132935648,
//         "name": "octocat",
//         "full_name": "octocat/boysenberry-repo-1",
//         "private": false
//         }
//         """;

//         GitHubRepo? gitHubInformation = JsonSerializer.Deserialize<GitHubRepo>(jsonString);
//         Console.WriteLine($"id: {gitHubInformation.id}, name: {gitHubInformation.name}, full_name: {gitHubInformation.full_name}, IsPrivate: {gitHubInformation?.IsPrivate}");
//     }

//     static void JsonDeserializerTime() 
//     {
//         var totalTicks = 0L;

//         string jsonString = """
//         {
//         "id": 132935648,
//         "name": "octocat",
//         "full_name": "octocat/boysenberry-repo-1",
//         "private": false
//         }
//         """;

//         for(int i = 0; i < 10_000_000; i++) 
//         {
//             var stopWatch = Stopwatch.StartNew();
//             GitHubRepo? gitHubInformation = JsonSerializer.Deserialize<GitHubRepo>(jsonString);
//             stopWatch.Stop();
//             totalTicks += stopWatch.ElapsedTicks;
//         }

//         Console.WriteLine($"СИНХРОННО: {totalTicks / Stopwatch.Frequency}с");
//     }

//     public static async Task asyncJsonDeserializeTime() 
//     {
//         var totalTicks = 0L;

//         string jsonString = """
//         {
//         "id": 132935648,
//         "name": "octocat",
//         "full_name": "octocat/boysenberry-repo-1",
//         "private": false
//         }
//         """;

//         byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);
//         using var stream = new MemoryStream(jsonBytes);

//         for(int i = 0; i < 10_000_000; i++) 
//         {   
//             stream.Position = 0;
//             var stopWatch = Stopwatch.StartNew();
//             GitHubRepo? gitHubInformationAsync = await JsonSerializer.DeserializeAsync<GitHubRepo>(stream); 
//             stopWatch.Stop();
//             totalTicks += stopWatch.ElapsedTicks;
//         }

//         Console.WriteLine($"АСИНХРОННО: {totalTicks / Stopwatch.Frequency}с");
//         //MemoryStream (RAM):    Sync: 0.045мс | Async: 0.060мс
//         //FileStream (SSD):     Sync: 2-5мс   | Async: 1-3мс (async ВЫИГРАЕТ!)
//         //NetworkStream (GitHub):Sync: 100-500мс | Async: 50-200мс (async ВЫИГРАЕТ!)
//     }

//     public static async Task asyncJsonDeserialize() 
//     {
//         string jsonString = """
//         {
//         "id": 132935648,
//         "name": "octocat",
//         "full_name": "octocat/boysenberry-repo-1",
//         "private": false
//         }
//         """;

//         //GitHubRepo? gitHubInformationAsync = await Task.Run(() => JsonSerializer.DeserializeAsync<GitHubRepo>(jsonString)); //лямбда, Кидает её в Thread Pool (отдельный поток), это многопоточность 
//         //короче, чтобы не допускать многопоточность в await Task.Run(() => Jso... и чтобы не выполнять код синхронно, 
//         //мы создаем файл-MemoryStream в памяти и уже его выполняем асинхронно как опрецию IO

//         await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)); 
//         //await using правильно освобождает ресурсы асинхронно (важно для файлов/сетей), у обычного using - метод Dispose(), у await using - await DisposeAsync()
//         //MemoryStream - поток ОЗУ. НЕ пишет на диск, НЕ читает с диска!
//         //Encoding.UTF8.GetBytes(jsonString)— строка → байты
//         GitHubRepo? gitHubInformationAsync = await JsonSerializer.DeserializeAsync<GitHubRepo>(stream); 
//         //GitHubRepo? gitHubInformationAsync = await Task.JsonSerializer.DeserializeAsync<GitHubRepo>(jsonString); //Task.JsonSerializer.DeserializeAsyncне существует!, DeserializeAsyncпринимает только Stream, НЕ string!
//         Console.WriteLine($"id: {gitHubInformationAsync?.id}, name: {gitHubInformationAsync?.name}, full_name: {gitHubInformationAsync?.full_name}, IsPrivate: {gitHubInformationAsync?.IsPrivate}");
//     }

    
// }

using InternetTechLab1.Services;

namespace InternetTechLab1;
class Program 
{
    static async Task Main(string[] args) {
        MenuService menu = new MenuService();
        menu.Run();
    }
}
