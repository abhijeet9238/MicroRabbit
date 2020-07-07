using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Transfer.Application.Models
{
    public class AccountTransfer
    {
        public int AccountSource { get; set; }
        public int AccountTarget { get; set; }
        public decimal TransferAmount  { get; set; }
    }
}
