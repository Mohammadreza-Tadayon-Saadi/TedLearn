
using Data.Entities.Persons.Transactions;
using Services.DTOs.Common;

namespace Services.DTOs.UserPanel;

public class TransactionReportDto : BaseDto<TransactionReportDto , Transaction>
{
    [Display(Name = "نوع تراکنش")]
    public int TypeId { get; set; }

    [Display(Name = "مبلغ")]
    public decimal Amount { get; set; }

    [Display(Name = "موجودی")]
    public decimal Stock { get; set; }
    public DateTime CreateDate { get; set; }

    [Display(Name = "نوع تراکنش")]
    public string Description { get; set; } = string.Empty;

    public override void CustomMappings(IMappingExpression<Transaction, TransactionReportDto> mapping)
    {
        mapping.ForMember(dto => dto.CreateDate , opt => opt.MapFrom(t => t.TransactionDate));
        mapping.ForMember(dto => dto.Stock , opt => opt.Ignore());
    }
}
