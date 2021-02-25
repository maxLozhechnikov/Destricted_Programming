namespace DBVAL
{
    public interface IStorage
    {
        void Put(string key, string value);
        void PutText(string value);
        string Get(string key);

        public bool HasTextDuplicate(string text);
    }
}