namespace ChatBot_API.Repositoty
{
   
        public interface IUnitOfWork
        {
            IChatMessageRepository ChatMessages { get; }
            IMessageEditRepository MessageEdits { get; }
        

            Task<int> SaveAsync();
    }
    }

