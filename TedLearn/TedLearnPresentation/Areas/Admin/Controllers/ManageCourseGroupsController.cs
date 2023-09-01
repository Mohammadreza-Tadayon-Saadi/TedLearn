using Data.Entities.Products.Courses;
using Microsoft.AspNetCore.Authorization;
using Services.DTOs.AdminPanel.Course.CourseGroup;

namespace TedLearnPresentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
[Route("/Admin/ManageCourseGroups")]
public class ManageCourseGroupsController : Controller
{
    #region ConstructorInjection

    private readonly ICourseGroupServices _courseGroupServices;
    private readonly ITransactionDbContextServices _transactions;
    public ManageCourseGroupsController(ICourseGroupServices courseGroupServices , ITransactionDbContextServices transactions)
    {
        _courseGroupServices = courseGroupServices;
        _transactions = transactions;
    }

    #endregion

    ///////////////////////

    #region GroupViews

    #region MainPage

    [Route("ShowGroups")]
    public IActionResult ShowGroups()
    {
        return View();
    }

    [Route("/Admin/ManageCourseGroups/GetGroups/GetAllGroups")]
    [Route("/Admin/ManageCourseGroups/GetGroups/GetAllDeletedGroups")]
    public async Task<IActionResult> GetGroups(CancellationToken cancellationToken)
    {
        IEnumerable<ShowCourseGroupDto> groups;
        var routePattern = HttpContext.Request.Path.Value;

        if (routePattern.Contains("GetAllGroups"))
            groups = await _courseGroupServices.GetCourseGroupsAsync(cancellationToken, isDeleted: false);
        else
            groups = await _courseGroupServices.GetCourseGroupsAsync(cancellationToken, isDeleted: true);

        return PartialView("GetGroupsAjax", groups);
    }

    #endregion


    #region AddGroup

    [Route("AddGroup")]
    public IActionResult AddGroup()
    {
        return View();
    }

