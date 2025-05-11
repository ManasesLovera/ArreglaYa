using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum TransactionStatus
    {
        Pending,
        Completed,
        Cancelled,
        Failed,
        Refunded
    }
}
