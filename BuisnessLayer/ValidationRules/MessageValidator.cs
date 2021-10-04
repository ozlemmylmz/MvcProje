using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.ValidationRules
{
    public class MessageValidator:AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(x => x.ReceiverMail).NotEmpty().WithMessage("Alııcı adresini boş geçemezsiniz!");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Konuyu boş geçemezsiniz!");
            RuleFor(x => x.MessageContent).NotEmpty().WithMessage("Mesajı adresini boş geçemezsiniz!");
            RuleFor(x => x.Subject).MinimumLength(2).WithMessage("Lütfen en az 2 karakter girişi yapın! ");
            RuleFor(x => x.Subject).MaximumLength(100).WithMessage("Lütfen 100 karakterden fazla değer girişi yapmayınız!");
        }
    }
}
