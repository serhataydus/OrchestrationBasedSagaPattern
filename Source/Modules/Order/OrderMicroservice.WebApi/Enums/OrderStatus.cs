namespace OrderMicroservice.WebApi.Enums
{
    public enum OrderStatus : byte
    {
        None = 0,
        Suspend = 1,
        Complete = 2,
        Fail = 3
    }
}
