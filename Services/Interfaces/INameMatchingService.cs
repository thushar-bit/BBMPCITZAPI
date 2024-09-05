using NUPMS_BA;

namespace BBMPCITZAPI.Services.Interfaces
{
    public interface INameMatchingService
    {

        public int CallNameMatchAPI(string name1, string name2);
        public List<NameMatchingResult> CompareDictionaries(Dictionary<int, string> srcDic, Dictionary<int, string> ekycDic);
    }
}
