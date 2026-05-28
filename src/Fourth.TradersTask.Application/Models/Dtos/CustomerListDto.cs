using Fourth.TradersTask.Domain;

namespace Fourth.TradersTask.Application.Models.Dtos;

public class CustomerListDto
{
    public List<Customer> Customers { get; set; }
    public int TotalCount { get; set; }
}
