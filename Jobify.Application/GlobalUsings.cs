// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Security.Claims;
global using AutoMapper;
global using AutoMapper.QueryableExtensions;
global using FluentValidation.Results;
global using Jobify.Application.Common.Exceptions;
global using Jobify.Application.Common.Interfaces;
global using Jobify.Application.Common.Interfaces.Data;
global using Jobify.Application.Common.Interfaces.Services;
global using Jobify.Application.Common.Models;
global using Jobify.Application.UseCases.Auths.AuthDtos;
global using Jobify.Domain.Entities;
global using MediatR;
global using Microsoft.EntityFrameworkCore;
