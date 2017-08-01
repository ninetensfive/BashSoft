using System;
using System.Collections.Generic;

namespace BashSoft.Contracts
{
    public interface ICourse
    {
        String Name { get; }

        IReadOnlyDictionary<string, IStudent> StudentsByName { get; }

        void EnrollStudent(IStudent student);
    }
}