    [Route("AddGroup")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> AddGroup(AddGroupDto model , CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        if (await _courseGroupServices.IsCourseGroupExistAsync(model.Title.Trim() , cancellationToken))
        {
            ModelState.AddModelError(nameof(model.Title), "گروه و یا زیر گروهی با این نام وجود دارد ، لطفا نام دیگری انتخاب کنید.");
            return View(model);
        }

        var courseGroup = new CourseGroup{ Title = model.Title, SubGroupId = 1 };

        await _courseGroupServices.AddCourseGroupAsync(courseGroup, cancellationToken);

        return Redirect("/Admin/ManageCourseGroups/ShowGroups");
    }

    #endregion


    #region EditGroup

    [Route("EditGroup/{groupId:int}")]
    public async Task<IActionResult> EditGroup(int groupId , CancellationToken cancellationToken)
    {
        var group = await _courseGroupServices.GetGroupForEditAsync(groupId , cancellationToken);
        if (group == null) return NotFound();

        if (TempData.ContainsKey("ConcurrencyInEditGroup"))
        {
            ViewBag.ConCurrency = TempData["ConcurrencyInEditGroup"];
            TempData.Remove("ConcurrencyInEditGroup");
        }

        return View(group);
    }

    [Route("EditGroup")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> EditGroup(EditGroupDto model, string preTitle, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        var group = await _courseGroupServices.GetCourseGroupByGroupIdAsync(model.GroupId , cancellationToken);
        if (group == null) return NotFound();

        //ConCurrency Check
        if (Convert.ToBase64String(group.Version) != model.Base64Version)
        {
            TempData["ConcurrencyInEditGroup"] = true;
            return Redirect($"/Admin/ManageCourseGroups/EditGroup/{model.GroupId}");
        }

        #region ValidationInput

        //Group Not Changed Because Just Title Can Edit By User
        if (model.Title.Trim() == preTitle)
            return Redirect("/Admin/ManageCourseGroups/ShowGroups");


        if (await _courseGroupServices.IsCourseGroupExistAsync(model.Title.Trim() , cancellationToken))
        {
            ModelState.AddModelError(nameof(model.Title), "گروه و یا زیر گروهی با این نام وجود دارد ، لطفا نام دیگری انتخاب کنید.");
            return View(model);
        }

        #endregion

        group.Title = model.Title;
        await _transactions.SaveChangesAsync(cancellationToken);

        return Redirect("/Admin/ManageCourseGroups/ShowGroups");
    }

    #endregion

    #endregion

    ///////////////////////

    #region SubGroupViews

    #region MainPage

    [Route("ShowSubGroups/{ParentId:int}")]
    public async Task<IActionResult> ShowSubGroups(int ParentId , CancellationToken cancellationToken)
    {
        if (!await _courseGroupServices.IsCourseGroupExistAsync(ParentId , cancellationToken)) return NotFound();

        ViewBag.ParentId = ParentId;

        return View();
    }

    [Route("GetSubGroups/GetAllSubGroupsForGroup/{groupId:int}")]
    [Route("GetSubGroups/GetAllDeletedSubGroupsForGroup/{groupId:int}")]
    public async Task<IActionResult> GetSubGroups(int groupId, CancellationToken cancellationToken)
    {
        IEnumerable<ShowCourseGroupDto> subGroup;
        var routePattern = HttpContext.Request.Path.Value;

        if (routePattern.Contains("GetAllSubGroupsForGroup"))
            subGroup = await _courseGroupServices.GetAllSubGroupsForGroupAsync(groupId, cancellationToken, isDeleted: false);
        else
            subGroup = await _courseGroupServices.GetAllSubGroupsForGroupAsync(groupId, cancellationToken, isDeleted: true);

        return PartialView("GetSubGroupsAjax", subGroup);
    }

    #endregion


    #region AddSubGroup

    [Route("AddSubGroup")]
    public async Task<IActionResult> AddSubGroup(CancellationToken cancellationToken)
    {
        var model = new AddSubGroupDto
        {
            GroupList = await _courseGroupServices.GetGroupListAsync(cancellationToken),
        };

        return View(model);
    }

    [Route("AddSubGroup")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> AddSubGroup(AddSubGroupDto model , CancellationToken cancellationToken)
    {
        #region ValidationInputs

        if (!ModelState.IsValid)
        {
            model.GroupList = await _courseGroupServices.GetGroupListAsync(cancellationToken);
            return View(model);
        }

        //Check Title IF Exist
        if (await _courseGroupServices.IsCourseGroupExistAsync(model.Title.Trim() , cancellationToken))
        {
            ModelState.AddModelError(nameof(model.Title), "گروه و یا زیر گروهی با این نام وجود دارد ، لطفا نام دیگری انتخاب کنید.");
            model.GroupList = await _courseGroupServices.GetGroupListAsync(cancellationToken);

            return View(model);
        }

        #endregion


        var courseGroup = new CourseGroup
        {
            Title = model.Title,
            SubGroupId = model.ParentId,
        };
        await _courseGroupServices.AddCourseGroupAsync(courseGroup, cancellationToken);

        return Redirect($"/Admin/ManageCourseGroups/ShowSubGroups/{model.ParentId}");
    }

    #endregion


    #region EditSubGroup

    [Route("EditSubGroup/{groupId:int}")]
    public async Task<IActionResult> EditSubGroup(int groupId , CancellationToken cancellationToken)
    {
        var subGroup = await _courseGroupServices.GetCourseGroupByGroupIdAsync(groupId, cancellationToken , withTracking:false);
        if (subGroup == null) return NotFound();

        var model = EditSubGroupDto.FromEntity(subGroup);
        model.GroupList = await _courseGroupServices.GetGroupListAsync(cancellationToken);

        if (TempData.ContainsKey("ConcurrencyInEditSubGroup"))
        {
            ViewBag.ConCurrency = TempData["ConcurrencyInEditSubGroup"];
            TempData.Remove("ConcurrencyInEditSubGroup");
        }

        return View(model);
    }

    [Route("EditSubGroup")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> EditSubGroup(EditSubGroupDto model, string preTitle , CancellationToken cancellationToken)
    {
        #region ValidationInputs

        if (!ModelState.IsValid)
        {
            model.GroupList = await _courseGroupServices.GetGroupListAsync(cancellationToken , parentId:model.ParentId);
            return View(model);
        }

        var courseGroup = await _courseGroupServices.GetCourseGroupByGroupIdAsync(model.GroupId , cancellationToken);
        if (courseGroup == null) return NotFound();

        //ConCurrency Check
        if (Convert.ToBase64String(courseGroup.Version) != model.Base64Version)
        {
            TempData["ConcurrencyInEditSubGroup"] = true;
            return Redirect($"/Admin/ManageCourseGroups/EditSubGroup/{model.GroupId}");
        }


        //Field Not Changed
        if (model.Title.Trim() == preTitle)
            return Redirect($"/Admin/ManageCourseGroups/ShowSubGroups/{model.ParentId}");

        // Duplicate Title Check
        if (await _courseGroupServices.IsCourseGroupExistAsync(model.Title.Trim() , cancellationToken))
        {
            ModelState.AddModelError(nameof(model.Title), "گروه و یا زیر گروهی با این نام وجود دارد ، لطفا نام دیگری انتخاب کنید.");
            model.GroupList = await _courseGroupServices.GetGroupListAsync(cancellationToken, parentId: model.ParentId);

            return View(model);
        }

        #endregion

        model.ToEntity(courseGroup);
        await _transactions.SaveChangesAsync(cancellationToken);

        return Redirect($"/Admin/ManageCourseGroups/ShowSubGroups/{model.ParentId}");
    }

    #endregion


    #endregion

    ///////////////////////

    #region DeleteCourseGroup

    [Route("DeleteCourseGroup/{groupId}/{parentId}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> DeleteCourseGroup(int groupId, int parentId , CancellationToken cancellationToken)
    {
        var group = await _courseGroupServices.GetCourseGroupByGroupIdAsync(groupId, cancellationToken, getActive: true);
        if (group == null) return NotFound();

        group.IsDelete = true;
        await _transactions.SaveChangesAsync(cancellationToken);

        return Redirect((parentId == 1) ? "/Admin/ManageCourseGroups/GetGroups/GetAllGroups" : $"/Admin/ManageCourseGroups/GetSubGroups/GetAllSubGroupsForGroup/{parentId}");
    }

    #endregion

    ///////////////////////

    #region RestoreCourseGroup

    [Route("RestoreGroup/{groupId}/{parentId:int}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> RestoreCourseGroup(int groupId, int parentId , CancellationToken cancellationToken)
    {
        var group = await _courseGroupServices.GetCourseGroupByGroupIdAsync(groupId, cancellationToken , getActive: false);
        if (group == null) return NotFound();

        group.IsDelete = false;
        await _transactions.SaveChangesAsync(cancellationToken);

        return Redirect((parentId == 1) ? "/Admin/ManageCourseGroups/GetGroups/GetAllDeletedGroups" : $"/Admin/ManageCourseGroups/GetSubGroups/GetAllDeletedSubGroupsForGroup/{parentId}");
    }

    #endregion
}
