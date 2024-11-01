namespace WebXeDapAPI.Models.Enum
{
    public enum StatusDelivery
    {
        NewOrder = 900,
        AwaitingPickup = 901,
        OutForPickup = 902,
        PickedUp = 903,
        OutForDelivery = 904,
        Delivered = 905,
        DeliveryFailed = 906,
        Returning = 907,
        Returned = 908,
        Reconciled = 909,
        CustomerReconciled = 910,
        CODTransferredToCustomer = 911,
        CODAwaitingPayment = 912,
        Completed = 913,
        Cancelled = 914,
        Delayed = 915,
        PartialDelivery = 916,
        Error = 1000
    }

}
