﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_MiniProject.Exceptions
{
    public class InvalidOrderDetailException:Exception
    {
        public InvalidOrderDetailException(string message) : base(message) { }

    }
}
