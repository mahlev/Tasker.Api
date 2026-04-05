using AutoMapper;
using Tasker.Api.Dtos.Tasks;
using Tasker.Api.Entities;

namespace Tasker.Api.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Task Item Mappings
        CreateMap<TaskItem, TaskResponseDto>()
            .ForMember(dest => dest.SubtaskCount, opt => opt.MapFrom(src => src.Subtasks != null ? src.Subtasks.Count : 0));


        CreateMap<TaskCreateDto, TaskItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.AssigneeId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Subtasks, opt => opt.Ignore());

        CreateMap<TaskUpdateDto, TaskItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.AssigneeId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Subtasks, opt => opt.Ignore());

        // Subtask Mappings
        CreateMap<Subtask, SubtaskResponseDto>();


        CreateMap<SubtaskCreateDto, Subtask>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TaskId, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.TaskItem, opt => opt.Ignore());
    }
}
