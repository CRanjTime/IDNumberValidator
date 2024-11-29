using IDNumberValidator.Svc.Model;

namespace IDNumberValidator.Svc.IServices
{
    public interface IIdNumberValidatorService
    {
        Task<bool> ValidateIdNumber(IdNumberValidationRequest request, CancellationToken ct);
    }
}
