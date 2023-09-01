namespace Services.DTOs.UserPanel.Order;

public class ShowOrdersInUserPanelDto : BaseDto<ShowOrdersInUserPanelDto , Data.Entities.Sales.Order>
{
    public int OrderId { get; set; }
    public bool IsFinaly { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal Discount { get; set; }
    public IEnumerable<GetOrderDetailsForUserPanelDto> OrderDetails { get; set; }
}
