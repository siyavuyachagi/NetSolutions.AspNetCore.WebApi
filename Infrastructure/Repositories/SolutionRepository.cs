using Application.DTOs;
using Application.DTOs.Response;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Configurations;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace Infrastructure.Repositories
{
    public class SolutionRepository : ISolution
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<SolutionRepository> _logger;
        private readonly GithubConfig _githubConfig;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SolutionRepository(
            ApplicationDbContext context,
            IMapper mapper,
            ILogger<SolutionRepository> logger,
            GithubConfig githubConfig,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _githubConfig = githubConfig;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Result<FileDownloadResult>> DownloadSolutionZipAsync(Guid id)
        {
            try
            {
                // 1. Get the Solution's data
                var solution = await _context.Solutions
                    .AsNoTrackingWithIdentityResolution()
                    .Where(s => s.Id == id)
                    .Include(s => s.Solution_FileMetadata_Documents)
                    .ThenInclude(sfd => sfd.FileMetadata)
                    .FirstOrDefaultAsync();
                if (solution is null) return Result.Failure<FileDownloadResult>($"Solution :{id} not found.");


                // 2. Create a temporary working directory
                var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempDir);

                // 3. Download GitHub source as ZIP
                var repositoryZipPath = await Github.DownloadRepositoryAsync(solution.SourceUrl, personalAccessToken: _githubConfig.PersonalAccessToken);
                var sourceExtractPath = Path.Combine(tempDir, "source");
                Directory.CreateDirectory(sourceExtractPath);
                ZipFile.ExtractToDirectory(repositoryZipPath, sourceExtractPath);

                // 4. Create final ZIP including source and optional documents
                var finalZipPath = Path.Combine(tempDir, $"netsolutions-{solution.Id}.zip");
                using (var archive = ZipFile.Open(finalZipPath, ZipArchiveMode.Create))
                {
                    // Add source code files
                    foreach (var file in Directory.GetFiles(sourceExtractPath, "*", SearchOption.AllDirectories))
                    {
                        var entryName = Path.GetRelativePath(sourceExtractPath, file);
                        archive.CreateEntryFromFile(file, Path.Combine(entryName));
                    }

                    // Add documents
                    foreach (var file in solution.Solution_FileMetadata_Documents.Select(x => x.FileMetadata))
                    {
                        try
                        {
                            switch (file.StorageProvider)
                            {
                                case FileMetadata.EStorageProvider.Local:
                                    {
                                        HandleLocalFile(file, archive);
                                        break;
                                    }
                                case FileMetadata.EStorageProvider.GoogleDrive:
                                case FileMetadata.EStorageProvider.OneDrive:
                                case FileMetadata.EStorageProvider.AWS_S3:
                                case FileMetadata.EStorageProvider.Cloudinary:
                                    // TODO: Implement support for cloud providers
                                    break;

                                default:
                                    // Optional: handle unsupported providers
                                    break;
                            }
                        }
                        catch (Exception innerEx)
                        {
                            _logger.LogWarning(innerEx, $"Failed to add document '{file.Name}' to archive.");
                        }
                    }
                }


                // 5. Return final zip as file download
                var zipBytes = await File.ReadAllBytesAsync(finalZipPath);
                var fileName = $"NetSolution-{solution.Name}-{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}.zip";

                var zipFile = new FileDownloadResult(zipBytes, "application/zip", fileName);
                return Result.Success(zipFile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Failure<FileDownloadResult>(ex.Message);
            }
        }

        public async Task<SolutionDto> GetSolutionAsync(Guid id)
        {
            try
            {
                var solution = await _context.Solutions
                    .AsNoTrackingWithIdentityResolution()
                    .Where(x => x.Id == id)
                    .Include(x => x.Project)
                        .ThenInclude(x => x.BusinessService)
                    .Include(x => x.SolutionFeatures)
                    .Include(x => x.Solution_TechnologyStacks)
                        .ThenInclude(x => x.TechnologyStack)
                    .Include(x => x.Solution_FileMetadata_Images)
                        .ThenInclude(x => x.FileMetadata)
                    .Include(x => x.Solution_FileMetadata_Documents)
                        .ThenInclude(x => x.FileMetadata)
                    .Include(x => x.Solution_Reviews)
                        .ThenInclude(x => x.Review)
                        .ThenInclude(x => x.Evaluator)
                    .Include(x => x.Solution_Likes)
                        .ThenInclude(x => x.Liker)
                    .FirstOrDefaultAsync();

                var solutionDto = _mapper.Map<SolutionDto>(solution);
                return solutionDto;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<SolutionDto>> GetSolutionsAsync()
        {
            try
            {
                var solutions = await _context.Solutions
                    .AsNoTrackingWithIdentityResolution()
                    .Include(x => x.SolutionFeatures)
                    .Include(x => x.Solution_TechnologyStacks)
                        .ThenInclude(x => x.TechnologyStack)
                    .Include(x => x.Solution_FileMetadata_Images)
                        .ThenInclude(x => x.FileMetadata)
                    .Include(x => x.Solution_FileMetadata_Documents)
                        .ThenInclude(x => x.FileMetadata)
                    .Include(x => x.Solution_Reviews)
                        .ThenInclude(x => x.Review)
                        .ThenInclude(x => x.Evaluator)
                    .Include(x => x.Solution_Likes)
                        .ThenInclude(x => x.Liker)
                    .ToListAsync();

                var result = _mapper.Map<List<SolutionDto>>(solutions);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        private void HandleLocalFile(FileMetadata file, ZipArchive archive)
        {
            string entryPath = Path.Combine("Documents", file.Name + file.Extension);

            if (File.Exists(file.DownloadLink))
            {
                archive.CreateEntryFromFile(file.DownloadLink, entryPath);
                return;
            }

            if (TryGetPhysicalPathFromViewLink(file.ViewLink, out var physicalPath) && File.Exists(physicalPath))
            {
                archive.CreateEntryFromFile(physicalPath, entryPath);
            }
        }
        private bool TryGetPhysicalPathFromViewLink(string viewLink, out string? physicalPath)
        {
            physicalPath = null;
            if (string.IsNullOrWhiteSpace(viewLink)) return false;

            try
            {
                var uri = new Uri(viewLink);
                var relativePath = uri.AbsolutePath.TrimStart('/');
                physicalPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
