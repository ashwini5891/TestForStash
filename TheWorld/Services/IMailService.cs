using System;

namespace FunWithGit.Services
{
  public interface IMailService
  {
    bool SendMail(string to, string from, string subject, string body);
  }
}
