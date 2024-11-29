using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDNumberValidator.Svc.IServices
{
    public interface IAlgorithm
    {
        TResult Perform<T, TResult>(T input, Func<T, TResult> algorithm);
        TResult Perform<T, TResult>(T input);
    }
}
