﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_MiniProject.Exceptions
{
    public class InvalidUserInformationException : Exception
    {

        public InvalidUserInformationException(string message) : base(message) { }

    }

}
