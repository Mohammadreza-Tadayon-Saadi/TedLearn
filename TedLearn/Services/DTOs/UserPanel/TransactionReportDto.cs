using Data.Entities.Persons.Transactions;

namespace Services.DTOs.UserPanel;

public class TransactionReportDto : BaseDto<TransactionReportDto , Transaction>
{
    public int TypeId { get; set; }
    public decimal Amount { get; set; }
    public decimal Stock { get; set; }
    public DateTime CreateDate { get; set; }
    public string Description { get; set; } = string.Empty;

    public override void CustomMappings(IMappingExpression<Transaction, TransactionReportDto> mapping)
    {
        mapping.ForMember(dto => dto.CreateDate , opt => opt.MapFrom(t => t.TransactionDate));
        mapping.ForMember(dto => dto.Stock , opt => opt.Ignore());
    }
}
