using Clinic.Core.Domain;

namespace Clinic.Core.Interfaces.Helpers;

public interface ITokenHelper
{
    string Create(User user);
}
