namespace DBVAL
{
    public interface IStorage
    {
        void Put(string key, string value);
        string Get(string key);

        public bool HasTextDuplicate(string text);
    }
}