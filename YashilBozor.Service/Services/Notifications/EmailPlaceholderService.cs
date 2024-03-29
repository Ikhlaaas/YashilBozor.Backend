﻿using System.Data;
using System.Text;
using YashilBozor.Domain.Entities.Notification;
using YashilBozor.Service.Exceptions;
using YashilBozor.Service.Interfaces.Identity;
using YashilBozor.Service.Interfaces.Notifications.Services;

namespace YashilBozor.Service.Services.Notifications;

public class EmailPlaceholderService : IEmailPlaceholderService
{
    private readonly IUserService _userService;
    private const string _fullName = "{{FullName}}";
    private const string _firstName = "{{FirstName}}";
    private const string _lastName = "{{LastName}}";
    private const string _email = "{{EmailAddress}}";
    private const string _date = "{{Date}}";
    private const string _companyName = "{{CompanyName}}";
    private const string _code = "{{Code}}";

    public EmailPlaceholderService(IUserService userService)
    {
        _userService = userService;
    }

    public async ValueTask<(EmailTemplate, Dictionary<string, string>)> GetTemplateValues(Guid userId,
        EmailTemplate template, string code = "")
    {
        var placeholders = GetPlaceholders(template.Body);

        var user = await _userService.GetByIdAsync(userId) ?? throw new CustomException(400, "User not found");

        var result = placeholders.Select(placeholder =>
        {
            var value = placeholder switch
            {
                _fullName => string.Join(user.FirstName, " ", user.LastName),
                _firstName => user.FirstName,
                _lastName => user.LastName,
                _date => DateTime.Now.ToString("dd.MM.yyyy"),
                _companyName => "Yashil bozor",
                _code => code,
                _ => throw new EvaluateException("Invalid placeholder")
            };

            return new KeyValuePair<string, string>(placeholder, value);
        });
        var values = new Dictionary<string, string>(result);
        return (template, values);
    }

    private IEnumerable<string> GetPlaceholders(string body)
    {
        var placeholder = new StringBuilder();
        var isStartedToGather = false;
        for (int i = 0; i < body.Length; i++)
        {
            if (body[i] == '{')
            {
                i += 1;
                placeholder = new StringBuilder();
                placeholder.Append("{{");
                isStartedToGather = true;
            }
            else if (body[i] == '}')
            {
                i += 1;
                placeholder.Append("}}");
                isStartedToGather = false;
                yield return placeholder.ToString();
            }
            else if (isStartedToGather)
            {
                placeholder.Append(body[i]);
            }
        }
    }
}