using System;

namespace BashSoft.Contracts
{
    public interface IInterpreter
    {
        void InterpretCommand(String command);
    }
}