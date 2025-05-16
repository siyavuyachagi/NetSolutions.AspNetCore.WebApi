using Application.DTOs.Response;
using Application.Helpers;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(dest => dest.PhysicalAddress, opt => opt.MapFrom(src => src.PhysicalAddress))
                .ForMember(dest => dest.UserActivities, opt => opt.MapFrom(src => src.UserActivities))
                .ForMember(d => d.Discriminator, o => o.MapFrom(s => s.Discriminator.ToFormattedString(Casing.Pascal)));

            CreateMap<BusinessService, BusinessServiceDto>()
                .ForMember(d => d.Testimonials, o => o.MapFrom(s => s.BusinessService_Testimonials.Select(x => x.Testimonial)))
                .ForMember(d => d.Thumbnail, o => o.MapFrom(s => s.Thumbnail.ViewLink));

            CreateMap<BusinessServicePackage, BusinessServicePackageDto>();

            CreateMap<BusinessServicePackageFeature, BusinessServicePackageFeatureDto>();

            CreateMap<Client, ClientDto>();

            CreateMap<Feedback, FeedbackDto>();

            CreateMap<FileMetadata, FileMetadataDto>();

            CreateMap<HardSkill, HardSkillDto>();

            CreateMap<Organization, OrganizationDto>();

            CreateMap<PhysicalAddress, PhysicalAddressDto>();

            CreateMap<Profession, ProfessionDto>();

            CreateMap<Project, ProjectDto>()
                .ForMember(d => d.Documents, o => o.MapFrom(s => s.Project_FileMetadata_Documents.Select(x => x.FileMetadata)))
                .ForMember(d => d.TechnologyStacks, o => o.MapFrom(s => s.Project_TechnologyStacks.Select(x => x.TechnologyStack)))
                .ForMember(d => d.Timeline, o => o.MapFrom(s => s.Project_Timeline.Timeline))
                .ForMember(d => d.Status, o => o.MapFrom(s => EnumHelper.GetDisplayName(s.Status)))
                .ForMember(d => d.Discriminator, o => o.MapFrom(s => s.Discriminator.Replace(nameof(Project), string.Empty).ToFormattedString(Casing.Pascal)));

            CreateMap<ProjectMilestone, ProjectMilestoneDto>();

            CreateMap<ProjectTask, ProjectTaskDto>();

            CreateMap<ProjectTeam, ProjectTeamDto>();

            CreateMap<ProjectTeamMember, ProjectTeamMemberDto>()
                .ForMember(d => d.TeamMemberRoles, o => o.MapFrom(s => s.ProjectTeamMember_TeamMemberRoles.Select(x => x.TeamMemberRole)));

            CreateMap<Review, ReviewDto>();

            CreateMap<SocialLink, SocialLinkDto>();

            CreateMap<Solution, SolutionDto>()
                .ForMember(d => d.Images, o => o.MapFrom(s => s.Solution_FileMetadata_Images.Select(x => x.FileMetadata)))
                .ForMember(d => d.Documents, o => o.MapFrom(s => s.Solution_FileMetadata_Documents.Select(x => x.FileMetadata)))
                .ForMember(d => d.TechnologyStacks, o => o.MapFrom(s => s.Solution_TechnologyStacks.Select(x => x.TechnologyStack)))
                .ForMember(d => d.Reviews, o => o.MapFrom(s => s.Solution_Reviews.Select(x => x.Review)))
                .ForMember(d => d.Likes, o => o.MapFrom(s => s.Solution_Likes.Select(x => x.Liker)))
                .ForMember(d => d.Discriminator, o => o.MapFrom(s => s.Discriminator.Replace(nameof(Solution), string.Empty).ToFormattedString(Casing.Pascal)));

            CreateMap<SolutionFeature, SolutionFeatureDto>();

            CreateMap<Solution_Like, Solution_LikeDto>();

            CreateMap<Staff_Skill, Staff_SkillDto>();

            CreateMap<Staff, StaffDto>();

            CreateMap<Subscription, SubscriptionDto>();

            CreateMap<Team, TeamDto>();

            CreateMap<TeamMemberRole, TeamMemberRoleDto>();

            CreateMap<TechnologyStack, TechnologyStackDto>();

            CreateMap<Testimonial, TestimonialDto>();

            CreateMap<Timeline, TimelineDto>();

            CreateMap<TimelineInterval, TimelineIntervalDto>();

            CreateMap<UserActivity, UserActivityDto>();

            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => EnumHelper.GetDisplayName(src.Status)));
        }
    }
}

