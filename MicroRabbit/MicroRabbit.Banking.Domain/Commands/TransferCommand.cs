using MicroRabbit.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Banking.Domain.Commands
{
    public abstract class TransferCommand:Command
    {
        public int ToAccount { get; protected set; }
        public int FromAccount  { get;protected set; }
        public decimal Amount { get;protected set; }
    }
}
