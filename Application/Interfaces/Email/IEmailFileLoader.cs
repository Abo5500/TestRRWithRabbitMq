﻿using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Email
{
    public interface IEmailFileLoader
    {
        Task LoadFileAsync(EmailFileInfo fileInfo, string outputPath);
    }
}
