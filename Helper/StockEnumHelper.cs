using WebXeDapAPI.Models.Enum;

namespace WebXeDapAPI.Utilities
{
    public static class StockEnumHelper
    {
        // Convert enum to string
        public static string EnumToString(InputStockStatus status)
        {
            return status.ToString();
        }

        // Convert string to enum with error handling
        public static InputStockStatus StringToEnum(string status)
        {
            return Enum.TryParse(status.ToUpper(), true, out InputStockStatus result) 
                ? result 
                : throw new ArgumentException($"Invalid value for InputStockStatus: {status}");
        }
    }
}
