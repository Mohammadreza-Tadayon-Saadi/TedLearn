using Microsoft.AspNetCore.Mvc.Rendering;
using Services.DTOs.AdminPanel.Course;
using Data.Entities.Products.Courses;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace TedLearnPresentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
[Route("/Admin/ManageCourses")]
public class ManageCoursesController : Controller
{
    #region ConstructorInjection

    private readonly ICourseGroupServices _courseGroupServices;
    private readonly ICourseServices _courseServices;
    private readonly ITransactionDbContextServices _transactions;
    public ManageCoursesController(ICourseServices courseServices , ICourseGroupServices courseGroupServices , 
        ITransactionDbContextServices transactions)
    {
        _courseServices = courseServices;
        _courseGroupServices = courseGroupServices;
        _transactions = transactions;
    }

    #endregion

    [Route("/Admin/ManageCourses")]
    public IActionResult Index()
    {
        return View();
    }

    #region Ajax/View

    [Route("GetCourses/GetAllCourses")]
    [Route("GetCourses/GetDeletedCourses")]
    public async Task<IActionResult> GetCourses(CancellationToken cancellationToken)
    {
        IEnumerable<ShowCourseDto> courses;
        var routePattern = HttpContext.Request.Path.Value;

        if (routePattern.Contains("GetAllCourses"))
            courses = await _courseServices.GetCoursesAsync(cancellationToken , isDeleted:false);
        else
            courses = await _courseServices.GetCoursesAsync(cancellationToken , isDeleted:true);
         
        return PartialView("GetCoursesAjax", courses);
    }

    [Route("GetCourseDetails/{courseId:int}")]
    public async Task<IActionResult> GetCourseDetails(int courseId , CancellationToken cancellationToken)
    {
        var course = await _courseServices.GetCourseDetailsAsync(courseId , cancellationToken);

        if (course == null) return PartialView("_Error404");

        return PartialView("GetCourseDetailsAjax", course);
    }

    #endregion


    #region AddCourse

    [Route("AddCourse")]
    public async Task<IActionResult> AddCourse(CancellationToken cancellationToken)
    {
        var model = new AddCourseDto()
        {
            GroupList = await _courseGroupServices.GetGroupListAsync(cancellationToken),
            TeacherList = await _courseServices.GetTeacherListAsync(cancellationToken),
            StatusList = await _courseServices.GetStatusListAsync(cancellationToken),
        };
        return View(model);
    }

    [Route("AddCourse")]
    [RequestSizeLimit(2147483647)]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> AddCourse(AddCourseDto model , CancellationToken cancellationToken)
    {
        #region ValidationInput

        if (!ModelState.IsValid)
        {
            model.GroupList = await _courseGroupServices.GetGroupListAsync(cancellationToken);
            model.TeacherList = await _courseServices.GetTeacherListAsync(cancellationToken);
            model.StatusList = await _courseServices.GetStatusListAsync(cancellationToken);
            return View(model);
        }

        #endregion

        //Create Image
        string imapgePath = Path.Combine(Directory.GetCurrentDirectory() , "wwwroot/Courses/course-image");
        var imageName = await FileHelper.CreateFileAsync(model.ImageFile,imapgePath);

        //Create Demo
        string demoPath = Path.Combine(Directory.GetCurrentDirectory() , "wwwroot/Courses/course-demos");
        var demoName = await FileHelper.CreateFileAsync(model.DemoFile, demoPath);

        var course = new Course()
        {
            CourseTitle = model.CourseTitle,
            GroupId = model.GroupId,
            SubGroupId = model.SubGroupId,
            TeacherId = model.TeacherId,
            StatusId = model.StatusId,
            CourseLevel = model.LevelId,
            CoursePrice = ((model.CoursePrice != null) ? (decimal)model.CoursePrice : 0),
            CreateDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            CourseTags = model.Tags,
            CourseDescription = model.Description.SanitizeText(),
            CourseRequirement = model.CourseRequirment,
            CourseImage = imageName,
            CourseDemoFile = demoName
        };

        try
        {
            await _courseServices.AddCourseAsync(course , cancellationToken);
        }
        catch
        {
            FileHelper.DeleteFile(imageName , imapgePath);
            FileHelper.DeleteFile(demoName , demoPath);
            throw;
        }

        return Redirect("/Admin/ManageCourses");
    }

