using Egapay.Customer.Business.Gateway.Common.Resources;
using Microsoft.Extensions.Localization;
using OmniDataAccess.NoSqlDatabases.Interfaces;

namespace Egapay.Customer.Business.Gateway.API.Repositories;

public class RepositoryBase
{
    internal IStringLocalizer<SuccessMessages> _successLocalizer;
    internal IStringLocalizer<ErrorMessages> _errorLocalizer;
    internal ICacheManager _redisManager;
    internal string _culture;
}