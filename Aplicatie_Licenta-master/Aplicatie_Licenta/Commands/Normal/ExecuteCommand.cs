using System;

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
