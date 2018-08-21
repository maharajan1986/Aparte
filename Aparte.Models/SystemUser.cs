namespace Aparte.Models
{
    public class SystemUser : Base
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual byte[] PasswordKey { get; set; }
        public virtual byte[] PasswordSalt { get; set; }
        public virtual bool IsGlobalAdmin { get; set; }
    }
}
