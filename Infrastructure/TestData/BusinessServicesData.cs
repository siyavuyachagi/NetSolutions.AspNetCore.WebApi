using Application.Helpers;
using Bogus;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.TestData;

public class BusinessServicesData
{
    private static readonly Faker _faker = new("en_ZA");
    private static readonly Random _random = new();

    public static void GenerateServices(ModelBuilder builder)
    {
        try
        {
            if (Seed.BusinessServices.Any()) return;
            var ourServices = new List<BusinessService>
            {
                new BusinessService
                {
                    Id = Guid.NewGuid(),
                    Name = "Mobile Development",
                    Description = "Our mobile development services cover native and cross-platform applications for iOS and Android devices. We specialize in creating intuitive, high-performance mobile applications that provide seamless user experiences across all devices. Whether you need a consumer-facing app or an enterprise mobility solution, our experienced team ensures your mobile application meets modern standards.",
                    CreatedAt = _faker.Date.Past(),
                    UpdatedAt = DateTime.Now,
                },
                new BusinessService
                {
                    Id = Guid.NewGuid(),
                    Name = "UI/UX Design",
                    Description = "Our UI/UX design services focus on creating intuitive, engaging, and user-centered digital experiences. We conduct thorough user research, develop wireframes and prototypes, and design responsive interfaces that look great and function flawlessly. Our design process emphasizes usability testing and iteration to ensure the final product meets both user needs and business goals.",
                    CreatedAt = _faker.Date.Past(),
                    UpdatedAt = DateTime.Now,
                },
                new BusinessService
                {
                    Id = Guid.NewGuid(),
                    Name = "Web Design",
                    Description = "Our web design services focus on creating visually stunning, responsive websites that captivate your audience and strengthen your brand presence online. We combine aesthetics with functionality to design custom layouts, engaging visual elements, and intuitive navigation systems. Our designers work closely with clients to ensure the final website not only looks impressive but also delivers an optimal browsing experience across all devices and screen sizes.",
                    CreatedAt = _faker.Date.Past(),
                    UpdatedAt = DateTime.Now,
                },
                new BusinessService
                {
                    Id = Guid.NewGuid(),
                    Name = "Web Development",
                    Description = "We offer comprehensive web development services using cutting-edge technologies such as ASP.NET Core and Vue.js. Our solutions include responsive websites, web applications, e-commerce platforms, and custom CMS implementations. We focus on creating scalable, maintainable, and secure web applications that provide exceptional user experiences while meeting your business requirements.",
                    CreatedAt = _faker.Date.Past(),
                    UpdatedAt = DateTime.Now,
                },
            };

            Seed.BusinessServices.AddRange(ourServices);
            builder.Entity<BusinessService>().HasData(ourServices);

            //GenerateBusinessServiceThumbnail(builder);
            GenerateBusinessServicePackages(builder);
            GenerateBusinessServicePackageFeatures(builder);

            Console.WriteLine($"BusinessServices generated: {Seed.BusinessServices.Count()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating BusinessService: {ex.Message}");
            throw;
        }
    }


    //public static void GenerateBusinessServiceThumbnail(ModelBuilder builder)
    //{
    //    try
    //    {
    //        var businessService = Seed.BusinessServices.ToList();

    //        foreach (var service in businessService)
    //        {
    //            switch (service.Name)
    //            {
    //                case "Mobile Development":
    //                    {
    //                        var file = new Faker("en_ZA").PickRandom(FilesManager.GetDirFiles("_development_files/mobile-dev"));
    //                        GenerateBusinessServiceThumbnail(builder, service, file);
    //                        break;
    //                    }
    //                case "UI/UX Design":
    //                    {
    //                        var file = new Faker("en_ZA").PickRandom(FilesManager.GetDirFiles("_development_files/uiux-design"));
    //                        GenerateBusinessServiceThumbnail(builder, service, file);
    //                        break;
    //                    }
    //                case "Web Design":
    //                    {
    //                        var file = new Faker("en_ZA").PickRandom(FilesManager.GetDirFiles("_development_files/web-design"));
    //                        GenerateBusinessServiceThumbnail(builder, service, file);
    //                        break;
    //                    }
    //                case "Web Development":
    //                    {
    //                        var file = new Faker("en_ZA").PickRandom(FilesManager.GetDirFiles("_development_files/web-dev"));
    //                        GenerateBusinessServiceThumbnail(builder, service, file);
    //                        break;
    //                    }
    //                default:
    //                    break;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error generating BusinessServiceThumbnail: {ex.Message}");
    //        throw;
    //    }
    //}

