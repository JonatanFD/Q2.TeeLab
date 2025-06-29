namespace Q2.TeeLab.OrderProcessing.Domain.Model.Queries;

public record GetOrdersByDateRangeQuery(DateTime StartDate, DateTime EndDate);
