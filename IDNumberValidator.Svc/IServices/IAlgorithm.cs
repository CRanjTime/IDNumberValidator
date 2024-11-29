using System;

namespace IDNumberValidator.Svc.IServices
{
    public interface IAlgorithm
    {
        TResult Perform<T, TResult>(T input, Func<T, TResult> algorithm);
        TResult Perform<T, TResult>(T input);
    }
}
