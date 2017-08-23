namespace BashSoft.Contracts
{
    using System;

    public interface IInterpreter
    {
        void InterpretCommand(string command);
    }
}