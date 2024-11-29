using IDNumberValidator.Svc.Model;
using System.Threading;
using System.Threading.Tasks;

namespace IDNumberValidator.Svc.IServices
{
    public interface IIdNumberValidatorService
    {
        Task<bool> ValidateIdNumber(IdNumberValidationRequest request, CancellationToken ct);
    }
}
