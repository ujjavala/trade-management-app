using System.Text.Json.Serialization;

namespace TradeManagementApp.Domain.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TradeStatus
    {
        Placed,
        Executed,
        Expired
    }
}