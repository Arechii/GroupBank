namespace Arechi.GroupBank
{
    public class Bank
    {
        public string GroupId;
        public decimal Money;
        public uint Experience;

        public Bank(string groupId, decimal money, uint experience)
        {
            GroupId = groupId;
            Money = money;
            Experience = experience;
        }
    }
}
