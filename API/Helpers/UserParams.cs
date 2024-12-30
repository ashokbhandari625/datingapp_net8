namespace API.Helpers;

public class UserParams : PaginationParams
{
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;
    public int MinAge{ get; set; }  = 18;
    public int MaxAge { get; set; }    = 100 ; 

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    public string? Gender{get; set; }
    public string? CurrentUserName { get; set; }
    public string OrderBuy { get; set; } = "lastactive" ; 
}