namespace TedLearnPresentation.ViewComponents.CourseGroupComponents;

public class CourseGroupViewComponent : ViewComponent
{
    #region ConstructorInjection

    private readonly ICourseGroupServices _courseGroupServices;

    public CourseGroupViewComponent(ICourseGroupServices courseGroupServices)
    {
        _courseGroupServices = courseGroupServices;
    }

    #endregion

    public async Task<IViewComponentResult> InvokeAsync(string viewFor = "navbar")
    {
        var model = await _courseGroupServices.GetCourseGroupsAsync(isDeleted: false, justGroup: false);
        return viewFor.ToLower() switch
        {
            "navbar" => await Task.FromResult(
                 (IViewComponentResult)View("/Views/Components/CourseGroupComponent/CourseGroup.cshtml", model)),

            "navbar-mobile" => await Task.FromResult(
                 (IViewComponentResult)View("/Views/Components/CourseGroupComponent/CourseGroupInMobile.cshtml", model)),

            _ => await Task.FromResult(
                 (IViewComponentResult)View("/Views/Components/CourseGroupComponent/CourseGroup.cshtml", model))
        };
    }
}
