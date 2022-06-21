using Aplicatie_Licenta.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Commands
{
    internal class ExecuteCommand : CommandBase
    {
        private readonly Action execFunc;

        public ExecuteCommand(Action execFunc)
        {
            this.execFunc = execFunc;
        }

        public override void Execute(object? parameter)
        {
            execFunc();
        }
    }
}
