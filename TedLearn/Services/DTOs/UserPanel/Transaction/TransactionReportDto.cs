namespace Services.DTOs.UserPanel.Transaction;

public class TransactionReportDto : BaseDto<TransactionReportDto, Data.Entities.Persons.Transactions.Transaction>
{
    public int TypeId { get; set; }
    public decimal Amount { get; set; }
    public decimal Stock { get; set; }
    public DateTime CreateDate { get; set; }
    public string Description { get; set; } = string.Empty;

    public override void CustomMappings(IMappingExpression<Data.Entities.Persons.Transactions.Transaction, TransactionReportDto> mapping)
    {
        mapping.ForMember(dto => dto.CreateDate, opt => opt.MapFrom(t => t.TransactionDate));
        mapping.ForMember(dto => dto.Stock, opt => opt.Ignore());
    }
}
