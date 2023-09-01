namespace Services.DTOs.Home.Order;

public class ShowBasketForUserDto : BaseDto<ShowBasketForUserDto , Data.Entities.Sales.Order>
{
    public int OrderId { get; set; }
    public decimal Discount { get; set; }
    public IEnumerable<DetailsBasketDto> OrderDetails { get; set; }
}
