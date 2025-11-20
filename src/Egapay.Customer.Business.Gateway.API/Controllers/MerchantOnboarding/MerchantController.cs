using System.Net.Mime;
using Egapay.Customer.Business.Gateway.API.Middlewares;
using Egapay.Customer.Business.Gateway.Common.ApiResponses;
using Egapay.Customer.Business.Gateway.Common.Configs;
using Egapay.Customer.Business.Gateway.Common.Controllers;
using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Services;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Egapay.Customer.Business.Gateway.API.Controllers.MerchantOnboarding;

[MerchantOnboardingAuthentication]
[ApiController]
[Route("api/v1/onboarding/merchants")]
public class MerchantController:EganowControllerBase
{
    private readonly ILogger<MerchantController> _logger;
    private readonly MerchantService _merchantService;
    public MerchantController(ILogger<MerchantController> logger,IOptions<ApplicationConfig> options, 
        IStringLocalizer<SuccessMessages> successLocalizer,
        IStringLocalizer<ErrorMessages> errorLocalizer,IStringLocalizer<ValidationMessages> validationLocalizer,MerchantService merchantService)
    {
        _logger = logger;
        _config = options.Value;
        _successLocalizer = successLocalizer;
        _errorLocalizer = errorLocalizer;
        _validationLocalizer = validationLocalizer;
        _merchantService = merchantService;
    }
    
