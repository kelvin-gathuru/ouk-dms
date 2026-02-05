using System;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI;
using OpenAI.Chat;

namespace DocumentManagement.MediatR.Handlers;
public class AddUpdateOpenAIApiKeyCommandHandler(ILogger<AddUpdateOpenAIApiKeyCommandHandler> logger, ICompanyProfileRepository companyProfileRepository, IUnitOfWork<DocumentContext> _uow) : IRequestHandler<AddUpdateOpenAIApiKeyCommand, ServiceResponse<bool>>
{
    public async Task<ServiceResponse<bool>> Handle(AddUpdateOpenAIApiKeyCommand request, CancellationToken cancellationToken)
    {
        // Fetch the first (and typically only) company profile
        var companyProfile = await companyProfileRepository
              .All
              .FirstOrDefaultAsync(cancellationToken);

        if (companyProfile == null)
        {
            return ServiceResponse<bool>.Return404("No company profile found.");
        }

        if (request.Company == "openai" && !string.IsNullOrEmpty(request.OpenAIAPIKey) && !TestOpenAIAPIKey(request.OpenAIAPIKey))
        {
            return ServiceResponse<bool>.Return404("OPENAI API Key is not correct.");
        }

        if (request.Company == "gemini" && !string.IsNullOrEmpty(request.OpenAIAPIKey) && !await TestGeminiKey(request.OpenAIAPIKey))
        {
            return ServiceResponse<bool>.Return404("Gemini API Key is not correct.");
        }

        if (request.Company == "openai")
        {
            companyProfile.OpenAIAPIKey = request.OpenAIAPIKey;
        }
        if (request.Company == "gemini")
        {
            companyProfile.GeminiAPIKey = request.OpenAIAPIKey;
        }
        companyProfileRepository.Update(companyProfile);
        if (await _uow.SaveAsync() <= -1)
        {
            return ServiceResponse<bool>.Return500();
        }
        return ServiceResponse<bool>.ReturnResultWith200(true);
    }

    private bool TestOpenAIAPIKey(string apiKey)
    {
        ChatClient client = new(model: "gpt-3.5-turbo", apiKey: apiKey);
        try
        {
            var result = client.CompleteChat("What's 2 + 2?");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return false;
        }
    }

    private async Task<bool> TestGeminiKey(string apiKey)
    {

        var googleAI = new GoogleAI(apiKey: apiKey);
        var geminiModel = "gemini-2.5-flash-lite";
        var generativeModel = googleAI.GenerativeModel(geminiModel);

        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            // 🔹 Small document → single API call
            string prompt = $"\"What's 2 + 2?\"";
            var response = await generativeModel.GenerateContent(prompt, cancellationToken: cts.Token);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return false;
        }
    }
}
