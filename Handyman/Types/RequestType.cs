﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Handyman.Types
{
    /// <summary>
    /// Represents the a request type.
    /// </summary>
    public class RequestType : MemberedBaseType
    {
        public RequestType(string name, IEnumerable<Member> members, string documentation)
            : base(name, "IRequest", members, documentation)
        {
        }
    }
}