    [HttpPost]
    [Route("check-account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> CheckAccount(CheckMerchantRequest? request)
    {
        try
        {
            //validate fields
            using (var validator = new CheckMerchantAccountRequestValidator(_validationLocalizer))
            {
                if (request is null || string.IsNullOrEmpty(request.Email))
                {
                    request = new CheckMerchantRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(CheckAccount)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new BadRequestResponse<Dictionary<string,object>>(_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.MerchantAccountExistAsync(request,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();
            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(CheckAccount)} exception: {ex.Message}");
            return StatusCode(500,new ApiResponse<string>(500,ex.Message));
        }
    }
    
    
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Register(CreateMerchantRequest? request)
    {
        try
        {
            //validate fields
            using (var validator = new CreateMerchantRequestValidator(_validationLocalizer))
            {
                if (request is null || string.IsNullOrWhiteSpace(request.Otp))
                {
                    request = new CreateMerchantRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(Register)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new BadRequestResponse<Dictionary<string,object>>(_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.RegisterMerchantAsync(request,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();
            
            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(Register)} exception: {ex.Message}");
            return StatusCode(500,new ApiResponse<string>(500,_errorLocalizer["DefaultError"]));
        }
    }
    
    //====================== Business Info =========================
    [HttpGet]
    [Route("accounts/info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetBusinessInformation()
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.GetMerchantBusinessInfoAsync(GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetBusinessInformation)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["GetMerchantBusinessInfoFailed"]));
        }
    }
    
    [HttpPut]
    [Route("accounts/info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> UpdateBusinessInfo(UpdateMerchantBusinessInfoRequest? request)
    {
        try
        {
            //validate fields
            using (var validator = new UpdateMerchantBusinessInfoRequestValidator(_validationLocalizer))
            {
                if (request is null)
                {
                    request = new UpdateMerchantBusinessInfoRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(Register)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new BadRequestResponse<Dictionary<string,object>>(_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.UpdateMerchantBusinessInfoAsync(request,GetRequestHeaders(),cts.Token);
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateBusinessInfo)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["UpdateMerchantBusinessInfoFailed"]));
        }
    }
    
    //======================END: Business Info =========================
    
    //====================== Contact Info =========================
    [HttpPut]
    [Route("accounts/contact-info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> UpdateBusinessContactInfo(UpdateBusinessContactRequest? request)
    {
        try
        {
            //validate fields
            using (var validator = new UpdateBusinessContactRequestValidator(_validationLocalizer))
            {
                if (request is null || string.IsNullOrWhiteSpace(request.OfficeMobileNumber))
                {
                    request = new UpdateBusinessContactRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(Register)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new BadRequestResponse<Dictionary<string,object>>(_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
     
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.UpdateMerchantBusinessContactInfoAsync(request,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }
            
            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateBusinessContactInfo)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["UpdateMerchantContactInfoFailed"]));
        }
    }
    
    [HttpGet]
    [Route("accounts/contact-info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetBusinessContactInfo()
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.GetMerchantBusinessContactInfoAsync(GetRequestHeaders(),cts.Token);
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }
            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetBusinessContactInfo)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["GetMerchantBusinessContactInfoFailed"]));
        }
    }
    
    //====================== END: Contact Info =========================
    
    //====================== Contact Person =========================
    [HttpGet]
    [Route("accounts/contact-persons")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetAllContactPersons()
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.GetAllContactPersonAsync(GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAllContactPersons)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["GetContactPersonFailed"]));
        }
    }
    
    [HttpPost]
    [Route("accounts/contact-persons")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> AddNewContactPerson(AddBusinessContactPersonRequest? request)
    {
        try
        {
            //validate fields
            using (var validator = new AddBusinessContactPersonRequestValidator(_validationLocalizer))
            {
                if (request is null || string.IsNullOrWhiteSpace(request.Email))
                {
                    request = new AddBusinessContactPersonRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(Register)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new BadRequestResponse<Dictionary<string,object>>(_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.AddContactPersonAsync(request,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(AddNewContactPerson)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["AddContactPersonFailed"]));
        }
    }
    
    [HttpPut]
    [Route("accounts/contact-persons")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> UpdateContactPerson(UpdateBusinessContactPersonRequest? request)
    {
        try
        {
            //validate fields
            using (var validator = new UpdateBusinessContactPersonRequestValidator(_validationLocalizer))
            {
                if (request is null)
                {
                    request = new UpdateBusinessContactPersonRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(Register)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new BadRequestResponse<Dictionary<string,object>>(_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.UpdateContactPersonAsync(request,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateContactPerson)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["UpdateContactPersonFailed"]));
        }
    }
    
    [HttpDelete]
    [Route("accounts/contact-persons/{contactId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> DeleteContactPerson(string? contactId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(contactId))
            {
                return StatusCode(400,new BadRequestResponse<object>(_errorLocalizer["DeleteContactPersonFailed"]));
            }
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.DeleteContactPersonAsync(contactId,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateContactPerson)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["DeleteContactPersonFailed"]));
        }
    }
    
    //======================END: Contact Person =========================
    
    
    //======================Director ShareHolder =========================
    [HttpGet]
    [Route("accounts/director-shareholders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetAllDirectorOrShareholder()
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.GetAllDirectorOrShareholdersAsync(GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }
            
            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAllDirectorOrShareholder)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["GetAllBusinessDirectorOrShareholderFailed"]));
        }
    }
    
    [HttpPost]
    [Route("accounts/director-shareholders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddDirectorOrShareholder([FromForm]AddBusinessDirectorOrShareholderRequest? request)
    {
        try
        {
            //validate fields
            using (var validator = new AddBusinessDirectorOrShareholderRequestValidator(_validationLocalizer))
            {
                if (request is null)
                {
                    request = new AddBusinessDirectorOrShareholderRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(Register)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new BadRequestResponse<Dictionary<string,object>>(_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.AddDirectorOrShareholderAsync(request,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }
            
            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(AddDirectorOrShareholder)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["AddBusinessDirectorOrShareholderFailed"]));
        }
    }
    
    [HttpPut]
    [Route("accounts/director-shareholders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDirectorOrShareholder([FromForm]UpdateBusinessDirectorOrShareholderRequest? request)
    {
        try
        {
            //validate fields
            using (var validator = new UpdateBusinessDirectorOrShareholderRequestValidator(_validationLocalizer))
            {
                if (request is null)
                {
                    request = new UpdateBusinessDirectorOrShareholderRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(Register)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new BadRequestResponse<Dictionary<string,object>>(_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.UpdateDirectorOrShareholderAsync(request,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateDirectorOrShareholder)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["UpdateBusinessDirectorOrShareholderFailed"]));
        }
    }

    
    [HttpDelete]
    [Route("accounts/director-shareholders/{directorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> DeleteDirectorOrShareholder(string? directorId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(directorId))
            {
                return StatusCode(400,new BadRequestResponse<object>("directorId not provided"));
            }
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.DeleteDirectorOrShareholdersAsync(directorId,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(DeleteDirectorOrShareholder)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["DeleteBusinessDirectorOrShareholderFailed"]));
        }
    }
    //======================END: Director ShareHolder =========================
    
    //======================Documents =========================
    [HttpGet]
    [Route("accounts/documents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetAllDocuments()
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.GetAllDocumentsAsync(GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAllDocuments)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["GetAllBusinessDocumentFailed"]));
        }
    }
    
    [HttpPost]
    [Route("accounts/documents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddNewDocument([FromForm]AddBusinessDocumentRequest? request)
    {
        try
        {
            //validate fields
            using (var validator = new AddBusinessDocumentValidator(_validationLocalizer))
            {
                if (request is null)
                {
                    request = new AddBusinessDocumentRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(AddNewDocument)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new BadRequestResponse<Dictionary<string,object>>(_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.AddDocumentAsync(request,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }
            
            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(AddNewDocument)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["AddBusinessDocumentFailed"]));
        }
    }
    
    [HttpPut]
    [Route("accounts/documents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddNewDocument([FromForm]UpdateBusinessDocumentRequest? request)
    {
        try
        {
            //validate fields
            using (var validator = new UpdateBusinessDocumentValidator(_validationLocalizer))
            {
                if (request is null)
                {
                    request = new UpdateBusinessDocumentRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(AddNewDocument)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new BadRequestResponse<Dictionary<string,object>>(_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.UpdateDocumentAsync(request,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(AddNewDocument)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["UpdateBusinessDocumentFailed"]));
        }
    }
    
    [HttpDelete]
    [Route("accounts/documents/{documentId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> DeleteDocument(string? documentId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(documentId))
            {
                return StatusCode(400,new BadRequestResponse<object>("documentId not provided"));
            }
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.DeleteDocumentAsync(documentId,GetRequestHeaders(),cts.Token);

            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(DeleteDirectorOrShareholder)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["DeleteBusinessDocumentFailed"]));
        }
    }
    //======================END: Documents =========================
    
    //============ MISC Merchant Endpoints
    [HttpGet]
    [Route("active-regulators")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetActiveRegulators()
    {
        try
        {
            //extract jwt from Bearer token
            var authHeaders = GetRequestHeaders();

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.GetAllActiveRegulatorsAsync(GetRequestHeaders(),cts.Token);
            

            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body); 


        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetActiveRegulators)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["GetActiveRegulatorsFailed"]));
        }
    }
    
    [HttpGet]
    [Route("active-industries")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetActiveIndustries()
    {
        try
        {
            //extract jwt from Bearer token
            var authHeaders = GetRequestHeaders();

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.GetAllActiveIndustriesAsync(GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body); 

        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetActiveIndustries)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, _errorLocalizer["GetActiveIndustriesFailed"]));
        }
    }
    
    [HttpPost]
    [Route("profile/upload-image")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadMerchantProfileImage([FromForm] UpdateMerchantProfileImageRequest? request)
    {
        try
        {
            //extract jwt from Bearer token
            var authHeaders = GetRequestHeaders();
            //validate fields
            using (var validator = new UpdateMerchantProfileImageValidator(_validationLocalizer))
            {
                if (request is null)
                {
                    request = new UpdateMerchantProfileImageRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(UploadMerchantProfileImage)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new BadRequestResponse<Dictionary<string,object>>(_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
            
      
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.UpdateMerchantProfileImageAsync(request,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }


            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UploadMerchantProfileImage)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, "failed to update profile image. try again later."));
        }
    }
    //============ END: MISC Merchant Endpoints
    
    
    [HttpGet]
    [Route("api-keys")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetGeneratedApiKeyForMerchant([FromQuery]string service)
    {
        try
        {
            var authHeaders = GetRequestHeaders();

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.GetGeneratedApiKeyForMerchantAsync(service,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }


            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetGeneratedApiKeyForMerchant)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, "failed to get api keys. try again later"));
        }
    }
    
    [HttpPost]
    [Route("api-keys")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GenerateApiKeyForMerchant([FromQuery]string service)
    {
        try
        {
            var authHeaders = GetRequestHeaders();

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.GenerateApiKeyForMerchantAsync(service,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }


            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetActiveIndustries)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, "failed to generate api keys. try again later"));
        }
    }
    
    [HttpGet]
    [Route("callback-urls")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetCallbackUrlForMerchant([FromQuery]string service)
    {
        try
        {
            var authHeaders = GetRequestHeaders();

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.GetCallbackUrlForMerchantAsync(service,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }


            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetCallbackUrlForMerchant)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, "failed to get api keys. try again later"));
        }
    }
    
    [HttpPost]
    [Route("callback-urls")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> AddCallbackUrlForMerchant(MerchantCallbackRequest request)
    {
        try
        {
            var authHeaders = GetRequestHeaders();

            if (request == null)
            {
                request = new MerchantCallbackRequest();
            }
            
            request!.CollectionCallbackUrl = request?.CollectionCallbackUrl.Trim();
            request!.PayoutCallbackUrl = request?.PayoutCallbackUrl.Trim();
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.AddCallbackUrlForMerchantAsync(request,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }


            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(AddCallbackUrlForMerchant)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, "failed to generate api keys. try again later"));
        }
    }
}