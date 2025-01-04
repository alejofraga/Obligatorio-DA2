namespace SmartHome.BusinessLogic.Args;
public class GetCompaniesArgs
{
    public int Offset { get; set; }

    public int Limit { get; set; }

    public string? CompanyName { get; set; }

    public string? OwnerFullname { get; set; }
}
