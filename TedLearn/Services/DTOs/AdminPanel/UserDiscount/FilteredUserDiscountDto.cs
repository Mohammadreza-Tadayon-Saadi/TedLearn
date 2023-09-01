namespace Services.DTOs.AdminPanel.UserDiscount;

public class FilteredUserDiscountDto
{
    public PaginantionDto Paginantion { get; set; }
    public IEnumerable<UserDiscountDto> UserDiscounts { get; set; }
}
