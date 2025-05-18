using System.Net.Http.Headers;
using System.Text.Json;

namespace Domain.Entities
{
    public class Github: CodeHost
    {
        /// <summary>
        /// Downloads a GitHub repository as a ZIP archive.
        /// </summary>
        /// <param name="repositoryUrl">The public GitHub repository URL (e.g., https://github.com/user/repo).</param>
        /// <param name="branchName">Optional branch name. If not specified, the default branch is used.</param>
        /// <returns>The full file path to the downloaded ZIP archive.</returns>
        /// <exception cref="ArgumentException">Thrown if the repository URL is invalid.</exception>
        /// <exception cref="HttpRequestException">Thrown if the download request fails.</exception>
        public static async Task<string> DownloadRepositoryAsync(string repositoryUrl, string? branchName = null, string? personalAccessToken = null)
        {
            // 1. Determine branch name
            if (string.IsNullOrWhiteSpace(branchName))
            {
                branchName = string.IsNullOrWhiteSpace(personalAccessToken) 
                    ? await GetRepositoryDefaultBranchNameAsync(repositoryUrl) 
                    : await GetRepositoryDefaultBranchNameAsync(repositoryUrl, personalAccessToken);
            }

            // 2. Construct download URL
            var uri = new Uri(repositoryUrl);
            var segments = uri.AbsolutePath.Trim('/').Split('/');
            if (segments.Length < 2)
                throw new ArgumentException("Invalid GitHub repository URL.");

            var user = segments[0];
            var repo = segments[1];
            var zipUrl = $"https://github.com/{user}/{repo}/archive/refs/heads/{branchName}.zip";

            // 3. Download zip to temp path
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            var zipPath = Path.Combine(tempDir, $"{repo}-{branchName}.zip");

            using (var client = new HttpClient { Timeout = TimeSpan.FromMinutes(5)}) // 5 mins timeout
            {
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                if (!string.IsNullOrWhiteSpace(personalAccessToken))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", personalAccessToken);
                }

                var response = await client.GetAsync(zipUrl);
                response.EnsureSuccessStatusCode();

                await using var fs = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None);
                await response.Content.CopyToAsync(fs);
            }

            return zipPath;
        }

        /// <summary>
        /// Retrieves the default branch name for a given GitHub repository.
        /// </summary>
        /// <param name="repositoryUrl">The public GitHub repository URL (e.g., https://github.com/user/repo).</param>
        /// <returns>The default branch name of the repository (e.g., "main" or "master").</returns>
        /// <exception cref="ArgumentException">Thrown if the repository URL is invalid.</exception>
        /// <exception cref="HttpRequestException">Thrown if the API request fails.</exception>
        private static async Task<string> GetRepositoryDefaultBranchNameAsync(string repositoryUrl)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

                var uri = new Uri(repositoryUrl);
                var segments = uri.AbsolutePath.Trim('/').Split('/');
                if (segments.Length < 2)
                    throw new ArgumentException("Invalid GitHub repository URL.");

                var apiUrl = $"https://api.github.com/repos/{segments[0]}/{segments[1]}";

                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(json);
                var branch = jsonDoc.RootElement.GetProperty("default_branch").GetString();

                return branch!;
            }
        }

        /// <summary>
        /// Retrieves the default branch name for a GitHub repository (public or private).
        /// </summary>
        /// <param name="repositoryUrl">The GitHub repository URL (e.g., https://github.com/user/repo).</param>
        /// <param name="personalAccessToken">
        /// Optional GitHub personal access token used to authenticate requests for private repositories or increase rate limits for public ones.
        /// </param>
        /// <returns>
        /// The default branch name of the repository (e.g., "main" or "master").
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the repository URL is invalid.</exception>
        /// <exception cref="HttpRequestException">Thrown if the API request fails or the repository is inaccessible.</exception>
        private static async Task<string> GetRepositoryDefaultBranchNameAsync(string repositoryUrl, string? personalAccessToken = null)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

                if (!string.IsNullOrWhiteSpace(personalAccessToken))
                {
                    client.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Bearer", personalAccessToken);
                }

                var uri = new Uri(repositoryUrl);
                var segments = uri.AbsolutePath.Trim('/').Split('/');
                if (segments.Length < 2)
                    throw new ArgumentException("Invalid GitHub repository URL.");

                var apiUrl = $"https://api.github.com/repos/{segments[0]}/{segments[1]}";

                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(json);
                var branch = jsonDoc.RootElement.GetProperty("default_branch").GetString();

                return branch!;
            }
        }
    }
}
