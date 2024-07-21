namespace Clicker.BL.Abstractions;

public interface IEnergyRefillService
{
    Task RefillEnergyAsync(CancellationToken cancellationToken);
}