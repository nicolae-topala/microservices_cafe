namespace Gateway.API.Settings;

public class GraphQlSettings
{
    public bool IsGenerateSchema { get; set; }
    public string PathToSchema { get; set; } = null!;
}
