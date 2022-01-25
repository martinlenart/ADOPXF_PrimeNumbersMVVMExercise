using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using PrimeNumbers.Models;

namespace PrimeNumbers.Services
{
    //IPrimeNumerService is used only for DI
    public interface IPrimeNumerService
    {
        Task<List<PrimeBatch>> GetPrimeBatchCountsAsync(int NrOfBatches);
        Task<List<PrimeBatch>> GetPrimeBatchCountsAsync(int NrOfBatches, IProgress<float> onProgressReporting);

        Task DisplayPrimeCountsAsync(int NrOfBatches, IProgress<(float, string)> onProgressReporting);

        Task<int> GetPrimesCountAsync(int start, int count);
        Task<List<int>> GetPrimesAsync(int start, int count);
    }
}
