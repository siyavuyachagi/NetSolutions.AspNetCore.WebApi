using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;
using NetSolutions.WebApi.Models.DTOs;

namespace NetSolutions.WebApi.Services;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, ApplicationUserDto>()
        //.ForMember(dest => dest.UserRoles, opt => opt.Ignore()) // Usually populated separately
            .ForMember(dest => dest.PhysicalAddress, opt => opt.MapFrom(src => src.PhysicalAddress))
            .ForMember(dest => dest.UserActivities, opt => opt.MapFrom(src => src.UserActivities))
            .ForMember(d => d.Discriminator, o => o.MapFrom(s => s.Discriminator.Replace(nameof(Project), string.Empty).ToFormattedString(Casing.Pascal)));

        CreateMap<BusinessService, BusinessServiceDto>()
            .ForMember(d => d.Testimonials, o => o.MapFrom(s => s.Testimonials))
            .ForMember(d => d.Thumbnail, o => o.MapFrom(s => s.Thumbnail.ViewLink));

        CreateMap<BusinessServicePackage, BusinessServicePackageDto>();

        CreateMap<BusinessServicePackageFeature, BusinessServicePackageFeatureDto>();

        CreateMap<Client, ClientDto>();

        CreateMap<Feedback, FeedbackDto>();

        CreateMap<FileMetadata, FileMetadataDto>();

        CreateMap<Organization, OrganizationDto>();

        CreateMap<PhysicalAddress, PhysicalAddressDto>();

        CreateMap<Profession, ProfessionDto>();

        CreateMap<Project, ProjectDto>()
            .ForMember(d => d.Documents, o => o.MapFrom(s => s.Project_FileMetadata_Documents.Select(x => x.FileMetadata)))
            .ForMember(d => d.TechnologyStacks, o => o.MapFrom(s => s.Project_TechnologyStacks.Select(x => x.TechnologyStack)))
            .ForMember(d => d.Discriminator, o => o.MapFrom(s => s.Discriminator.Replace(nameof(Project), string.Empty).ToFormattedString(Casing.Pascal)));

        CreateMap<ProjectMilestone, ProjectMilestoneDto>();

        CreateMap<ProjectTask, ProjectTaskDto>();

        CreateMap<ProjectTeam, ProjectTeamDto>();

        CreateMap<ProjectTeamMember, ProjectTeamMemberDto>();

        CreateMap<Review, ReviewDto>();

        CreateMap<SocialLink, SocialLinkDto>();

        CreateMap<Solution, SolutionDto>()
            .ForMember(d => d.TechnologyStacks, o => o.MapFrom(s => s.Solution_TechnologyStacks.Select(x => x.TechnologyStack)))
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Solution_FileMetadata_Images.Select(x => x.FileMetadata)))
            .ForMember(d => d.Documents, o => o.MapFrom(s => s.Solution_FileMetadata_Documents.Select(x => x.FileMetadata)))
            .ForMember(d => d.Reviews, o => o.MapFrom(s => s.Solution_Reviews.Select(x => x.Review)))
            .ForMember(d => d.Likes, o => o.MapFrom(s => s.Solution_Likes.Select(x => x.Liker)));

        CreateMap<SolutionFeature, SolutionFeatureDto>();

        CreateMap<Solution_Like, SolutionLikeDto>();

        CreateMap<Staff, StaffDto>()
            .ForMember(d => d.Profession, o => o.MapFrom(s => s.Profession))
            .ForMember(d => d.UserSkills, o => o.MapFrom(s => s.Staff_UserSkills.Select(x => x.UserSkill)));

        CreateMap<Subscription, SubscriptionDto>();

        CreateMap<TeamMemberRole, TeamMemberRoleDto>();

        CreateMap<TechnologyStack, TechnologyStackDto>();

        CreateMap<Testimonial, TestimonialDto>();

        CreateMap<UserActivity, UserActivityDto>();

        CreateMap<UserSkill, UserSkillDto>();
    }
}
