﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UnAuthorizedException(string message = "Invaid Email or Password") : Exception(message)
    {
    }
}