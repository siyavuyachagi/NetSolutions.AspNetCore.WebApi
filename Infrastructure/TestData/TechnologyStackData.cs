using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.TestData
{
    public class TechnologyStackData
    {
        public static void GenerateTechnologyStacks(ModelBuilder builder)
        {
            try
            {
                var technologyStacks = new List<TechnologyStack>
            {
                // Programming Languages
                new TechnologyStack { Id = Guid.NewGuid(), Name = "C#", NameAbbr = "C#", IconUrl = "csharp.png", Type = TechnologyStack.EType.ProgrammingLanguage, Description = "A versatile, object-oriented language developed by Microsoft." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "JavaScript", NameAbbr = "JS", IconUrl = "javascript.png", Type = TechnologyStack.EType.ProgrammingLanguage, Description = "A widely-used language for web development, enabling interactive web pages." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Python", NameAbbr = "Py", IconUrl = "python.png", Type = TechnologyStack.EType.ProgrammingLanguage, Description = "A high-level, interpreted language known for its readability and versatility." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Java", NameAbbr = "Java", IconUrl = "java.png", Type = TechnologyStack.EType.ProgrammingLanguage, Description = "A robust, object-oriented language designed for cross-platform compatibility." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "TypeScript", NameAbbr = "TS", IconUrl = "typescript.png", Type = TechnologyStack.EType.ProgrammingLanguage, Description = "A superset of JavaScript that adds static types, making it easier to manage large codebases." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "PHP", NameAbbr = "PHP", IconUrl = "php.png", Type = TechnologyStack.EType.ProgrammingLanguage, Description = "A server-side scripting language designed for web development." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Go", NameAbbr = "Go", IconUrl = "go.png", Type = TechnologyStack.EType.ProgrammingLanguage, Description = "A statically typed, compiled language known for its efficiency and simplicity." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Rust", NameAbbr = "Rust", IconUrl = "rust.png", Type = TechnologyStack.EType.ProgrammingLanguage, Description = "A systems programming language focused on safety and performance." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Swift", NameAbbr = "Swift", IconUrl = "swift.png", Type = TechnologyStack.EType.ProgrammingLanguage, Description = "A powerful and intuitive programming language for iOS, macOS, watchOS, and tvOS app development." },

                // Frameworks
                new TechnologyStack { Id = Guid.NewGuid(), Name = ".NET Core", NameAbbr = ".NET", IconUrl = "dotnet.png", Type = TechnologyStack.EType.Framework, Description = "A cross-platform framework for building modern, cloud-based applications." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "ASP.NET Core", NameAbbr = "ASP.NET", IconUrl = "aspnet.png", Type = TechnologyStack.EType.Framework, Description = "A framework for building web applications and services with .NET." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Vue.js", NameAbbr = "Vue", IconUrl = "vue.png", Type = TechnologyStack.EType.Framework, Description = "A progressive framework for building user interfaces." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "React", NameAbbr = "React", IconUrl = "react.png", Type = TechnologyStack.EType.Framework, Description = "A JavaScript library for building user interfaces, particularly single-page applications." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Angular", NameAbbr = "Angular", IconUrl = "angular.png", Type = TechnologyStack.EType.Framework, Description = "A platform for building mobile and desktop web applications." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Django", NameAbbr = "Django", IconUrl = "django.png", Type = TechnologyStack.EType.Framework, Description = "A high-level Python web framework that encourages rapid development." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Flask", NameAbbr = "Flask", IconUrl = "flask.png", Type = TechnologyStack.EType.Framework, Description = "A micro web framework for Python, designed for simplicity and flexibility." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Spring Boot", NameAbbr = "Spring", IconUrl = "springboot.png", Type = TechnologyStack.EType.Framework, Description = "A framework for building stand-alone, production-grade Spring-based applications." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = ".NET MAUI", NameAbbr = ".NET MAUI", IconUrl = "dotnetmaui.png", Type = TechnologyStack.EType.Framework, Description = "A cross-platform framework for building native applications on mobile and desktop." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Xamarin", NameAbbr = "Xamarin", IconUrl = "xamarin.png", Type = TechnologyStack.EType.Framework, Description = "A platform for building modern mobile applications using .NET." },

                // Libraries
                new TechnologyStack { Id = Guid.NewGuid(), Name = "jQuery", NameAbbr = "jQ", IconUrl = "jquery.png", Type = TechnologyStack.EType.Library, Description = "A fast, small, and feature-rich JavaScript library." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Lodash", NameAbbr = "Lodash", IconUrl = "lodash.png", Type = TechnologyStack.EType.Library, Description = "A modern JavaScript utility library delivering modularity, performance, and extras." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Moment.js", NameAbbr = "Moment", IconUrl = "moment.png", Type = TechnologyStack.EType.Library, Description = "A JavaScript date library for parsing, validating, manipulating, and formatting dates." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Bootstrap", NameAbbr = "Bootstrap", IconUrl = "bootstrap.png", Type = TechnologyStack.EType.Library, Description = "A popular front-end framework for developing responsive and mobile-first websites." },

                // Databases
                new TechnologyStack { Id = Guid.NewGuid(), Name = "SQL Server", NameAbbr = "MSSQL", IconUrl = "sqlserver.png", Type = TechnologyStack.EType.Database, Description = "A relational database management system developed by Microsoft." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "MySQL", NameAbbr = "MySQL", IconUrl = "mysql.png", Type = TechnologyStack.EType.Database, Description = "An open-source relational database management system." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "PostgreSQL", NameAbbr = "PostgreSQL", IconUrl = "postgresql.png", Type = TechnologyStack.EType.Database, Description = "A powerful, open-source object-relational database system." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "MongoDB", NameAbbr = "Mongo", IconUrl = "mongodb.png", Type = TechnologyStack.EType.Database, Description = "A NoSQL database designed for high performance and scalability." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "SQLite", NameAbbr = "SQLite", IconUrl = "sqlite.png", Type = TechnologyStack.EType.Database, Description = "A lightweight, disk-based database that doesn't require a separate server process." },

                // APIs
                new TechnologyStack { Id = Guid.NewGuid(), Name = "RESTful API", NameAbbr = "REST", IconUrl = "rest.png", Type = TechnologyStack.EType.API, Description = "An architectural style for designing networked applications." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "GraphQL", NameAbbr = "GraphQL", IconUrl = "graphql.png", Type = TechnologyStack.EType.API, Description = "A query language for APIs and a runtime for executing those queries." },

                // Cloud Computing
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Azure", NameAbbr = "Azure", IconUrl = "azure.png", Type = TechnologyStack.EType.CloudComputing, Description = "A cloud computing service created by Microsoft for building, testing, deploying, and managing applications and services." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "AWS", NameAbbr = "AWS", IconUrl = "aws.png", Type = TechnologyStack.EType.CloudComputing, Description = "A comprehensive cloud platform offering over 200 fully featured services." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Google Cloud", NameAbbr = "GCP", IconUrl = "gcp.png", Type = TechnologyStack.EType.CloudComputing, Description = "A suite of cloud computing services that runs on the same infrastructure that Google uses internally." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Firebase", NameAbbr = "Firebase", IconUrl = "firebase.png", Type = TechnologyStack.EType.CloudComputing, Description = "A platform developed by Google for creating mobile and web applications." },

                // Containerization & Orchestration
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Docker", NameAbbr = "Docker", IconUrl = "docker.png", Type = TechnologyStack.EType.ContainerizationOrchestration, Description = "A platform for developing, shipping, and running applications in containers." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Kubernetes", NameAbbr = "K8s", IconUrl = "kubernetes.png", Type = TechnologyStack.EType.ContainerizationOrchestration, Description = "An open-source system for automating the deployment, scaling, and management of containerized applications." },

                // CI/CD Tools
                new TechnologyStack { Id = Guid.NewGuid(), Name = "GitHub Actions", NameAbbr = "GHA", IconUrl = "githubactions.png", Type = TechnologyStack.EType.CICDTool, Description = "A CI/CD tool that automates workflows directly within GitHub." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Jenkins", NameAbbr = "Jenkins", IconUrl = "jenkins.png", Type = TechnologyStack.EType.CICDTool, Description = "An open-source automation server that enables developers to build, test, and deploy their software." },

                // DevOps Tools
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Terraform", NameAbbr = "Terraform", IconUrl = "terraform.png", Type = TechnologyStack.EType.DevOpsTool, Description = "An open-source infrastructure as code software tool." },

                // Version Control
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Git", NameAbbr = "Git", IconUrl = "git.png", Type = TechnologyStack.EType.VersionControl, Description = "A distributed version control system for tracking changes in source code." },

                // IDEs
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Visual Studio", NameAbbr = "VS", IconUrl = "visualstudio.png", Type = TechnologyStack.EType.IDEs, Description = "An integrated development environment from Microsoft." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "VS Code", NameAbbr = "VSCode", IconUrl = "vscode.png", Type = TechnologyStack.EType.IDEs, Description = "A lightweight but powerful source code editor from Microsoft." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Android Studio", NameAbbr = "Android Studio", IconUrl = "androidstudio.png", Type = TechnologyStack.EType.IDEs, Description = "The official integrated development environment for Google's Android operating system." },

                // Testing Tools
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Selenium", NameAbbr = "Selenium", IconUrl = "selenium.png", Type = TechnologyStack.EType.TestingTool, Description = "A suite of tools for automating web browsers." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "xUnit", NameAbbr = "xUnit", IconUrl = "xunit.png", Type = TechnologyStack.EType.TestingTool, Description = "A free, open-source, community-focused unit testing tool for the .NET framework." },

                // Styling Tools
                new TechnologyStack { Id = Guid.NewGuid(), Name = "CSS", NameAbbr = "CSS", IconUrl = "css.png", Type = TechnologyStack.EType.Styling, Description = "A style sheet language used for describing the presentation of a document written in HTML or XML." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "TailwindCSS", NameAbbr = "Tailwind", IconUrl = "tailwind.png", Type = TechnologyStack.EType.Styling, Description = "A utility-first CSS framework for rapidly building custom user interfaces." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "SASS", NameAbbr = "SASS", IconUrl = "sass.png", Type = TechnologyStack.EType.Styling, Description = "A preprocessor scripting language that is interpreted or compiled into CSS." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Bootstrap.css", NameAbbr = "Bootstrap.css", IconUrl = "bootstrap.png", Type = TechnologyStack.EType.Styling, Description = "A popular front-end framework for developing responsive and mobile-first websites." },

                // Design Tools
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Adobe Photoshop", NameAbbr = "PS", IconUrl = "photoshop.png", Type = TechnologyStack.EType.DesignTool, Description = "A raster graphics editor developed and published by Adobe Inc." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Adobe Illustrator", NameAbbr = "AI", IconUrl = "illustrator.png", Type = TechnologyStack.EType.DesignTool, Description = "A vector graphics editor developed and marketed by Adobe Inc." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Figma", NameAbbr = "Figma", IconUrl = "figma.png", Type = TechnologyStack.EType.DesignTool, Description = "A web-based vector graphics editor and prototyping tool." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Sketch", NameAbbr = "Sketch", IconUrl = "sketch.png", Type = TechnologyStack.EType.DesignTool, Description = "A digital design platform used for creating user interfaces and experiences." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Adobe XD", NameAbbr = "XD", IconUrl = "adobexd.png", Type = TechnologyStack.EType.DesignTool, Description = "A vector-based user experience design software for web and mobile apps." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "InVision", NameAbbr = "InVision", IconUrl = "invision.png", Type = TechnologyStack.EType.DesignTool, Description = "A digital product design platform used for creating interactive mockups." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Zeplin", NameAbbr = "Zeplin", IconUrl = "zeplin.png", Type = TechnologyStack.EType.DesignTool, Description = "A collaboration tool that connects designers and developers." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "UXPin", NameAbbr = "UXPin", IconUrl = "uxpin.png", Type = TechnologyStack.EType.DesignTool, Description = "A design and prototyping tool for creating interactive wireframes and prototypes." },

                // Web Development
                new TechnologyStack { Id = Guid.NewGuid(), Name = "HTML5", NameAbbr = "HTML5", IconUrl = "html5.png", Type = TechnologyStack.EType.WebDevelopment, Description = "The latest version of the HTML standard, used for structuring and presenting content on the web." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "Node.js", NameAbbr = "Node", IconUrl = "nodejs.png", Type = TechnologyStack.EType.WebDevelopment, Description = "A JavaScript runtime built on Chrome's V8 JavaScript engine." },
                new TechnologyStack { Id = Guid.NewGuid(), Name = "WordPress", NameAbbr = "WP", IconUrl = "wordpress.png", Type = TechnologyStack.EType.WebDevelopment, Description = "A free and open-source content management system written in PHP." }
            };


                Seed.TechnologyStacks.AddRange(technologyStacks);
                builder.Entity<TechnologyStack>().HasData(technologyStacks);


                var codeHosts = new List<CodeHost>
            {
                // Code Hosting
                new CodeHost { Id = Guid.NewGuid(), Name = "GitHub", NameAbbr = "GH", Url = "https://wwww.github.com", IconUrl = "github.png", Type = TechnologyStack.EType.CodeHosting, Description = "A web-based platform used for version control and collaboration." },
                new CodeHost { Id = Guid.NewGuid(), Name = "GitLab", NameAbbr = "GL", Url = "https://wwww.gitlab.com", IconUrl = "gitlab.png", Type = TechnologyStack.EType.CodeHosting, Description = "A web-based DevOps lifecycle tool that provides a Git repository manager." },
            };
                Seed.TechnologyStacks.AddRange(codeHosts);
                builder.Entity<CodeHost>().HasData(codeHosts);

                Console.WriteLine($"TechnologyStacks generated: {Seed.TechnologyStacks.Count()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating TechnologyStack: {ex.Message}");
                throw;
            }
        }

    }

}

