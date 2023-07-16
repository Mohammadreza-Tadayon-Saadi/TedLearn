﻿namespace Services.DTOs.UserPanel;

public class TransactionReportsWithPaginationDto
{
    public List<TransactionReportDto> TransactionReports { get; set; }
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }
}