    //private static void GenerateBusinessServiceThumbnail(ModelBuilder builder, BusinessService service, FilesManager.TestFile file)
    //{
    //    try
    //    {
    //        var fileResource = new FileMetadata
    //        {
    //            Id = Guid.NewGuid().ToString(),
    //            Name = file.FileName,
    //            ContentType = file.Type,
    //            Extension = file.Extension,
    //            Size = file.Size,
    //            ViewLink = file.VirtualPath,
    //            DownloadLink = file.PhysicalPath,
    //            StorageProvider = FileMetadata.EStorageProvider.Local,
    //            CreatedAt = DateTime.Now,
    //            UpdatedAt = DateTime.Now
    //        };
    //        Seed.FileMetadatas.Add(fileResource);
    //        builder.Entity<FileMetadata>().HasData(fileResource);

    //        var ourServiceThumbnail = new BusinessService_FileMetadata_Thumbnail
    //        {
    //            Id = Guid.NewGuid(),
    //            BusinessServiceId = service.Id,
    //            FileMetadataId = fileResource.Id,
    //        };

    //        Seed.BusinessService_FileMetadata_Thumbnails.Add(ourServiceThumbnail);
    //        builder.Entity<BusinessService_FileMetadata_Thumbnail>().HasData(ourServiceThumbnail);
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error generating BusinessServiceThumbnail File: {ex.Message}");
    //        throw;
    //    }
    //}

    private static void GenerateBusinessServicePackages(ModelBuilder builder)
    {
        try
        {
            var businessServices = Seed.BusinessServices.ToList() ?? throw new Exception("BusinessServices is null at GenerateBusinessServicePackages()");

            var servicePackages = new List<BusinessServicePackage>();
            foreach (var service in businessServices)
            {
                servicePackages.AddRange(new List<BusinessServicePackage>
                {
                    new BusinessServicePackage
                    {
                        Id = Guid.NewGuid(),
                        BusinessServiceId = service.Id,
                        Name = "Basic",
                        Description = "Essential features to get started with this service.",
                        Price = 499.99m,
                        BillingCycle = EnumHelper.GetDisplayName(BillingCycle.Monthly),
                        CreatedAt = DateTime.Now
                    },
                    new BusinessServicePackage
                    {
                        Id = Guid.NewGuid(),
                        BusinessServiceId = service.Id,
                        Name = "Standard",
                        Description = "Expanded features and customization for growing needs.",
                        Price = 999.99m,
                        BillingCycle = EnumHelper.GetDisplayName(BillingCycle.Monthly),
                        CreatedAt = DateTime.Now
                    },
                    new BusinessServicePackage
                    {
                        Id = Guid.NewGuid(),
                        BusinessServiceId = service.Id,
                        Name = "Premium",
                        Description = "Advanced features and premium support for businesses.",
                        Price = 1499.99m,
                        BillingCycle = EnumHelper.GetDisplayName(BillingCycle.Monthly),
                        CreatedAt = DateTime.Now
                    },
                });
            }

            Seed.BusinessServicePackages.AddRange(servicePackages);
            builder.Entity<BusinessServicePackage>().HasData(servicePackages);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating BusinessServicePackage: {ex.Message}");
            throw;
        }
    }

