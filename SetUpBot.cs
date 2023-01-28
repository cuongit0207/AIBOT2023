using System.Data;
using System.Globalization;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramFunctions
{
    public class SetUpBot
    {
        private readonly ILogger _logger;

        //public SetUpBot(ILoggerFactory loggerFactory)
        //{
        //    _logger = loggerFactory.CreateLogger<SetUpBot>();
        //}
        private readonly TelegramBotClient _botClient;

        public SetUpBot(ILoggerFactory loggerFactory)
        {
            //_botClient = new TelegramBotClient("5773662344:AAFntj8qjsRTha1E2OuWp2Lj4Tk0HzHebSc");//Dev Bot
            var botClient = new TelegramBotClient("5924715807:AAE5EskOWFH8ffNBeoG-1cl_QtfId-rBd3k");//Campuchia Bot
            _logger = loggerFactory.CreateLogger<SetUpBot>();
        }

        //[Function("SetUpBot")]
        //public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        //{
        //    _logger.LogInformation("C# HTTP trigger function processed a request.");

        //    var response = req.CreateResponse(HttpStatusCode.OK);
        //    response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        //    response.WriteString("Welcome to Azure Functions!");

        //    return response;
        //}
        private const string SetUpFunctionName = "setup";
        private const string UpdateFunctionName = "handleupdate";

        [Function(SetUpFunctionName)]
        public async Task RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            var handleUpdateFunctionUrl = req.Url.ToString().Replace(SetUpFunctionName, UpdateFunctionName,
                                                ignoreCase: true, culture: CultureInfo.InvariantCulture);//.Replace("http://localhost:7265", "https://6e35-2402-800-6232-bf22-9d5b-8c4c-3c45-8de0.ap.ngrok.io");
            await _botClient.SetWebhookAsync(handleUpdateFunctionUrl);
        }
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
        };

        [Function(UpdateFunctionName)]
        public async Task Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            var request = await req.ReadAsStringAsync();
            var update = JsonConvert.DeserializeObject<Telegram.Bot.Types.Update>(request);

            if (update.Type != UpdateType.Message)
                return;
            if (update.Message!.Type != MessageType.Text)
                return;
            await HandleUpdateAsync(update.Message.Text, update.Message.Chat.Id, update.Message);
            //await _botClient.SendTextMessageAsync(
            //chatId: update.Message.Chat.Id,
            //text: GetBotResponseForInput(update.Message.Text));

            //            using CancellationTokenSource cts = new();
            //            _botClient.StartReceiving(
            //    updateHandler: HandleUpdateAsync,
            //    pollingErrorHandler: HandlePollingErrorAsync,
            //    receiverOptions: receiverOptions,
            //    cancellationToken: cts.Token
            //);
        }
        async Task GetBotResponseForInputNew(string? text, long chatId)
        {
            using CancellationTokenSource cts = new();
            if (text.Equals("/start"))
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                {
            new KeyboardButton[] { "BACCARAT", "TÀI XỈU" },
        })
                {
                    ResizeKeyboard = true
                };

                Message sentMessage1 = await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Bạn chơi Baccarat hay Tài xỉu?",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cts.Token);
            }
        }
        private string GetBotResponseForInput(string text)
        {
            try
            {


                if (text.Contains("pod bay doors", StringComparison.InvariantCultureIgnoreCase))
                {
                    return "I'm sorry Dave, I'm afraid I can't do that ";
                }

                return new DataTable().Compute(text, null).ToString();
            }
            catch
            {
                return $"Dear human, I can solve math for you, try '2 + 2 * 3' ";
            }
        }

        async Task HandleUpdateAsync(string? messageText, long chatId, Message message)
        {
            using CancellationTokenSource cts = new();
            var cancellationToken = cts.Token;
            //var chatIdMaster = 5506051750;//daniel 2
            var chatIdMaster = 5771751358;//Campuchia tele
            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            var chatFullName = message.Chat.FirstName + " " + message.Chat.LastName;
            var tmm = await _botClient.SendTextMessageAsync(chatIdMaster, $"Khách nhắn: FullName: {chatFullName}, Tin nhắn: @{messageText}");//5771751358

            if (messageText.Equals("/start"))
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                {
            new KeyboardButton[] { "BACCARAT", "TÀI XỈU" },
        })
                {
                    ResizeKeyboard = true
                };

                Message sentMessage1 = await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Bạn chơi Baccarat hay Tài xỉu?",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);
            }
            else if (messageText.Equals("BACCARAT"))
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                {
            new KeyboardButton[] { "SEXY", "DG" },
        })
                {
                    ResizeKeyboard = true
                };

                Message sentMessage1 = await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Bạn chơi sảnh nào?",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);
            }
            else if (messageText.Equals("TÀI XỈU"))
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                {
            new KeyboardButton[] { "SẢNH SEXY", "SẢNH DG", "SẢNH EVOLUTION" },
        })
                {
                    ResizeKeyboard = true
                };

                Message sentMessage1 = await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Bạn chơi sảnh nào?",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);
            }
            else if (messageText.Equals("SEXY") || messageText.Equals("DG"))
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                    {
                new KeyboardButton[] { "01", "02","03", "04","05", "06","07", "08","09" },
            })
                {
                    ResizeKeyboard = true
                };

                Message sentMessage1 = await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Bạn chơi bàn nào?",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);
            }
            else if (messageText.Equals("SẢNH SEXY"))
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                    {
                new KeyboardButton[] { "TÀI XỈU 51" },
            })
                {
                    ResizeKeyboard = true
                };

                Message sentMessage1 = await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Bạn chơi bàn nào?",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);
            }
            else if (messageText.Equals("TÀI XỈU 51"))
            {
                var chatName = message.Chat.FirstName + " " + message.Chat.LastName;
                var t = await _botClient.SendTextMessageAsync(chatIdMaster, $"Thông tin chat: ChatId: {chatId} - FullName: {chatName}, UserName: @{message.Chat.Username} , Bàn: TÀI XỈU 51");//5503977113
                Message sentMessage = await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Vui lòng vào bàn chờ",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
            }
            else if (messageText.Equals("SẢNH DG"))
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                    {
                new KeyboardButton[] { "XÚC XẮC 22" },
            })
                {
                    ResizeKeyboard = true
                };

                Message sentMessage1 = await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Bạn chơi bàn nào?",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);
            }
            else if (messageText.Equals("XÚC XẮC 22"))
            {
                var chatName = message.Chat.FirstName + " " + message.Chat.LastName;
                var t = await _botClient.SendTextMessageAsync(chatIdMaster, $"Thông tin chat: ChatId: {chatId} - FullName: {chatName}, UserName: @{message.Chat.Username} , Bàn: XÚC XẮC 22");//5503977113
                Message sentMessage = await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Vui lòng vào bàn chờ",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
            }
            else if (messageText.Equals("SẢNH EVOLUTION"))
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                    {
                new KeyboardButton[] { "SIÊU TÀI XỈU" ,"HOÀNG ĐẾ TÀI XỈU"},
            })
                {
                    ResizeKeyboard = true
                };

                Message sentMessage1 = await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Bạn chơi bàn nào?",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);


            }
            else if (messageText.Equals("SIÊU TÀI XỈU"))
            {
                var chatName = message.Chat.FirstName + " " + message.Chat.LastName;
                var t = await _botClient.SendTextMessageAsync(chatIdMaster, $"Thông tin chat: ChatId: {chatId} - FullName: {chatName}, UserName: @{message.Chat.Username} , Bàn: SIÊU TÀI XỈU");//5503977113
                Message sentMessage = await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Vui lòng vào bàn chờ",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
            }
            else if (messageText.Equals("HOÀNG ĐẾ TÀI XỈU"))
            {
                var chatName = message.Chat.FirstName + " " + message.Chat.LastName;
                var t = await _botClient.SendTextMessageAsync(chatIdMaster, $"Thông tin chat: ChatId: {chatId} - FullName: {chatName}, UserName: @{message.Chat.Username} , Bàn: HOÀNG ĐẾ TÀI XỈU");//5503977113
                Message sentMessage = await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Vui lòng vào bàn chờ",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
            }
            else if (messageText.StartsWith("0"))
            {
                //#region Send ms
                //var ms = await botClient.SendTextMessageAsync(chatId, $"Boot tổng sẽ liên hệ với bạn");
                //#endregion
                var chatName = message.Chat.FirstName + " " + message.Chat.LastName;
                var t = await _botClient.SendTextMessageAsync(chatIdMaster, $"Thông tin chat: ChatId: {chatId} - FullName: {chatName}, UserName: @{message.Chat.Username}");
                Message sentMessage = await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Vui lòng vào bàn chờ",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
            }

        }

        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

    }
}
