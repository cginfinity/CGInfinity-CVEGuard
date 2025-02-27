using CVEGuard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVEGuard.Library.Contracts
{
    public interface ICVELoaderService
    {
        Task ExecuteTaskAsync(Action<List<Vulnerability>> save, int count = 0);
    }
}
