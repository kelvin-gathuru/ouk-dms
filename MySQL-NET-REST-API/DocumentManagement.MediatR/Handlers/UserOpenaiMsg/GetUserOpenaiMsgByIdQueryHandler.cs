using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace DocumentManagement.MediatR.Handlers;
public class GetUserOpenaiMsgByIdQueryHandler(
    ILogger<GetUserOpenaiMsgByIdQueryHandler> logger,
    IUserOpenaiMsgRepository userOpenaiMsgRepository,
    IHttpClientFactory httpClientFactory,
    Helper.PathHelper pathHelper,
    UserInfoToken userInfoToken,
    IUnitOfWork<DocumentContext> _uow,
     IConnectionMappingRepository connectionMappingRepository,
     ICompanyProfileRepository companyProfileRepository,
    IHubContext<UserHub, IHubClient> hubContext) : IRequestHandler<GetUserOpenaiMsgByIdQuery, ServiceResponse<bool>>
{
    public async Task<ServiceResponse<bool>> Handle(GetUserOpenaiMsgByIdQuery request, CancellationToken cancellationToken)
    {

        var userOpenaiMsg = await userOpenaiMsgRepository.All.Where(c => c.Id == request.Id).FirstOrDefaultAsync();
        if (userOpenaiMsg == null)
        {
            return ServiceResponse<bool>.Return404("Open ai message not found");
        }

        // Use GEMINI_APIKEY from appsettings as the OpenRouter Key
        if (string.IsNullOrEmpty(pathHelper.GEMINI_APIKEY))
        {
            return ServiceResponse<bool>.ReturnFailed(404, "Configure Gemini APIKey in appsettings.json (GEMINI_APIKEY).");
        }

        var prompt = userOpenaiMsg.PromptInput;

        if (!string.IsNullOrWhiteSpace(userOpenaiMsg.Language))
        {
            prompt = $"{prompt} Language is {userOpenaiMsg.Language}.";
        }
        if (userOpenaiMsg.Creativity > 0)
        {
            prompt = $"{prompt} Creativity is {userOpenaiMsg.Creativity} between 0 and 1.";
        }

        if (userOpenaiMsg.MaximumLength > 0)
        {
            prompt = $"{prompt} Maximum {userOpenaiMsg.MaximumLength} words.";
        }
        if (!string.IsNullOrWhiteSpace(userOpenaiMsg.ToneOfVoice))
        {
            prompt = $"{prompt} Tone of voice must be {userOpenaiMsg.ToneOfVoice}.";
        }

        string response = await streamOpenRouterResponse(userOpenaiMsg.Id, prompt, userOpenaiMsg.SelectedModel, pathHelper.GEMINI_APIKEY);

        if (string.IsNullOrEmpty(response) || response.StartsWith("ERROR:"))
        {
            return ServiceResponse<bool>.ReturnFailed(404, response ?? "Error While Generating message");
        }
        userOpenaiMsg.AiResponse = response;
        userOpenaiMsgRepository.Update(userOpenaiMsg);
        
        var saveResult = await _uow.SaveAsync();
        if (saveResult <= 0)
        {
            return ServiceResponse<bool>.ReturnFailed(500, "Error While Added UserOpenaimsg");
        }
        
        // Detach the entity to ensure it's fully persisted and committed to the database
        _uow.Context.Entry(userOpenaiMsg).State = EntityState.Detached;

        return ServiceResponse<bool>.ReturnResultWith200(true);
    }

    public async Task<string> streamOpenRouterResponse(Guid msgId, string message, string model, string apiKey)
    {
        string finalResponse = string.Empty;
        string selectedModel = "google/gemini-2.5-flash"; // Default for OpenRouter
        try 
        {
            // Map frontend model selection to OpenRouter models if necessary
            if (!string.IsNullOrEmpty(model))
            {
                if (model.Contains("pro", StringComparison.OrdinalIgnoreCase))
                {
                    selectedModel = "google/gemini-2.5-pro";
                }
                else if (model.Contains("flash", StringComparison.OrdinalIgnoreCase))
                {
                    selectedModel = "google/gemini-2.5-flash";
                }
                else 
                {
                    selectedModel = model; // Use the value directly if it's already an OpenRouter identifier
                }
            }

            OpenAIClientOptions options = new OpenAIClientOptions { Endpoint = new Uri("https://openrouter.ai/api/v1") };
            ChatClient chatClient = new ChatClient(selectedModel, new ApiKeyCredential(apiKey), options);

            var chatOptions = new ChatCompletionOptions
            {
                MaxOutputTokenCount = 2048,
            };

            var responseStream = chatClient.CompleteChatStreamingAsync(new ChatMessage[] { ChatMessage.CreateUserMessage(message) }, chatOptions);

            await foreach (var chunk in responseStream)
            {
                if (chunk.ContentUpdate != null && chunk.ContentUpdate.Count > 0)
                {
                    foreach (var part in chunk.ContentUpdate)
                    {
                         var content = part.Text;
                         if (string.IsNullOrEmpty(content)) continue;

                         content = content.Replace("\r\n", "<br/>")
                                  .Replace("\r", "<br/>")
                                  .Replace("\n", "<br/>");

                        finalResponse += content;
                        
                        var userInfonew = connectionMappingRepository.GetUserInfoById(userInfoToken.Id);
                        if (userInfonew != null)
                        {
                            var connectionId = userInfonew.ConnectionId;
                            if (connectionId != null)
                            {
                                await hubContext.Clients.Client(connectionId).SendAiPromptResponse(msgId, content);
                            }
                        }
                    }
                }
            }
            
            var userInfo = connectionMappingRepository.GetUserInfoById(userInfoToken.Id);
            if (userInfo != null)
            {
                var connectionId = userInfo.ConnectionId;
                if (connectionId != null)
                {
                    await hubContext.Clients.Client(connectionId).SendAiPromptResponse(msgId, "[[DONE]]");
                }
            }
        }
        catch (Exception ex)
        {
             var logMsg = $"Error during OpenRouter streaming with model {selectedModel}: {ex.Message} | StackTrace: {ex.StackTrace}\n";
             System.IO.File.AppendAllText("/tmp/ai_error.txt", logMsg);
             Console.WriteLine($"Error during streaming: {ex.Message}");
             return $"ERROR: {ex.Message}";
        }

        return finalResponse;
    }
}
