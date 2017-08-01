namespace BashSoft.Contracts
{
    public interface IDatabase : IRequester, IOrderedTaker, IFilteredTaker
    {
        void LoadData(string fileName);

        void UnloadData();
    }
}