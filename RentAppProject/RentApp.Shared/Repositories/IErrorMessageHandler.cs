using System;
namespace RentApp
{
    public interface IErrorMessageHandler
    {
        Action DisplayLoadErrorMessage();
    }
}
