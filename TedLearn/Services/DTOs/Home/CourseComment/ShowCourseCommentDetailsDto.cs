namespace Services.DTOs.Home.CourseComment;

public class ShowCourseCommentDetailsDto : BaseDto<ShowCourseCommentDetailsDto , Data.Entities.Products.Comment.CourseComment>
{
    public int CourseId { get; set; }
    public int TeacherId { get; set; }
    public int CommentId { get; set; }
    public int UserId { get; set; }
    public int? ParentId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public string UserAvatar { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    public override void CustomMappings(IMappingExpression<Data.Entities.Products.Comment.CourseComment, ShowCourseCommentDetailsDto> mapping)
    {
        mapping.ForMember(dto => dto.TeacherId, opt => opt.MapFrom(cc => cc.Course.TeacherId));
        mapping.ForMember(dto => dto.UserAvatar, opt => opt.MapFrom(cc => cc.User.UserAvatar));
        mapping.ForMember(dto => dto.UserName, opt => opt.MapFrom(cc => cc.User.UserName));
    }
}