    private static void GenerateBusinessServicePackageFeatures(ModelBuilder builder)
    {
        try
        {
            var servicePackages = Seed.BusinessServicePackages.ToList();
            var packageFeatures = new List<BusinessServicePackageFeature>();

            foreach (var package in servicePackages)
            {
                switch (package.Name)
                {
                    case "Basic":
                        packageFeatures.AddRange(new List<BusinessServicePackageFeature>
                        {
                            new BusinessServicePackageFeature
                            {
                                Id = Guid.NewGuid(),
                                BusinessServicePackageId = package.Id,
                                Name = "Static Site Setup",
                                Description = "Setup of a modern static website with a clean design.",
                                CreatedAt = DateTime.Now,
                                Price = 300m // Price in Rands
                            },
                            new BusinessServicePackageFeature
                            {
                                Id = Guid.NewGuid(),
                                BusinessServicePackageId = package.Id,
                                Name = "Five Pages Included",
                                Description = "Includes five essential pages: Home, About, Services, Contact, and Blog.",
                                CreatedAt = DateTime.Now,
                                Price = 500m
                            },
                            new BusinessServicePackageFeature
                            {
                                Id = Guid.NewGuid(),
                                BusinessServicePackageId = package.Id,
                                Name = "Basic SEO",
                                Description = "Basic search engine optimization to improve online visibility.",
                                CreatedAt = DateTime.Now,
                                Price = 200m
                            }
                        });
                        break;

                    case "Standard":
                        packageFeatures.AddRange(new List<BusinessServicePackageFeature>
                        {
                            new BusinessServicePackageFeature
                            {
                                Id = Guid.NewGuid(),
                                BusinessServicePackageId = package.Id,
                                Name = "Responsive Design",
                                Description = "A responsive website design that adapts to all devices.",
                                CreatedAt = DateTime.Now,
                                Price = 400m
                            },
                            new BusinessServicePackageFeature
                            {
                                Id = Guid.NewGuid(),
                                BusinessServicePackageId = package.Id,
                                Name = "Up to 10 Pages",
                                Description = "Customizable website with up to 10 unique pages.",
                                CreatedAt = DateTime.Now,
                                Price = 800m
                            },
                            new BusinessServicePackageFeature
                            {
                                Id = Guid.NewGuid(),
                                BusinessServicePackageId = package.Id,
                                Name = "CMS Integration",
                                Description = "Integration with a content management system for easy content updates.",
                                CreatedAt = DateTime.Now,
                                Price = 600m
                            },
                            new BusinessServicePackageFeature
                            {
                                Id = Guid.NewGuid(),
                                BusinessServicePackageId = package.Id,
                                Name = "Enhanced SEO",
                                Description = "Improved SEO techniques to boost search engine rankings.",
                                CreatedAt = DateTime.Now,
                                Price = 300m
                            }
                        });
                        break;

                    case "Premium":
                        packageFeatures.AddRange(new List<BusinessServicePackageFeature>
                        {
                            new BusinessServicePackageFeature
                            {
                                Id = Guid.NewGuid(),
                                BusinessServicePackageId = package.Id,
                                Name = "Custom Design & Development",
                                Description = "Bespoke design and advanced development for a standout website.",
                                CreatedAt = DateTime.Now,
                                Price = 1500m
                            },
                            new BusinessServicePackageFeature
                            {
                                Id = Guid.NewGuid(),
                                BusinessServicePackageId = package.Id,
                                Name = "Unlimited Pages",
                                Description = "No limitations on page count, allowing a comprehensive online presence.",
                                CreatedAt = DateTime.Now,
                                Price = 1000m
                            },
                            new BusinessServicePackageFeature
                            {
                                Id = Guid.NewGuid(),
                                BusinessServicePackageId = package.Id,
                                Name = "Advanced SEO & Analytics",
                                Description = "Advanced SEO strategies with integrated analytics for performance tracking.",
                                CreatedAt = DateTime.Now,
                                Price = 500m
                            },
                            new BusinessServicePackageFeature
                            {
                                Id = Guid.NewGuid(),
                                BusinessServicePackageId = package.Id,
                                Name = "E-commerce Integration",
                                Description = "Ready-to-use e-commerce functionalities for online sales and transactions.",
                                CreatedAt = DateTime.Now,
                                Price = 1200m
                            }
                        });
                        break;
                    default:
                        break;
                }
            }

            Seed.BusinessServicePackageFeatures.AddRange(packageFeatures);
            builder.Entity<BusinessServicePackageFeature>().HasData(packageFeatures);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating BusinessServicePackageFeatures: {ex.Message}");
            throw;
        }
    }
}