    #region GetSubGroup

    [Route("GetSubGroupForGroup/{groupId:int}")]
    [Route("/ManageCourses/GetSubGroupForGroup/{groupId:int}")]
    public async Task<IActionResult> GetSubGroupForGroup(int groupId , CancellationToken cancellationToken)
    {
        var subGroupList = new List<SelectListItem>()
        {
            new SelectListItem()
            {
                Value = "",
                Text = "---- انتخاب کنید ----"
            }
        };
        subGroupList.AddRange(await _courseGroupServices.GetSubGroupListForGroup(groupId , cancellationToken));

        return Json(new SelectList(subGroupList, "Value", "Text"));
    }

    #endregion

    #endregion


    #region EditCourse

    [Route("EditCourse/{courseId:int}")]
    public async Task<IActionResult> EditCourse(int courseId , CancellationToken cancellationToken)
    {
        var course = await _courseServices.GetCourseForEditAsync(courseId , cancellationToken);
        if (course == null) return NotFound();

        if (TempData.ContainsKey("ConcurrencyInEditCourse"))
        {
            ViewBag.ConCurrency = TempData["ConcurrencyInEditCourse"];
            TempData.Remove("ConcurrencyInEditCourse");
        }

        return View(course);
    }

    [Route("EditCourse")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    [RequestSizeLimit(2147483647)]
    public async Task<IActionResult> EditCourse(EditCourseDto model , CancellationToken cancellationToken)
    {
        #region ValidationInput

        if (!ModelState.IsValid)
        {
            await _courseServices.GetSelectListsForCourseAsync(model, cancellationToken);
            return View(model);
        }

        #endregion

        var course = await _courseServices.GetCourseByIdAsync(model.CourseId , cancellationToken);
        if (course == null) return NotFound();

        // ConCurrency Check
        if (Convert.ToBase64String(course.Version) != model.Base64Version)
        {
            TempData["ConcurrencyInEditCourse"] = true;
            return Redirect($"/ManageCourses/EditCourse/{model.CourseId}");
        }

        #region Save NewImg And NewDemo IF Exists

        var image = "";
        var imageDirectory = Path.Combine(Directory.GetCurrentDirectory() , "wwwroot/Courses/course-image");
        if (model.ImageFile != null)
        {
            image = await FileHelper.CreateFileAsync(model.ImageFile, imageDirectory);
            course.CourseImage = image;
        }

        var demo = "";
        var demoDirectory = Path.Combine(Directory.GetCurrentDirectory() , "wwwroot/Courses/course-demos");
        if (model.DemoFile != null)
        {
            demo = await FileHelper.CreateFileAsync(model.DemoFile, demoDirectory);
            course.CourseDemoFile = demo;
        }

        #endregion

        #region UpdateCourse

        model.ToEntity(course);
        course.CoursePrice = ((model.CoursePrice != null) ? (decimal)model.CoursePrice : 0);
        course.CourseDescription = model.Description.SanitizeText();

        await _transactions.SaveChangesAsync(cancellationToken);

        #endregion

        //Delete preImage And preDemo File
        if (model.ImageFile != null)
            FileHelper.DeleteFile(model.PreImage , imageDirectory);
        if (model.DemoFile != null)
            FileHelper.DeleteFile(model.PreDemo , demoDirectory);

        return Redirect("/Admin/ManageCourses");
    }

    #endregion


    #region DeleteCourse

    [Route("DeleteCourse/{courseId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> DeleteCourse(int courseId , CancellationToken cancellationToken)
    {
        var course = await _courseServices.GetCourseByIdAsync(courseId , cancellationToken);
        if (course == null) return PartialView("_Error404");

        course.IsDelete = true;
        await _transactions.SaveChangesAsync(cancellationToken);

        return Redirect("/Admin/ManageCourses/GetCourses/GetAllCourses");
    }

    #endregion


    #region RestoreCourse

    [Route("RestoreCourse/{courseId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> RestoreCourse(int courseId, CancellationToken cancellationToken)
    {
        var course = await _courseServices.GetCourseByIdAsync(courseId, cancellationToken);
        if (course == null) return PartialView("_Error404");

        course.IsDelete = false;
        await _transactions.SaveChangesAsync(cancellationToken);

        return Redirect("/Admin/ManageCourses/GetCourses/GetDeletedCourses");
    }

    #endregion
}
